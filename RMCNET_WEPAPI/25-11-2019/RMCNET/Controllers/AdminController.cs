using Newtonsoft.Json.Linq;
using RMCNET.Models;
using SCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace RMCNET.Controllers
{
    public class AdminController : ApiController
    {
        [HttpGet]
        public CommonModal getLogindetails()
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_AdminCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GET";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = "True";
                e1.ResponseMessage = "Data Successfully Received";
                e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Data Failed";
                e1.data = Dt;
            }
            return e1;
        }

        [HttpPost]
        public int deleteUser(AdminModel a)
        {
            try
            {
                SqlConnection con = new SqlConnection(commonCode.conStr);
                con.Open();
                SqlCommand cmd = new SqlCommand("Pro_AdminCRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@login_Id", SqlDbType.NVarChar).Value = a.login_Id;
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;

                int s = (int)cmd.ExecuteNonQuery();
                con.Close();
                if (s < 0) s = -1;
                return s;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        [HttpPost]
        public DataTable getAdList(AdminModel a)
        {

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_AdminCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@lastDate", SqlDbType.DateTime).Value = a.lastDate == Convert.ToDateTime("1/1/0001 12:00:00 AM") ? DateTime.Now : a.lastDate;
            cmd.Parameters.Add("@dataCount", SqlDbType.Int).Value = a.dataCount;
            cmd.Parameters.Add("@login_Id", SqlDbType.Int).Value = a.login_Id;
             cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            return Dt;

        }

        [HttpPost]
        public int deleteAdList(AdminModel a)
        {
            try
            {
                SqlConnection con = new SqlConnection(commonCode.conStr);
                con.Open();
                SqlCommand cmd = new SqlCommand("Pro_AdminCRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ad_Id", SqlDbType.Int).Value = a.ad_id;
                cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = a.category_id;
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;

                int s = (int)cmd.ExecuteNonQuery();
                con.Close();
                if (s < 0) s = -1;
                return s;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        [HttpGet]
        public CommonModal getTotalCount()
        {
            CommonModal e1 = new CommonModal();

            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_AdminCRUD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETCOUNT";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = "True";
                e1.ResponseMessage = "Data Successfully Received";
                e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Data Failed";
                e1.data = Dt;
            }
            return e1;
        }

    }
}
