using RMCNET.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace RMCNET.Controllers
{
    public class HomescreenController : ApiController
    {
        [HttpPost]
        public DataTable get(HomeModel a)
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_Home_Screen", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@login_id", SqlDbType.Int).Value = a.login_Id;
            cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = a.category_Id;
            cmd.Parameters.Add("@subCategory_Id", SqlDbType.Int).Value = a.subCategory_Id;
            cmd.Parameters.Add("@location_id", SqlDbType.NVarChar).Value = a.location_id;
            cmd.Parameters.Add("@lastDate", SqlDbType.DateTime).Value = a.lastDate == Convert.ToDateTime("1/1/0001 12:00:00 AM") ? DateTime.Now : a.lastDate;
            cmd.Parameters.Add("@ad_Id", SqlDbType.Int).Value = a.ad_Id;
            cmd.Parameters.Add("@dataCount", SqlDbType.Int).Value = a.dataCount;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = a.mode;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            if (Dt.Rows.Count > 0)
            {
                    List<HomeModel> fm = new List<HomeModel>();
                    fm = commonCode.ConvertDataTable<HomeModel>(Dt);
                    for (int i = 0; i < fm.Count; i++)
                    {
                      if (fm[i].category_Id == 26)
                      {
                        fm[i].favlogins = listfavjob(fm[i].ad_Id);
                      }
                      else
                      {
                        fm[i].favlogins = listfav(fm[i].ad_Id);
                      }
                    }
                    Dt = ToDataTable(fm);
            }
            return Dt;
        }
        public int[] listfav(int ad_id)
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_favourite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ad_Id", SqlDbType.Int).Value = ad_id;

            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETADLOGINLIST";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            int[] fm = new int[Dt.Rows.Count];
            if (Dt.Rows.Count > 0)
            {
                fm = Dt.AsEnumerable().Select(r => r.Field<int>("login_id")).ToArray();
                // fm = Dt.Rows[0].ItemArray.Select(x =>x.).ToArray();

            }

            return fm;
        }
        public int[] listfavjob(int ad_id)
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_favourite", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ad_Id", SqlDbType.Int).Value = ad_id;

            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETJOBLOGINLIST";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            int[] fm = new int[Dt.Rows.Count];
            if (Dt.Rows.Count > 0)
            {
                fm = Dt.AsEnumerable().Select(r => r.Field<int>("login_id")).ToArray();
                // fm = Dt.Rows[0].ItemArray.Select(x =>x.).ToArray();

            }

            return fm;
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
