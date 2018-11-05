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
using System.Collections;
using DAL_TCG;

namespace HealthcareAnalytics.Controllers
{
    public class Users_DataController : Controller
    {
        private TCG_DataEntities db = new TCG_DataEntities();
        private List<String> roles_db = new List<String>();

        public Users_DataController()
        {
            roles_db = db.Roles.Select(r => r.role_code).Distinct().ToList();
            ViewBag.roles = roles_db;

        }
        // GET: Users_Data

        public async Task<ActionResult> Index()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            //return View(await db.Users_Data.ToListAsync());
            var res =
                 from s in db.Users_Data
                 join q in db.Roles
                 on s.user_role_key equals q.role_key
                 where s.user_delete_flag == 0
                 select new ViewModel { user_info = s, User_roles = q };
            return View(await res.ToListAsync());
        }




        // GET: Users_Data/Details/5
        public ActionResult Details(Guid? id)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res =
           from s in db.Users_Data
           join q in db.Roles
           on s.user_role_key equals q.role_key
           where s.user_ID == id
           select new ViewModel { user_info = s, User_roles = q };

            if (res == null)
            {
                return HttpNotFound();
            }
            return View(res.First());
        }

        // GET: Users_Data/Create
        public ActionResult Create()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            var roles = db.Roles.Select(r => r.role_code).Distinct();
            ViewBag.roles = roles.ToList();

            return View();
        }

        // POST: Users_Data/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModel model, String role_value, Users_Data users_data)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            var name = Session["first"];
            //if (ModelState.IsValid)
            //{
            var checkEmail = db.Users_Data.Where(m => m.user_email_id == users_data.user_email_id).FirstOrDefault();

            if (checkEmail == null)
            {
                //var _id = users_data.user_ID;
                var firstname = model.first_name.ToString();
                var lastname = model.last_name.ToString();
                var middleName = model.user_middle_name;
                var email = model.email_id.ToString();
                var phonenum = model.Phone.ToString();
                var pwd = model.user_web_pwd;
                var role = role_value;
                //var id = (users_Data.user_ID);
                var addedby = name.ToString();
                var keyword = "Create";
                var username = model.username.ToString();

                db.Database.ExecuteSqlCommand("create_user @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @role ={6}, @keyword={7}, @addedby={8}, @username={9}", firstname, lastname, middleName, email, phonenum, pwd, role, keyword, addedby, username);
                return View("Index");

            }
            else
            {
                ModelState.AddModelError("", "Email id already exist");

                return View("Create");
            }
        }
        // ModelState.AddModelError("", "Please check all the fields.");
        // return View("Create");


        // GET: Users_Data/Edit/5
        public ActionResult Edit(Guid? id)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res =
           from s in db.Users_Data
           join q in db.Roles
           on s.user_role_key equals q.role_key
           where s.user_ID == id
           select new ViewModel { user_info = s, User_roles = q };

            if (res == null)
            {
                return HttpNotFound();
            }
            return View(res.First());

        }

        // POST: Users_Data/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModel model,  String role_value)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            var name = Session["first"];

            ModelState.Remove("Users_Data");

            var resIndex =
                from s in db.Users_Data
                join q in db.Roles
                on s.user_role_key equals q.role_key
                where s.user_email_id == model.user_info.user_email_id
                select new ViewModel { user_info = s, User_roles = q };
            if (ModelState.IsValid)
            {
                var firstname = isNullCheck(model.user_info.user_first_name);
                var lastname = isNullCheck(model.user_info.user_last_name);
                var middleName = isNullCheck(model.user_info.user_middle_name);
                var email = isNullCheck(model.user_info.user_email_id);
                var phonenum = isNullCheck(model.user_info.user_phone_number);
                var pwd = isNullCheck(model.user_info.user_web_pwd);
                var role = isNullCheck(role_value);
                var id = (model.user_info.user_ID);
                var addedby = name.ToString();
                var keyword = "Edit";
                var username = isNullCheck(model.user_info.user_web_login);

                db.Database.ExecuteSqlCommand("create_user @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @role ={6}, @keyword={7}, @addedby={8},@username={9}", firstname, lastname, middleName, email, phonenum, pwd, role, keyword, addedby, username);
                return View("Index", resIndex.ToList());

            }

            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            return View(resIndex.First());

        }

        // GET: Users_Data/Delete/5
        public ActionResult Delete(Guid? id)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            //log.Debug("[User:" + Session["first"] + "]  " + "Loading Credit AR  Page..");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res =
           from s in db.Users_Data
           join q in db.Roles
           on s.user_role_key equals q.role_key
           where s.user_ID == id
           select new ViewModel { user_info = s, User_roles = q };

            if (res == null)
            {
                return HttpNotFound();
            }
            return View(res.First());
        }

        // POST: Users_Data/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var firstname = "";
            var lastname = "";
            var middleName = "";
            var email = "";
            var phonenum = "";
            var pwd = "";
            var role = "";
            var admin = "admin";
            var keyword = "Delete";
            db.Database.ExecuteSqlCommand("create_user @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @role ={6}, @keyword={7}, @admin={8}, @ID={9}", firstname, lastname, middleName, email, phonenum, pwd, role, keyword, admin, id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private String isNullCheck(String val)
        {

            return val == null ? " " : val;
        }

    }

    }

