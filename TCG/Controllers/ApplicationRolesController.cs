using HealthcareAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL_TCG;

namespace HealthcareAnalytics.Controllers
{
    public class ApplicationRolesController : Controller
    {
        // GET: ApplicationRoles


        private TCG_DataEntities db = new TCG_DataEntities();

        public ActionResult Roles()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                List<ApplicationRoles> RoleList = (from roles in db.Roles
                                                   select new ApplicationRoles
                                                   {
                                                       CreatedBy = roles.role_createdBy_user,
                                                       CreatedDate = roles.role_created_date,
                                                       Role_Description = roles.role_designation,
                                                       Role_Name = roles.role_code_short

                                                   }).ToList();
                return View(RoleList);

            }
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(String role)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                return View();
            }
        }
    }
}