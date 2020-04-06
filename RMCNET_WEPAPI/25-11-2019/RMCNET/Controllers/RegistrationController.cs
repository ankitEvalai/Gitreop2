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
    public class RegistrationController : ApiController
    {
        RegistrationModel a = new RegistrationModel();
        [HttpPost]
        public CommonModal Insert(RegistrationModel a)
        {

            string link = "";
            CommonModal e1 = new CommonModal();
            string encrypt_pw = encryptPassword(a.passWord);
           
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            string email_id="";
           if(a.email_Id==null || a.email_Id == "")
            {
                email_id = a.first_Name + a.last_Name + "@gmail.com";
                cmd.Parameters.Add("@email_Id", SqlDbType.NVarChar).Value = email_id;
            }
            else
            {
                cmd.Parameters.Add("@email_Id", SqlDbType.NVarChar).Value = a.email_Id;
            }

            string otp = GenerateNewRandom();
            cmd.Parameters.Add("@otp", SqlDbType.NVarChar).Value = otp;
            cmd.Parameters.Add("@passWord", SqlDbType.NVarChar).Value = encrypt_pw;
            cmd.Parameters.Add("@first_Name", SqlDbType.NVarChar).Value = a.first_Name;
            cmd.Parameters.Add("@last_Name", SqlDbType.NVarChar).Value = a.last_Name;
            cmd.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = a.mobile_No;
            cmd.Parameters.Add("@userType_Id", SqlDbType.Int).Value = a.userType_Id;
            cmd.Parameters.Add("@ip_Address", SqlDbType.NVarChar).Value = a.ip_Address;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            cmd.Parameters.Add("@token", SqlDbType.NVarChar).Value = token; 
            cmd.Parameters.Add("@web_mobile", SqlDbType.NVarChar).Value = a.web_mobile;
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

                //SqlConnection conn1 = new SqlConnection(commonCode.conStr);
                //conn1.Open();
                //SqlCommand cmd1 = new SqlCommand("Pro_LoginCRUD", conn);
                //cmd1.CommandType = CommandType.StoredProcedure;
                //cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "LINKURL";
                //DataSet ds1 = new DataSet();
                //SqlDataAdapter da1 = new SqlDataAdapter();
                //da1.SelectCommand = cmd1;
                //da1.Fill(ds1);
                //DataTable Dt1 = ds1.Tables[0];
                //conn1.Close();

                //link = Dt1.Rows[0]["link"].ToString();






              //  SendEmail(a.email_Id, "Confirm Registration", "<div><b>Welcome to RMCNET</b>,<br/><br/>  Please click the below link to confirm to your registration " + link + token+ "<br/><br/><br/>Best Regards,"+ "<br/><span style='color: Tomato; '><b>RMCNET</b></span>" + "</div>");
                //e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Registration Failed";
            }
            return e1;
        }

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

        public string getbody(string link)
        {
            string body = "Welcome to RMCNET  please click the below link to confirm to your link " + link;
            return body;
        }


        [HttpGet]
        public string confirmregistration(string token)
        {

            CommonModal e2 = new CommonModal();
            SqlConnection conn1 = new SqlConnection(commonCode.conStr);
            conn1.Open();
            SqlCommand cmd1 = new SqlCommand("Pro_LoginCRUD", conn1);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;
            
            cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "UPDATETOKEN";
            int n = (int)cmd1.ExecuteNonQuery();
          
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd1;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn1.Close();
            if (Dt.Rows.Count > 0)
            {
                return "<div style='text-align:center'>Your eMail is verified.<br/><a href='http://rmcnet.in/#/login'>Please click here to login on RMCNET</a></div>";
            }
            else
            {
                return "Registration Failed";
            }
        }

        public string SendEmail(string toAddress, string subject, string body)
        {
            //string toAddress = "ramkumar@evalai.com";
            //string subject = "Mail from C#";
            //string body = "Sample mail from c#";
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
                //MailAddress bcc = new MailAddress("vasu@evalai.com");
                //message.Bcc.Add(bcc);
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
        public CommonModal getLogin(RegistrationModel l)
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = l.mobile_No;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETHASH_PWD";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(Dt.Rows[0]["is_Active"]) == 0)
                {
                    SqlConnection conn1 = new SqlConnection(commonCode.conStr);
                    conn1.Open();
                    SqlCommand cmd1 = new SqlCommand("Pro_LoginCRUD", conn);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    string otp = GenerateNewRandom();
                    cmd1.Parameters.Add("@otp", SqlDbType.NVarChar).Value = otp;
                    cmd1.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = l.mobile_No;
                    cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "SECOND_TIME_NEW_OTP";
                    DataSet ds1 = new DataSet();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    da1.SelectCommand = cmd1;
                    da1.Fill(ds1);
                    DataTable Dt1 = ds1.Tables[0];
                    conn1.Close();

                    e1.ResponseStatus = Dt1.Rows[0]["status"].ToString(); 
                    e1.ResponseMessage = Dt1.Rows[0]["MSG"].ToString();
                    e1.data = Dt1;
                    return e1;
                }
                else if (decrypePassword(l.passWord, Dt.Rows[0]["passWord"].ToString()))
                {
                    e1 = getLogindetails(l.mobile_No);
                }
                else
                {
                    e1.ResponseStatus = "False";
                    e1.ResponseMessage = "Login Failed - Password Wrong";
                    e1.data = Dt;
                }
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Login Failed - Mobile Number not registered";
                e1.data = Dt;
            }

            return e1;
        }
        [HttpGet]
        public CommonModal getLogindetails(string mobile_No)
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = mobile_No;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GET_LOGIN_DETAILS";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = "True";
                e1.ResponseMessage = Dt.Rows[0]["Message"].ToString();
                e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Login Failed!! Please Check username and Password";
                e1.data = Dt;
            }
            return e1;
        }
        [HttpPost]
        public CommonModal Update(RegistrationModel a)
        {
            CommonModal e1 = new CommonModal();
            //string encrypt_pw = encryptPassword(a.passWord);
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = a.login_Id;
            cmd.Parameters.Add("@first_Name", SqlDbType.NVarChar).Value = a.first_Name;
            cmd.Parameters.Add("@last_Name", SqlDbType.NVarChar).Value = a.last_Name;
            cmd.Parameters.Add("@email_Id", SqlDbType.NVarChar).Value = a.email_Id;
          
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            DataTable Dt1 = ds.Tables[1];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = "True";
                e1.ResponseMessage = "Update Successfull";
                e1.data = Dt1;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Registration Failed";
            }
            return e1;
        }
       [HttpPost]
        public CommonModal Checkpassword(RegistrationModel l)
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = l.login_Id;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "CHECKPASSWORD";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {

                if (decrypePassword(l.passWord, Dt.Rows[0]["passWord"].ToString()))
                {
                    e1 = updateLogindetails(l.login_Id, l.passWordnew);
                }
                else
                {
                    e1.ResponseStatus = "False";
                    e1.ResponseMessage = "Old Password Wrong";
                    e1.data = Dt;
                }
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Old Password Wrong";
                e1.data = Dt;
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
        public bool decrypePassword(string password, string fetched_Password)
        {
            return BCrypt.Net.BCrypt.Verify(password, fetched_Password);
        }
        [HttpGet]
        public CommonModal updateLogindetails( int login_Id,  string passWordnew)
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = login_Id;
            cmd.Parameters.Add("@passWord", SqlDbType.NVarChar).Value = encryptPassword(passWordnew);

            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "Update_Password";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = Dt.Rows[0]["status"].ToString();
                e1.ResponseMessage = Dt.Rows[0]["Msg"].ToString();
                e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Password Update failed";
                e1.data = Dt;
            }
            return e1;
        }

        [HttpPost]
        public CommonModal verifyotp(SendMobileMessage sm)
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_Id", SqlDbType.NVarChar).Value = sm.login_Id;
            cmd.Parameters.Add("@otp", SqlDbType.NVarChar).Value = sm.otp;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "verifyotp";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = Dt.Rows[0]["status"].ToString() ;
                e1.ResponseMessage = Dt.Rows[0]["Msg"].ToString();
                e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Verification Failed!!";
               // e1.data = Dt;
            }
            return e1;
        }

        [HttpPost]
        public CommonModal onlynuberlogin(RegistrationModel a)
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = a.mobile_No;
            //cmd.Parameters.Add("@userType_Id", SqlDbType.Int).Value = a.userType_Id;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "ONLYNUMBERLOGIN";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
           
            if (Dt.Rows.Count > 0 )
            {
   
             
                    if (Convert.ToInt32(Dt.Rows[0]["is_Active"]) == 0)
                    {
                        SqlConnection conn1 = new SqlConnection(commonCode.conStr);
                        conn1.Open();
                        SqlCommand cmd1 = new SqlCommand("Pro_LoginCRUD", conn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        string otp = GenerateNewRandom();
                        cmd1.Parameters.Add("@otp", SqlDbType.NVarChar).Value = otp;
                        cmd1.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = a.mobile_No;
                        cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "SECOND_TIME_NEW_OTP";
                        DataSet ds1 = new DataSet();
                        SqlDataAdapter da1 = new SqlDataAdapter();
                        da1.SelectCommand = cmd1;
                        da1.Fill(ds1);
                        DataTable Dt1 = ds1.Tables[0];
                        conn1.Close();

                        e1.ResponseStatus = Dt1.Rows[0]["status"].ToString();
                        e1.ResponseMessage = Dt1.Rows[0]["MSG"].ToString();
                        e1.data = Dt1;
                        // return e1;
                    }

                    else if (Convert.ToInt32(Dt.Rows[0]["is_Active"]) == 1)
                    {
                        e1.ResponseStatus = "True";
                        e1.ResponseMessage = "";
                        e1.data = Dt;
                    }
                


                else
                {
                    string link = "";
                    CommonModal e2 = new CommonModal();
                    string encrypt_pw = encryptPassword(a.mobile_No);

                    SqlConnection conn1 = new SqlConnection(commonCode.conStr);
                    conn1.Open();
                    SqlCommand cmd1 = new SqlCommand("Pro_LoginCRUD", conn1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    string email_id  = "rmcnet" + a.mobile_No + "@gmail.com";
                    
                   

                    string otp = GenerateNewRandom();
                    cmd1.Parameters.Add("@otp", SqlDbType.NVarChar).Value = otp;
                    cmd1.Parameters.Add("@passWord", SqlDbType.NVarChar).Value = encrypt_pw;
                    cmd1.Parameters.Add("@email_Id", SqlDbType.NVarChar).Value = email_id;
                    //cmd1.Parameters.Add("@last_Name", SqlDbType.NVarChar).Value = a.last_Name;
                    cmd1.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = a.mobile_No;
                    cmd1.Parameters.Add("@userType_Id", SqlDbType.Int).Value = a.userType_Id;
                    cmd1.Parameters.Add("@ip_Address", SqlDbType.NVarChar).Value = a.ip_Address;
                    cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "Newnumberregistration";
                    string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    cmd1.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;
                    cmd1.Parameters.Add("@web_mobile", SqlDbType.NVarChar).Value = a.web_mobile;
                    DataSet ds1 = new DataSet();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    da1.SelectCommand = cmd1;
                    da1.Fill(ds1);
                    DataTable Dt1 = ds1.Tables[0];
                    conn1.Close();

                    if (Dt1.Rows.Count > 0)
                    {
                        e2.ResponseStatus = Dt1.Rows[0]["status"].ToString();
                        e2.ResponseMessage = Dt1.Rows[0]["MSG"].ToString();
                        e2.data = Dt1;


                    }
                    else
                    {
                        e2.ResponseStatus = "False";
                        e2.ResponseMessage = "Registration Failed";
                    }


                    return e2;
                }
            }
            return e1;


        }

        [HttpPost]
        public CommonModal OnlynumberCheckpassword(string mobile_No,string password)
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mobile_No", SqlDbType.NVarChar).Value = mobile_No;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "CHECKPASSWORD1";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {

                if (decrypePassword(password, Dt.Rows[0]["passWord"].ToString()))
                {
                    e1 = getLogindetails(mobile_No);
                }
                else
                {
                    e1.ResponseStatus = "False";
                    e1.ResponseMessage = "Password Wrong";
                    e1.data = Dt;
                }
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Number Wrong";
                e1.data = Dt;
            }
            return e1;
        }

        [HttpPost]
        public CommonModal Onlynumberverifyotp(SendMobileMessage sm)
        {
            CommonModal e1 = new CommonModal();
            if (sm.mode1== 1)
            {

                

                SqlConnection conn = new SqlConnection(commonCode.conStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@login_Id", SqlDbType.NVarChar).Value = sm.login_Id;
                cmd.Parameters.Add("@otp", SqlDbType.NVarChar).Value = sm.otp;

                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "verifyotp1";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataTable Dt = ds.Tables[0];
                conn.Close();
                if (Dt.Rows[0]["status"].ToString().Equals("True"))
                {
                    CommonModal e2 = new CommonModal();
                    string encrypt_pw = encryptPassword(sm.passWord);
                    SqlConnection conn1 = new SqlConnection(commonCode.conStr);
                    conn1.Open();
                    SqlCommand cmd1 = new SqlCommand("Pro_LoginCRUD", conn1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@login_Id", SqlDbType.Int).Value = sm.login_Id;
                    cmd1.Parameters.Add("@passWord", SqlDbType.NVarChar).Value = encrypt_pw;
                    cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "updatepassword1";
                    int n = (int)cmd1.ExecuteNonQuery();
                    conn1.Close();
                    //DataSet ds1 = new DataSet();
                    //SqlDataAdapter da1 = new SqlDataAdapter();
                    //da1.SelectCommand = cmd1;
                    //da1.Fill(ds1);
                    //DataTable Dt1 = ds1.Tables[0];
                    //conn1.Close();

                    e2.ResponseStatus = "True";
                    e2.ResponseMessage = "Registration success";

                    
                    e2.data = Dt;
                    return e2;


                }

                else
                {
                    e1.ResponseStatus = "False";
                    e1.ResponseMessage = "Verification Failed!!";
                    // e1.data = Dt;
                }
                return e1;
            }
            else
            {

              e1=  OnlynumberCheckpassword(sm.mobile_No,sm.passWord);
                return e1;
            }

        }

        [HttpGet]
        public DataTable getvideo()
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);

            SqlCommand cmd = new SqlCommand("Pro_LoginCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "VIDEOUPLOAD";

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            return Dt;
        }


    }
}