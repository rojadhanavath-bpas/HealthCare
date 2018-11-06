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
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Runtime.Remoting.Contexts;
using DAL_TCG;

namespace HealthcareAnalytics.Controllers
{
    public class HomeController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
      //  readonly string tableauServer = "http://tableau.bpa.services/trusted/";

        readonly string tableauServer_Bpas = System.Configuration.ConfigurationManager.AppSettings["TableauSingleSignOn"];


        //private healthcareEntities db = new healthcareEntities();
        
        private User_Login UL = new User_Login();
        private TCG_DataEntities db2 = new TCG_DataEntities();
        private Account_AR_Status AARS = new Account_AR_Status();
        private Account_Bill_Status ABS = new Account_Bill_Status();
        private Account_Case_Details ACS = new Account_Case_Details();
        private Account_Case_Detials_History ACDH = new Account_Case_Detials_History();
        private Account_Case_Task ACT = new Account_Case_Task();
        private Account_Case_Task_History ACTH = new Account_Case_Task_History();
        private Account_Source AS = new Account_Source();
        private Encounter_Type ECT = new Encounter_Type();
        private Insurance_Company_Name ICN = new Insurance_Company_Name();
        private Payor_Financial_Class PFC = new Payor_Financial_Class();
        private PrimaryReason_Master PRM = new PrimaryReason_Master();
        private Status_Master SM = new Status_Master();
        private Task_Master TM = new Task_Master();
        private Priority_Master PM = new Priority_Master();

        private Get_AR_Info_for_Balance_Range_Result Receivable_M = new Get_AR_Info_for_Balance_Range_Result();
        private List<String> FC_DD = new List<string>();
        private List<String> Payor_DD = new List<string>();
        private List<String> Encounter_DD = new List<string>();
        private List<String> Denial_DD = new List<string>();
        private List<String> LName_DD = new List<string>();

        public HomeController(){
           // FC_DD = db2.;

            }

        public string editOpenTask_id ;

