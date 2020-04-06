using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Drawing;
using System.Diagnostics;
using System.Drawing;
using System.Xml.XPath;
using System.Drawing.Drawing2D;
using System.IO;
using PdfSharp.Drawing.Layout;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using RMCNET.Models;


namespace RMCNET.Controllers
{
    public class InvoiceController : ApiController
    {
        [HttpGet]
        public List<FavouriteModel> getinvoicereport(int transaction_Id)
        {
            SqlConnection conn = new SqlConnection(commonCode.conStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Pro_onlinePayment", conn);
            cmd.CommandType = CommandType.StoredProcedure;
          //  cmd.Parameters.Add("@login_id", SqlDbType.Int).Value = login_id;
           cmd.Parameters.Add("@transaction_Id", SqlDbType.Int).Value = transaction_Id;
        //   cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = category_id;
            cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "GETinvoicereport";
            List<FavouriteModel> getinvoicereport = new List<FavouriteModel>();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            DataTable Dt = ds.Tables[0];
            conn.Close();
            getinvoicereport = commonCode.ConvertDataTable<FavouriteModel>(Dt);
            return getinvoicereport;
        }
        [HttpGet]
        public String getinvoicepaymentlink( string mode)
        {
            SqlConnection conn1 = new SqlConnection(commonCode.conStr);
            conn1.Open();
            SqlCommand cmd1 = new SqlCommand("Pro_onlinePayment", conn1);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.Add("@mode", SqlDbType.NVarChar).Value = mode;
            DataSet ds1 = new DataSet();
            SqlDataAdapter da1 = new SqlDataAdapter();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1);
            DataTable Dt1 = ds1.Tables[0];
            conn1.Close();

         String   link = Dt1.Rows[0]["link"].ToString();

            return link;
        }
        [HttpGet]
        public String invoicereport( int transaction_Id)
        {


            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            //string path ="E:\\Source";

         String path=  getinvoicepaymentlink("GETinvoicereportdetails12");

            var newUploadPath = path;

            PdfDocument document = new PdfSharp.Pdf.PdfDocument();


            List<FavouriteModel> invoicedetails = getinvoicereport(transaction_Id);
         

            //document.Info.Title = "Study Certificate";
            PdfPage page = document.AddPage();
                page.Size = PageSize.A4;

                XGraphics gfx = XGraphics.FromPdfPage(page);
                XTextFormatter tf = new XTextFormatter(gfx);
               // var x1 = 40;
            XFont fonthead = new XFont("Verdana", 13, XFontStyle.Bold);
            XFont fontbody = new XFont("Times New Roman", 9, XFontStyle.Regular);
            XFont fontbodybold = new XFont("Times New Roman", 11, XFontStyle.Bold);
            XFont subjectdata = new XFont("Times New Roman", 10, XFontStyle.Regular);
            XFont subjectheader = new XFont("Times New Roman", 10, XFontStyle.Bold);
            XFont subjectheader1 = new XFont("Times New Roman", 9, XFontStyle.Bold);
            XFont fontbodybold1 = new XFont("Times New Roman", 11, XFontStyle.Bold);
            XFont fontbodybold12 = new XFont("Times New Roman", 15, XFontStyle.Bold);
            XFont fontbodybold121 = new XFont("Times New Roman", 28, XFontStyle.Bold);
            XFont fontbodybold123 = new XFont("Times New Roman", 15, XFontStyle.Bold);
            XFont fontbodybold2 = new XFont("Times New Roman", 9, XFontStyle.Bold);
            XFont fontbodybold3 = new XFont("Times New Roman", 12, XFontStyle.Regular);

           
            var issdate = Convert.ToDateTime(invoicedetails[0].transaction_Date);
            var dat2 = issdate.ToString("dd/MM/yyyy");
            //school logo
            //getinvoicepaymentlink("GETinvoicereportdetails12");
            string uriPath = getinvoicepaymentlink("GETinvoicereportdetails");

            try
            {
                XImage image = XImage.FromFile(uriPath);
                gfx.DrawImage(image, 50, 20, 80, 80);
                if (!File.Exists(uriPath))
                    throw new FileNotFoundException();
            }
            catch (FileNotFoundException e)
            {

                // gfx.DrawImage(image, 260, x1, 80, 80);
            }


            int y = 0;
            y = y + 30;
           
            gfx.DrawString("INVOICE ", fontbodybold121, XBrushes.Black,
          new XRect(450, y + 7, page.Width, page.Height),
          XStringFormat.TopLeft);
            y = y + 70;
            gfx.DrawString( invoicedetails[0].customerName.ToString(), fontbodybold123, XBrushes.Black,
              new XRect(50, y + 7, page.Width, page.Height),
              XStringFormat.TopLeft);
            y = y + 18;
            gfx.DrawString("Mobile No:", fontbodybold3, XBrushes.Black,
             new XRect(50, y + 7, page.Width, page.Height),
             XStringFormat.TopLeft);
            gfx.DrawString("" + invoicedetails[0].mobile_No.ToString(), fontbodybold3, XBrushes.Black,
                new XRect(109, y + 7, page.Width, page.Height),
                XStringFormat.TopLeft);
            y = y + 20;
            gfx.DrawString("Bill To", fontbodybold1, XBrushes.Black,
            new XRect(50, y + 7, page.Width, page.Height),
            XStringFormat.TopLeft);
            y = y + 15;
            gfx.DrawString("Email:", fontbodybold3, XBrushes.Black,
             new XRect(50, y + 7, page.Width, page.Height),
             XStringFormat.TopLeft);
            gfx.DrawString("" + invoicedetails[0].email_id.ToString(), fontbodybold3, XBrushes.Black,
                new XRect(95, y + 7, page.Width, page.Height),
                XStringFormat.TopLeft);
            y = y + 50;
            gfx.DrawRectangle(XBrushes.LightGray, new XRect(50, y, 500, y - 100));
            gfx.DrawString("Invoice Number ", fontbodybold1, XBrushes.Black,
            new XRect(80, y + 7, page.Width, page.Height),
            XStringFormat.TopLeft);
            gfx.DrawString(" " + invoicedetails[0].transaction_Id.ToString(), subjectdata, XBrushes.Black,
                 new XRect(160, y + 7, page.Width, page.Height),
                 XStringFormat.TopLeft);
            y = y + 20;
            gfx.DrawString("Date", fontbodybold1, XBrushes.Black,
              new XRect(80, y + 7, page.Width, page.Height),
              XStringFormat.TopLeft);
            gfx.DrawString( dat2, subjectdata, XBrushes.Black,
              new XRect(160, y + 7, page.Width, page.Height),
              XStringFormat.TopLeft);
            y = y + 20;
            gfx.DrawString("Payment Terms", fontbodybold1, XBrushes.Black,
            new XRect(80, y + 7, page.Width, page.Height),
            XStringFormat.TopLeft);
            gfx.DrawString("Due On receipt", subjectdata, XBrushes.Black,
              new XRect(160, y + 9, page.Width, page.Height),
              XStringFormat.TopLeft);
            y = y + 20;
            gfx.DrawString("Due Date", fontbodybold1, XBrushes.Black,
            new XRect(80, y + 7, page.Width, page.Height),
            XStringFormat.TopLeft);
            gfx.DrawString(dat2, subjectdata, XBrushes.Black,
              new XRect(160, y + 7, page.Width, page.Height),
              XStringFormat.TopLeft);
            y = y + 20;
          
            y = y + 40;
            gfx.DrawString("DESCRIPTION", fontbodybold1, XBrushes.Black,
              new XRect(50, y + 7, page.Width, page.Height),
              XStringFormat.TopLeft);
         
            gfx.DrawString("PRICE", fontbodybold1, XBrushes.Black,
              new XRect(180, y + 7, page.Width, page.Height),
              XStringFormat.TopLeft);
           
            gfx.DrawString("DURATION", fontbodybold1, XBrushes.Black,
               new XRect(320, y + 7, page.Width, page.Height),
               XStringFormat.TopLeft);
          

            gfx.DrawString("TOTAL", fontbodybold1, XBrushes.Black,
             new XRect(450, y + 7, page.Width, page.Height),
             XStringFormat.TopLeft);

            y = y + 20;
            gfx.DrawLine(XPens.Gray, 50, y, 510, y);
            y = y + 10;
            gfx.DrawString(invoicedetails[0].category.ToString(), subjectdata, XBrushes.Black,
            new XRect(50, y + 7, page.Width, page.Height),
            XStringFormat.TopLeft);
            gfx.DrawString(invoicedetails[0].transaction_Amount.ToString(), subjectdata, XBrushes.Black,
          new XRect(455, y + 7, page.Width, page.Height),
          XStringFormat.TopLeft);
            gfx.DrawString("6 Months", subjectdata, XBrushes.Black,
             new XRect(325, y + 7, page.Width, page.Height),
             XStringFormat.TopLeft);
            gfx.DrawString(invoicedetails[0].transaction_Amount.ToString(), subjectdata, XBrushes.Black,
            new XRect(175, y + 7, page.Width, page.Height),
            XStringFormat.TopLeft);
            y = y + 30;
            gfx.DrawLine(XPens.Gray, 50, y, 510, y);
          //  y = y + 50;
          //  gfx.DrawString("Thank you", fontbodybold3, XBrushes.Black,
          //new XRect(50, y + 7, page.Width, page.Height),
          //XStringFormat.TopLeft);
     //       y = y + 15;
     //       gfx.DrawString("It is a long established fact that a reader will be distracted by the readable ", fontbodybold3, XBrushes.Black,
     //  new XRect(50, y + 7, page.Width, page.Height),
     //  XStringFormat.TopLeft);
     //       y = y + 15;
     //       gfx.DrawString("content of a page when looking at its layout.", fontbodybold3, XBrushes.Black,
     //new XRect(50, y + 7, page.Width, page.Height),
     //XStringFormat.TopLeft);
            try
            {
                 string filename ="/InvoiceReport"+ invoicedetails[0].transaction_Id + ".pdf";
                document.Save(newUploadPath + filename);
                if (!File.Exists(newUploadPath + filename))
                    throw new System.IO.DirectoryNotFoundException();
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = (newUploadPath + filename);
                //process.Start();
                return newUploadPath + filename;
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                path = path.Replace("\\api\\", "\\Group\\");
                Directory.CreateDirectory(path);
                string filename2 = "/InvoiceReport.pdf";
                document.Save(path + filename2);
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = (path + filename2);
                //  process.Start();
                return newUploadPath + filename2;
            }
        }

    }
}
