using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMCNET.Models
{
    public class PaymentModel
    {
        public int login_id { get; set; }
        public int transaction_Id { get; set; }
        public int ad_id { get; set; }
        public int category_id { get; set; }
        public int subcategory_id { get; set; }
        public int amount { get; set; }
        public string Payment_Status { get; set; }
        public string email_id { get; set; }
        public int transaction_Amount { get; set; }
    }
}