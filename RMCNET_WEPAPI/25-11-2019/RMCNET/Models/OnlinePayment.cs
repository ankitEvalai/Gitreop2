using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCHOOL.Models
{

    public class OnlinePayment
    {

       
        public int payEnable { get; set; }
        public string merchent_id { get; set; }
        public string security_id { get; set; }
        public string checksum_key { get; set; }
        public int transId { get; set; }
        public string payment_status_Id { get; set; }
        public int status { get; set; }
        public int login_id { get; set; }
     
        //############## TRACK N PAY PARAMETERS ######################
        public string provider { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string customerName { get; set; }
        public string mobileNumber { get; set; }
        public string zipCode { get; set; }
        public string amount { get; set; }
        public string apiKey { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public string mode { get; set; }
        public string order_id { get; set; }
        public string phone { get; set; }
        public string return_url { get; set; }
        public string state { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        //public string zip_code { get; set; }
        public string salt { get; set; }
        public string currency { get; set; }
        public string name { get; set; }
     


    }
    public class transStatus
    {
        public string payment_status_Id { get; set; }
        public string transaction_status { get; set; }
        public string transaction_Id { get; set; }
    }
    public class transStatusDetails
    {
        public string customerId { get; set; }
        public string transId { get; set; }
        public string transStatus { get; set; }
        public Decimal transAmount { get; set; }
        public string studentName { get; set; }

    }
    //public class Payment
    //{
    //    public int org_Id { get; set; }
    //    public decimal amount { get; set; }
    //    public string admission_No { get; set; }
    //    public int academic_Id { get; set; }
    //    public int student_Id { get; set; }
    //    public int transId { get; set; }

    //}
    public class makepayment
    {
        //public string msg { get { return "VYASAAPPLI|ARP10234|NA|2.00|NA|NA|NA|INR|NA|R|vyasaappli|NA|NA|F|NA|NA|NA|NA|NA|NA|NA|http://valaischool.com/api/Payment/return_response|A92C24FE50F334C2BA142F9C61E8DC0330784755F8A4FB5CBAFAA1898F455581"; } set { } }
        public string msg { get; set; }
    }

    public class PaymentProcessResponse
    {
        public string msg { get; set; }
        public string hidOperation { get; set; }
        public string hidRequestId { get; set; }
    }

    // for mobile payment gateway by Ashik VP
    public class TrackNpayModal
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string amount { get; set; }
        public string api_key { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public string mode { get; set; }
        public string order_id { get; set; }
        public string phone { get; set; }
        public string return_url { get; set; }
        public string state { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string zip_code { get; set; }
        public string salt { get; set; }
        public string currency { get; set; }
        public string name { get; set; }

        public string cardmasked { get; set; }
        public string error_desc { get; set; }
        public string payment_channel { get; set; }
        public string payment_datetime { get; set; }
        public string payment_mode { get; set; }
        public string response_code { get; set; }
        public string responseMessage { get; set; }
        public string transaction_id { get; set; }
        public string hashValue { get; set; }
        public string keys { get; set; }
    }


}