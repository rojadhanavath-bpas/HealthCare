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
using System.Web.Routing;
using System.Globalization;

namespace HealthcareAnalytics.Controllers
{
    public class WorkDriverController : Controller
    {

        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        readonly string tableauServer = "http://tableau.bpa.services/trusted/";

        //private healthcareEntities db = new healthcareEntities();
        public TCG_Worklist TCG_WL = new TCG_Worklist();
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
        public Get_Under_Paymnent_Accounts_Result underPayments = new Get_Under_Paymnent_Accounts_Result();

        private Get_AR_Info_for_Balance_Range_Result Receivable_M = new Get_AR_Info_for_Balance_Range_Result();
        private List<String> FC_DD = new List<string>();
        private List<String> Payor_DD = new List<string>();
        private List<String> Encounter_DD = new List<string>();
        private List<String> Denial_DD = new List<string>();
        private List<String> LName_DD = new List<string>();


        // GET: WorkDriver
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult worklist_Home()
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];

          
            return View();
        }

        public ActionResult UserAcc_List(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.CurrentSort = sortOrder;
            ViewBag.sort_AccountID = String.IsNullOrEmpty(sortOrder) ? "accID_Sorting" : "";
            ViewBag.DateSortParm = sortOrder == "date_Sorting" ? "date_DesSorting" : "date_Sorting";
            ViewBag.sort_PayorName = sortOrder == "payorName_Asc" ? "payorName_Des" : "payorName_Asc";
            ViewBag.sort_PatientName = sortOrder == "patientName_Asc" ? "patientName_Des" : "patientName_Asc";


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
                     List<Get_AR_Info_for_Balance_Range_Result>  result = db2.Get_AR_Info_for_Balance_Range(4, 1).ToList();

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        result = result.Where(s => s.Hospital_Account_ID.Contains(searchString.ToLower())
                                               || s.Primary_Coverage_Payor_Name.ToLower().Contains(searchString.ToLower())).ToList();
                    }

                switch (sortOrder)
                {
                    case "accID_Sorting":
                        result = result.OrderByDescending(s => s.Hospital_Account_ID).ToList();
                        break;
                    case "date_Sorting":
                        result = result.OrderBy(s => s.Admission_Date).ToList();
                        break;
                    case "date_DesSorting":
                        result = result.OrderByDescending(s => s.Admission_Date).ToList();
                        break;
                    case "payorName_Asc":
                        result = result.OrderBy(s => s.Primary_Coverage_Payor_Name).ToList();
                        break;
                    case "payorName_Des":
                        result = result.OrderByDescending(s => s.Primary_Coverage_Payor_Name).ToList();
                        break;
                    case "patientName_Asc":
                        result = result.OrderBy(s => s.Account_Patient_Name).ToList();
                        break;
                    case "patientName_Des":
                        result = result.OrderByDescending(s => s.Account_Patient_Name).ToList();
                        break;
                    default:  // Name Descending 
                        result = result.OrderByDescending(s => s.Total_Account_Balance).ToList();
                        break;
                }

                //totalRecords = result.Count();
                //var data = result.Skip(skip).Take(pageSize).ToList();
                //// var pagedData = Pagination.PagedResult(result, pagenumber, pagesize);
                // return View(pagedData);

                // return Json(new {  data = data, recordtotal = totalRecords, recordsfiltered = totalRecords }, JsonRequestBehavior.AllowGet);
                int pageSize = 13;
                int pageNumber = (page ?? 1);
                for (int i = 0; i < result.Count; i++)
                {
                    //result[i].Admission_Date = Convert.ToDateTime(Convert.ToDateTime(result[i].Admission_Date).ToString("d"));
                    result[i].Admission_Date = Convert.ToDateTime(result[i].Admission_Date.ToString()).Date;
                    //result[i].Admission_Date = DateTime.ParseExact(result[i].Admission_Date.ToString(), "mm/dd/yyyy", CultureInfo.InvariantCulture);
                }
                return View(result.ToPagedList(pageNumber, pageSize));



                }

          
           
        }


        public ActionResult Underpayemnts_UserAcc_List(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.UserFirst = Session["first"];
            ViewBag.UserLast = Session["last"];
            ViewBag.CurrentSort = sortOrder;
            ViewBag.sort_AccountID = String.IsNullOrEmpty(sortOrder) ? "accID_Sorting" : "";
            ViewBag.DateSortParm = sortOrder == "date_Sorting" ? "date_DesSorting" : "date_Sorting";
            ViewBag.sort_PayorName = sortOrder == "payorName_Asc" ? "payorName_Des" : "payorName_Asc";
            ViewBag.sort_PatientName = sortOrder == "patientName_Asc" ? "patientName_Des" : "patientName_Asc";



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
                List<Get_Under_Paymnent_Accounts_Result> result = db2.Get_Under_Paymnent_Accounts().ToList();

                //if (!String.IsNullOrEmpty(searchString))
                //{
                //    result = result.Where(s => s.Account.Contains(searchString.ToLower())
                //                           || s.Payor_Name.ToLower().Contains(searchString.ToLower())).ToList();
                //}

                switch (sortOrder)
                {
                    case "accID_Sorting":
                        result = result.OrderByDescending(s => s.Account).ToList();
                        break;
                    case "date_Sorting":
                        result = result.OrderBy(s => s.Disch_Date).ToList();
                        break;
                    case "date_DesSorting":
                        result = result.OrderByDescending(s => s.Disch_Date).ToList();
                        break;
                    case "payorName_Asc":
                        result = result.OrderBy(s => s.Payor_Name).ToList();
                        break;
                    case "payorName_Des":
                        result = result.OrderByDescending(s => s.Payor_Name).ToList();
                        break;
                    case "patientName_Asc":
                        result = result.OrderBy(s => s.Account_Name).ToList();
                        break;
                    case "patientName_Des":
                        result = result.OrderByDescending(s => s.Account_Name).ToList();
                        break;
                    default:  // Name Descending 
                        result = result.OrderByDescending(s => s.Acct_Bal).ToList();
                        break;
                }

                //totalRecords = result.Count();
                //var data = result.Skip(skip).Take(pageSize).ToList();
                //// var pagedData = Pagination.PagedResult(result, pagenumber, pagesize);
                // return View(pagedData);

                // return Json(new {  data = data, recordtotal = totalRecords, recordsfiltered = totalRecords }, JsonRequestBehavior.AllowGet);
                int pageSize = 13;
                int pageNumber = (page ?? 1);
                for (int i = 0; i < result.Count; i++)
                {

                    result[i].Disch_Date = Convert.ToDateTime(result[i].Disch_Date.ToString()).Date;
                    //result[i].Disch_Date = DateTime.ParseExact(result[i].Disch_Date.ToString(), "mm/dd/yyyy", CultureInfo.InvariantCulture);
                }
                return View(result.ToPagedList(pageNumber, pageSize));



            }



        }
    }
}