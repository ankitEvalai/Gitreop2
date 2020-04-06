using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class ForgetPasswordModel
    {
        public int login_Id { get; set; }
        public string email_Id { get; set; }
        public string ip_Address { get; set; }
        public string mode { get; set; }
    }
}