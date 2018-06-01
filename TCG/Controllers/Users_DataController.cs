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
        public async Task<ActionResult> Create([Bind(Include = "user_ID,user_first_name,user_last_name,user_full_name,user_role_key,user_phone_number,user_email_id,user_added_by,user_add_date,user_updated_by,user_updated_date,user_delete_flag,user_middle_name,user_web_pwd,otp_key,otp_time,confirm_pwd")] Users_Data users_Data, String role_value)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            var checkEmail = db.Users_Data.Where(m => m.user_email_id == users_Data.user_email_id).FirstOrDefault();

            if (checkEmail != null)
            {
                ViewBag.message = "Email Already exist!";
           
                return View("Create");
            }

            if (ModelState.IsValid)
            {
                if (users_Data.user_web_pwd == users_Data.confirm_pwd)
                {
                    var firstname = isNullCheck(users_Data.user_first_name);
                    var lastname = isNullCheck(users_Data.user_last_name);
                    var middleName = isNullCheck(users_Data.user_middle_name);
                    var email = isNullCheck(users_Data.user_email_id);
                    var phonenum = isNullCheck(users_Data.user_phone_number);
                    var pwd = isNullCheck(users_Data.user_web_pwd);
                    var role = role_value;
                    var id = (users_Data.user_ID);
                    var admin = "admin";
                    var keyword = "Create";

                    db.Database.ExecuteSqlCommand("create_user @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @role ={6}, @keyword={7}, @admin={8}, @ID={9}", firstname, lastname, middleName, email, phonenum, pwd, role, keyword,admin,id);
                    return View("Details");
                }
                else
                {
                    ViewBag.message = "Passwords Didn't match";
                 
                    return View("Create");
                }
            }
            ViewBag.message = "Error: Please Check your details ";
            return View("Create");
        }

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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModel users_Data, String role_value)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

            var res =
         from s in db.Users_Data
         join q in db.Roles
         on s.user_role_key equals q.role_key
         where s.user_ID == users_Data.user_info.user_ID
         select new ViewModel { user_info = s, User_roles = q };


            var resIndex =
                from s in db.Users_Data
                join q in db.Roles
                on s.user_role_key equals q.role_key
                where s.user_delete_flag == 0
                select new ViewModel { user_info = s, User_roles = q };
            if (ModelState.IsValid)
            {
                var firstname = isNullCheck(users_Data.user_info.user_first_name);
                var lastname = isNullCheck(users_Data.user_info.user_last_name);
                var middleName = isNullCheck(users_Data.user_info.user_middle_name);
                var email = isNullCheck(users_Data.user_info.user_email_id);
                var phonenum = isNullCheck(users_Data.user_info.user_phone_number);
                var pwd = isNullCheck(users_Data.user_info.user_web_pwd);
                var role = isNullCheck(role_value);
                var id = (users_Data.user_info.user_ID);
                var admin = ViewBag.UserFirst;
                var keyword = "Edit";

                db.Database.ExecuteSqlCommand("create_user @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @role ={6}, @keyword={7}, @admin={8}, @ID={9}", firstname, lastname, middleName, email, phonenum, pwd, role, keyword, admin, id);
                return View("Index", resIndex.ToList());
            }
            return View(res.First());
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


        private String isNullCheck(String val) {

            return val == null ? " " : val;
        }



    }
}
