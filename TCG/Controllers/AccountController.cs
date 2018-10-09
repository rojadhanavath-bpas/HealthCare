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
    public class AccountController : Controller
    {

        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(AccountController));
        private TCG_DataEntities db = new TCG_DataEntities();
        private TCG_Registration db2 = new TCG_Registration();


        [HttpPost]
        public ActionResult IdentifyAccount(LoginModel getemail){
            var UserEmail = db.Users_Data.Where(m => m.user_email_id == getemail.email).SingleOrDefault();

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

                    forgotPassword fgt = new forgotPassword();
                    fgt.email = UserEmail.user_email_id;

                    return View("ResetPassword", fgt);
                }
                catch (Exception ex)
                {
                    log.Debug("checking" + ex);
                }
            }
            else
            {
                ModelState.AddModelError("", "Emaild doesn't exist");

            }


            return View();

        }


        [HttpPost]
        public ActionResult ResetPassword(forgotPassword fwd)
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
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Please check your OTP");
                }
            }
            return View("PwdChanged");

        }





        // GET: Email
        [HttpGet]
        public ActionResult IdentifyAccount()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult CheckEmail(string user_email_id)
        {
            if (db.Users_Data.Where(m => m.user_email_id == user_email_id).FirstOrDefault() != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        // POST: Email/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegistrationModel registration)
        {
            if (ModelState.IsValid)
            {
                var firstname = registration.FirstName;
                var lastname = registration.LastName;
                var middleName = registration.MiddleName;
                var username = registration.UserName;
                var email = registration.Email;
                var phonenum = registration.Phone;
                var pwd = registration.Password;
                var role = "1fbdb62a-724d-e811-a97b-0a436ee98f76";
                db.Database.ExecuteSqlCommand("CreateAccount @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @UserName={6}, @role={7}", firstname, lastname, middleName, email, phonenum, pwd,username, role);
                return View("Successfully");
            }
            else
            {
                ModelState.AddModelError("", "PLease Enter Mandatroty feilds");
                return View();
            }
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
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
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

        public ActionResult ChangePassword()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(PwdChange cpwd)
        {
            if (ModelState.IsValid)
            {
                var email = Session["email"].ToString();
                //string email = "roja.d293@gmail.com";
                Users_Data userdata = db.Users_Data.Where(c => c.user_email_id == email && c.user_web_pwd == cpwd.current_pwd).FirstOrDefault();
                if (userdata != null)
                {
                    //Users_Data _uda = new Users_Data();

                    try
                    {
                        userdata.user_web_pwd = cpwd.new_pwd;
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting  
                                // the current instance as InnerException  
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        throw raise;
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Please enter valid password");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please enter all the fields");
                return View();
            }
            return View();
        }

    }
}
