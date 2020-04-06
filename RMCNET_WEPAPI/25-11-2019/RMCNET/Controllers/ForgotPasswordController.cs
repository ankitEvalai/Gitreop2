using RMCNET.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using BCrypt.Net;
using System.Net.Mail;


namespace RMCNET.Controllers
{
    public class ForgotPasswordController : ApiController
    {
          [HttpPost]
        public CommonModal ForgetPassword(ForgetPasswordModel a)
        {
            CommonModal e1 = new CommonModal();
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@email_Id", SqlDbType.NVarChar).Value = a.email_Id;
            cmd.Parameters.Add("@ip_Address", SqlDbType.NVarChar).Value = a.ip_Address;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
          
         //   cmd.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                string name= Dt.Rows[0]["name"].ToString();
                string token = Dt.Rows[0]["token"].ToString();
                e1.ResponseStatus = Dt.Rows[0]["status"].ToString();
                e1.ResponseMessage = Dt.Rows[0]["MSG"].ToString();
               SendEmail(a.email_Id, "Password Reset - RMCNET", "<div><b>Hi "+ name + "</b>,<br/><br/>  Please click the below link to reset your password <br/> http://rmcnet.in/#/changepassword?token=" + token + "<br/><br/><br/>Best Regards,"+ "<br/><span style='color: Tomato; '><b>RMCNET TEAM</b></span>" + "</div>");
                //e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Change Password Failed";
            }
            return e1;
        }

        public string SendEmail(string toAddress, string subject, string body)
        {
            
            string result = "Message Sent Successfully..!!";
            string senderID = "thevalaiinfotech@gmail.com";// use sender’s email id here..
            const string senderPassword = "Thevalai!nfotech"; // sender password here…
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // smtp server address here…
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailAddress from = new MailAddress("thevalaiinfotech@gmail.com", "RMCNET");
                MailMessage message = new MailMessage(from.ToString(), toAddress, subject, body);
               MailAddress bcc = new MailAddress("ravikiran@evalai.com");
                message.Bcc.Add(bcc);
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!!";
            }
            return result;
        }
         [HttpPost]
        public CommonModal UpdatePassword(RegistrationModel a)
        {
      
            CommonModal e1 = new CommonModal();
            string encrypt_pw = encryptPassword(a.passWord);

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@passWord", SqlDbType.NVarChar).Value = encrypt_pw;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
            cmd.Parameters.Add("@token", SqlDbType.NVarChar).Value = a.token;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();

            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = Dt.Rows[0]["status"].ToString();
                e1.ResponseMessage = Dt.Rows[0]["MSG"].ToString();
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "password update Failed";
            }
            return e1;
        }

        public string encryptPassword(string l)
        {
            string myPassword = l;
            string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
            //mySalt == "$2a$10$rBV2JDeWW3.vKyeQcM8fFO"
            string myHash = BCrypt.Net.BCrypt.HashPassword(myPassword, mySalt);
            return myHash;
        }


       // [HttpPost]
        //public CommonModal mobileForgetPassword(ForgetPasswordModel a)
        //{
        //    CommonModal e1 = new CommonModal();
        //    SqlConnection conn = new SqlConnection(commonCode.conStr);
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = a.email_Id;
        //    //cmd.Parameters.Add("@ip_Address", SqlDbType.NVarChar).Value = a.ip_Address;
        //    cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;

        //    //   cmd.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    da.Fill(ds);
        //    DataTable Dt = ds.Tables[0];
        //    conn.Close();
        //    if (Dt.Rows.Count > 0)
        //    {
        //        string name = Dt.Rows[0]["name"].ToString();
        //        string token = Dt.Rows[0]["token"].ToString();
        //        e1.ResponseStatus = Dt.Rows[0]["status"].ToString();
        //        e1.ResponseMessage = Dt.Rows[0]["MSG"].ToString();
        //        SendEmail(a.email_Id, "Password Reset - RMCNET", "<div><b>Hi " + name + "</b>,<br/><br/>  Please click the below link to reset your password <br/> http://rmcnet.in/#/changepassword?token=" + token + "<br/><br/><br/>Best Regards," + "<br/><span style='color: Tomato; '><b>RMCNET TEAM</b></span>" + "</div>");
        //        //e1.data = Dt;
        //    }
        //    else
        //    {
        //        e1.ResponseStatus = "False";
        //        e1.ResponseMessage = "Change Password Failed";
        //    }
        //    return e1;
        //}

        private static string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }



        [HttpPost]
        public CommonModal sendMobileMessage(SendMobileMessage sms)
        {
            CommonModal e1 = new CommonModal();
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            SqlCommand cmd = new SqlCommand("Pro_SendMobileMessage", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = sms.mobile_No;
            cmd.Parameters.Add("@otp", SqlDbType.NVarChar).Value = GenerateNewRandom();
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = sms.mode;
            conn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = Dt.Rows[0]["status"].ToString();
                e1.ResponseMessage = Dt.Rows[0]["MSG"].ToString();
                e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Error in c#";
            }
            return e1;

        }


        [HttpPost]
        public string otpupdatepassword(RegistrationModel a)
        {

            string encrypt_pw = encryptPassword(a.passWord);

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = a.login_Id;
            cmd.Parameters.Add("@passWord", SqlDbType.NVarChar).Value = encrypt_pw;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "OTP_Forgot_Update_Password";

            int n = (int)cmd.ExecuteNonQuery();
            conn.Close();
            if (n > 0)
                return "true";
                    
            return "False";
        }

    }
}
