using RMCNET.Models;
using SCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace RMCNET.Controllers
{
    public class PaymentController : ApiController
    {
        public string[] output = new string[3];
        public static string chesksumValue;
        public static string chesksumKey;
        public static int transId;
        public string testurl;
        public static string salt;
        PaymentModel frm = new PaymentModel();
        public string[] feeArray = new string[100];
        public static PaymentModel det;
        public static PaymentModel detmobile;

        [HttpPost]
        public string getArray(PaymentModel things)
        {
            try
            {

                int n = 0;
                det = things;
                SqlConnection conn = new SqlConnection(commonCode.conStr);
                SqlCommand cmd = new SqlCommand("Pro_onlinePayment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = things.login_id;
                cmd.Parameters.Add("@ad_id", SqlDbType.Int).Value = things.ad_id;
                cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = things.category_id;
                cmd.Parameters.Add("@subcategory_id", SqlDbType.Int).Value = things.subcategory_id;
                cmd.Parameters.Add("@transaction_Amount", SqlDbType.Int).Value = things.transaction_Amount;
                cmd.Parameters.Add("@email_id", SqlDbType.NVarChar).Value = things.email_id;


                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GET_MERCHANT_DETAILS";
                conn.Open();
                List<OnlinePayment> data = new List<OnlinePayment>();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataTable Dt = ds.Tables[0];
                data = commonCode.ConvertDataTable<OnlinePayment>(Dt);
                if (data[0].provider == "TRACKNPAY")
                {
                    // chesksumKey = data[0].checksum_key;
                    transId = data[0].transId;

                    things.transaction_Id = transId;



                    det = things;

                    // Conver the string to the checksum value
                    chesksumValue = Generatehash512(data[0].salt + "|"+things.category_id+"|"+ things.ad_id + "|" + things.transaction_Amount + "|" + data[0].apiKey + "|city|IND|INR|" + data[0].description + "|" + data[0].email + "|" + data[0].mode + "|" + data[0].customerName.TrimEnd() + "|" + data[0].transId + "|" +
                        data[0].mobileNumber + "|http://api.rmcnet.in//api/Payment/return_responseTracknPay|state|udf1|udf2|udf3|udf4|udf5|000000");
                    //string msg = data[0].merchent_id + "|" + data[0].transId + "|NA|" + things[0].amount + "|NA|NA|NA|INR|NA|R|" + data[0].security_id + "|NA|NA|F|NA|NA|NA|NA|NA|NA|NA|http://localhost:1675/api/Payment/return_response" + "|" + chesksumValue;
                    string url = HttpContext.Current.Request.Url.AbsoluteUri;
                    salt = data[0].salt;
                    //  chesksumValue = Generatehash512(data[0].salt + "|" + things[0].amount + "|" + data[0].apiKey + "|city|IND|INR|" + data[0].email + "|LIVE|" + data[0].StudentName + "|" + data[0].order_id + "|" + data[0].mobileNumber + "|http://localhost:59904/api/Payment/Return|" + data[0].zipCode);
                    RemotePost remotepost = new RemotePost();
                    remotepost.Url = "https://biz.traknpay.in/v2/getpaymentrequest?api_key=" + data[0].apiKey +
                        "&return_url=http://api.rmcnet.in//api/Payment/return_responseTracknPay&mode=" + data[0].mode +
                        "&order_id=" + data[0].transId + "&amount=" + things.transaction_Amount + "&name=" + data[0].customerName.TrimEnd() +
                        "&currency=INR&description=" + data[0].description +
                        "&address_line_1="+ things.category_id + "&address_line_2="+things.ad_id+"&phone=" + data[0].mobileNumber +
                        "&email=" + data[0].email +
                        "&city=city&state=state&country=IND&zip_code=000000&udf1=udf1&udf2=udf2&udf3=udf3&udf4=udf4&udf5=udf5&hash=" + chesksumValue;
                    testurl = remotepost.Url.ToString();

                    //enterMsg(things[0].org_Id, things[0].academic_Id, things[0].student_Id, chesksumValue, url, testurl);
                    return testurl;
                }
                return testurl;
            }
            catch (Exception e)
            {
                return "fail";
            }
        }
        //private void SubmitRequest(List<OnlinePayment> data)
        #region  ############# TRACK N PAY INTEGRATION  BEGINS ##########################
        [HttpPost]
        public string SubmitRequest(string url)
        {

            string[] hash_columns = {
            "amount",
            "api_key",
            "city",
            "country",
            "currency",
            "email",
            "mode",
            "name",
            "order_id",
            "phone",
            "return_url",
            "zip_code"
            };
            RemotePost remotepost = new RemotePost();
            return remotepost.Url;
        }

        private string gethash(string[] hash_columns, TrackNpayModal request)
        {
            throw new NotImplementedException();
        }


        #region Get Response From the payment Gateway After payment
        [HttpPost]
        public IHttpActionResult return_responseTracknPay(FormDataCollection model)
        {

            string link = "";

            string return_url = model.Get("return_url");
            string address_line_1 = model.Get("address_line_1");
            string address_line_2 = model.Get("address_line_2");
            string amount = model.Get("amount");
            string api_key = model.Get("api_key");
            string cardmasked = model.Get("cardmasked");
            string city = model.Get("city");
            string country = model.Get("country");
            string currency = model.Get("currency");
            string description = model.Get("description");
            string email = model.Get("email");
            string error_desc = model.Get("error_desc");
            string name = model.Get("name");
            string order_id = model.Get("order_id");
            string payment_channel = model.Get("payment_channel");
            string payment_datetime = model.Get("payment_datetime");
            string payment_mode = model.Get("payment_mode");
            string phone = model.Get("phone");
            string state = model.Get("state");
            string response_code = model.Get("response_code");
            string responseMessage = model.Get("response_message");
            string transaction_id = model.Get("transaction_id");
            string udf1 = model.Get("udf1");
            string udf2 = model.Get("udf2");
            string udf3 = model.Get("udf3");
            string udf4 = model.Get("udf4");
            string udf5 = model.Get("udf5");
            string zip_code = model.Get("zip_code");
            string hashValue = model.Get("hash");
            string keys = Request.ToString();


            var checkSumData = salt + "|" + address_line_1 + "|" + address_line_2 + "|" + amount + "|";
            if (cardmasked != "")
            {
                checkSumData += cardmasked + "|" + city + "|" + country + "|" + currency + "|" + description + "|" + email + "|";
            }
            else
            {
                checkSumData += city + "|" + country + "|" + currency + "|" + description + "|" + email + "|";
            }
            if (error_desc != "")
            {
                checkSumData += error_desc + "|" + name + "|" + order_id + "|";
            }
            else
            {
                checkSumData += name + "|" + order_id + "|";
            }
            if (payment_channel != "")
            {
                checkSumData += payment_channel + "|" + payment_datetime + "|";
            }
            else
            {
                checkSumData += payment_datetime + "|";
            }
            if (payment_mode != "")
            {
                checkSumData += payment_mode + "|" + phone + "|" + response_code + "|" + responseMessage + "|" + state + "|";
            }
            else
            {
                checkSumData += phone + "|" + response_code + "|" + responseMessage + "|" + state + "|";
            }
            if (transaction_id != "")
            {
                checkSumData += transaction_id + "|" + udf1 + "|" + udf2 + "|" + udf3 + "|" + udf4 + "|" + udf5 + "|" + zip_code;
            }
            else
            {
                checkSumData += udf1 + "|" + udf2 + "|" + udf3 + "|" + udf4 + "|" + udf5 + "|" + zip_code;
            }
            chesksumValue = Generatehash512(checkSumData);
            if (chesksumValue == hashValue) //Check whether the both checksum are equal or not
            {
               // salt = "";
                SqlConnection conn = new SqlConnection(commonCode.conStr);
                SqlCommand cmd = new SqlCommand("Pro_onlinePayment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@msg", SqlDbType.NVarChar).Value = chesksumValue;
                cmd.Parameters.Add("@transaction_Id", SqlDbType.NVarChar).Value = order_id;
                cmd.Parameters.Add("@payment_trans_Id", SqlDbType.NVarChar).Value = transaction_id;
                cmd.Parameters.Add("@payment_Vendor", SqlDbType.NVarChar).Value = "TrackNPay";
                cmd.Parameters.Add("@merchant_id", SqlDbType.NVarChar).Value = 0;
                cmd.Parameters.Add("@currency_Code", SqlDbType.NVarChar).Value = currency;
                cmd.Parameters.Add("@payment_status_Id", SqlDbType.NVarChar).Value = response_code;
                cmd.Parameters.Add("@transaction_status", SqlDbType.NVarChar).Value = responseMessage;
                cmd.Parameters.Add("@transaction_amount", SqlDbType.Decimal).Value = Convert.ToDecimal(amount);
                cmd.Parameters.Add("@transaction_Date", SqlDbType.DateTime).Value = payment_datetime;
                cmd.Parameters.Add("@web_mobile", SqlDbType.NVarChar).Value = "website";
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "INSERT_PAYMENT_TRANSACTION";
                conn.Open();
                //List<OnlinePayment> data = new List<OnlinePayment>();

                int ad = (int)cmd.ExecuteNonQuery();
                conn.Close();

                // det[0].reference_Code = values[2].ToString();

                if (ad >= 0)
                {
                    SqlConnection conn1 = new SqlConnection(commonCode.conStr);
                    SqlCommand cmd1 = new SqlCommand("Pro_onlinePayment", conn1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@category_id", SqlDbType.Int).Value = Convert.ToInt32(address_line_1);
                    cmd1.Parameters.Add("@payment_status", SqlDbType.Int).Value = response_code;
                    cmd1.Parameters.Add("@ad_id", SqlDbType.Int).Value = Convert.ToInt32(address_line_2);
                    cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "UPDATEPAYMENTSTATUS";
                    conn1.Open();

                    int ad1 = (int)cmd1.ExecuteNonQuery();
                    conn1.Close();
                    SqlConnection conn2 = new SqlConnection(commonCode.conStr);
                    SqlCommand cmd2 = new SqlCommand("Pro_LoginCRUD", conn2);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "LINKURL";
                    DataSet ds1 = new DataSet();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    da1.SelectCommand = cmd2;
                    da1.Fill(ds1);
                    DataTable Dt1 = ds1.Tables[0];
                   
              
                    link = Dt1.Rows[1]["link"].ToString();
                    conn2.Close();
                    System.Uri uri = new System.Uri(link + order_id); // After Insertion Redirect the payment gateway page to valaischool.com
                    return Redirect(uri);
                }
                else
                {
                    System.Uri uri = new System.Uri(link + order_id); // After Insertion Redirect the payment gateway page to valaischool.com
                    return Redirect(uri);
                }





            }
            else
            {
                System.Uri uri = new System.Uri(link + order_id); // After Insertion Redirect the payment gateway page to valaischool.com
                return Redirect(uri);
            }


        }
        #endregion

        public string Generatehash512(string text)
        {

            byte[] message = System.Text.Encoding.UTF8.GetBytes(text);

            System.Text.UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex.ToUpper();

        }

        public class RemotePost
        {
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
                System.Web.HttpContext.Current.Response.ToString();
            }
        }
        #endregion ############# TRACK N PAY INTEGRATION  ENDS ##########################

        #region Mobile App Payment
        // ------------------------- Mobile App Parent Payment ---------------------------//
        [HttpPost]
        public OnlinePayment getMobileAppArray(PaymentModel things1)
        {
            List<OnlinePayment> data = new List<OnlinePayment>();
            OnlinePayment retData = new OnlinePayment();
            try
            {
                int n = 0;
                detmobile = things1;
                SqlConnection conn = new SqlConnection(commonCode.conStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Pro_onlinePayment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = things1.login_id;
                cmd.Parameters.Add("@ad_id", SqlDbType.Int).Value = things1.ad_id;
                cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = things1.category_id;
                cmd.Parameters.Add("@subcategory_id", SqlDbType.Int).Value = things1.subcategory_id;
                cmd.Parameters.Add("@transaction_Amount", SqlDbType.Int).Value = things1.transaction_Amount;
                cmd.Parameters.Add("@email_id", SqlDbType.NVarChar).Value = things1.email_id;
                
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GET_MERCHANT_DETAILS";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataTable Dt = ds.Tables[0];
                conn.Close();
                data = commonCode.ConvertDataTable<OnlinePayment>(Dt);
                // chesksumKey = data[0].checksum_key;
                if (data[0].provider == "TRACKNPAY")
                {
                    transId = data[0].transId;
                    things1.transaction_Id = transId;


                    retData.address_line_1 = things1.category_id.ToString();
                    retData.address_line_2 = things1.ad_id.ToString();
                    retData.amount = things1.transaction_Amount.ToString();
                    retData.apiKey = data[0].apiKey;
                    retData.city = "city";
                    retData.country = "IND";
                    retData.currency = "INR";
                    retData.description = data[0].description;
                    retData.email = data[0].email;
                    retData.mobileNumber = data[0].mobileNumber;
                    retData.mode = data[0].mode;
                    retData.order_id = data[0].transId.ToString();
                    retData.return_url = "http://rmcnet.in/";
                    retData.salt = data[0].salt;
                    retData.state = "state";
                    retData.customerName = data[0].customerName;
                    retData.transId = data[0].transId;
                    retData.udf1 = "udf1";
                    retData.udf2 = "udf2";
                    retData.udf3 = "udf3";
                    retData.udf4 = "udf4";
                    retData.udf5 = "udf5";
                    retData.zipCode = "000000";
                    retData.provider = data[0].provider;

                    // Convert the string to the checksum value
                    chesksumValue = Generatehash512(data[0].salt + "|"+things1.category_id+"|"+things1.ad_id+"|" + things1.transaction_Amount + "|" + data[0].apiKey + "|city|IND|INR|" + data[0].description + "|" + data[0].email + "|" + data[0].mode + "|" +
                      data[0].customerName + "|" + data[0].transId + "|" + data[0].mobileNumber + "|http://api.rmcnet.in//api/Payment/return_responseTracknPay|state|udf1|udf2|udf3|udf4|udf5|" + data[0].zipCode);
                    string url = HttpContext.Current.Request.Url.AbsoluteUri;
                    salt = data[0].salt;

                    //retData.checksum_key = chesksumValue;

                    //RemotePost remotepost = new RemotePost();
                    //remotepost.Url = "https://biz.traknpay.in/v2/getpaymentrequest?api_key=" + data[0].apiKey + "&return_url=https://valaischool.com/api/Payment/return_responseTracknPay&mode=" + data[0].mode + "&order_id=" + data[0].transId + "&amount=" + things1[0].amount + "&name=" + data[0].StudentName + "&currency=INR&description=" + data[0].description + "&address_line_1=address_line_1&address_line_2=address_line_2&phone=" + data[0].mobileNumber + "&email=" + data[0].email + "&city=" + data[0].city + "&state=state&country=IND&zip_code=" + data[0].zipCode + "&udf1=udf1&udf2=udf2&udf3=udf3&udf4=udf4&udf5=udf5&hash=" + chesksumValue;
                    //testurl = remotepost.Url.ToString();
                    //enterMsg(things1[0].org_Id, things1[0].academic_Id, things1[0].student_Id, chesksumValue, url, testurl);


                }
                return retData;
            }
            catch (Exception e)
            {
                return retData;
            }
        }
        #endregion
        #region Mobile App Payment Response From the payment Gateway

        // ------------------------- TracknPay Response ---------------------------//
        // Return 1- success     0- failure payment  3- hash mismatch
        [HttpPost]
        public int return_response_mobileApp_TracknPay(TrackNpayModal MobTPmodel)
        {
            MobTPmodel.keys = Request.ToString();
            int status;

            var checkSumData = salt + "|" + MobTPmodel.address_line_1 + "|" + MobTPmodel.address_line_2 + "|" + MobTPmodel.amount + "|";
            if (MobTPmodel.cardmasked != "")
            {
                checkSumData += MobTPmodel.cardmasked + "|" + MobTPmodel.city + "|" + MobTPmodel.country + "|" + MobTPmodel.currency + "|" + MobTPmodel.description + "|" + MobTPmodel.email + "|";
            }
            else
            {
                checkSumData += MobTPmodel.city + "|" + MobTPmodel.country + "|" + MobTPmodel.currency + "|" + MobTPmodel.description + "|" + MobTPmodel.email + "|";
            }
            if (MobTPmodel.error_desc != "")
            {
                checkSumData += MobTPmodel.error_desc + "|" + MobTPmodel.name + "|" + MobTPmodel.order_id + "|";
            }
            else
            {
                checkSumData += MobTPmodel.name + "|" + MobTPmodel.order_id + "|";
            }
            if (MobTPmodel.payment_channel != "")
            {
                checkSumData += MobTPmodel.payment_channel + "|" + MobTPmodel.payment_datetime + "|";
            }
            else
            {
                checkSumData += MobTPmodel.payment_datetime + "|";
            }
            if (MobTPmodel.payment_mode != "")
            {
                checkSumData += MobTPmodel.payment_mode + "|" + MobTPmodel.phone + "|" + MobTPmodel.response_code + "|" + MobTPmodel.responseMessage + "|" + MobTPmodel.state + "|";
            }
            else
            {
                checkSumData += MobTPmodel.phone + "|" + MobTPmodel.response_code + "|" + MobTPmodel.responseMessage + "|" + MobTPmodel.state + "|";
            }
            if (MobTPmodel.transaction_id != "")
            {
                checkSumData += MobTPmodel.transaction_id + "|" + MobTPmodel.udf1 + "|" + MobTPmodel.udf2 + "|" + MobTPmodel.udf3 + "|" + MobTPmodel.udf4 + "|" + MobTPmodel.udf5 + "|" + MobTPmodel.zip_code;
            }
            else
            {
                checkSumData += MobTPmodel.udf1 + "|" + MobTPmodel.udf2 + "|" + MobTPmodel.udf3 + "|" + MobTPmodel.udf4 + "|" + MobTPmodel.udf5 + "|" + MobTPmodel.zip_code;
            }
            chesksumValue = Generatehash512(checkSumData);

            if (chesksumValue == MobTPmodel.hashValue) //Check whether the both checksum are equal or not
            {
               // salt = "";
                SqlConnection conn = new SqlConnection(commonCode.conStr);
                SqlCommand cmd = new SqlCommand("Pro_onlinePayment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@msg", SqlDbType.NVarChar).Value = chesksumValue;
                cmd.Parameters.Add("@transaction_Id", SqlDbType.NVarChar).Value = MobTPmodel.order_id;
                cmd.Parameters.Add("@payment_trans_Id", SqlDbType.NVarChar).Value = MobTPmodel.transaction_id;
                cmd.Parameters.Add("@payment_Vendor", SqlDbType.NVarChar).Value = "TrackNPay";
                cmd.Parameters.Add("@merchant_id", SqlDbType.NVarChar).Value = 0;
                cmd.Parameters.Add("@currency_Code", SqlDbType.NVarChar).Value = MobTPmodel.currency;
                cmd.Parameters.Add("@payment_status_Id", SqlDbType.NVarChar).Value = MobTPmodel.response_code;
                cmd.Parameters.Add("@transaction_status", SqlDbType.NVarChar).Value = MobTPmodel.responseMessage;
                cmd.Parameters.Add("@transaction_amount", SqlDbType.Decimal).Value = Convert.ToDecimal(MobTPmodel.amount);
                cmd.Parameters.Add("@transaction_Date", SqlDbType.DateTime).Value = MobTPmodel.payment_datetime;
                cmd.Parameters.Add("@web_mobile", SqlDbType.NVarChar).Value = "mobile";
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "INSERT_PAYMENT_TRANSACTION";
                conn.Open();
                int ad = (int)cmd.ExecuteNonQuery();
                conn.Close();

                // det[0].reference_Code = values[2].ToString();

                if (ad >= 0)
                {
                    SqlConnection conn1 = new SqlConnection(commonCode.conStr);
                    SqlCommand cmd1 = new SqlCommand("Pro_onlinePayment", conn1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@category_id", SqlDbType.Int).Value = Convert.ToInt32(MobTPmodel.address_line_1);
                    cmd1.Parameters.Add("@payment_status", SqlDbType.Int).Value = MobTPmodel.response_code;
                    cmd1.Parameters.Add("@ad_id", SqlDbType.Int).Value = Convert.ToInt32(MobTPmodel.address_line_2);
                    cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "UPDATEPAYMENTSTATUS";
                    conn1.Open();

                    int ad1 = (int)cmd1.ExecuteNonQuery();
                    conn1.Close();
                    status = 1;// success payment
                }
                else
                {
                    status = 0; // failure payment
                }


            }
            else
            {
                status = 3; // No matched transaction and failure payment 
            }
            return status;
        }
        #endregion

        [HttpGet]
        public DataTable get(int transaction_Id)
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_onlinePayment", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@transaction_Id", SqlDbType.Int).Value = transaction_Id;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETPAYMENTTRANSACTIONDETAIL";
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
    

