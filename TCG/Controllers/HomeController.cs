using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareAnalytics.Models;
using System.Web.Security;
using System.Net;
using System.Text;  // for class Encoding
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net.Core;
using log4net;
using PagedList;
using PagedList.Mvc;
using System.Security.Cryptography;
using System.Data.Entity.Core.Objects.DataClasses;



namespace HealthcareAnalytics.Controllers
{
    public class HomeController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        readonly string tableauServer = "http://tableau.bpa.services/trusted/";

        //private healthcareEntities db = new healthcareEntities();
       private TCG_DataEntities db2 = new TCG_DataEntities();

        public ActionResult Login()
        {

            log.Debug("Loading Login Page..");

            return View();
        }

        


        public ActionResult Home()
        {
            log.Debug("Loading Home Page..");
            return View();
        }
        public ActionResult Worklist()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = GetTableauToken();

            log.Debug("[User:" + Session["first"] + "]  " + "Loading Debit AR  Page..");
            
            return View();
        }
        public ActionResult Priority(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            ////var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //var start = Request.Form.GetValues("start").FirstOrDefault();
            //var length = Request.Form.GetValues("length").FirstOrDefault();
            //int pageSize = length != null ? Convert.ToInt32(length) : 0;
            //int skip = start != null ? Convert.ToInt32(start) : 0;
            //int totalRecords = 0;

            using (TCG_DataEntities db2 = new TCG_DataEntities())
            {
                List<Get_AR_Info_for_Balance_Range_Result> result  = db2.Get_AR_Info_for_Balance_Range(1).ToList();



                if (!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Hospital_Account_ID.Contains(searchString.ToLower())
                                           || s.Primary_Coverage_Payor_Name.ToLower().Contains(searchString.ToLower())).ToList();
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        result = result.OrderByDescending(s => s.Primary_Coverage_Payor_Name).ToList();
                        break;
                    case "hospital_acc_id":
                        result = result.OrderBy(s => s.Hospital_Account_ID).ToList();
                        break;
                    case "hospital_acc_id_desc":
                        result = result.OrderByDescending(s => s.Hospital_Account_ID).ToList();
                        break;
                    default:  // Name ascending 
                        result = result.OrderBy(s => s.Hospital_Account_ID).ToList();
                        break;
                }
                int pageSize = 13;
                int pageNumber = (page ?? 1);
                //totalRecords = result.Count();
                //var data = result.Skip(skip).Take(pageSize).ToList();
                //// var pagedData = Pagination.PagedResult(result, pagenumber, pagesize);
                // return View(pagedData);
                return View(result.ToPagedList(pageNumber, pageSize));
               // return Json(new {  data = data, recordtotal = totalRecords, recordsfiltered = totalRecords }, JsonRequestBehavior.AllowGet);

            }

        }        


        public ActionResult DebitAR()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = GetTableauToken();

            log.Debug("[User:"+ Session["first"] +"]  "+ "Loading Debit AR  Page..");
            return View();
        }

        public ActionResult CreditAR()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = GetTableauToken();
            log.Debug("[User:" + Session["first"] + "]  " + "Loading Credit AR  Page..");

            return View();
        }
               
        public ActionResult ARManagement()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.Message = GetTableauToken();
            log.Debug("[User:" + Session["first"] + "]  " + "Loading AR Management Page..");
            //this.getFile("http://ec2-34-194-103-217.compute-1.amazonaws.com:8111/api/GeneratePDF");
            //this.getFile("http://www.pdf995.com/samples/pdf.pdf");
            return View();
        }
       
        public ActionResult DenialsManagement()
        {

            
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.Message = GetTableauToken();

            log.Debug("[User:" + Session["first"] + "]  " + "Loading Denials Management  Page..");
            return View();


        }


         public ActionResult LogOff()
        {
            Session["User"] = null; //it's my session variable
            Session.Clear();
            Session.Abandon();
            log.Debug("Cleared session..");
            log.Debug("Signing out user");
            FormsAuthentication.SignOut(); //you write this when you use FormsAuthentication
            log.Debug("Signing Successful! Redirecting to login page..");
            return RedirectToAction("login", "Home");
        }

        private String GetTableauToken() {

            log.Debug("Retrieving Tableau token ");
            var user = "Rdhanavath";
            var request = (HttpWebRequest)WebRequest.Create(tableauServer);

            var encoding = new UTF8Encoding();
            var postData = "username=" + user;
            
            byte[] data = encoding.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

         

            var response = (HttpWebResponse)request.GetResponse();
           
            return new StreamReader(response.GetResponseStream()).ReadToEnd();

            
        }

        
        [HttpPost]
        public void DispatchPdf(String pageName) {

            log.Debug("[User:" + Session["first"] + "]  " + "[Home][dispatchPdf]: Dispatching PDF  with parameter: " + pageName);
            string WatermarkLocation = Server.MapPath("~/Images/hennepin healthcare new logo.png");
            string imageUrl = "http://34.194.103.217:8111/api/GeneratePDF?url=" + pageName;
            string saveLocation = Server.MapPath("~")+"\\Images\\temp.pdf";

            byte[] imageBytes;

            log.Debug("[Home][dispatchPdf]: Creating post request with url : " + imageUrl);
            HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
            imageRequest.Method = "POST";
            var postData = "url="+pageName;
           


            imageRequest.ContentType = "application/x-www-form-urlencoded";
            imageRequest.ContentLength = 0;

           


            WebResponse imageResponse = imageRequest.GetResponse();

            Stream responseStream = imageResponse.GetResponseStream();
            log.Debug("[Home][dispatchPdf]: Retrieving Response" );
            using (BinaryReader br = new BinaryReader(responseStream))
            {
                imageBytes = br.ReadBytes(500000);
                br.Close();
            }
            responseStream.Close();
            imageResponse.Close();
            log.Debug("[Home][dispatchPdf]: Closing Stream!");
            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            log.Debug("[Home][dispatchPdf]: Writing to file at "+ saveLocation);
            try
            {
                bw.Write(imageBytes);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }
            log.Debug("[Home][dispatchPdf]: Closed File Stream  " );
            //downloaded the document
            Document document = new Document();
            PdfReader pdfReader = new PdfReader(saveLocation);
            PdfStamper stamp = new PdfStamper(pdfReader, new FileStream(saveLocation.Replace("temp.pdf", "[temp][file].pdf"), FileMode.Create));

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(WatermarkLocation);
            img.ScaleToFit(120f, 155.25f);
            img.SetAbsolutePosition(document.Left, document.Top-50); // set the position in the document where you want the watermark to appear (0,0 = bottom left corner of the page)


            log.Debug("[Home][dispatchPdf]: Added Image to PDF  " );
            PdfContentByte waterMark;
            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                waterMark = stamp.GetOverContent(page);
                waterMark.AddImage(img);
            }
            stamp.FormFlattening = true;
            stamp.Close();

            pdfReader.Close();
            stamp.Close();
            document.Close();
            // now delete the original file and rename the temp file to the original file
            System.IO.File.Delete(saveLocation);
            log.Debug("[Home][dispatchPdf]: Saving Image with PDF" );
            System.IO.File.Move(saveLocation.Replace("temp.pdf", "[temp][file].pdf"), saveLocation);
            //File.Delete(url);
            //File.Move(url.Replace(".pdf", "[temp][file].pdf"), url);


            //client side download 
            log.Debug("[Home][dispatchPdf]: Sending the PDF to Client Browser " );
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename='"+pageName+".pdf'");
            Response.WriteFile(saveLocation);
            Response.End();
            //byte[] fileBytes = System.IO.File.ReadAllBytes(saveLocation);
            //string fileName = "AR Management.pdf";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            log.Debug("[Home][dispatchPdf]: Deleting the temporary PDF File ");
            System.IO.File.Delete(saveLocation);


        }

        //SHA2_512 to String
        
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User_Login Role)
        {
            if (ModelState.IsValid)
            {
                var details = (from logindetails in db2.User_Login 
                               where logindetails.user_web_login == Role.user_web_login && logindetails.user_web_pwd == Role.user_web_pwd
                               //&& logindetails.user_password == Role.user_password
                               select new
                               {
                                   logindetails.user_web_login,
                                   logindetails.user_web_pwd,
                                   logindetails.user_first_name,
                                   logindetails.user_last_name

                               }).ToList();

                

                if (details.FirstOrDefault() != null)
                {
                    log.Debug(details.FirstOrDefault().user_web_pwd);
                    log.Debug((Role.user_web_pwd));


                    log.Debug("[Home][login]: Succesfully logged in  "+ details.FirstOrDefault().user_web_login);
                    Session["username"] = details.FirstOrDefault().user_web_login;
                    Session["first"] = details.FirstOrDefault().user_first_name;
                    Session["last"] = details.FirstOrDefault().user_last_name;
                    return RedirectToAction("ARManagement", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "invalid credentials");
                }
            }
            else
            {
                ModelState.AddModelError("", "Username and password is required");
            }
            return View(Role);
        }

     
       //private static Byte[] convertGetSHA512(Byte[] strInput)
       // {

       //     byte[] data = Encoding.GetEncoding(1252).GetBytes(Convert.ToString(strInput));
          
       //     SHA512 shaM = new SHA512Managed();

       //     byte[] arrHash = shaM.ComputeHash(strInput);


       //     return arrHash;


       // }

       // private static string convertString(Byte[] input) {

       //     string hashedpassword = Convert.ToBase64String(input);
       //     return hashedpassword;
       // }
    }


   


}