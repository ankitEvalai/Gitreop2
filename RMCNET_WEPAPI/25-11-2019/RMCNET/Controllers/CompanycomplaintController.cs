using Newtonsoft.Json.Linq;
using RMCNET.Models;
using SCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace RMCNET.Controllers
{
    public class CompanycomplaintController : ApiController
    {
        public String result1;
        public String attachmentURL;

        [HttpPost]
        public CommonModal insert(CompanycomplaintModel a)
        {
            CommonModal e1 = new CommonModal();
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_companycomplaints", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ad_compy_id", SqlDbType.Int).Value = a.ad_compy_id;
            cmd.Parameters.Add("@login_id", SqlDbType.Int).Value = a.login_id;
            cmd.Parameters.Add("@name_loadgcompt", SqlDbType.NVarChar).Value = a.name_loadgcompt;
            cmd.Parameters.Add("@name_compy", SqlDbType.NVarChar).Value = a.name_compy;
            cmd.Parameters.Add("@mobileno", SqlDbType.NVarChar).Value = a.mobileno;
            cmd.Parameters.Add("@name_defaultcompy", SqlDbType.NVarChar).Value = a.name_defaultcompy;
            cmd.Parameters.Add("@appro_compy", SqlDbType.Int).Value = a.appro_compy;
            cmd.Parameters.Add("@detail_persncont", SqlDbType.NVarChar).Value = a.detail_persncont;
            cmd.Parameters.Add("@amount", SqlDbType.Int).Value = a.amount;
            cmd.Parameters.Add("@durat_pend_pay", SqlDbType.NVarChar).Value = a.durat_pend_pay;
            cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = a.category_id;
            cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = a.location;
            cmd.Parameters.Add("@location_id", SqlDbType.NVarChar).Value = a.location_id;
            cmd.Parameters.Add("@image1", SqlDbType.NVarChar).Value = a.image1;
            cmd.Parameters.Add("@payment_status", SqlDbType.Int).Value = a.payment_status;
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
                e1.ResponseStatus = Dt1.Rows[0]["status"].ToString();
                e1.ResponseMessage = Dt1.Rows[0]["MSG"].ToString();
                e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Ad Posting Failed";

            }
            return e1;
        }

        [HttpPost]
        public DataTable get(CompanycomplaintModel a)
        {

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_companycomplaints", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_id", SqlDbType.Int).Value = a.login_id;
            cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = a.category_id;
            cmd.Parameters.Add("@lastDate", SqlDbType.DateTime).Value = a.lastDate == Convert.ToDateTime("1/1/0001 12:00:00 AM") ? DateTime.Now : a.lastDate;
            cmd.Parameters.Add("@dataCount", SqlDbType.Int).Value = a.dataCount;
            cmd.Parameters.Add("@location_id", SqlDbType.NVarChar).Value = a.location_id;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            return Dt;

        }

        [HttpPost]
        public int delete(CompanycomplaintModel a)
        {
            try
            {
                SqlConnection con = new SqlConnection(commonCode.conStr);
                con.Open();
                SqlCommand cmd = new SqlCommand("Pro_companycomplaints", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ad_compy_id", SqlDbType.Int).Value = a.ad_compy_id;
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;

                int s = (int)cmd.ExecuteNonQuery();
                con.Close();
                if (s < 0) s = -1;
                return s;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        [HttpPost, HttpGet]
        public async System.Threading.Tasks.Task<CommonModal> imageFileUpload()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["Content"];
            string file_name = HttpContext.Current.Request.Headers["file_name"];
            string ad_compy_id = HttpContext.Current.Request.Headers["ad_compy_id"];
            string image = HttpContext.Current.Request.Headers["image"];

            int index = file_name.LastIndexOf('.');

            string fileExtension = file_name.Substring(index);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmsstt");
            file_name = "IMG" + timestamp + fileExtension;
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("pro_dropbox", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            CommonModal e1 = new CommonModal();
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "UPLOAD_FILE";
            List<UploadFile> getDropboxDetails = new List<UploadFile>();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            getDropboxDetails = commonCode.ConvertDataTable<UploadFile>(Dt);
            var folderPath = getDropboxDetails[0].Upload_Path;
            string URLAuth = folderPath; //URL path
            var token = getDropboxDetails[0].App_Token;
            var folderName = "{\"path\": \"/" + getDropboxDetails[0].Folder_Name + "/" + file_name + "\",\"mode\": \"add\",\"autorename\": true,\"mute\": false,\"strict_conflict\": false}";
            var httpRequest = HttpContext.Current.Request;
            string Response = null;
            HttpWebRequest WebReq = null;
            HttpWebResponse WebRes = null;
            StreamReader StreamResponseReader = null;
            Stream requestStream = null;
            try
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    WebReq = (HttpWebRequest)WebRequest.Create(URLAuth);
                    WebReq.Method = "POST";
                    WebReq.Accept = "*/*";
                    WebReq.Timeout = 50000;
                    WebReq.KeepAlive = false;
                    WebReq.AllowAutoRedirect = false;
                    WebReq.AllowWriteStreamBuffering = true;
                    WebReq.Headers.Add("Dropbox-API-Arg", folderName);
                    WebReq.Headers.Add("authorization", "Bearer " + token);
                    WebReq.ContentType = "application/octet-stream";
                    WebReq.ContentLength = postedFile.ContentLength;

                    BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
                    byte[] converted_File = b.ReadBytes(httpPostedFile.ContentLength);


                    //using (var reader = new BinaryReader(postedFile.InputStream))
                    //{
                    //    imgData = reader.ReadBytes(postedFile.ContentLength);
                    //}

                    requestStream = WebReq.GetRequestStream();
                    requestStream.Write(converted_File, 0, postedFile.ContentLength);

                    requestStream.Close();

                    WebRes = (HttpWebResponse)WebReq.GetResponse();
                    StreamResponseReader = new StreamReader(WebRes.GetResponseStream(), Encoding.UTF8);
                    Response = StreamResponseReader.ReadToEnd();
                    result1 = "Success";
                    //   return result1;
                }
            }
            catch (Exception e)
            {
                throw;
                result1 = "Failure";
                //  return result1;
            }

            ///**** Getting the Share Link Begins ******///
            if (result1 == "Success")
            {
                SqlConnection conn3 = new SqlConnection(commonCode.conStr);
                conn3.Open();
                SqlCommand cmd3 = new SqlCommand("pro_dropbox", conn3);
                cmd3.CommandType = CommandType.StoredProcedure;

                cmd3.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "SHARE_PATH";
                List<ShareLink> getDropboxDetailsFile = new List<ShareLink>();
                DataSet dsFile = new DataSet();
                SqlDataAdapter daFile = new SqlDataAdapter();
                daFile.SelectCommand = cmd3;
                daFile.Fill(dsFile);
                DataTable DtFile = dsFile.Tables[0];
                conn3.Close();
                getDropboxDetailsFile = commonCode.ConvertDataTable<ShareLink>(DtFile);
                var token2 = getDropboxDetailsFile[0].App_Token;

                //Finding List Share path
                var listSharePathData = "{\"path\":  \"/" + getDropboxDetailsFile[0].Folder_Name + "/" + file_name + "\", \"direct_only\": true}";
                //  var listSharePathData = "{\"path\":  \"/" + getDropboxDetailsFile[0].Folder_Name + "/"  + "\", \"direct_only\": true}";

                //     var listSharePathData = "{\"path\": \"/+"+ getDropboxDetailsFile[0].Folder_Name + "/" + file_name + "\",\"settings\": {\"requested_visibility\": \"public\",\"audience\": \"public\",\"access\": \"editor\"}}";

                //{ 'path': " + getDropboxDetails[0].Folder_Name + ",'autorename :' 'false'}";
                var List_shared_Path = getDropboxDetailsFile[0].List_shared_Path;
                string URLAuth2 = List_shared_Path; //URL path


                using (var stringContent = new StringContent(listSharePathData, System.Text.Encoding.UTF8, "application/json"))
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token2);
                        var response = await client.PostAsync(URLAuth2, stringContent);
                        var result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(result);



                        //Finding Share path 
                        var sharePathData = "{\"path\":  \"/" + getDropboxDetailsFile[0].Folder_Name + "/" + file_name + "\" ,\"settings\": { \"requested_visibility\": \"public\"}}";
                        //  var sharePathData = "{\"path\":  \"/" + getDropboxDetailsFile[0].Folder_Name + "/"  + "\" ,\"settings\": { \"requested_visibility\": \"public\"}}";

                        //{ 'path': " + getDropboxDetails[0].Folder_Name + ",'autorename :' 'false'}";
                        var Share_Path = getDropboxDetailsFile[0].Share_Path;
                        string URLAuth3 = Share_Path; //URL path
                        using (var stringContent1 = new StringContent(sharePathData, System.Text.Encoding.UTF8, "application/json"))
                        using (var client1 = new HttpClient())
                        {
                            try
                            {
                                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                                var response1 = await client.PostAsync(URLAuth3, stringContent1);
                                var result3 = await response1.Content.ReadAsStringAsync();
                                Console.WriteLine(result3);
                                // ListData[] arr1 = JObject.Parse(result1)["url"];
                                result3 = JObject.Parse(result3)["url"].ToString();
                                var download_File = result3.Remove(result3.Length - 4, 4) + "raw=1";
                                // str = str.Remove(str.Length - 1, 1) + ",";
                                attachmentURL = download_File;
                                CommonModal e2 = new CommonModal();
                                SqlConnection conn1 = new SqlConnection(commonCode.conStr);
                                conn1.Open();
                                SqlCommand cmd1 = new SqlCommand("Pro_companycomplaints", conn1);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.Add("@ad_compy_id", SqlDbType.Int).Value = ad_compy_id;
                                cmd1.Parameters.Add("@image", SqlDbType.NVarChar).Value = image;
                                cmd1.Parameters.Add("@image1", SqlDbType.NVarChar).Value = attachmentURL;
                                cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "UPDATE";
                                int n = (int)cmd1.ExecuteNonQuery();
                                conn1.Close();
                                e1.ResponseMessage = "Image uploaded successfully";
                                e1.ResponseStatus = "True";


                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                                e1.ResponseMessage = "Image upload failed";
                                e1.ResponseStatus = "False";
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        e1.ResponseMessage = "Image upload failed";
                        e1.ResponseStatus = "False";
                    }
                }



            }



            return e1;

        }





        }
    }

