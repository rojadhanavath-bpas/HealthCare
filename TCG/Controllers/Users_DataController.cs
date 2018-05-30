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

namespace HealthcareAnalytics.Controllers
{
    public class Users_DataController : Controller
    {
        private TCG_DataEntities db = new TCG_DataEntities();

        // GET: Users_Data
        public async Task<ActionResult> Index()
        {
            //return View(await db.Users_Data.ToListAsync());
            var res =
                 from s in db.Users_Data
                 join q in db.Roles
                 on s.user_role_key equals q.role_key
                 select new ViewModel { user_info = s, User_roles = q };
            return View(await res.ToListAsync());

        }
        

        // GET: Users_Data/Details/5
        public async Task<ActionResult> Details(Guid? id)
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

        // GET: Users_Data/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users_Data/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "user_ID,user_first_name,user_last_name,user_full_name,user_role_key,user_phone_number,user_email_id,user_added_by,user_add_date,user_updated_by,user_updated_date,user_delete_flag,user_middle_name,user_web_pwd,otp_key,otp_time")] Users_Data users_Data)
        {
            if (ModelState.IsValid)
            {
                users_Data.user_ID = Guid.NewGuid();
                db.Users_Data.Add(users_Data);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(users_Data);
        }

        // GET: Users_Data/Edit/5
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

        // POST: Users_Data/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "user_ID,user_first_name,user_last_name,user_full_name,user_role_key,user_phone_number,user_email_id,user_added_by,user_add_date,user_updated_by,user_updated_date,user_delete_flag,user_middle_name,user_web_pwd,otp_key,otp_time")] Users_Data users_Data)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users_Data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(users_Data);
        }

        // GET: Users_Data/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
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

        // POST: Users_Data/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Users_Data users_Data = await db.Users_Data.FindAsync(id);
            db.Users_Data.Remove(users_Data);
            await db.SaveChangesAsync();
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
    }
}
