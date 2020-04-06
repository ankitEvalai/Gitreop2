using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RMCNET.Models
{
    public class commonCode
    {
        public static string conStr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString.ToString();

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, (dr[column.ColumnName] == DBNull.Value ? string.Empty : dr[column.ColumnName]), null);
                    else
                        continue;
                }
            }
            return obj;
        }


        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        internal static List<T> ConvertDataTable<T>(object dt)
        {
            throw new NotImplementedException();
        }

        internal static List<T> ConvertDataTable1<T>(DataTable Dt1)
        {
            //throw new NotImplementedException();

            List<T> data = new List<T>();
            foreach (DataRow row in Dt1.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        internal static List<T> ConvertDataTable2<T>(DataTable Dt4)
        {
            //throw new NotImplementedException();

            List<T> data = new List<T>();
            foreach (DataRow row in Dt4.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

    }
    public class CommonModal
    {
        public string ResponseStatus { get; set; }
        //public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public DataTable data { get; set; }
    }
}