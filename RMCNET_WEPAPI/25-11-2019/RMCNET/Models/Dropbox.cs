using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHOOL.Models
{
    public class Dropbox
    {
        public int org_Id { get; set; }
       public string App_Token { get; set; }
       public string Folder_Name { get; set; }
       public string Folder_Path { get; set; }
       public byte[] content { get; set; }
    }

    public class UploadFile
    {
        public int org_Id { get; set; }
        public string App_Token { get; set; }
        public string Folder_Name { get; set; }
        public string Upload_Path { get; set; }
        public HttpPostedFileBase Content { get; set; }
    }

    public class DownloadFile
    {
        public int org_Id { get; set; }
        public string App_Token { get; set; }
        public string Folder_Name { get; set; }
        public string File_Name { get; set; }
        public string Download_Path { get; set; }
    }

    public class ShareLink
    {
        public int org_Id { get; set; }
        public string App_Token { get; set; }
        public string Share_Path { get; set; }
        public string Folder_Name { get; set; }
        public string File_Name { get; set; }
        public string List_shared_Path { get; set; }
    }

    public class ListData
    {
        [JsonProperty("url")]
        public string url { get; set; }

        //public string url { get; set; }
    }

}