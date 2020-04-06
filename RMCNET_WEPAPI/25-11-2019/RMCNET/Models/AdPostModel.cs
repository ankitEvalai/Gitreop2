using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class AdPostModel
    {
        public int ad_Id { get; set; }
        public int login_Id { get; set; }
        public string email_Id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public int category_Id { get; set; }
        public int subCategory_Id { get; set; }
        public string companyName { get; set; }
        public string contactPerson { get; set; }
        public string mobileNo { get; set; }
        public string landlineNo { get; set; }
        public string companyAddress { get; set; }
        public string plantCompany { get; set; }
        public string modelYear { get; set; }
        public string serialNo { get; set; }
        public int newCost { get; set; }
        public int expectCost { get; set; }
        public string location { get; set; }
        public string location_id { get; set; }
        public string comment { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get; set; }
        public string mode { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public int dataCount { get; set; }
        public DateTime lastDate { get; set; }
        public string Parking { get; set; }
        public string Features { get; set; }
        public string SuperBuildUparea { get; set; }
        public string PlotArea { get; set; }
        public string CallibrateDate { get; set; }
        public string CallibrateMachine { get; set; }
        public string GSTNumber { get; set; }
        public string PlantCapacity { get; set; }
        public int payment_status { get; set; }
        public int[] favlogins { get; set; }
        public string category { get; set; }
        public int no_labours { get; set; }
        public string available { get; set; }
}
    //public class FavouriteListModel
    //{
    //    public int login_id { get; set; }
    //}
}