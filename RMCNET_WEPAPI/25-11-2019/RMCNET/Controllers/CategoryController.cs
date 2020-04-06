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
    public class CategoryController : ApiController
    {
        [HttpGet]
        public List<CategoryModel> getCategory()
        {
            List<CategoryModel> category = new List<CategoryModel>();
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_getCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETCATEGORY";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            category = commonCode.ConvertDataTable<CategoryModel>(Dt);
            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < category.Count; i++)
                {
                    category[i].subcategorydetails = getSubCategory(category[i].category_Id);
                }
            }
            //else
            //{
                
            //}

            return category;
        }
        [HttpGet]
        public List<subCategoryModel> getSubCategory(int category_Id)
        {
            List<subCategoryModel> subcategory = new List<subCategoryModel>();
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_getCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure; 
            cmd.Parameters.Add("@category_Id", SqlDbType.Int).Value = category_Id;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETSUBCATEGORY";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            subcategory = commonCode.ConvertDataTable<subCategoryModel>(Dt);
            return subcategory;
        }
        [HttpGet]
        public List<CategoryModel> getCategorypost()
        {
            List<CategoryModel> category = new List<CategoryModel>();
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_getCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETCATEGORYPOST";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            category = commonCode.ConvertDataTable<CategoryModel>(Dt);
            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < category.Count; i++)
                {
                    category[i].subcategorydetails = getSubCategory(category[i].category_Id);
                }
            }
            //else
            //{

            //}

            return category;
        }

    }
}
