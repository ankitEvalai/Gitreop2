using RMCNET.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMCNET.Controllers
{
    public class FavouriteController : ApiController
    {


        [HttpPost]
        public CommonModal insert(FavouriteModel a)
        {
            CommonModal e1 = new CommonModal();
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_favourite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_id", SqlDbType.Int).Value = a.login_id;
            cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = a.category_id;
            cmd.Parameters.Add("@ad_id", SqlDbType.Int).Value = a.ad_id;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                e1.ResponseStatus = Dt.Rows[0]["status"].ToString();
                e1.ResponseMessage = Dt.Rows[0]["MSG"].ToString();
                e1.data = Dt;
            }
            else
            {
                e1.ResponseStatus = "False";
                e1.ResponseMessage = "Ad Posting Failed";

            }
            return e1;
        
    }

        [HttpPost]
        public DataTable get(FavouriteModel a)
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_favourite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_id", SqlDbType.Int).Value = a.login_id;
            cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = a.category_id;
            cmd.Parameters.Add("@ad_id", SqlDbType.Int).Value = a.ad_id;
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
        public List<FavouriteHomeModel> gethome(FavouriteModel a)
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_favourite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_id", SqlDbType.Int).Value = a.login_id;
           
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            List<FavouriteHomeModel> fm = new List<FavouriteHomeModel>();
            fm = commonCode.ConvertDataTable<FavouriteHomeModel>(Dt);
            
            return fm;

        }


        [HttpPost]
        public int delete(FavouriteModel a)
        {
            try
            {
                SqlConnection con = new SqlConnection(commonCode.conStr);
                con.Open();
                SqlCommand cmd = new SqlCommand("Pro_favourite", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fav_id", SqlDbType.Int).Value = a.fav_id;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;

                int s = (int)cmd.ExecuteNonQuery();
                con.Close();
                return s;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
