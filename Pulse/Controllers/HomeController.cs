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

namespace HealthcareAnalytics.Controllers
{
    public class HomeController : Controller
    {
        readonly string tableauServer = "http://tableau.bpa.services/trusted/";

        private healthcareEntities db = new healthcareEntities();

        public ActionResult login()
        {

     
            return View();
        }
        public ActionResult home()
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
        public ActionResult LogOff()
        {
            Session["User"] = null; //it's my session variable
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
        public ActionResult login(role Role)
        {
            if (ModelState.IsValid)
            {
                var details = (from logindetails in db.roles
                               where logindetails.cst_web_login == Role.cst_web_login && logindetails.cst_web_password == Role.cst_web_password
                               select new
                               {
                                   logindetails.cst_web_login,
                                   logindetails.cst_web_password,
                                   logindetails.cst_first_name,
                                   logindetails.cst_last_name

                               }).ToList();

                if (details.FirstOrDefault() != null)
                {
                    Session["username"] = details.FirstOrDefault().cst_web_login;
                    Session["first"] = details.FirstOrDefault().cst_first_name;
                    Session["last"] = details.FirstOrDefault().cst_last_name;
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
            return View(Role);
        }


    }


   


}