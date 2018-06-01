using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthcareAnalytics.Models;
using log4net;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Mail;
using System.Data.SqlClient;

namespace HealthcareAnalytics.Controllers
{
    public class EmailController : Controller
    {

        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(EmailController));
        private TCG_DataEntities db = new TCG_DataEntities();
        private TCG_Registration db2 = new TCG_Registration();

        [HttpPost]
        public ActionResult Index(Users_Data email)
        {
            var UserEmail = db.Users_Data.Where(m => m.user_email_id == email.user_email_id).SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (UserEmail == null)
                {
                    ViewBag.message = "The given email address doesn't exist.";
                }
                else {

                    ViewBag.message = "YES";                                        
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

                        forgotPassword fgt =  new forgotPassword();
                        fgt.email = UserEmail.user_email_id;

                        return View("SendSimpleMessage", fgt);
                    }
                    catch (Exception ex)
                    {
                        log.Debug("checking" + ex);
                    }
                              }
                
                            } 
            else{
                ViewBag.message = "Error: Contact Admin if problem persists";               

            }

            return View();

        }


        [HttpPost]
        public ActionResult passwordReset(forgotPassword fwd)
        {
            var UserEmail = db.Users_Data.Where(m => m.user_email_id == fwd.email).SingleOrDefault();
            //OPT same, SQL Database Update

            if (UserEmail != null && UserEmail.otp_key == fwd.otp_text)
            {

                DateTime startTime = (DateTime) UserEmail.otp_time;

                DateTime endTime = DateTime.Now;
                TimeSpan span = endTime.Subtract(startTime);
                if (span.TotalMinutes <= 5)
                {

                    if (fwd.password == fwd.confirm_pwd)
                    {

                        UserEmail.user_web_pwd = fwd.password;

                        db.Entry(UserEmail).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.message = "Done";
                    }
                    else
                    {

                        ViewBag.message = "NOTVALID";
                        return View("SendSimpleMessage");
                    }

                }
                else {
                    ViewBag.message = "Token expired. Request again";
                    return View("SendSimpleMessage");
                }

            }
            else {
                // ViewBag.message = "One Time Password Expired or No Matching User!";
                ViewBag.message = "Please check all the fields";
                return View("SendSimpleMessage");
            }
            
            
            return View("PwdChanged");
            
        }


            // GET: Email
            public ActionResult Index()
        {
           return View();
        }

     
        // POST: Email/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(Registration registration)
        {
            var checkEmail = db.Users_Data.Where(m => m.user_email_id ==registration.user_email_id).FirstOrDefault();

            if (checkEmail != null) {
                ViewBag.message = "Email Already exist!";
                return View("Create");
            }

            if (ModelState.IsValid)
            {
                if (registration.user_web_pwd == registration.confirm_pwd)
                {
                    var firstname = registration.user_first_name;
                    var lastname = registration.user_last_name;
                    var middleName = registration.user_middle_name;
                    var email = registration.user_email_id;
                    var phonenum = registration.user_phone_number;
                    var pwd = registration.user_web_pwd;

                    db.Database.ExecuteSqlCommand("CreateAccount @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}", firstname, lastname, middleName, email, phonenum, pwd);
                    return View("Successfully");
                }
                else {

                    ViewBag.message = "Passwords Didn't match";
                    return View("Create");
                }
                
               
            }

            ViewBag.message = "Error: Please Check your details ";
            return View("Create");


        }
               
        // GET: Email/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users_Data users_Data = await db.Users_Data.FindAsync(id);
            if (users_Data == null)
            {
                return HttpNotFound();
            }
            return View(users_Data);
        }

        // GET: Email/Create
        public ActionResult Create()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
