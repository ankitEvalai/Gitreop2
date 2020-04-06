using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class FavouriteModel
    {
        public int login_id { get; set; }
        public int ad_id { get; set; }
        public string mode { get; set; }
        public DateTime createdDate { get; set; }
        public int fav_id { get; set; }
        public int category_id { get; set; }
        public string customerName { get; set; }
        public string email_id { get; set; }
        public string category { get; set; }
        public decimal transaction_Amount { get; set; }
        public string mobile_No { get; set; }
        public int transaction_Id { get; set; }
        public DateTime transaction_Date { get; set; }
    }

    public class FavouriteHomeModel
    {
        public int fav_id { get; set; }
        public int ad_Id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string image1 { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}