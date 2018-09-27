using System;
using System.Linq;
using System.Web.Mvc;
using HealthcareAnalytics.Models;
using System.Web.Security;
using System.Net;
using System.Text;  // for class Encoding
using System.IO;
using System.Net.Mail;
using System.Data.Entity;
using log4net;

namespace HealthcareAnalytics.Controllers
{
    public class HomeController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        readonly string tableauServer = "http://tableau.bpa.services/trusted/";

        private healthcareEntities db = new healthcareEntities();

        public ActionResult Login()
        {

           // log.Debug("Loading Login Page..");

            return View();
        }
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult ExecutiveSummary()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = getTableauToken();


            return View();
        }


        public ActionResult RVU()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = getTableauToken();


            return View();
        }


        public ActionResult debitAR()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = getTableauToken();


            return View();
        }

        public ActionResult creditAR()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = getTableauToken();


            return View();
        }

        public ActionResult newpatient()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = getTableauToken();


            return View();
        }

        public ActionResult subsequent()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = getTableauToken();


            return View();
        }


        public ActionResult Billing()


        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = getTableauToken();


            return View();
        }
        public ActionResult ARManagement()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.Message = getTableauToken();

            return View();
        }
        public ActionResult Financial_Management()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            ViewBag.Message = getTableauToken();

            return View();
        }
        public ActionResult DenialsManagement()
        {

            
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.Message = getTableauToken();


            return View();


        }

        public ActionResult ChargeLiquidation()
        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.Message = getTableauToken();

            return View();
        }
        public ActionResult Coding()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.Message = getTableauToken();

            return View();
        }


        public ActionResult StJoseph()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.Message = getTableauToken();

            return View();
        }


        //GET Method
        public ActionResult IdentifyAccount()
        {        
            return View();
        }

        //POST Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IdentifyAccount(UserLogin email_id)
        {
            var UserEmail = db.Users_Data.Where(m => m.user_email_id == email_id.email).SingleOrDefault();

            if (UserEmail != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("bpaservicesllc@gmail.com");
                    mail.To.Add(UserEmail.user_email_id);
                    mail.Subject = "Your BPAS Account | Password Reset Action";
                    string password = System.Web.Security.Membership.GeneratePassword(6, 0);

                    /**
                     * Inserting in to Database  
                     */
                    UserEmail.otp_key = password;
                    UserEmail.otp_time = DateTime.Now;
                    db.Entry(UserEmail).State = EntityState.Modified;
                    db.SaveChanges();


                    mail.Body = "Let's get you back into your account .Please use this one time password: " + password + " ";

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("bpaservicesllc@gmail.com", "Bpas2018");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);

                    VerifyAccount fgt = new VerifyAccount();
                    fgt.email = UserEmail.user_email_id;

                    return View("RecoverPassword", fgt);
                }
                catch (Exception ex)
                {
                    log.Debug("checking" + ex);
                }
            }
            else {
                ModelState.AddModelError("", "Emaild doesn't exist");
            
            }         
           

            return View();

        }


        [HttpPost]
        public ActionResult RecoverPassword(VerifyAccount fwd)
        {
            if (ModelState.IsValid)
            {
                var UserEmail = db.Users_Data.Where(m => m.user_email_id == fwd.email).SingleOrDefault();
                if (UserEmail != null && UserEmail.otp_key == fwd.otp_text)
                {
                    DateTime startTime = (DateTime)UserEmail.otp_time;
                    DateTime endTime = DateTime.Now;
                    TimeSpan span = endTime.Subtract(startTime);
                    if (span.TotalMinutes <= 5)
                    {
                        UserEmail.user_web_pwd = fwd.password;
                        db.Entry(UserEmail).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("", "OTP has expired!, Please try again");
                    }
                    return View("Reset");              
                }
                else
                {
                    ModelState.AddModelError("", "Please check your OTP");
                }
            }
            return View();
        }

        public ActionResult LogOff()
        {
            Session["User"] = null; //it's a session variable
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut(); //you write this when you use FormsAuthentication
            return RedirectToAction("login", "Home");
        }

        private String getTableauToken() {
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
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login)
        {
            if (ModelState.IsValid)
            {
                var details = (from logindetails in db.Users_Data
                               where logindetails.user_web_login == login.username && logindetails.user_web_pwd == login.password
                               select new
                               {
                                   logindetails.user_web_login,
                                   logindetails.user_web_pwd,
                                   logindetails.user_first_name,
                                   logindetails.user_last_name

                               }).ToList();

                if (details.FirstOrDefault() != null)
                {
                    Session["username"] = details.FirstOrDefault().user_web_login;
                    Session["first"] = details.FirstOrDefault().user_first_name;
                    Session["last"] = details.FirstOrDefault().user_last_name;
                    return RedirectToAction("ExecutiveSummary", "Home");
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

        public ActionResult Email()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Email(EmailModel model)
        {
            using (MailMessage mm = new MailMessage(model.Email, model.To))
            {
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                if (model.Attachment.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(model.Attachment.FileName);
                    mm.Attachments.Add(new Attachment(model.Attachment.InputStream, fileName));
                }
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.office365.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Email sent.";
                }
            }

            return View();
        }




    }


   


}