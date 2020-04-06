using Newtonsoft.Json.Linq;
using RMCNET.Models;
using SCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;

namespace RMCNET.Controllers
{
    public class AdjobController : ApiController
    {
        public String result1;
        public String attachmentURL;
        [HttpPost]
        public CommonModal Insert(AdJobModel a)
        {
            CommonModal e1 = new CommonModal();
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_Adjobmaster", conn);
            cmd.CommandType = CommandType.StoredProcedure;
           
            cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = a.login_Id;
            cmd.Parameters.Add("@category_Id", SqlDbType.Int).Value = a.category_Id;
            cmd.Parameters.Add("@subCategory_Id ", SqlDbType.Int).Value = a.subCategory_Id;
            cmd.Parameters.Add("@Job_Title", SqlDbType.NVarChar).Value = a.Job_Title;
            cmd.Parameters.Add("@JobType", SqlDbType.NVarChar).Value = a.JobType;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = a.Description;
            cmd.Parameters.Add("@Salary_From", SqlDbType.Int).Value = a.Salary_From;
            cmd.Parameters.Add("@Salary_To", SqlDbType.Int).Value = a.Salary_To;
            cmd.Parameters.Add("@Contact_Person", SqlDbType.NVarChar).Value = a.Contact_Person;
            cmd.Parameters.Add("@Mobile_No", SqlDbType.NVarChar).Value = a.Mobile_No;
            cmd.Parameters.Add("@Landline_No", SqlDbType.NVarChar).Value = a.Landline_No;
            cmd.Parameters.Add("@Experience", SqlDbType.NVarChar).Value = a.Experience;
            cmd.Parameters.Add("@Location", SqlDbType.NVarChar).Value = a.Location;
            cmd.Parameters.Add("@location_id", SqlDbType.NVarChar).Value = a.location_id;
            cmd.Parameters.Add("@payment_status ", SqlDbType.Int).Value = a.payment_status;
            cmd.Parameters.Add("@Addressofcompany", SqlDbType.NVarChar).Value = a.Addressofcompany;
            cmd.Parameters.Add("@image1", SqlDbType.NVarChar).Value = a.image1;
       
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
                e1.ResponseMessage = "Job Apply Failed";
            }
            return e1;
        }
        [HttpPost]
        public DataTable getjob(AdJobModel a)
        {

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_Adjobmaster", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Job_Id", SqlDbType.Int).Value = a.Job_Id;
            cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = a.login_Id;
            cmd.Parameters.Add("@category_Id", SqlDbType.Int).Value = a.category_Id;
            cmd.Parameters.Add("@subCategory_Id", SqlDbType.Int).Value = a.subCategory_Id;
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
            if (Dt.Rows.Count > 0)
            {
                List<AdJobModel> fm = new List<AdJobModel>();
                fm = commonCode.ConvertDataTable<AdJobModel>(Dt);
                for (int i = 0; i < fm.Count; i++)
                {
                    fm[i].favlogins = listfav(fm[i].Job_Id);
                }
                Dt = ToDataTable(fm);
            }
            return Dt;

        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public int[] listfav(int ad_id)
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_favourite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ad_Id", SqlDbType.Int).Value = ad_id;

            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETJOBLOGINLIST";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            int[] fm = new int[Dt.Rows.Count];
            if (Dt.Rows.Count > 0)
            {
                fm = Dt.AsEnumerable().Select(r => r.Field<int>("login_id")).ToArray();
                // fm = Dt.Rows[0].ItemArray.Select(x =>x.).ToArray();

            }

            return fm;
        }

        [HttpPost]
        public int delete(AdJobModel a)
        {
            try
            {
                SqlConnection con = new SqlConnection(commonCode.conStr);
                con.Open();
                SqlCommand cmd = new SqlCommand("Pro_Adjobmaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Job_Id", SqlDbType.Int).Value = a.Job_Id;
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
            string Job_Id = HttpContext.Current.Request.Headers["Job_Id"];
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


                   

                    //  BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
                    byte[] converted_File = SaveJpeg(httpPostedFile).ToArray();//imagecompress
                    WebReq.ContentLength = converted_File.Length;//imagecompress
                   

                    //using (var reader = new BinaryReader(postedFile.InputStream))
                    //{
                    //    imgData = reader.ReadBytes(postedFile.ContentLength);
                    //}

                    requestStream = WebReq.GetRequestStream();
                    requestStream.Write(converted_File, 0, converted_File.Length);

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
                                SqlCommand cmd1 = new SqlCommand("Pro_Adjobmaster", conn1);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.Add("@Job_Id", SqlDbType.Int).Value = Job_Id;
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

        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
        public MemoryStream SaveJpeg(HttpPostedFile httpPostedFile)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(httpPostedFile.InputStream);// create an image
            MemoryStream stream;
            if (httpPostedFile.ContentLength > 1000000)
            {
                stream = Zip(image, ImageFormat.Jpeg, 10);  // get the zipped stream
            }
            else
            {
                stream = Zip(image, ImageFormat.Jpeg, 0);  // get the zipped stream
            }

            //File.WriteAllBytes("E:\\New folder\\" + Path.GetFileName(httpPostedFile.FileName), stream.ToArray());//save

            return stream;
        }


        public static MemoryStream Zip(Image img, ImageFormat format, long targetLen, long srcLen = 0)
        {

            var ms = new MemoryStream();
            if (targetLen == 0)
            {
                ms.SetLength(0);
                ms.Position = 0;
                img.Save(ms, format);
                return ms;
            }
            const long nearlyLen = 1024;



            if (0 == srcLen)
            {
                img.Save(ms, format);
                srcLen = ms.Length;
            }


            targetLen *= 1024;



            var exitLen = targetLen - nearlyLen;

            //   long quality = 10;
            var quality = (long)Math.Floor(100.00 * targetLen / srcLen);
            var parms = new EncoderParameters(1);


            ImageCodecInfo formatInfo = null;
            var encoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo icf in encoders)
            {
                if (icf.FormatID == format.Guid)
                {
                    formatInfo = icf;
                    break;
                }
            }


            long startQuality = 60;
            long endQuality = 100;
            quality = (startQuality + endQuality) / 2;

            while (true)
            {

                parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);


                ms.SetLength(0);
                ms.Position = 0;
                img.Save(ms, formatInfo, parms);


                if (ms.Length >= exitLen && ms.Length <= targetLen)
                {
                    break;
                }
                else if (startQuality >= endQuality)
                {
                    break;
                }
                else if (ms.Length < exitLen)
                {
                    startQuality = quality;
                }
                else
                {
                    endQuality = quality;
                }


                var newQuality = (startQuality + endQuality) / 2;
                if (newQuality == quality)
                {
                    break;
                }
                quality = newQuality;


            }
            return ms;
        }

        /// <summary>
        /// Returns the image codec with the given mime type
        /// </summary>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }




    }
}
