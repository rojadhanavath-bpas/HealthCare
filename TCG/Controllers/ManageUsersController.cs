using HealthcareAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HealthcareAnalytics.Controllers
{
    public class ManageUsersController : Controller
    {

        private TCG_DataEntities db = new TCG_DataEntities();
        // GET: ManageUsers
        public ActionResult All()
        {

            List<ManageUsersModel> allusers = (from ud in db.Users_Data
                                               join r in db.Roles on ud.user_role_key equals r.role_key
                                               select new ManageUsersModel
                                               {
                                                   UserID = ud.user_ID,
                                                   FirstName = ud.user_first_name,
                                                   LastName = ud.user_last_name,
                                                   EmailId = ud.user_email_id,
                                                   PhoneNumber = ud.user_phone_number,
                                                   UserRole = r.role_code,
                                                   Password=ud.user_web_pwd,
                                                   AddedBy=ud.user_added_by,
                                                   UserName=ud.user_web_login,
                                                   IsActive = ud.user_active_flag == 0 ? false : true
                                               }
                                                  ).ToList();
            return View(allusers);
        }


        [AllowAnonymous]
        public ActionResult User(Guid? id)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res =
           from ud in db.Users_Data
           join r in db.Roles
           on ud.user_role_key equals r.role_key
           where ud.user_ID == id
           select new ManageUsersModel
           {
               FirstName = ud.user_first_name,
               LastName = ud.user_last_name,
               UserName=ud.user_web_login,
               EmailId = ud.user_email_id,
               PhoneNumber = ud.user_phone_number,
               UserRole = r.role_code,
               AddedBy=ud.user_added_by,
               Password=ud.user_web_pwd,
               IsActive = ud.user_active_flag == 0 ? false : true
           };
            ViewBag.roles = db.Roles.Select(r => r.role_code).Distinct().ToList();
            if (res == null)
            {
                return HttpNotFound();
            }
            return View(res.First());

        }

        [HttpPost]
        public ActionResult Save(ManageUsersModel model, String role_value)
        {

            Users_Data udata = new Users_Data();
            Role role = new Role();

            // udata.user_first_name = model.FirstName;
            // udata.user_last_name = model.LastName;
            //// udata.user_middle_name = model.MiddleName;
            // udata.user_phone_number = model.PhoneNumber;
            // udata.user_email_id = model.EmailId;
            // //udata.user_full_name = model.FullName;
            // udata.user_web_login = model.UserName;
            // role.role_code = role_value;
            
            var firstname= model.FirstName;
            var lastname= model.LastName;
            var middlename = model.MiddleName;
            var email= model.EmailId;
            var phonenum= model.PhoneNumber;
            var pwd= model.Password;
            var newrole = role_value;
            var keyword= "Edit";
            var addedby = model.AddedBy;
            var username= model.UserName;

            db.Database.ExecuteSqlCommand("create_user @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @role ={6}, @keyword={7}, @addedby={8}, @username={9}", firstname, lastname, middlename, email, phonenum, pwd, newrole, keyword, addedby, username);
            // return View("Index");

            var resIndex = (from ud in db.Users_Data
                            join r in db.Roles on ud.user_role_key equals r.role_key
                            select new ManageUsersModel
                            {
                                UserID = ud.user_ID,
                                FirstName = ud.user_first_name,
                                LastName = ud.user_last_name,
                                EmailId = ud.user_email_id,
                                PhoneNumber = ud.user_phone_number,
                                UserRole = r.role_code,
                                Password = ud.user_web_pwd,
                                AddedBy = ud.user_added_by,
                                UserName = ud.user_web_login,
                                IsActive = ud.user_active_flag == 0 ? false : true
                            }
                                                    ).ToList();
            //db.SaveChanges();
            return View("All", resIndex);

        }

        public ActionResult Details(Guid? id)
        {

            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res =
           from ud in db.Users_Data
           join r in db.Roles
           on ud.user_role_key equals r.role_key
           where ud.user_ID == id
           select new ManageUsersModel
           {
               FirstName = ud.user_first_name,
               LastName = ud.user_last_name,
               EmailId = ud.user_email_id,
               MiddleName=ud.user_middle_name,
               PhoneNumber = ud.user_phone_number,
               UserName=ud.user_web_login,
               AddedBy=ud.user_added_by,
               UserRole = r.role_code,
               IsActive = ud.user_active_flag == 0 ? false : true
           };


            if (res == null)
            {
                return HttpNotFound();
            }
            return View(res.First());
            
        }
        [HttpGet]
        public ActionResult Remove(Guid? id)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var res =
           from ud in db.Users_Data
           join r in db.Roles
           on ud.user_role_key equals r.role_key
           where ud.user_ID == id
           select new ManageUsersModel
           {
               FirstName = ud.user_first_name,
               LastName = ud.user_last_name,
               EmailId = ud.user_email_id,
               MiddleName = ud.user_middle_name,
               PhoneNumber = ud.user_phone_number,
               UserName = ud.user_web_login,
               AddedBy = ud.user_added_by,
               UserRole = r.role_code,
               IsActive = ud.user_active_flag == 0 ? false : true
           };


            if (res == null)
            {
                return HttpNotFound();
            }
            return View(res.First());
        }

        //delete method [post]

        [HttpPost, ActionName("Remove")]
        public ActionResult Remove(Guid id)
        {
            //Users_Data udata = new Users_Data();
            //udata.user_delete_flag = 1;
            //db.SaveChanges();


            var resIndex = (from ud in db.Users_Data
                            join r in db.Roles on ud.user_role_key equals r.role_key
                            select new ManageUsersModel
                            {
                                UserID = ud.user_ID,
                                FirstName = ud.user_first_name,
                                LastName = ud.user_last_name,
                                EmailId = ud.user_email_id,
                                PhoneNumber = ud.user_phone_number,
                                UserRole = r.role_code,
                                Password = ud.user_web_pwd,
                                AddedBy = ud.user_added_by,
                                UserName = ud.user_web_login,
                                IsActive = ud.user_active_flag == 0 ? false : true
                            }
                                                    ).ToList();



            var email = resIndex.Where(x => x.UserID == id).FirstOrDefault().EmailId;

            var firstname = "";
            var lastname = "";
            var middlename = "";
            
            var phonenum = "";
            var pwd = "";
            var newrole = "";
            var keyword = "Delete";
            var addedby = "";
            var username = "";

            db.Database.ExecuteSqlCommand("create_user @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @role ={6}, @keyword={7}, @addedby={8}, @username={9}", firstname, lastname, middlename, email, phonenum, pwd, newrole, keyword, addedby, username);
            // return View("Index");

            //var resIndex = (from ud in db.Users_Data
            //                join r in db.Roles on ud.user_role_key equals r.role_key
            //                select new ManageUsersModel
            //                {
            //                    UserID = ud.user_ID,
            //                    FirstName = ud.user_first_name,
            //                    LastName = ud.user_last_name,
            //                    EmailId = ud.user_email_id,
            //                    PhoneNumber = ud.user_phone_number,
            //                    UserRole = r.role_code,
            //                    Password = ud.user_web_pwd,
            //                    AddedBy = ud.user_added_by,
            //                    UserName = ud.user_web_login,
            //                    IsActive = ud.user_active_flag == 0 ? false : true
            //                }
            //                                        ).ToList();
            //db.SaveChanges();
            return View("All", resIndex);

            //db.Database.ExecuteSqlCommand("create_user @First_name = {0}, @last_name = {1}, @middle_name = {2}, @Email = {3}, @Phone_number = {4}, @pwd = {5}, @role ={6}, @keyword={7}, @addedby={8}, @username={9}", firstname, lastname, middlename, email, phonenum, pwd, newrole, keyword, addedby, username);

            //return View("All");


        }

        public ActionResult AddUser()
        {
            ViewBag.roles = db.Roles.Select(r => r.role_code).Distinct().ToList();
            return View();
        }
        private String isNullCheck(String val)
        {

            return val == null ? " " : val;
        }

    }
}