using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class HomeModel
    {

     

        public int ad_Id { get; set; }
        public string category { get; set; }
        public int login_Id { get; set; }
        public string email_Id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public int category_Id { get; set; }
        public int subCategory_Id { get; set; }
        public string location { get; set; }
        public string location_id { get; set; }
        public string image1 { get; set; }
        public DateTime modifiedDate { get; set; }
        public int dataCount { get; set; }
        public DateTime lastDate { get; set; }
      
        public string mode { get; set; }
        public int[] favlogins { get; set; }

    }
    public class SendMobileMessage
    {

        public int login_Id { get; set; }
        public string mobile_No { get; set; }
        public string otp { get; set; }
        public string mode { get; set; }
        public string passWordnew { get; set; }
        public string passWord { get; set; }

        public int mode1 { get; set; }
    }
}