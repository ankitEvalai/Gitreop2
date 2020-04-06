using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class CategoryModel
    {
        public int category_Id { get; set; }    
        public string category { get; set; }
        public int order_Id { get; set; }
        public string icon { get; set; }

        public string page_name_get { get; set; }

        public string page_name_post { get; set; }
        public int payment { get; set; }
        public List<subCategoryModel> subcategorydetails { get; set; }
    }
    public class subCategoryModel
    {
        public int subCategory_Id { get; set; }
        public string subCategory { get; set; }
    }
}