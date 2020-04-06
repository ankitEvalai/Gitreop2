using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class RegistrationModel
    {
        public int login_Id { get; set; }
        public int userType_Id { get; set; }
        public string email_Id { get; set; }
        public string passWord { get; set; }
        public string first_Name { get; set; }
        public string last_Name { get; set; }
        public string mobile_No { get; set; }
        public int is_Active { get; set; }
        public string ip_Address { get; set; }
        public string mode { get; set; }
        public string passWordnew { get; set; }
        public string token { get; set; }
        public string otp { get; set; }
        public string web_mobile { get; set; }
    }
}