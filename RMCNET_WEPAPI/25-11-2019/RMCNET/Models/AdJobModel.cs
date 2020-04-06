using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class AdJobModel
    {
        public int Job_Id { get; set; }
       public int login_Id { get; set; }
        public string email_Id { get; set; }
        public int category_Id { get; set; }
        public int subCategory_Id { get; set; }
        public string Job_Title { get; set; }
        public string JobType { get; set; }
       public string Description { get; set; }
       public int Salary_From { get; set; }
       public int Salary_To { get; set; }
       public string Contact_Person { get; set; }
      public string Mobile_No { get; set; }
       public string Landline_No { get; set; }
      public string Experience { get; set; }
       public string  Location { get; set; }
        public string Addressofcompany { get; set; }
        public string mode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime modifieddate { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get; set; }
        public int dataCount { get; set; }
        public DateTime lastDate { get; set; }
        public string location_id { get; set; }
        public int payment_status { get; set; }
        public int[] favlogins { get; set; }
    }
}