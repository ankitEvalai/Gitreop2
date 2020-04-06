using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class CompanycomplaintModel
    {
        public int ad_compy_id { get; set; }
        public int login_id { get; set; }
        public string email_Id { get; set; }
        public string name_loadgcompt { get; set; }
        public string name_compy { get; set; }
        public string mobileno { get; set; }
        public string name_defaultcompy { get; set; }
        public int appro_compy { get; set; }
        public string detail_persncont { get; set; }
        public int amount { get; set; }
        public string durat_pend_pay { get; set; }
        public int category_id { get; set; }
        public string location { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get;set; }
        public string mode { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public int dataCount { get; set; }
        public string location_id { get; set; }
        public DateTime lastDate { get; set; }
        public int payment_status { get; set; }

    }
}