        public ActionResult Login()
        {
          
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
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
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
                List<Get_AR_Info_for_Balance_Range_Result> result  = db2.Get_AR_Info_for_Balance_Range(4,1).ToList();
                
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
        [HttpGet]
        public ActionResult e(string logout)
        {
            if (logout == "true")
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
            else { return null; }
        }
        
        public ActionResult DebitAR()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                log.Debug("[User:" + Session["first"] + "]  " + "Loading Denials Management  Page..");
                return View();
            }
        }

        public ActionResult CreditAR()
        {

            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                log.Debug("[User:" + Session["first"] + "]  " + "Loading Denials Management  Page..");
                return View();
            }
            
        }
               
        public ActionResult ARManagement()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                log.Debug("[User:" + Session["first"] + "]  " + "Loading Denials Management  Page..");
                return View();
            }
        }
       
        public ActionResult DenialsManagement()
        {

            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                log.Debug("[User:" + Session["first"] + "]  " + "Loading Denials Management  Page..");
                return View();
            }           
            
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
            var request = (HttpWebRequest)WebRequest.Create(tableauServer_Bpas);

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

        public void SetCookie()
        {
            HttpCookie timeout = new HttpCookie("timeoutcookie");
            DateTime now = DateTime.Now;
            timeout.Value = now.ToString();
            timeout.Expires = now.AddMinutes(15);
            //Response.Cookies.Add(timeout);
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
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var details = (from logindetails in db2.Users_Data 
                               where logindetails.user_web_login == login.username && logindetails.user_web_pwd == login.password
                              select new
                               {
                                   
                                   logindetails.user_web_login,
                                   logindetails.user_web_pwd,
                                   logindetails.user_first_name,
                                   logindetails.user_last_name,
                                   logindetails.user_email_id
                               }).ToList();                              

                if (details.FirstOrDefault() != null)
                {
                    log.Debug(details.FirstOrDefault().user_web_pwd);
                    log.Debug((login.password));


                    log.Debug("[Home][login]: Succesfully logged in  "+ details.FirstOrDefault().user_web_login);
                    Session["username"] = details.FirstOrDefault().user_web_login;
                    Session["first"] = details.FirstOrDefault().user_first_name;
                    Session["last"] = details.FirstOrDefault().user_last_name;
                    Session["email"] = details.FirstOrDefault().user_email_id;

                    HttpCookie timeout = new HttpCookie("timeoutcookie");
                    DateTime now = DateTime.Now;
                    timeout.Value = now.ToString();
                    timeout.Expires = now.AddMinutes(45);
                    Response.Cookies.Add(timeout);

                    //return RedirectToAction("ARManagement", "Home");
                    return RedirectToAction("worklist_Home", "WorkDriver");

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
            return View(login);
                        
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
            catch (FormatException)
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

        public string encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        private static List<SelectListItem> populateStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Status_Master.Select(x => new SelectListItem { Text = x.SM_Name, Value = x.SM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateAccount_ARStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Account_AR_Status.Select(x => new SelectListItem { Text = x.ARSts_Name, Value = x.ARSts_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_BillStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Account_Bill_Status.Select(x => new SelectListItem { Text = x.BillSts_Name, Value = x.BillSts_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_AccountSource()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Account_Source.Select(x => new SelectListItem { Text = x.AccSrc_Name, Value = x.AccSrc_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_EncounterType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Encounter_Type.Select(x => new SelectListItem { Text = x.EncType_Name, Value = x.EncType_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_Insurance()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Insurance_Company_Name.Select(x => new SelectListItem { Text = x.InsCmp_Name, Value = x.InsCmp_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_PayorFC()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Payor_Financial_Class.Select(x => new SelectListItem { Text = x.PyrFC_Name, Value = x.PyrFC_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_PrimaryReason()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.PrimaryReason_Master.Select(x => new SelectListItem { Text = x.PRM_Name, Value = x.PRM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_Task()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Task_Master.Select(x => new SelectListItem { Text = x.TM_Name, Value = x.TM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_UserLogin()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.User_Login.Select(x => new SelectListItem { Text = x.user_web_login, Value = x.user_Id.ToString() }).ToList();
            
        }

        private static List<SelectListItem> populate_Priority()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Priority_Master.Select(x => new SelectListItem { Text = x.PM_Name, Value = x.PM_ID.ToString() }).ToList();

        }        
        public ActionResult TCG_CaseDetails_Modified(string id, int case_ID)
        {
            AARS = new Account_AR_Status();
            ABS = new Account_Bill_Status();
            ACS = new Account_Case_Details();
            ACDH = new Account_Case_Detials_History();
            ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            AS = new Account_Source();
            ECT = new Encounter_Type();
            ICN = new Insurance_Company_Name();
            PFC = new Payor_Financial_Class();
            PRM = new PrimaryReason_Master();
            SM = new Status_Master();
            TM = new Task_Master();
            UL = new User_Login();
            PM = new Priority_Master();

            ViewBag.SM = populateStatus();
            ViewBag.AARS = populateAccount_ARStatus();
            ViewBag.ABS = populate_BillStatus();
            ViewBag.AS = populate_AccountSource();
            ViewBag.ECT = populate_EncounterType();
            ViewBag.ICN = populate_Insurance();
            ViewBag.PFC = populate_PayorFC();
            ViewBag.PRM = populate_PrimaryReason();
            ViewBag.TM = populate_Task();
            ViewBag.UL = populate_UserLogin();
            ViewBag.PM = populate_Priority();

            Session["AccountID"] = id;

            editOpenTask_id = id;
            string HospitalAccountID = editOpenTask_id;
            TCG_DataEntities context = new TCG_DataEntities();
            TCG_DataEntities context_tcg = new TCG_DataEntities();

            //TCG_WL = new TCG_DataEntities();

            List<Account_Case_Task> ACDT = new List<Account_Case_Task>();
            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            Get_Account_Info_for_ARandDenial_Result AIARDR = new Get_Account_Info_for_ARandDenial_Result();
            List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();

            try
            {
                using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                {
                    taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                    if (taskDetails.Count > 0 || taskDetails != null)
                    {
                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, case_ID);

                        int a = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_Status);
                        ViewBag.SM = new SelectList(context.Status_Master.Select(x => new { Value = x.SM_ID.ToString(), Text = x.SM_Name }), "Value", "Text", a);
                                                
                        string b = ViewBag.ACD_Data[0].ACD_Owner;
                        ViewBag.UL = new SelectList(context_tcg.User_Login.Select(x => new { Value = x.user_Id.ToString(), Text = x.user_web_login }), "Value", "Text", b);

                        int c= Convert.ToInt32(ViewBag.ACD_Data[0].ACD_Type);
                        ViewBag.ECT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", c);

                        int d = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_SubType);
                        ViewBag.SbT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", d);

                        int e = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrimaryReason);
                        ViewBag.PRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", e);

                        int f = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_SecondaryReason);
                        ViewBag.SRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", f);

                        int g = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PayerReason);
                        ViewBag.PR = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", g);

                        int h = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrinDiag);
                        ViewBag.PD = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", h);

                        int i = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrinProc);
                        ViewBag.PP = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", i);
                        

                        ViewBag.ACD_data= get_CaseDetails(HospitalAccountID, case_ID);
                        ViewBag.ACDT_data = get_CaseTaskDetails(HospitalAccountID, case_ID);
                        ViewBag.OriginalData = taskDetails;
                        ACDT = get_CaseTaskDetails(HospitalAccountID, case_ID);
                        string ownerName = "";
                        if (ACDT.Count > 0)
                        {
                            for (int k = 0; k < ACDT.Count; k++)
                            {
                                if (ACDT[k].ACT_Priority == "2")
                                {
                                    ACDT[k].ACT_Priority = "High";
                                }
                                else if (ACDT[k].ACT_Priority == "3")
                                {
                                    ACDT[k].ACT_Priority = "Medium";
                                }
                                else if (ACDT[k].ACT_Priority == "4")
                                {
                                    ACDT[k].ACT_Priority = "Low";
                                }
                                else if (ACDT[k].ACT_Priority == "1")
                                {
                                    ACDT[k].ACT_Priority = "None";
                                }

                                ownerName = ACDT[k].ACT_Owner;
                                ACDT[k].ACT_Owner = get_Owner_dropDownText(ownerName);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ACDT);

        }


        public ActionResult _CaseDetails (string id, int case_ID)
        {
            AARS = new Account_AR_Status();
            ABS = new Account_Bill_Status();
            ACS = new Account_Case_Details();
            ACDH = new Account_Case_Detials_History();
            ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            AS = new Account_Source();
            ECT = new Encounter_Type();
            ICN = new Insurance_Company_Name();
            PFC = new Payor_Financial_Class();
            PRM = new PrimaryReason_Master();
            SM = new Status_Master();
            TM = new Task_Master();
            UL = new User_Login();
            PM = new Priority_Master();

            ViewBag.SM = populateStatus();
            ViewBag.AARS = populateAccount_ARStatus();
            ViewBag.ABS = populate_BillStatus();
            ViewBag.AS = populate_AccountSource();
            ViewBag.ECT = populate_EncounterType();
            ViewBag.ICN = populate_Insurance();
            ViewBag.PFC = populate_PayorFC();
            ViewBag.PRM = populate_PrimaryReason();
            ViewBag.TM = populate_Task();
            ViewBag.UL = populate_UserLogin();
            ViewBag.PM = populate_Priority();

            Session["AccountID"] = id;

            editOpenTask_id = id;
            string HospitalAccountID = editOpenTask_id;
            TCG_DataEntities context = new TCG_DataEntities();
            TCG_DataEntities context_tcg = new TCG_DataEntities();

            //TCG_WL = new TCG_DataEntities();

            List<Account_Case_Task> ACDT = new List<Account_Case_Task>();
            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            Get_Account_Info_for_ARandDenial_Result AIARDR = new Get_Account_Info_for_ARandDenial_Result();
            List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();

            try
            {
                using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                {
                    taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                    if (taskDetails.Count > 0 || taskDetails != null)
                    {
                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, case_ID);

                        int a = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_Status);
                        ViewBag.SM = new SelectList(context.Status_Master.Select(x => new { Value = x.SM_ID.ToString(), Text = x.SM_Name }), "Value", "Text", a);

                        string b = ViewBag.ACD_Data[0].ACD_Owner;
                        ViewBag.UL = new SelectList(context_tcg.User_Login.Select(x => new { Value = x.user_Id.ToString(), Text = x.user_web_login }), "Value", "Text", b);

                        int c = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_Type);
                        ViewBag.ECT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", c);

                        int d = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_SubType);
                        ViewBag.SbT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", d);

                        int e = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrimaryReason);
                        ViewBag.PRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", e);

                        int f = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_SecondaryReason);
                        ViewBag.SRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", f);

                        int g = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PayerReason);
                        ViewBag.PR = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", g);

                        int h = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrinDiag);
                        ViewBag.PD = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", h);

                        int i = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrinProc);
                        ViewBag.PP = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", i);


                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, case_ID);
                        ViewBag.ACDT_data = get_CaseTaskDetails(HospitalAccountID, case_ID);
                        ViewBag.OriginalData = taskDetails;
                        ACDT = get_CaseTaskDetails(HospitalAccountID, case_ID);
                        ACD = get_CaseDetails(HospitalAccountID, case_ID);
                        string ownerName = "";
                        if (ACDT.Count > 0)
                        {
                            for (int k = 0; k < ACDT.Count; k++)
                            {
                                if (ACDT[k].ACT_Priority == "2")
                                {
                                    ACDT[k].ACT_Priority = "High";
                                }
                                else if (ACDT[k].ACT_Priority == "3")
                                {
                                    ACDT[k].ACT_Priority = "Medium";
                                }
                                else if (ACDT[k].ACT_Priority == "4")
                                {
                                    ACDT[k].ACT_Priority = "Low";
                                }
                                else if (ACDT[k].ACT_Priority == "1")
                                {
                                    ACDT[k].ACT_Priority = "None";
                                }

                                ownerName = ACDT[k].ACT_Owner;
                                ACDT[k].ACT_Owner = get_Owner_dropDownText(ownerName);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ACD);

        }


        public List<Account_Case_Details> get_CaseDetailsList(string HospitalAccountID)
        {

            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            using (var db = new TCG_DataEntities())
            {
                return (from c in db.Account_Case_Details
                        where c.ACD_HspAccID == HospitalAccountID 
                        select c).ToList();
            }
        }


        public List<Account_Case_Details> get_CaseDetails(string HospitalAccountID, int case_ID)
        {

            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            using (var db = new TCG_DataEntities())
            {
                return (from c in db.Account_Case_Details
                        where c.ACD_HspAccID == HospitalAccountID && c.ACD_ID == case_ID
                        select c).ToList();
            }
        }


        public List<Account_Case_Task> get_CaseTaskDetails(string HospitalAccountID, int case_ID)
        {

            List<Account_Case_Task> ACDT = new List<Account_Case_Task>();
            using (var db = new TCG_DataEntities())
            {
                ACDT = (from c in db.Account_Case_Task
                        where c.ACT_HspAccID == HospitalAccountID && c.ACT_ACD_ID == case_ID && c.ACT_DeleteFlag == 0
                        select c).ToList();
            }
            return ACDT;
            
        }

        public string get_OnlyOneCaseDetails(string HospitalAccountID)
        {

            Account_Case_Details  ACD = new Account_Case_Details();
            
                ACD = db2.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
           

            return ACD.ACD_HspAccID;
        }



        //[HttpGet]
        public ActionResult EditOpenTask_Details(string id, int case_ID, int task_ID)
        {

            AARS = new Account_AR_Status();
            ABS = new Account_Bill_Status();
            ACS = new Account_Case_Details();
            ACDH = new Account_Case_Detials_History();
            ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            AS = new Account_Source();
            ECT = new Encounter_Type();
            ICN = new Insurance_Company_Name();
            PFC = new Payor_Financial_Class();
            PRM = new PrimaryReason_Master();
            SM = new Status_Master();
            TM = new Task_Master();
            UL = new User_Login();
            PM = new Priority_Master();

            ViewBag.SM = populateStatus();
            ViewBag.AARS = populateAccount_ARStatus();
            ViewBag.ABS = populate_BillStatus();
            ViewBag.AS = populate_AccountSource();
            ViewBag.ECT = populate_EncounterType();
            ViewBag.ICN = populate_Insurance();
            ViewBag.PFC = populate_PayorFC();
            ViewBag.PRM = populate_PrimaryReason();
            ViewBag.TM = populate_Task();
            ViewBag.UL = populate_UserLogin();
            ViewBag.PM = populate_Priority();

            editOpenTask_id = id;
            string HospitalAccountID = editOpenTask_id;

            var checkHospitalAccID = db2.Account_Case_Task.Where(m => m.ACT_HspAccID == HospitalAccountID).FirstOrDefault();

            Get_Account_Info_for_ARandDenial_Result AIARDR = new Get_Account_Info_for_ARandDenial_Result();
            TCG_DataEntities caseResult = new TCG_DataEntities();
            //List<Account_Case_Task> ACT = new List<Account_Case_Task>();            
            List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();

            taskDetails = caseResult.Get_Account_Info_for_ARandDenial(id).ToList();
            try
            {
                if (checkHospitalAccID != null)
                {
                    ViewBag.IsUpdate = true;

                  
                    ACT = db2.Account_Case_Task.Where(m => m.ACT_HspAccID == HospitalAccountID && m.ACT_ID == task_ID).FirstOrDefault();
                    if (ACT.ACT_Priority == "Billed")
                        ACT.ACT_Priority = "High";

                  
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return PartialView("EditOpenTask", ACT);
            return PartialView("EditOpenTask", ACT);
        }


        [HttpPost]
        public ActionResult EditOpenTask_Details(Account_Case_Task taskDetails)
        {
            ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            ViewData["AccountCaseTask"] = ACT;

            editOpenTask_id = taskDetails.ACT_HspAccID;
            string id = editOpenTask_id;
            var case_TasK_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
           
            try
            {
                if (ModelState.IsValid)
                {

                    db2.Case_Task_InsUpd(taskDetails.ACT_ID, taskDetails.ACT_HspAccID, taskDetails.ACT_ACD_ID , taskDetails.ACT_Completed, taskDetails.ACT_Priority, taskDetails.ACT_Description, taskDetails.ACT_Owner,
                            taskDetails.ACT_Comment, taskDetails.ACT_DueDate, 0, Session["username"].ToString(), DateTime.Now, "", DateTime.Now, "", case_TasK_idParameter);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            int case_ID = taskDetails.ACT_ACD_ID;            
            return RedirectToActionPermanent("TCG_CaseDetails_Modified",new { id, case_ID });

        }


        [HttpGet]
        public ActionResult DeleteOpenTask(string id, int case_ID, int task_ID)
        {

            ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            ViewData["AccountCaseTask"] = ACT;

            editOpenTask_id = ACT.ACT_HspAccID;          
            var case_TasK_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
           
            try
            {
                if (ModelState.IsValid)
                {
                    ACT = db2.Account_Case_Task.Where(m => m.ACT_HspAccID == id && m.ACT_ID == task_ID && m.ACT_ACD_ID == case_ID).FirstOrDefault();                    

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ACT);

        }

        [HttpPost]
        public ActionResult DeleteOpenTask(Account_Case_Task ACT)
        {

           // ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            ViewData["AccountCaseTask"] = ACT;

            editOpenTask_id = ACT.ACT_HspAccID;
            var case_TasK_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
            
            try
            {
                if (ModelState.IsValid)
                {
                    db2.Case_Task_InsUpd(ACT.ACT_ID, ACT.ACT_HspAccID, ACT.ACT_ACD_ID, ACT.ACT_Completed, ACT.ACT_Priority, ACT.ACT_Description, ACT.ACT_Owner,
                            ACT.ACT_Comment, ACT.ACT_DueDate, 1, Session["username"].ToString(), DateTime.Now, "", DateTime.Now, "", case_TasK_idParameter);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string id = editOpenTask_id;
            int case_ID = ACT.ACT_ACD_ID;
            
            return RedirectToActionPermanent("TCG_CaseDetails_Modified", new { id, case_ID });
          

        }
        
        [HttpGet]
        public ActionResult CreateNewTask_Details(string id, int case_ID, int task_ID)
        {

            AARS = new Account_AR_Status();
            ABS = new Account_Bill_Status();
            ACS = new Account_Case_Details();
            ACDH = new Account_Case_Detials_History();
            ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            AS = new Account_Source();
            ECT = new Encounter_Type();
            ICN = new Insurance_Company_Name();
            PFC = new Payor_Financial_Class();
            PRM = new PrimaryReason_Master();
            SM = new Status_Master();
            TM = new Task_Master();
            UL = new User_Login();
            PM = new Priority_Master();

            ViewBag.SM = populateStatus();
            ViewBag.AARS = populateAccount_ARStatus();
            ViewBag.ABS = populate_BillStatus();
            ViewBag.AS = populate_AccountSource();
            ViewBag.ECT = populate_EncounterType();
            ViewBag.ICN = populate_Insurance();
            ViewBag.PFC = populate_PayorFC();
            ViewBag.PRM = populate_PrimaryReason();
            ViewBag.TM = populate_Task();
            ViewBag.UL = populate_UserLogin();
            ViewBag.PM = populate_Priority();


            editOpenTask_id = id;
            try
            {

                if (id != null)

                {
                    ViewBag.IsUpdate = true;

                    ACT.ACT_ID = 0;
                    ACT.ACT_HspAccID = editOpenTask_id;
                    ACT.ACT_ACD_ID = case_ID;
                    ACT.ACT_Completed = false;
                    ACT.ACT_Priority = "Select";
                    ACT.ACT_Description = " ";
                    ACT.ACT_Owner = " ";
                    ACT.ACT_Comment = " ";
                    ACT.ACT_DueDate = DateTime.Now;
                    ACT.ACT_DeleteFlag = 0;
                }           

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("CreateNewTask", ACT);
        }


        [HttpPost]
        public ActionResult CreateNewTask_Details(Account_Case_Task taskDetails)
        {
            ACTH = new Account_Case_Task_History();
            ViewData["AccountCaseTask"] = ACT;

            editOpenTask_id = taskDetails.ACT_HspAccID;
            string id = editOpenTask_id;
            var case_TasK_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
            int new_Case_Task_Value = 0;

            try
            {
                if (ModelState.IsValid)
                {

                    db2.Case_Task_InsUpd(taskDetails.ACT_ID, taskDetails.ACT_HspAccID, taskDetails.ACT_ACD_ID, false, taskDetails.ACT_Priority, taskDetails.ACT_Description, Session["username"].ToString(),
                            taskDetails.ACT_Comment, taskDetails.ACT_DueDate, 0, Session["username"].ToString(), DateTime.Now, "", DateTime.Now, "", case_TasK_idParameter);


                    new_Case_Task_Value = Convert.ToInt32(case_TasK_idParameter.Value);


                    return RedirectToAction("TCG_CaseDetails_Modified", editOpenTask_id);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            int case_ID = taskDetails.ACT_ACD_ID;

            return RedirectToActionPermanent("TCG_CaseDetails_Modified", new { id, case_ID });
            

        }


        [HttpGet]
        public ActionResult createNewCase_Details(string id, int case_ID)
        {
            AARS = new Account_AR_Status();
            ABS = new Account_Bill_Status();
            ACS = new Account_Case_Details();
            ACDH = new Account_Case_Detials_History();
            ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            AS = new Account_Source();
            ECT = new Encounter_Type();
            ICN = new Insurance_Company_Name();
            PFC = new Payor_Financial_Class();
            PRM = new PrimaryReason_Master();
            SM = new Status_Master();
            TM = new Task_Master();
            UL = new User_Login();
            PM = new Priority_Master();

            ViewBag.SM = populateStatus();
            ViewBag.AARS = populateAccount_ARStatus();
            ViewBag.ABS = populate_BillStatus();
            ViewBag.AS = populate_AccountSource();
            ViewBag.ECT = populate_EncounterType();
            ViewBag.ICN = populate_Insurance();
            ViewBag.PFC = populate_PayorFC();
            ViewBag.PRM = populate_PrimaryReason();
            ViewBag.TM = populate_Task();
            ViewBag.UL = populate_UserLogin();
            ViewBag.PM = populate_Priority();


            editOpenTask_id = id;

            try
            {
                if (id != null)

                {
                    ViewBag.IsUpdate = true;

                    ACS.ACD_ID = case_ID;
                    ACS.ACD_HspAccID = editOpenTask_id;
                   // ACS.ACD_Status = "Select";
                    ACS.ACD_Owner = "Select";
                    ACS.ACD_Type = "Select";
                    ACS.ACD_SubType = "Select";
                    //ACS.ACD_PayerReason = "Select";
                    ACS.ACD_Amount = "0.0";
                    ACS.ACD_PrimaryReason = "Select";
                    ACS.ACD_SecondaryReason = "Select";
                    ACS.ACD_PrinDiag = "Select";
                    ACS.ACD_PrinProc = "Select";
                    ACS.ACD_Comments = " ";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("CreateNewCase", ACS);
            
        }


        [HttpPost]
        public ActionResult createNewCase_Details(Account_Case_Details caseDetails)
        {
            ACDH = new Account_Case_Detials_History();
            ViewData["AccountCaseTask"] = ACT;

            editOpenTask_id = caseDetails.ACD_HspAccID;
            string id = editOpenTask_id;
            var case_TasK_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
           
            try
            {
                if (ModelState.IsValid)
                {

                    //TCG_WL.Case_InsUpd(caseDetails.ACD_ID, caseDetails.ACD_HspAccID, caseDetails.ACD_Amount, caseDetails.ACD_Status, caseDetails.ACD_Owner,
                    //    caseDetails.ACD_Type, caseDetails.ACD_SubType, caseDetails.ACD_PayerReason, caseDetails.ACD_PrimaryReason, caseDetails.ACD_SecondaryReason
                    //     , caseDetails.ACD_PrinDiag, caseDetails.ACD_PrinProc, caseDetails.ACD_Comments, "", "", "", 0, DateTime.Now, DateTime.Now, false, Session["username"].ToString(),
                    //      DateTime.Now, "", DateTime.Now, "", case_TasK_idParameter);


                    //new_Case_Value = Convert.ToInt32(case_TasK_idParameter.Value);




                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          

            return RedirectToActionPermanent("prioirty_CaseList", new { id });

        }

        [HttpGet]
        public ActionResult Edit_CaseList(string id, int case_ID)
        {
            ACS = new Account_Case_Details();
            editOpenTask_id = id;

            try
            {
                if (id != null)

                {
                    ViewBag.IsUpdate = true;

                    TCG_DataEntities context = new TCG_DataEntities();
                    TCG_DataEntities context_tcg = new TCG_DataEntities();

                    ACS = db2.Account_Case_Details.Where(m => m.ACD_HspAccID == id && m.ACD_ID == case_ID).FirstOrDefault();

                    int a = Convert.ToInt32(ACS.ACD_Status);
                    ViewBag.SM = new SelectList(context.Status_Master.Select(x => new { Value = x.SM_ID.ToString(), Text = x.SM_Name }), "Value", "Text", a);
                                        
                    string b = ACS.ACD_Owner;
                    ViewBag.UL = new SelectList(context_tcg.User_Login.Select(x => new { Value = x.user_Id.ToString(), Text = x.user_web_login }), "Value", "Text", b);

                    int c = Convert.ToInt32(ACS.ACD_Type);
                    ViewBag.ECT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", c);

                    int d = Convert.ToInt32(ACS.ACD_SubType);
                    ViewBag.SbT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", d);

                    int e = Convert.ToInt32(ACS.ACD_PrimaryReason);
                    ViewBag.PRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", e);

                    int f = Convert.ToInt32(ACS.ACD_SecondaryReason);
                    ViewBag.SRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", f);

                    int g = Convert.ToInt32(ACS.ACD_PayerReason);
                    ViewBag.PR = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", g);

                    int h = Convert.ToInt32(ACS.ACD_PrinDiag);
                    ViewBag.PD = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", h);

                    int i = Convert.ToInt32(ACS.ACD_PrinProc);
                    ViewBag.PP = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", i);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("Edit_CaseList", ACS);
        }

        [HttpPost]
        public ActionResult Edit_CaseList(Account_Case_Details caseDetails)
        {

            editOpenTask_id = caseDetails.ACD_HspAccID;
            string id = editOpenTask_id;
            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
            
            try
            {
                if (ModelState.IsValid)
                {

                    //TCG_WL.Case_InsUpd(caseDetails.ACD_ID, caseDetails.ACD_HspAccID, caseDetails.ACD_Amount, caseDetails.ACD_Status, caseDetails.ACD_Owner,
                    //    caseDetails.ACD_Type, caseDetails.ACD_SubType, caseDetails.ACD_PayerReason, caseDetails.ACD_PrimaryReason, caseDetails.ACD_SecondaryReason
                    //     , caseDetails.ACD_PrinDiag, caseDetails.ACD_PrinProc, caseDetails.ACD_Comments, "", "", "", 0, DateTime.Now, DateTime.Now, false, Session["username"].ToString(),
                    //      DateTime.Now, "", DateTime.Now, "", case_idParameter);


                    //new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            int case_ID = caseDetails.ACD_ID;
            return RedirectToActionPermanent("prioirty_CaseList", new { id });            
        }


        public ActionResult prioirty_CaseList(string id)
        {
            AARS = new Account_AR_Status();
            ABS = new Account_Bill_Status();
            ACS = new Account_Case_Details();
            ACDH = new Account_Case_Detials_History();
            ACT = new Account_Case_Task();
            ACTH = new Account_Case_Task_History();
            AS = new Account_Source();
            ECT = new Encounter_Type();
            ICN = new Insurance_Company_Name();
            PFC = new Payor_Financial_Class();
            PRM = new PrimaryReason_Master();
            SM = new Status_Master();
            TM = new Task_Master();
            UL = new User_Login();
            PM = new Priority_Master();

            ViewBag.SM = populateStatus();
            ViewBag.AARS = populateAccount_ARStatus();
            ViewBag.ABS = populate_BillStatus();
            ViewBag.AS = populate_AccountSource();
            ViewBag.ECT = populate_EncounterType();
            ViewBag.ICN = populate_Insurance();
            ViewBag.PFC = populate_PayorFC();
            ViewBag.PRM = populate_PrimaryReason();
            ViewBag.TM = populate_Task();
            ViewBag.UL = populate_UserLogin();
            ViewBag.PM = populate_Priority();

            Session["AccountID"] = id;

            editOpenTask_id = id;
            string HospitalAccountID = editOpenTask_id;

            // TCG_WL = new TCG_DataEntities();

            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            Get_Account_Info_for_ARandDenial_Result AIARDR = new Get_Account_Info_for_ARandDenial_Result();
            List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();

            try
            {
                using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                {
                    taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                    if (taskDetails.Count > 0 || taskDetails != null)
                    {
                        var Case_checkHospitalAccID = db2.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                        var CaseTask_checkHospitalAccID = db2.Account_Case_Task.Where(m => m.ACT_HspAccID == HospitalAccountID).FirstOrDefault();

                        var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                        var case_TasK_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                        var model = new TCG_DataEntities();
                        int new_Case_Value;
                        if (Case_checkHospitalAccID != null)
                        {
                            new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                        }
                        else
                        {
                            new_Case_Value = 0;
                        }
                        int new_Case_Task_Value = 0;
                        string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                        if (Case_checkHospitalAccID == null)
                        {

                           // TCG_WL.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), 1, ownerId, "1",
                           //"2", 2, "1", "2", "3","2", (taskDetails[0].Reporting_Rsn_Code_w__Desc == null) ? "None" : taskDetails[0].Reporting_Rsn_Code_w__Desc,"" ,"", "",0, DateTime.Now,DateTime.Now,false, Session["username"].ToString(), DateTime.Now, "", DateTime.Now, "", case_idParameter);


                            new_Case_Value = Convert.ToInt32(case_idParameter.Value);
                        }

                        if (CaseTask_checkHospitalAccID == null)
                        {

                            db2.Case_Task_InsUpd(0, taskDetails[0].Hospital_Account_ID, new_Case_Value, false, "2", taskDetails[0].Primary_Coverage_Payor_Name, ownerId,
                            taskDetails[0].Primary_Coverage_Payor_Name, taskDetails[0].Admission_Date, 0, Session["username"].ToString(), DateTime.Now, "", DateTime.Now, "", case_TasK_idParameter);


                            new_Case_Task_Value = Convert.ToInt32(case_TasK_idParameter.Value);

                        }

                        TCG_DataEntities context = new TCG_DataEntities();
                        TCG_DataEntities context_tcg = new TCG_DataEntities();


                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                        ViewBag.ACDT_data = get_CaseTaskDetails(HospitalAccountID, new_Case_Value);
                        ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                        ViewBag.OriginalData = taskDetails;
                        ACD = get_CaseDetailsList(HospitalAccountID);


                        int a = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_Status);
                        ViewBag.SM = new SelectList(context.Status_Master.Select(x => new { Value = x.SM_ID.ToString(), Text = x.SM_Name }), "Value", "Text", a);

                        string b = ViewBag.ACD_Data[0].ACD_Owner;
                        ViewBag.UL = new SelectList(context_tcg.User_Login.Select(x => new { Value = x.user_Id.ToString(), Text = x.user_web_login }), "Value", "Text", b);

                        int c = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_Type);
                        ViewBag.ECT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", c);

                        int d = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_SubType);
                        ViewBag.SbT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", d);

                        int e = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrimaryReason);
                        ViewBag.PRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", e);

                        int f = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_SecondaryReason);
                        ViewBag.SRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", f);

                        int g = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PayerReason);
                        ViewBag.PR = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", g);

                        int h = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrinDiag);
                        ViewBag.PD = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", h);

                        int i = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrinProc);
                        ViewBag.PP = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", i);


                       
                       if(ACD.Count > 0)
                        {

                            for(int x = 0; x<ACD.Count;x++)
                            {
                                int aa = Convert.ToInt32(ACD[x].ACD_Status);
                                int bb = Convert.ToInt32(ACD[x].ACD_Type);
                                int cc = Convert.ToInt32(ACD[x].ACD_PrimaryReason);
                                int dd = Convert.ToInt32(ACD[x].ACD_PrinDiag);
                                int ee = Convert.ToInt32(ACD[x].ACD_PrinProc);

                                ACD[x].ACD_Status = aa;
                                ACD[x].ACD_Type = get_Type_dropDownValue(bb);
                                ACD[x].ACD_PrimaryReason = get_PrimaryRsn_dropDownValue(cc);
                                ACD[x].ACD_PrinDiag = get_PrimaryRsn_dropDownValue(dd);
                                ACD[x].ACD_PrinProc = get_PrimaryRsn_dropDownValue(ee);
                            }
                        }
                                              

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ACD);

        }

        public string get_Status_dropDownValue(int x)
        {

            Status_Master ACD = new Status_Master();

            ACD = db2.Status_Master.Where(m => m.SM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.SM_Name;
            return DDL_Name;

        }
        
        public string get_Type_dropDownValue(int x)
        {

            Encounter_Type ACD = new Encounter_Type();

            ACD = db2.Encounter_Type.Where(m => m.EncType_ID == x).FirstOrDefault();

            string DDL_Name = ACD.EncType_Name;
            return DDL_Name;

        }

        public string get_PrimaryRsn_dropDownValue(int x)
        {

            PrimaryReason_Master ACD = new PrimaryReason_Master();

            ACD = db2.PrimaryReason_Master.Where(m => m.PRM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.PRM_Name;
            return DDL_Name;

        }

        public string get_Owner_dropDownText(string x)
        {
            TCG_DataEntities context_tcg = new TCG_DataEntities();
            User_Login ACD = new User_Login();

            ACD = context_tcg.User_Login.Where(m => m.user_Id.ToString() == x).FirstOrDefault();

            string DDL_Name = ACD.user_web_login;
            return DDL_Name;

        }

        public string get_Owner_dropDownValue(string x)
        {
            TCG_DataEntities context_tcg = new TCG_DataEntities();
            User_Login ACD = new User_Login();

            ACD = context_tcg.User_Login.Where(m => m.user_web_login.ToString() == x).FirstOrDefault();

            string DDL_Name_Value = ACD.user_Id.ToString();
            return DDL_Name_Value;

        }



        [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
        public class CheckSessionOutAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var context = filterContext.HttpContext;
                if (context.Session != null)
                {
                    if (context.Session.IsNewSession)
                    {
                        string sessionCookie = context.Request.Headers["Cookie"];

                        if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                        {
                            FormsAuthentication.SignOut();
                            string redirectTo = "~/Home/login";
                            if (!string.IsNullOrEmpty(context.Request.RawUrl))
                            {
                                redirectTo = string.Format("~/Home/login?ReturnUrl={0}", HttpUtility.UrlEncode(context.Request.RawUrl));
                                filterContext.Result = new RedirectResult(redirectTo);
                                return;
                            }

                        }
                    }
                }

                base.OnActionExecuting(filterContext);
            }
        }

            }


    


}