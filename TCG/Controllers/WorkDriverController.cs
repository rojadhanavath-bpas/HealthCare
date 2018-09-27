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
using System.Data.SqlClient;

namespace HealthcareAnalytics.Controllers
{
    public class WorkDriverController : Controller
    {

        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        readonly string tableauServer = "http://tableau.bpa.services/trusted/";

        //private healthcareEntities db = new healthcareEntities();

        public TCG_WorklistModel TCG_WLM = new TCG_WorklistModel();
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

        public string openCaseID;  //editOpenTask_id

        // GET: WorkDriver
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult worklist_Home()
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



        public ActionResult UserAcc_List(string sortOrder, string currentFilter, string searchString, int? page)
        {

            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.HPPB = populateHPorPB();
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.CurrentSort = sortOrder;
                ViewBag.sort_AccountID = String.IsNullOrEmpty(sortOrder) ? "accID_Sorting" : "";
                ViewBag.DateSortParm = sortOrder == "date_Sorting" ? "date_DesSorting" : "date_Sorting";
                ViewBag.sort_PatientName = sortOrder == "patientName_Asc" ? "patientName_Des" : "patientName_Asc";
                ViewBag.sort_PayorName = sortOrder == "payorName_Asc" ? "payorName_Des" : "payorName_Asc";
                ViewBag.sort_Bal = sortOrder == "Bal_Asc" ? "Bal_Des" : "Bal_Asc";
                ViewBag.sort_FC = sortOrder == "FC_Asc" ? "FC_Des" : "FC_Asc";
                ViewBag.sort_Provider = sortOrder == "Provider_Asc" ? "Provider_Des" : "Provider_Asc";


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
                    List<Get_AR_Info_for_Balance_Range_Result> result = db2.Get_AR_Info_for_Balance_Range(4, 1).ToList();

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        result = result.Where(s => s.Hospital_Account_ID.Contains(searchString.ToLower())
                                               || s.Primary_Coverage_Payor_Name.ToLower().Contains(searchString.ToLower())
                                               ).ToList();
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
                        case "Bal_Asc":
                            result = result.OrderBy(s => s.convertAmount).ToList();
                            break;
                        case "Bal_Des":
                            result = result.OrderByDescending(s => s.convertAmount).ToList();
                            break;
                        case "FC_Asc":
                            result = result.OrderBy(s => s.Primary_Coverage_Payor_Financial_Class).ToList();
                            break;
                        case "FC_Des":
                            result = result.OrderByDescending(s => s.Primary_Coverage_Payor_Financial_Class).ToList();
                            break;
                        case "Provider_Asc":
                            result = result.OrderByDescending(s => s.Admitting_Provider_Name).ToList();
                            break;
                        case "Provider_Des":
                            result = result.OrderBy(s => s.Admitting_Provider_Name).ToList();
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
                        if (result[i].Discharge_Date.HasValue)
                        {
                            result[i].New_DischargeDate = result[i].Discharge_Date.Value.ToShortDateString();
                        }

                        result[i].Total_Account_Balance = result[i].Total_Account_Balance.HasValue ? Decimal.Round(result[i].Total_Account_Balance.Value, 2) : 0;
                        string testAmt = result[i].Total_Account_Balance.Value.ToString("0.00");
                        Decimal testDecimal = Convert.ToDecimal(testAmt);
                        result[i].convertAmount = Math.Round(testDecimal, 2);
                        result[i].convertBal = "$" + result[i].convertAmount.ToString();
                    }
                    return View(result.ToPagedList(pageNumber, pageSize));



                }

            }

        }

        public ActionResult Underpayemnts_UserAcc_List(string sortOrder, string currentFilter, string searchString, int? page, string DDLValue, string type)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.HPPB = populateHPorPB();
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.CurrentSort = sortOrder;
                ViewBag.sort_AccountID = String.IsNullOrEmpty(sortOrder) ? "accID_Sorting" : "";
                ViewBag.DateSortParm = sortOrder == "date_Sorting" ? "date_DesSorting" : "date_Sorting";
                ViewBag.sort_PatientName = sortOrder == "patientName_Asc" ? "patientName_Des" : "patientName_Asc";
                ViewBag.sort_PayorName = sortOrder == "payorName_Asc" ? "payorName_Des" : "payorName_Asc";
                ViewBag.sort_Bal = sortOrder == "Bal_Asc" ? "Bal_Des" : "Bal_Asc";
                ViewBag.sort_Plan = sortOrder == "Plan_Asc" ? "Plan_Des" : "Plan_Asc";
                ViewBag.sort_AccClass = sortOrder == "AccClass_Asc" ? "AccClass_Des" : "AccClass_Asc";
                int pageSize = 10;
                int pageNumber = (page ?? 1);


                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;


                using (TCG_WorklistModel db2 = new TCG_WorklistModel())
                {
                    List<Get_Under_Paymnent_Accounts_Result> result = db2.Get_Under_Paymnent_Accounts().ToList();

                    if (type == "All")
                    {
                        using (var db = new TCG_WorklistModel())
                        {
                            result = (from c in db.Get_Under_Paymnent_Accounts()                                      
                                      select c).ToList();
                        }
                    }
                    else if(type == "New" || type== "Zero Balance" || type=="Credit Balance")
                    {
                        using (var db = new TCG_WorklistModel())
                        {
                            result = (from c in db.Get_Under_Paymnent_Accounts()
                                      where c.CaseStatus == type
                                      select c).ToList();
                        }
                    }

                    List<Get_Under_Paymnent_Accounts_PB_Result> flagRes = new List<Get_Under_Paymnent_Accounts_PB_Result>();
                    flagRes.Add(new Get_Under_Paymnent_Accounts_PB_Result() { flag = 2 });
                    ViewBag.flagDetails = flagRes;

                    for (int i = 0; i < result.Count; i++)
                    {
                        //if (result[i].Account.HasValue)
                        //{
                        //    result[i].AccId = Convert.ToString(result[i].Account);
                        //}


                        result[i].AccId = Convert.ToString(result[i].Account);

                        string[] test = (result[i].Account_Name.ToString()).Split(':');
                        result[i].PatientName = test[0];
                        result[i].caseFlag = test[1];
                        if (result[i].caseFlag == "NEW")
                            result[i].flagCase = true;
                        else
                            result[i].flagCase = false;


                        if (!string.IsNullOrEmpty(result[i].Brief_Summary))
                            result[i].flagCase = false;

                        if (result[i].CaseStatus == "New")
                            result[i].flagCaseValue = 1;
                        else if (result[i].CaseStatus == "Credit Balance")
                            result[i].flagCaseValue = 2;
                        else
                            result[i].flagCaseValue = 3;

                        if (string.IsNullOrEmpty(result[i].Payor_Name))
                            result[i].Payor_Name = "";
                        if (string.IsNullOrEmpty(result[i].Acct_Class))
                            result[i].Acct_Class = "";
                        if (string.IsNullOrEmpty(result[i].Plan_Name))
                            result[i].Plan_Name = "";
                        if (result[i].Underpayment_Reason_Code == null)
                            result[i].Underpayment_Reason_Code = 0;
                    }

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        result = result.Where(s => s.AccId.Contains(searchString.Trim().ToString())
                                               || s.Payor_Name.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.Account_Name.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.Acct_Class.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.Plan_Name.ToLower().Contains(searchString.Trim().ToLower())).ToList();
                    }

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
                        case "Bal_Asc":
                            result = result.OrderBy(s => s.Acct_Bal).ToList();
                            break;
                        case "Bal_Des":
                            result = result.OrderByDescending(s => s.Acct_Bal).ToList();
                            break;
                        case "Plan_Asc":
                            result = result.OrderBy(s => s.Plan_Name).ToList();
                            break;
                        case "Plan_Des":
                            result = result.OrderByDescending(s => s.Plan_Name).ToList();
                            break;
                        case "AccClass_Asc":
                            result = result.OrderByDescending(s => s.Acct_Class).ToList();
                            break;
                        case "AccClass_Des":
                            result = result.OrderBy(s => s.Acct_Class).ToList();
                            break;
                        default:  // Name Descending 
                            result = result.OrderByDescending(s => s.Acct_Bal).ToList();
                            break;
                    }



                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result[i].Disch_Date.HasValue)
                        {
                            result[i].DischargeDate = result[i].Disch_Date.Value.ToShortDateString();
                        }

                        string testAmt = result[i].Acct_Bal.Value.ToString("0.00");
                        Decimal testDecimal = Convert.ToDecimal(testAmt);
                        result[i].convertAmount = Math.Round(testDecimal, 2);
                        result[i].convertBal = "$" + result[i].convertAmount.ToString();
                    }


                    return View(result.ToPagedList(pageNumber, pageSize));
                }

            }

        }


        public ActionResult PB_Underpayments_UserAccList(string sortOrder, string currentFilter, string searchString, int? page, string DDLValue,string type)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.HPPB = populateHPorPB();
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.CurrentSort = sortOrder;
                ViewBag.sort_AccountID = String.IsNullOrEmpty(sortOrder) ? "accID_Sorting" : "";
                ViewBag.DateSortParm = sortOrder == "date_Sorting" ? "date_DesSorting" : "date_Sorting";
                ViewBag.sort_PatientName = sortOrder == "patientName_Asc" ? "patientName_Des" : "patientName_Asc";
                ViewBag.sort_PayorName = sortOrder == "payorName_Asc" ? "payorName_Des" : "payorName_Asc";
                ViewBag.sort_Bal = sortOrder == "Bal_Asc" ? "Bal_Des" : "Bal_Asc";
                ViewBag.sort_Plan = sortOrder == "Plan_Asc" ? "Plan_Des" : "Plan_Asc";
                ViewBag.sort_AccClass = sortOrder == "AccClass_Asc" ? "AccClass_Des" : "AccClass_Asc";
                int pageSize = 10;
                int pageNumber = (page ?? 1);


                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;


                using (TCG_WorklistModel db2 = new TCG_WorklistModel())
                {
                    List<Get_Under_Paymnent_Accounts_PB_Result> result = db2.Get_Under_Paymnent_Accounts_PB().ToList();

                    if (type == "All")
                    {
                        using (var db = new TCG_WorklistModel())
                        {
                            result = (from c in db.Get_Under_Paymnent_Accounts_PB()
                                      select c).ToList();
                        }
                    }
                    else if (type == "New" || type == "Zero Balance" || type == "Credit Balance")
                    {
                        using (var db = new TCG_WorklistModel())
                        {
                            result = (from c in db.Get_Under_Paymnent_Accounts_PB()
                                      where c.Acct_Status == type
                                      select c).ToList();
                        }
                    }

                    List<Get_Under_Paymnent_Accounts_PB_Result> flagRes = new List<Get_Under_Paymnent_Accounts_PB_Result>();
                    flagRes.Add(new Get_Under_Paymnent_Accounts_PB_Result() { flag = 1 });
                    ViewBag.flagDetails = flagRes;

                    for (int i = 0; i < result.Count; i++)
                    {
                        //if (result[i].Account.HasValue)
                        //{
                        //    result[i].AccId = Convert.ToString(result[i].Account);
                        //}


                        result[i].AccId = Convert.ToString(result[i].Account);

                        if (result[i].Account_Name != null)
                        {
                            string[] test = (result[i].Account_Name.ToString()).Split(':');
                            result[i].PatientName = test[0];
                            result[i].caseFlag = test[1];
                        }
                        else
                        {
                            result[i].PatientName = " ";
                            result[i].caseFlag = "NEW";
                        }

                        if (result[i].caseFlag == "NEW")
                            result[i].flagCase = true;
                        else
                            result[i].flagCase = false;
                        if (!string.IsNullOrEmpty(result[i].Brief_Summary))
                            result[i].flagCase = false;

                        if(result[i].Acct_Status== "New")
                            result[i].flagCaseValue = 1;
                        else if (result[i].Acct_Status == "Credit Balance")
                            result[i].flagCaseValue = 2;
                        else
                            result[i].flagCaseValue = 3;

                        if (string.IsNullOrEmpty(result[i].Payor_Name))
                            result[i].Payor_Name = "";
                        if (string.IsNullOrEmpty(result[i].Acct_Class))
                            result[i].Acct_Class = "";
                        if (string.IsNullOrEmpty(result[i].Plan_Name))
                            result[i].Plan_Name = "";
                        if (result[i].Underpayment_Reason_Code == null)
                            result[i].Underpayment_Reason_Code = 0;

                    }

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        result = result.Where(s => s.AccId.Contains(searchString.Trim().ToString())
                                               || s.Payor_Name.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.PatientName.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.Acct_Class.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.Plan_Name.ToLower().Contains(searchString.Trim().ToLower())).ToList();
                    }

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
                        case "Bal_Asc":
                            result = result.OrderBy(s => s.Acct_Bal).ToList();
                            break;
                        case "Bal_Des":
                            result = result.OrderByDescending(s => s.Acct_Bal).ToList();
                            break;
                        case "Plan_Asc":
                            result = result.OrderBy(s => s.Plan_Name).ToList();
                            break;
                        case "Plan_Des":
                            result = result.OrderByDescending(s => s.Plan_Name).ToList();
                            break;
                        case "AccClass_Asc":
                            result = result.OrderByDescending(s => s.Acct_Class).ToList();
                            break;
                        case "AccClass_Des":
                            result = result.OrderBy(s => s.Acct_Class).ToList();
                            break;
                        default:  // Name Descending 
                            result = result.OrderByDescending(s => s.Acct_Bal).ToList();
                            break;
                    }



                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result[i].Disch_Date.HasValue)
                        {
                            result[i].DischargeDate = result[i].Disch_Date.Value.ToShortDateString();
                        }

                        string testAmt = result[i].Acct_Bal.Value.ToString("0.00");
                        Decimal testDecimal = Convert.ToDecimal(testAmt);
                        result[i].convertAmount = Math.Round(testDecimal, 2);
                        result[i].convertBal = "$" + result[i].convertAmount.ToString();
                    }


                    return View(result.ToPagedList(pageNumber, pageSize));
                }


            }
        }


        public ActionResult APD_UserAcc_List(string sortOrder, string currentFilter, string searchString, int? page, string DDLValue, string type)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.HPPB = populateHPorPB();
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.CurrentSort = sortOrder;
                ViewBag.sort_AccountID = String.IsNullOrEmpty(sortOrder) ? "accID_Sorting" : "";
                ViewBag.DateSortParm = sortOrder == "date_Sorting" ? "date_DesSorting" : "date_Sorting";
                ViewBag.sort_PatientName = sortOrder == "patientName_Asc" ? "patientName_Des" : "patientName_Asc";
                ViewBag.sort_PayorName = sortOrder == "payorName_Asc" ? "payorName_Des" : "payorName_Asc";
                ViewBag.sort_Bal = sortOrder == "Bal_Asc" ? "Bal_Des" : "Bal_Asc";
                ViewBag.sort_Plan = sortOrder == "Plan_Asc" ? "Plan_Des" : "Plan_Asc";
                ViewBag.sort_AccClass = sortOrder == "AccClass_Asc" ? "AccClass_Des" : "AccClass_Asc";
                int pageSize = 10;
                int pageNumber = (page ?? 1);


                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;


                using (TCG_WorklistModel db2 = new TCG_WorklistModel())
                {
                    List<Get_Under_Paymnent_Accounts_APD_Result> result = db2.Get_Under_Paymnent_Accounts_APD().ToList();

                    if (type == "All")
                    {
                        using (var db = new TCG_WorklistModel())
                        {
                            result = (from c in db.Get_Under_Paymnent_Accounts_APD()
                                      select c).ToList();
                        }
                    }
                    else if (type == "New" || type == "Zero Balance" || type == "Credit Balance")
                    {
                        using (var db = new TCG_WorklistModel())
                        {
                            result = (from c in db.Get_Under_Paymnent_Accounts_APD()
                                      where c.CaseStatus == type
                                      select c).ToList();
                        }
                    }

                    //List<Get_Under_Paymnent_Accounts_PB_Result> flagRes = new List<Get_Under_Paymnent_Accounts_PB_Result>();
                    //flagRes.Add(new Get_Under_Paymnent_Accounts_PB_Result() { flag = 1 });
                    //ViewBag.flagDetails = flagRes;

                    for (int i = 0; i < result.Count; i++)
                    {
                        //if (result[i].Account.HasValue)
                        //{
                        //    result[i].AccId = Convert.ToString(result[i].Account);
                        //}


                        result[i].AccId = Convert.ToString(result[i].Account);

                        if (result[i].Account_Name != null)
                        {
                            string[] test = (result[i].Account_Name.ToString()).Split(':');
                            result[i].PatientName = test[0];
                            result[i].caseFlag = test[1];
                        }
                        else
                        {
                            result[i].PatientName = " ";
                            result[i].caseFlag = "NEW";
                        }

                        if (result[i].caseFlag == "NEW")
                            result[i].flagCase = true;
                        else
                            result[i].flagCase = false;
                        if (!string.IsNullOrEmpty(result[i].Brief_Summary))
                            result[i].flagCase = false;

                        if (result[i].CaseStatus == "New")
                            result[i].flagCaseValue = 1;
                        else if (result[i].CaseStatus == "Credit Balance")
                            result[i].flagCaseValue = 2;
                        else
                            result[i].flagCaseValue = 3;

                        if (string.IsNullOrEmpty(result[i].Payor_Name))
                            result[i].Payor_Name = "";
                        if (string.IsNullOrEmpty(result[i].Acct_Class))
                            result[i].Acct_Class = "";
                        if (string.IsNullOrEmpty(result[i].Plan_Name))
                            result[i].Plan_Name = "";
                        if (result[i].Underpayment_Reason_Code == null)
                            result[i].Underpayment_Reason_Code = 0;

                    }

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        result = result.Where(s => s.AccId.Contains(searchString.Trim().ToString())
                                               || s.Payor_Name.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.PatientName.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.Acct_Class.ToLower().Contains(searchString.Trim().ToLower())
                                               || s.Plan_Name.ToLower().Contains(searchString.Trim().ToLower())).ToList();
                    }

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
                        case "Bal_Asc":
                            result = result.OrderBy(s => s.Acct_Bal).ToList();
                            break;
                        case "Bal_Des":
                            result = result.OrderByDescending(s => s.Acct_Bal).ToList();
                            break;
                        case "Plan_Asc":
                            result = result.OrderBy(s => s.Plan_Name).ToList();
                            break;
                        case "Plan_Des":
                            result = result.OrderByDescending(s => s.Plan_Name).ToList();
                            break;
                        case "AccClass_Asc":
                            result = result.OrderByDescending(s => s.Acct_Class).ToList();
                            break;
                        case "AccClass_Des":
                            result = result.OrderBy(s => s.Acct_Class).ToList();
                            break;
                        default:  // Name Descending 
                            result = result.OrderByDescending(s => s.Acct_Bal).ToList();
                            break;
                    }



                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result[i].Disch_Date.HasValue)
                        {
                            result[i].DischargeDate = result[i].Disch_Date.Value.ToShortDateString();
                        }

                        string testAmt = result[i].Acct_Bal.Value.ToString("0.00");
                        Decimal testDecimal = Convert.ToDecimal(testAmt);
                        result[i].convertAmount = Math.Round(testDecimal, 2);
                        result[i].convertBal = "$" + result[i].convertAmount.ToString();
                    }


                    return View(result.ToPagedList(pageNumber, pageSize));
                }


            }
        }


        private static List<SelectListItem> populateHPorPB()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.HBorPB_Master.Select(x => new SelectListItem { Text = x.HP_Name, Value = x.HP_ID.ToString() }).Take(2).ToList();

        }

        private static List<SelectListItem> populateStatus() // 9 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Status_Master.Select(x => new SelectListItem { Text = x.SM_Name, Value = x.SM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateAccount_ARStatus() // 3 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Account_AR_Status.Select(x => new SelectListItem { Text = x.ARSts_Name, Value = x.ARSts_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_BillStatus() // 5 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Account_Bill_Status.Select(x => new SelectListItem { Text = x.BillSts_Name, Value = x.BillSts_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_AccountSource()  // 11 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Account_Source.Select(x => new SelectListItem { Text = x.AccSrc_Name, Value = x.AccSrc_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_EncounterType() // 22 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Encounter_Type.Select(x => new SelectListItem { Text = x.EncType_Name, Value = x.EncType_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_Insurance()  // 151 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Insurance_Company_Name.Select(x => new SelectListItem { Text = x.InsCmp_Name, Value = x.InsCmp_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_PayorFC() // 136 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Payor_Financial_Class.Select(x => new SelectListItem { Text = x.PyrFC_Name, Value = x.PyrFC_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_PrimaryReason() // 49 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.PrimaryReason_Master.Select(x => new SelectListItem { Text = x.PRM_Name, Value = x.PRM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_Task() // 40 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Task_Master.Select(x => new SelectListItem { Text = x.TM_Name, Value = x.TM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_UserLogin()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.Users_Data.Select(x => new SelectListItem { Text = x.user_web_login, Value = x.user_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_Priority()  // 1 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Priority_Master.Select(x => new SelectListItem { Text = x.PM_Name, Value = x.PM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateUnderPayReason() // 
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.Underpayment_ReasonCode.Select(x => new SelectListItem { Text = x.Reason_Title, Value = x.Reason_Code.ToString() }).ToList();

        }

        private static List<SelectListItem> populateRootCause() // 1 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.RootCause_Master.Select(x => new SelectListItem { Text = x.RC_Name, Value = x.RC_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateDenialCategory() //1 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.RootCause_Master.Select(x => new SelectListItem { Text = x.RC_Name, Value = x.RC_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateCompleted() //2 - No
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.CaseCompleted_Master.Select(x => new SelectListItem { Text = x.CC_Name, Value = x.CC_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateDenialStatusMaster() //1 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_WorklistModel context = new TCG_WorklistModel();
            return context.DenialStatus_Master.Select(x => new SelectListItem { Text = x.DS_Name, Value = x.DS_ID.ToString() }).ToList();

        }

       
        [HttpGet]
        public ActionResult editCaseDetails(string id, string linkName)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = id;

                openCaseID = id;
                string HospitalAccountID = openCaseID;
                int new_Case_Value = 0;
                Decimal testAmt1, testAmt2, testAmt3, testAmt4, AmtAllowedNAA, AmtAllowedPay, ExpAmt;
                string ownerId = string.Empty;


                TCG_WLM = new TCG_WorklistModel();
                Account_Case_Details acdDetails = new Account_Case_Details();
                Account_Case_Detials_History acdhDetails = new Account_Case_Detials_History();
                List<Account_Case_Details> ACD = new List<Account_Case_Details>();
                List<Account_Case_Detials_History> ACDH = new List<Account_Case_Detials_History>();

                List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();
                List<Get_Under_Paymnent_Accounts_Result> underPaymentsListByID = getUnderPaymentsDetailsList(openCaseID);
                if (underPaymentsListByID.Count > 0)
                {
                    underPaymentsListByID[0].DischargeDate = underPaymentsListByID[0].Disch_Date.Value.ToShortDateString();
                }

                if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                {
                    TCG_DataEntities tcguserdata = new TCG_DataEntities();
                    Users_Data tcglogin = new Users_Data();
                    tcglogin = tcguserdata.Users_Data.Where(m => m.user_first_name.ToString() == underPaymentsListByID[0].Biller).FirstOrDefault();
                    ownerId = tcglogin.user_ID.ToString();
                }
                else
                {
                    ownerId = get_Owner_dropDownValue("Select Owner");
                }


                try
                {
                    using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                    {
                        taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                        if (taskDetails.Count > 0 && taskDetails != null)
                        {
                            if (taskDetails[0].Total_Charge_Amount.HasValue)
                            {
                                testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                                testAmt1 = Math.Round(testAmt1, 2);
                                taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                            }
                            else
                            {
                                testAmt1 = 0;
                                taskDetails[0].convTotChrgAmt = "$0.00";
                            }


                            if (taskDetails[0].Total_Payment_Amount.HasValue)
                            {
                                testAmt2 = Convert.ToDecimal(taskDetails[0].Total_Payment_Amount);
                                testAmt2 = Math.Round(testAmt2, 2);
                                taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotPayAmt = "$0.00";
                                testAmt2 = 0;
                            }


                            if (taskDetails[0].Total_Adjustment_Amount.HasValue)
                            {
                                testAmt3 = Convert.ToDecimal(taskDetails[0].Total_Adjustment_Amount);
                                testAmt3 = Math.Round(testAmt3, 2);
                                taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                            }
                            else
                            {
                                taskDetails[0].convAdjAmt = "$0.00";
                                testAmt3 = 0;
                            }


                            if (taskDetails[0].Total_Account_Balance.HasValue)
                            {
                                testAmt4 = Convert.ToDecimal(taskDetails[0].Total_Account_Balance);
                                testAmt4 = Math.Round(testAmt4, 2);
                                taskDetails[0].convTotAccBal = "$" + testAmt4.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotAccBal = "$0.00";
                                testAmt4 = 0;
                            }


                            if (underPaymentsListByID[0].Allowed_Amount_Difference != null || underPaymentsListByID[0].Allowed_Amount_Difference != 0)
                            {
                                AmtAllowedNAA = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference);
                                AmtAllowedNAA = Math.Round(AmtAllowedNAA, 2);
                                acdDetails.convAllAmtDiff = "$" + AmtAllowedNAA.ToString();
                            }
                            else
                            {
                                acdDetails.convAllAmtDiff = "$" + "0.00";
                                AmtAllowedNAA = 0;
                            }

                            if (underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != null || underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != 0)
                            {
                                AmtAllowedPay = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified);
                                AmtAllowedPay = Math.Round(AmtAllowedPay, 2);
                                acdDetails.convAllAmtDiffPyr = "$" + AmtAllowedPay.ToString();
                            }
                            else
                            {
                                AmtAllowedPay = 0;
                                acdDetails.convAllAmtDiffPyr = "$" + "0.00";
                            }


                            if (!string.IsNullOrEmpty(underPaymentsListByID[0].Expected_Allowed_Amount) || underPaymentsListByID[0].Expected_Allowed_Amount != "0")
                            {
                                ExpAmt = Convert.ToDecimal(underPaymentsListByID[0].Expected_Allowed_Amount);
                                ExpAmt = Math.Round(ExpAmt, 2);
                                acdDetails.convExpAmtDiff = "$" + ExpAmt.ToString();
                            }
                            else
                            {
                                acdDetails.convExpAmtDiff = "$" + "0.00";
                                ExpAmt = 0;
                            }


                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

                            if (taskDetails[0].Admission_Date.HasValue)
                                taskDetails[0].convAdmDate = taskDetails[0].Admission_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convAdmDate = " ";


                            if (taskDetails[0].Discharge_Date.HasValue)
                                taskDetails[0].convDischDate = taskDetails[0].Discharge_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convDischDate = " ";


                            if (taskDetails[0].First_Billed_Date.HasValue)
                                taskDetails[0].convFirstBillDate = taskDetails[0].First_Billed_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convFirstBillDate = " ";


                            if (!string.IsNullOrEmpty(taskDetails[0].Last_Payor_Rcvd_Dt))
                            {
                                DateTime testDate = Convert.ToDateTime(taskDetails[0].Last_Payor_Rcvd_Dt);
                                taskDetails[0].convLastPayDate = testDate.ToShortDateString();
                            }
                            else
                                taskDetails[0].convLastPayDate = " ";


                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();
                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }

                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "1",
                                       strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(), underPay, " ", "22",
                                       "49", "49", Cmnt, "2", "1", "", 40,
                                        underPaymentsListByID[0].Disch_Date, DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                                else if (linkName == "Other")
                                {
                                    int billStatus_type;
                                    if (string.IsNullOrEmpty(taskDetails[0].Account_Bill_Status))
                                        billStatus_type = 5;
                                    else
                                        billStatus_type = get_BillStatus_dropDownValue(taskDetails[0].Account_Bill_Status);

                                    int accClass_subTpe;
                                    if (string.IsNullOrEmpty(taskDetails[0].Acct_Class))
                                        accClass_subTpe = 11;
                                    else
                                        accClass_subTpe = getAccClassDDLValue(taskDetails[0].Acct_Class); //Acct_Class -Denial , Account_Source - AR => Account_Source[Table in DB]

                                    int rootCause = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Root_Cause))
                                        rootCause = 1;
                                    else
                                        rootCause = getRootCauseDDLValue(taskDetails[0].Root_Cause);

                                    int denialReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Denial_Cat))
                                        denialReason = 1;
                                    else
                                        denialReason = getDenialReason(taskDetails[0].Denial_Cat);

                                    int denialStatusReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Status))
                                        denialStatusReason = 1;
                                    else
                                        denialStatusReason = getDenialStatusReason(taskDetails[0].Status);


                                    DateTime date;
                                    if (string.IsNullOrEmpty(taskDetails[0].Rcvd_Dt))
                                        date = DateTime.Now;
                                    else
                                        date = DateTime.Parse(taskDetails[0].Rcvd_Dt);

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    TCG_WLM.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "1", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                       rootCause, denialStatusReason.ToString(), denialReason.ToString(), "49", "49", "",
                                       "2", "1", "", 40, date,
                                       DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                       "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 2;


                                }
                            }

                        }

                        else if (taskDetails.Count == 0 || taskDetails == null)
                        {

                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


                            Decimal testAmount = Convert.ToDecimal(underPaymentsListByID[0].Acct_Bal);
                            testAmount = Math.Round(testAmount, 2);

                            taskDetails.Add(new Get_Account_Info_for_ARandDenial_Result() { Hospital_Account_ID = Convert.ToString(underPaymentsListByID[0].Account), Account_Patient_Name = underPaymentsListByID[0].Account_Name, convAdmDate = "", Encounter_Type = "", Account_Financial_Class = "", convFirstBillDate = "", convLastPayDate = "", convTotChrgAmt = "$0.00", convTotPayAmt = "$0.00", convAdjAmt = "$0.00", convTotAccBal = "$" + testAmount.ToString() });

                            testAmt1 = 0;
                            taskDetails[0].convTotChrgAmt = "$0.00";

                            taskDetails[0].convTotPayAmt = "$0.00";
                            testAmt2 = 0;

                            taskDetails[0].convAdjAmt = "$0.00";
                            testAmt3 = 0;

                            taskDetails[0].convTotAccBal = "$0.00";
                            testAmt4 = 0;

                            acdDetails.convAllAmtDiff = "$" + "0.00";
                            AmtAllowedNAA = 0;

                            AmtAllowedPay = 0;
                            acdDetails.convAllAmtDiffPyr = "$" + "0.00";

                            acdDetails.convExpAmtDiff = "$" + "0.00";
                            ExpAmt = 0;

                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();

                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }
                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }
                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                                    //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                                    //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                                    //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "1", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                        underPay, " ", "22", "49", "49", Cmnt,
                                        "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                        DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                            }
                        }


                        TCG_WorklistModel context = new TCG_WorklistModel();
                        TCG_DataEntities context_tcg = new TCG_DataEntities();


                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                        ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                        ViewBag.OriginalData = taskDetails;
                        ACD = get_CaseDetailsList(HospitalAccountID);
                        ACD[0].dateConvert = ACD[0].ACD_DueDate.Value.ToShortDateString();

                        //CASE DETAILS
                        acdDetails = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                        acdDetails.dateConvert = acdDetails.ACD_DueDate.Value.ToShortDateString();
                        acdDetails.dateFollowUp = acdDetails.ACD_FollowUpDate.Value.ToShortDateString();
                        Decimal AmtConvert = Convert.ToDecimal(acdDetails.ACD_Amount);
                        acdDetails.convertAmount = Math.Round(AmtConvert, 2);
                        acdDetails.convAmt = acdDetails.convertAmount.ToString();
                        Decimal conv1 = Convert.ToDecimal(acdDetails.ACD_Amount);
                        conv1 = Math.Round(conv1, 2);
                        acdDetails.TotAccBal = "$" + Convert.ToString(conv1);
                        Decimal conv2 = Math.Round(acdDetails.ACD_TotalCharges, 2);
                        acdDetails.TotChrgAmt = "$" + Convert.ToString(conv2);
                        Decimal conv3 = Math.Round(acdDetails.ACD_TotalPay, 2);
                        acdDetails.TotPayAmt = "$" + Convert.ToString(conv3);
                        Decimal conv4 = Math.Round(acdDetails.ACD_TotalAdj, 2);
                        acdDetails.TotAdjAmt = "$" + Convert.ToString(conv4);
                        Decimal conv5 = Math.Round(acdDetails.ACD_AmtDiffNAA, 2);
                        acdDetails.convAllAmtDiff = "$" + Convert.ToString(conv5);
                        Decimal conv6 = Math.Round(acdDetails.ACD_AmtDiffPayor, 2);
                        acdDetails.convAllAmtDiffPyr = "$" + Convert.ToString(conv6);
                        Decimal conv7 = Math.Round(acdDetails.ACD_ExpAmt, 2);
                        acdDetails.convExpAmtDiff = "$" + Convert.ToString(conv7);

                        if (linkName == "underPay")
                        {
                            acdDetails.link = 1;
                        }
                        else
                        {
                            acdDetails.link = 2;
                        }
                        //acdDetails.userName = underPaymentsListByID[0].Biller;
                        //if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                        //{
                        //    acdDetails.userName = underPaymentsListByID[0].Biller;
                        //    acdDetails.userFlag = 1;
                        //}
                        //else
                        //{
                        //    acdDetails.userFlag = 2;
                        //}
                        acdDetails.userFlag = 2;
                        acdDetails.userID = acdDetails.ACD_Owner;

                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Comments = underPaymentsListByID[0].Brief_Summary;
                        }


                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Status = 5;
                        }


                        //CASE DETAILS HISTORY
                        ACDH = getCaseDetailsHistoryList(HospitalAccountID);
                        if (ACDH.Count > 0)
                        {
                            for (int j = 0; j < ACDH.Count; j++)
                            {
                                ACDH[j].ACDH_Status = ACDH[j].ACDH_Status;
                                ACDH[j].ACDH_PayerReason = ACDH[j].ACDH_PayerReason;
                                Double amt = Convert.ToDouble(ACDH[j].ACDH_Amount);
                                amt = Math.Round(amt, 2);
                                ACDH[j].ACDH_Amount = "$" + Convert.ToString(amt);
                                ACDH[j].ACDH_Owner = get_Owner_dropDownText(ACDH[j].ACDH_Owner);
                                ACDH[j].convertDate = ACDH[j].ACDH_CreatedDate.ToShortDateString();
                                ACDH[j].ACDH_Priority = getPrioritydropDownText(Convert.ToInt32(ACDH[j].ACDH_Priority));
                                ACDH[j].convertFollowUpDate = ACDH[j].ACDH_FollowUpDate.Value.ToShortDateString();
                                ACDH[j].ACDH_TaskFollowUp = ACDH[j].ACDH_TaskFollowUp;
                                Decimal amt1 = Math.Round(ACDH[j].ACDH_TotalCharges, 2);
                                ACDH[j].totalCharges = "$" + Convert.ToString(amt1);
                                Decimal amt2 = Math.Round(ACDH[j].ACDH_TotalPay, 2);
                                ACDH[j].totalPay = "$" + Convert.ToString(amt2);
                                Decimal amt3 = Math.Round(ACDH[j].ACDH_TotalAdj, 2);
                                ACDH[j].totalAdj = "$" + Convert.ToString(amt3);

                            }

                        }

                        ViewBag.underPaymentsLinkData = underPaymentsListByID;
                        ViewBag.caseDetailsHistory = ACDH;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return View(acdDetails);
            }
        }

        [HttpPost]
        public ActionResult editCaseDetails(Account_Case_Details ACD)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = ACD.ACD_HspAccID;

                openCaseID = ACD.ACD_HspAccID;
                string HospitalAccountID = openCaseID;

                TCG_WLM = new TCG_WorklistModel();
                string linkName = string.Empty;
                if (ACD.link == 1)
                {
                    linkName = "underPay";
                }
                else
                {
                    linkName = "Other";
                }

                try
                {
                    using (TCG_WorklistModel tcg_CaseDetails = new TCG_WorklistModel())
                    {
                        var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));

                        string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                        string[] test = ACD.ACD_Amount.Split('$');
                        ACD.ACD_Amount = test[1];
                        string[] test2 = ACD.TotChrgAmt.Split('$');
                        ACD.ACD_TotalCharges = Convert.ToDecimal(test2[1]);
                        string[] test3 = ACD.TotPayAmt.Split('$');
                        ACD.ACD_TotalPay = Convert.ToDecimal(test3[1]);
                        string[] test4 = ACD.TotAdjAmt.Split('$');
                        ACD.ACD_TotalAdj = Convert.ToDecimal(test4[1]);


                        //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                        //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                        //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                        //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                        //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                        //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                        string desc = string.Empty;
                        if (string.IsNullOrEmpty(ACD.ACD_Description))
                        { desc = " "; }
                        else
                        { desc = ACD.ACD_Description; }
                        TCG_WLM.Case_InsUpd(ACD.ACD_ID, ACD.ACD_HspAccID, ACD.ACD_Amount, ACD.ACD_TotalCharges, ACD.ACD_TotalPay, ACD.ACD_TotalAdj,
                            ACD.ACD_AmtDiffNAA, ACD.ACD_AmtDiffPayor, ACD.ACD_ExpAmt, "1", "1", "1",
                            ACD.ACD_Status, ACD.ACD_Owner, ACD.ACD_Type, ACD.ACD_SubType,
                            ACD.ACD_PayerReason, ACD.ACD_PrimaryReason, "22", "49", "49", ACD.ACD_Comments,
                            "2", ACD.ACD_Priority, desc, ACD.ACD_TaskFollowUp, ACD.ACD_DueDate,
                            ACD.ACD_FollowUpDate, false, Session["username"].ToString(), DateTime.Now,
                            "", DateTime.Now, "", case_idParameter);


                        //new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return RedirectToAction("viewCaseDetails", new { id = ACD.ACD_HspAccID, linkName });
            }
        }

        [HttpGet]
        public ActionResult viewCaseDetails(string id, string linkName)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = id;

                openCaseID = id;
                string HospitalAccountID = openCaseID;
                int new_Case_Value = 0;
                Decimal testAmt1, testAmt2, testAmt3, testAmt4, AmtAllowedNAA, AmtAllowedPay, ExpAmt;
                string ownerId = string.Empty;


                TCG_WLM = new TCG_WorklistModel();
                Account_Case_Details acdDetails = new Account_Case_Details();
                Account_Case_Detials_History acdhDetails = new Account_Case_Detials_History();
                List<Account_Case_Details> ACD = new List<Account_Case_Details>();
                List<Account_Case_Detials_History> ACDH = new List<Account_Case_Detials_History>();

                List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();
                List<Get_Under_Paymnent_Accounts_Result> underPaymentsListByID = getUnderPaymentsDetailsList(openCaseID);
                if (underPaymentsListByID.Count > 0)
                {
                    underPaymentsListByID[0].DischargeDate = underPaymentsListByID[0].Disch_Date.Value.ToShortDateString();
                }


                if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                {
                    TCG_DataEntities tcguserdata = new TCG_DataEntities();
                    Users_Data tcglogin = new Users_Data();
                    tcglogin = tcguserdata.Users_Data.Where(m => m.user_first_name.ToString() == underPaymentsListByID[0].Biller).FirstOrDefault();
                    ownerId = tcglogin.user_ID.ToString();
                }
                else
                {
                    ownerId = get_Owner_dropDownValue("Select Owner");
                }

                try
                {
                    using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                    {
                        taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                        if (taskDetails.Count > 0 && taskDetails != null)
                        {
                            if (taskDetails[0].Total_Charge_Amount.HasValue)
                            {
                                testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                                testAmt1 = Math.Round(testAmt1, 2);
                                taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                            }
                            else
                            {
                                testAmt1 = 0;
                                taskDetails[0].convTotChrgAmt = "$0.00";
                            }


                            if (taskDetails[0].Total_Payment_Amount.HasValue)
                            {
                                testAmt2 = Convert.ToDecimal(taskDetails[0].Total_Payment_Amount);
                                testAmt2 = Math.Round(testAmt2, 2);
                                taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotPayAmt = "$0.00";
                                testAmt2 = 0;
                            }


                            if (taskDetails[0].Total_Adjustment_Amount.HasValue)
                            {
                                testAmt3 = Convert.ToDecimal(taskDetails[0].Total_Adjustment_Amount);
                                testAmt3 = Math.Round(testAmt3, 2);
                                taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                            }
                            else
                            {
                                taskDetails[0].convAdjAmt = "$0.00";
                                testAmt3 = 0;
                            }


                            if (taskDetails[0].Total_Account_Balance.HasValue)
                            {
                                testAmt4 = Convert.ToDecimal(taskDetails[0].Total_Account_Balance);
                                testAmt4 = Math.Round(testAmt4, 2);
                                taskDetails[0].convTotAccBal = "$" + testAmt4.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotAccBal = "$0.00";
                                testAmt4 = 0;
                            }


                            if (underPaymentsListByID[0].Allowed_Amount_Difference != null || underPaymentsListByID[0].Allowed_Amount_Difference != 0)
                            {
                                AmtAllowedNAA = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference);
                                AmtAllowedNAA = Math.Round(AmtAllowedNAA, 2);
                                acdDetails.convAllAmtDiff = "$" + AmtAllowedNAA.ToString();
                            }
                            else
                            {
                                acdDetails.convAllAmtDiff = "$" + "0.00";
                                AmtAllowedNAA = 0;
                            }

                            if (underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != null || underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != 0)
                            {
                                AmtAllowedPay = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified);
                                AmtAllowedPay = Math.Round(AmtAllowedPay, 2);
                                acdDetails.convAllAmtDiffPyr = "$" + AmtAllowedPay.ToString();
                            }
                            else
                            {
                                AmtAllowedPay = 0;
                                acdDetails.convAllAmtDiffPyr = "$" + "0.00";
                            }


                            if (!string.IsNullOrEmpty(underPaymentsListByID[0].Expected_Allowed_Amount) || underPaymentsListByID[0].Expected_Allowed_Amount != "0")
                            {
                                ExpAmt = Convert.ToDecimal(underPaymentsListByID[0].Expected_Allowed_Amount);
                                ExpAmt = Math.Round(ExpAmt, 2);
                                acdDetails.convExpAmtDiff = "$" + ExpAmt.ToString();
                            }
                            else
                            {
                                acdDetails.convExpAmtDiff = "$" + "0.00";
                                ExpAmt = 0;
                            }


                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

                            if (taskDetails[0].Admission_Date.HasValue)
                                taskDetails[0].convAdmDate = taskDetails[0].Admission_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convAdmDate = " ";


                            if (taskDetails[0].Discharge_Date.HasValue)
                                taskDetails[0].convDischDate = taskDetails[0].Discharge_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convDischDate = " ";


                            if (taskDetails[0].First_Billed_Date.HasValue)
                                taskDetails[0].convFirstBillDate = taskDetails[0].First_Billed_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convFirstBillDate = " ";


                            if (!string.IsNullOrEmpty(taskDetails[0].Last_Payor_Rcvd_Dt))
                            {
                                DateTime testDate = Convert.ToDateTime(taskDetails[0].Last_Payor_Rcvd_Dt);
                                taskDetails[0].convLastPayDate = testDate.ToShortDateString();
                            }
                            else
                                taskDetails[0].convLastPayDate = " ";


                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();
                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                            
                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }

                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "1",
                                       strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(), underPay, " ", "22",
                                       "49", "49", Cmnt, "2", "1", "", 40,
                                        underPaymentsListByID[0].Disch_Date, DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                                else if (linkName == "Other")
                                {
                                    int billStatus_type;
                                    if (string.IsNullOrEmpty(taskDetails[0].Account_Bill_Status))
                                        billStatus_type = 5;
                                    else
                                        billStatus_type = get_BillStatus_dropDownValue(taskDetails[0].Account_Bill_Status);

                                    int accClass_subTpe;
                                    if (string.IsNullOrEmpty(taskDetails[0].Acct_Class))
                                        accClass_subTpe = 11;
                                    else
                                        accClass_subTpe = getAccClassDDLValue(taskDetails[0].Acct_Class); //Acct_Class -Denial , Account_Source - AR => Account_Source[Table in DB]

                                    int rootCause = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Root_Cause))
                                        rootCause = 1;
                                    else
                                        rootCause = getRootCauseDDLValue(taskDetails[0].Root_Cause);

                                    int denialReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Denial_Cat))
                                        denialReason = 1;
                                    else
                                        denialReason = getDenialReason(taskDetails[0].Denial_Cat);

                                    int denialStatusReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Status))
                                        denialStatusReason = 1;
                                    else
                                        denialStatusReason = getDenialStatusReason(taskDetails[0].Status);


                                    DateTime date;
                                    if (string.IsNullOrEmpty(taskDetails[0].Rcvd_Dt))
                                        date = DateTime.Now;
                                    else
                                        date = DateTime.Parse(taskDetails[0].Rcvd_Dt);

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    TCG_WLM.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "1", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                       rootCause, denialStatusReason.ToString(), denialReason.ToString(), "49", "49", "",
                                       "2", "1", "", 40, date,
                                       DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                       "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 2;


                                }
                            }

                        }

                        else if (taskDetails.Count == 0 || taskDetails == null)
                        {

                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


                            Decimal testAmount = Convert.ToDecimal(underPaymentsListByID[0].Acct_Bal);
                            testAmount = Math.Round(testAmount, 2);

                            taskDetails.Add(new Get_Account_Info_for_ARandDenial_Result() { Hospital_Account_ID = Convert.ToString(underPaymentsListByID[0].Account), Account_Patient_Name = underPaymentsListByID[0].Account_Name, convAdmDate = "", Encounter_Type = "", Account_Financial_Class = "", convFirstBillDate = "", convLastPayDate = "", convTotChrgAmt = "$0.00", convTotPayAmt = "$0.00", convAdjAmt = "$0.00", convTotAccBal = "$" + testAmount.ToString() });

                            testAmt1 = 0;
                            taskDetails[0].convTotChrgAmt = "$0.00";

                            taskDetails[0].convTotPayAmt = "$0.00";
                            testAmt2 = 0;

                            taskDetails[0].convAdjAmt = "$0.00";
                            testAmt3 = 0;

                            taskDetails[0].convTotAccBal = "$0.00";
                            testAmt4 = 0;

                            acdDetails.convAllAmtDiff = "$" + "0.00";
                            AmtAllowedNAA = 0;

                            AmtAllowedPay = 0;
                            acdDetails.convAllAmtDiffPyr = "$" + "0.00";

                            acdDetails.convExpAmtDiff = "$" + "0.00";
                            ExpAmt = 0;

                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();

                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            
                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }
                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }
                                   
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "1", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                        underPay, " ", "22", "49", "49", Cmnt,
                                        "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                        DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                            }
                        }


                        TCG_WorklistModel context = new TCG_WorklistModel();
                        TCG_DataEntities context_tcg = new TCG_DataEntities();


                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                        ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                        ViewBag.OriginalData = taskDetails;
                        ACD = get_CaseDetailsList(HospitalAccountID);
                        ACD[0].dateConvert = ACD[0].ACD_DueDate.Value.ToShortDateString();

                        //CASE DETAILS
                        acdDetails = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                        acdDetails.dateConvert = acdDetails.ACD_DueDate.Value.ToShortDateString();
                        acdDetails.dateFollowUp = acdDetails.ACD_FollowUpDate.Value.ToShortDateString();
                        Decimal AmtConvert = Convert.ToDecimal(acdDetails.ACD_Amount);
                        acdDetails.convertAmount = Math.Round(AmtConvert, 2);
                        acdDetails.convAmt = acdDetails.convertAmount.ToString();
                        Decimal conv1 = Convert.ToDecimal(acdDetails.ACD_Amount);
                        conv1 = Math.Round(conv1, 2);
                        acdDetails.TotAccBal = "$" + Convert.ToString(conv1);
                        Decimal conv2 = Math.Round(acdDetails.ACD_TotalCharges, 2);
                        acdDetails.TotChrgAmt = "$" + Convert.ToString(conv2);
                        Decimal conv3 = Math.Round(acdDetails.ACD_TotalPay, 2);
                        acdDetails.TotPayAmt = "$" + Convert.ToString(conv3);
                        Decimal conv4 = Math.Round(acdDetails.ACD_TotalAdj, 2);
                        acdDetails.TotAdjAmt = "$" + Convert.ToString(conv4);
                        Decimal conv5 = Math.Round(acdDetails.ACD_AmtDiffNAA, 2);
                        acdDetails.convAllAmtDiff = "$" + Convert.ToString(conv5);
                        Decimal conv6 = Math.Round(acdDetails.ACD_AmtDiffPayor, 2);
                        acdDetails.convAllAmtDiffPyr = "$" + Convert.ToString(conv6);
                        Decimal conv7 = Math.Round(acdDetails.ACD_ExpAmt, 2);
                        acdDetails.convExpAmtDiff = "$" + Convert.ToString(conv7);

                        if (linkName == "underPay")
                        {
                            acdDetails.link = 1;
                        }
                        else
                        {
                            acdDetails.link = 2;
                        }
                        //acdDetails.userName = underPaymentsListByID[0].Biller;
                        //if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                        //{
                        //    acdDetails.userName = underPaymentsListByID[0].Biller;
                        //    acdDetails.userFlag = 1;
                        //}
                        //else
                        //{
                        //    acdDetails.userFlag = 2;
                        //}
                        acdDetails.userFlag = 2;
                        acdDetails.userID = acdDetails.ACD_Owner;

                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Comments = underPaymentsListByID[0].Brief_Summary;
                        }


                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Status = 5;
                        }


                        //CASE DETAILS HISTORY
                        ACDH = getCaseDetailsHistoryList(HospitalAccountID);
                        if (ACDH.Count > 0)
                        {
                            for (int j = 0; j < ACDH.Count; j++)
                            {
                                ACDH[j].ACDH_Status = ACDH[j].ACDH_Status;
                                ACDH[j].ACDH_PayerReason = ACDH[j].ACDH_PayerReason;
                                Double amt = Convert.ToDouble(ACDH[j].ACDH_Amount);
                                amt = Math.Round(amt, 2);
                                ACDH[j].ACDH_Amount = "$" + Convert.ToString(amt);
                                ACDH[j].ACDH_Owner = get_Owner_dropDownText(ACDH[j].ACDH_Owner);
                                ACDH[j].convertDate = ACDH[j].ACDH_CreatedDate.ToShortDateString();
                                ACDH[j].ACDH_Priority = getPrioritydropDownText(Convert.ToInt32(ACDH[j].ACDH_Priority));
                                ACDH[j].convertFollowUpDate = ACDH[j].ACDH_FollowUpDate.Value.ToShortDateString();
                                ACDH[j].ACDH_TaskFollowUp = ACDH[j].ACDH_TaskFollowUp;
                                Decimal amt1 = Math.Round(ACDH[j].ACDH_TotalCharges, 2);
                                ACDH[j].totalCharges = "$" + Convert.ToString(amt1);
                                Decimal amt2 = Math.Round(ACDH[j].ACDH_TotalPay, 2);
                                ACDH[j].totalPay = "$" + Convert.ToString(amt2);
                                Decimal amt3 = Math.Round(ACDH[j].ACDH_TotalAdj, 2);
                                ACDH[j].totalAdj = "$" + Convert.ToString(amt3);

                            }

                        }

                        ViewBag.underPaymentsLinkData = underPaymentsListByID;
                        ViewBag.caseDetailsHistory = ACDH;
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return View(acdDetails);
            }
        }

        [HttpGet]
        public ActionResult PBviewCaseDetails(string id, string linkName)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = id;

                openCaseID = id;
                string HospitalAccountID = openCaseID;
                int new_Case_Value = 0;
                Decimal testAmt1, testAmt2, testAmt3, testAmt4, AmtAllowedNAA, AmtAllowedPay, ExpAmt;
                string ownerId = string.Empty;


                TCG_WLM = new TCG_WorklistModel();
                Account_Case_Details acdDetails = new Account_Case_Details();
                Account_Case_Detials_History acdhDetails = new Account_Case_Detials_History();
                List<Account_Case_Details> ACD = new List<Account_Case_Details>();
                List<Account_Case_Detials_History> ACDH = new List<Account_Case_Detials_History>();

                List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();
                List<Get_Under_Paymnent_Accounts_PB_Result> underPaymentsListByID = getPBUnderPaymentsDetailsList(openCaseID);
                if (underPaymentsListByID.Count > 0)
                {
                    underPaymentsListByID[0].DischargeDate = underPaymentsListByID[0].Disch_Date.Value.ToShortDateString();
                    string[] cptcode;
                    if (underPaymentsListByID[0].CPTCODES.Contains(','))
                    {
                        cptcode = underPaymentsListByID[0].CPTCODES.Split(',');
                        List<string> key = new List<string>();
                        for(int i=0;i<cptcode.Length;i++)
                        {
                            key.Add(cptcode[i]);
                        }
                        ViewBag.cptvalues = new SelectList(key);                       

                    }
                    else
                    {
                        ViewBag.cptvalues = new SelectList(underPaymentsListByID[0].CPTCODES);
                    }
                }

                if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                {
                    TCG_DataEntities tcguserdata = new TCG_DataEntities();
                    Users_Data tcglogin = new Users_Data();
                    tcglogin = tcguserdata.Users_Data.Where(m => m.user_first_name.ToString() == underPaymentsListByID[0].Biller).FirstOrDefault();
                    ownerId = tcglogin.user_ID.ToString();
                }
                else
                {
                    ownerId = get_Owner_dropDownValue("Select Owner");
                }

                try
                {
                    using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                    {
                        taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                        if (taskDetails.Count > 0 && taskDetails != null)
                        {

                            //if (taskDetails[0].Total_Charge_Amount.HasValue)
                            if (underPaymentsListByID[0].TOTALCHARGES.HasValue)
                            {
                                //testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                                testAmt1 = Convert.ToDecimal(underPaymentsListByID[0].TOTALCHARGES);
                                testAmt1 = Math.Round(testAmt1, 2);
                                taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                            }
                            else
                            {
                                testAmt1 = 0;
                                taskDetails[0].convTotChrgAmt = "$0.00";
                            }


                            if (underPaymentsListByID[0].TOTALPAYMENTS.HasValue)
                            {
                                testAmt2 = Convert.ToDecimal(underPaymentsListByID[0].TOTALPAYMENTS);
                                testAmt2 = Math.Round(testAmt2, 2);
                                taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotPayAmt = "$0.00";
                                testAmt2 = 0;
                            }


                            if (underPaymentsListByID[0].TOTALADJUSTMENTS.HasValue)
                            {
                                testAmt3 = Convert.ToDecimal(underPaymentsListByID[0].TOTALADJUSTMENTS);
                                testAmt3 = Math.Round(testAmt3, 2);
                                taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                            }
                            else
                            {
                                taskDetails[0].convAdjAmt = "$0.00";
                                testAmt3 = 0;
                            }


                            if (taskDetails[0].Total_Account_Balance.HasValue)
                            {
                                testAmt4 = Convert.ToDecimal(taskDetails[0].Total_Account_Balance);
                                testAmt4 = Math.Round(testAmt4, 2);
                                taskDetails[0].convTotAccBal = "$" + testAmt4.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotAccBal = "$0.00";
                                testAmt4 = 0;
                            }


                            if (underPaymentsListByID[0].Allowed_Amount_Difference != null || underPaymentsListByID[0].Allowed_Amount_Difference != 0)
                            {
                                AmtAllowedNAA = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference);
                                AmtAllowedNAA = Math.Round(AmtAllowedNAA, 2);
                                acdDetails.convAllAmtDiff = "$" + AmtAllowedNAA.ToString();
                            }
                            else
                            {
                                acdDetails.convAllAmtDiff = "$" + "0.00";
                                AmtAllowedNAA = 0;
                            }

                            if (underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != null || underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != 0)
                            {
                                AmtAllowedPay = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified);
                                AmtAllowedPay = Math.Round(AmtAllowedPay, 2);
                                acdDetails.convAllAmtDiffPyr = "$" + AmtAllowedPay.ToString();
                            }
                            else
                            {
                                AmtAllowedPay = 0;
                                acdDetails.convAllAmtDiffPyr = "$" + "0.00";
                            }


                            if (underPaymentsListByID[0].Expected_Allowed_Amount != null || Convert.ToInt32(underPaymentsListByID[0].Expected_Allowed_Amount) != 0)
                            {
                                ExpAmt = Convert.ToDecimal(underPaymentsListByID[0].Expected_Allowed_Amount);
                                ExpAmt = Math.Round(ExpAmt, 2);
                                acdDetails.convExpAmtDiff = "$" + ExpAmt.ToString();
                            }
                            else
                            {
                                acdDetails.convExpAmtDiff = "$" + "0.00";
                                ExpAmt = 0;
                            }


                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

                            if (taskDetails[0].Admission_Date.HasValue)
                                taskDetails[0].convAdmDate = taskDetails[0].Admission_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convAdmDate = " ";


                            if (taskDetails[0].Discharge_Date.HasValue)
                                taskDetails[0].convDischDate = taskDetails[0].Discharge_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convDischDate = " ";


                            if (taskDetails[0].First_Billed_Date.HasValue)
                                taskDetails[0].convFirstBillDate = taskDetails[0].First_Billed_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convFirstBillDate = " ";


                            if (!string.IsNullOrEmpty(taskDetails[0].Last_Payor_Rcvd_Dt))
                            {
                                DateTime testDate = Convert.ToDateTime(taskDetails[0].Last_Payor_Rcvd_Dt);
                                taskDetails[0].convLastPayDate = testDate.ToShortDateString();
                            }
                            else
                                taskDetails[0].convLastPayDate = " ";

                            

                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();
                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type;
                                    //if (underPaymentsListByID[0].Acct_Status.HasValue)
                                    if (underPaymentsListByID[0].Acct_Status != null)
                                    {
                                        string test = Convert.ToString(underPaymentsListByID[0].Acct_Status);
                                        if (test == "New" || test == "Zero Balance" || test == "Credit Balance")
                                        {
                                            test = "Select";
                                        }
                                        billStatus_type = get_BillStatus_dropDownValue(test.ToString());
                                    }
                                    else
                                        billStatus_type = 5;

                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }

                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "2",
                                       strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(), underPay, " ", "22",
                                       "49", "49", Cmnt, "2", "1", "", 40,
                                        underPaymentsListByID[0].Disch_Date, DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                                else if (linkName == "Other")
                                {
                                    int billStatus_type;
                                    if (string.IsNullOrEmpty(taskDetails[0].Account_Bill_Status))
                                        billStatus_type = 5;
                                    else
                                        billStatus_type = get_BillStatus_dropDownValue(taskDetails[0].Account_Bill_Status);

                                    int accClass_subTpe;
                                    if (string.IsNullOrEmpty(taskDetails[0].Acct_Class))
                                        accClass_subTpe = 11;
                                    else
                                        accClass_subTpe = getAccClassDDLValue(taskDetails[0].Acct_Class); //Acct_Class -Denial , Account_Source - AR => Account_Source[Table in DB]

                                    int rootCause = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Root_Cause))
                                        rootCause = 1;
                                    else
                                        rootCause = getRootCauseDDLValue(taskDetails[0].Root_Cause);

                                    int denialReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Denial_Cat))
                                        denialReason = 1;
                                    else
                                        denialReason = getDenialReason(taskDetails[0].Denial_Cat);

                                    int denialStatusReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Status))
                                        denialStatusReason = 1;
                                    else
                                        denialStatusReason = getDenialStatusReason(taskDetails[0].Status);


                                    DateTime date;
                                    if (string.IsNullOrEmpty(taskDetails[0].Rcvd_Dt))
                                        date = DateTime.Now;
                                    else
                                        date = DateTime.Parse(taskDetails[0].Rcvd_Dt);

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    TCG_WLM.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "2", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                       rootCause, denialStatusReason.ToString(), denialReason.ToString(), "49", "49", "",
                                       "2", "1", "", 40, date,
                                       DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                       "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 2;


                                }
                            }

                        }

                        else if (taskDetails.Count == 0 || taskDetails == null)
                        {

                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


                            Decimal testAmount = Convert.ToDecimal(underPaymentsListByID[0].Acct_Bal);
                            testAmount = Math.Round(testAmount, 2);

                            taskDetails.Add(new Get_Account_Info_for_ARandDenial_Result() { Hospital_Account_ID = Convert.ToString(underPaymentsListByID[0].Account), Account_Patient_Name = underPaymentsListByID[0].Account_Name, convAdmDate = "", Encounter_Type = "", Account_Financial_Class = "", convFirstBillDate = "", convLastPayDate = "", convTotChrgAmt = "$0.00", convTotPayAmt = "$0.00", convAdjAmt = "$0.00", convTotAccBal = "$" + testAmount.ToString() });


                            //if (taskDetails[0].Total_Charge_Amount.HasValue)
                            if (underPaymentsListByID[0].TOTALCHARGES.HasValue)
                            {
                                //testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                                testAmt1 = Convert.ToDecimal(underPaymentsListByID[0].TOTALCHARGES);
                                testAmt1 = Math.Round(testAmt1, 2);
                                taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                            }
                            else
                            {
                                testAmt1 = 0;
                                taskDetails[0].convTotChrgAmt = "$0.00";
                            }


                            if (underPaymentsListByID[0].TOTALPAYMENTS.HasValue)
                            {
                                testAmt2 = Convert.ToDecimal(underPaymentsListByID[0].TOTALPAYMENTS);
                                testAmt2 = Math.Round(testAmt2, 2);
                                taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotPayAmt = "$0.00";
                                testAmt2 = 0;
                            }


                            if (underPaymentsListByID[0].TOTALADJUSTMENTS.HasValue)
                            {
                                testAmt3 = Convert.ToDecimal(underPaymentsListByID[0].TOTALADJUSTMENTS);
                                testAmt3 = Math.Round(testAmt3, 2);
                                taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                            }
                            else
                            {
                                taskDetails[0].convAdjAmt = "$0.00";
                                testAmt3 = 0;
                            }


                            //testAmt1 = 0;
                            //taskDetails[0].convTotChrgAmt = "$0.00";

                            //taskDetails[0].convTotPayAmt = "$0.00";
                            //testAmt2 = 0;

                            //taskDetails[0].convAdjAmt = "$0.00";
                            //testAmt3 = 0;

                            taskDetails[0].convTotAccBal = "$0.00";
                            testAmt4 = 0;

                            acdDetails.convAllAmtDiff = "$" + "0.00";
                            AmtAllowedNAA = 0;

                            AmtAllowedPay = 0;
                            acdDetails.convAllAmtDiffPyr = "$" + "0.00";

                            acdDetails.convExpAmtDiff = "$" + "0.00";
                            ExpAmt = 0;

                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();

                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type;
                                    if (underPaymentsListByID[0].Acct_Status != null)
                                    {
                                        string test = Convert.ToString(underPaymentsListByID[0].Acct_Status);
                                        if (test == "New" || test == "Zero Balance" || test == "Credit Balance")
                                        {
                                            test = "Select";
                                        }
                                        billStatus_type = get_BillStatus_dropDownValue(test.ToString());
                                    }
                                    else
                                        billStatus_type = 5;

                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }
                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }
                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "2", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                        underPay, " ", "22", "49", "49", Cmnt,
                                        "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                        DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                            }
                        }


                        TCG_WorklistModel context = new TCG_WorklistModel();
                        TCG_DataEntities context_tcg = new TCG_DataEntities();


                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                        ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                        ViewBag.OriginalData = taskDetails;
                        ACD = get_CaseDetailsList(HospitalAccountID);
                        ACD[0].dateConvert = ACD[0].ACD_DueDate.Value.ToShortDateString();

                        //CASE DETAILS
                        acdDetails = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                        acdDetails.dateConvert = acdDetails.ACD_DueDate.Value.ToShortDateString();
                        acdDetails.dateFollowUp = acdDetails.ACD_FollowUpDate.Value.ToShortDateString();
                        Decimal AmtConvert = Convert.ToDecimal(acdDetails.ACD_Amount);
                        acdDetails.convertAmount = Math.Round(AmtConvert, 2);
                        acdDetails.convAmt = acdDetails.convertAmount.ToString();
                        Decimal conv1 = Convert.ToDecimal(acdDetails.ACD_Amount);
                        conv1 = Math.Round(conv1, 2);
                        acdDetails.TotAccBal = "$" + Convert.ToString(conv1);
                        Decimal conv2 = Math.Round(acdDetails.ACD_TotalCharges, 2);
                        acdDetails.TotChrgAmt = "$" + Convert.ToString(conv2);
                        Decimal conv3 = Math.Round(acdDetails.ACD_TotalPay, 2);
                        acdDetails.TotPayAmt = "$" + Convert.ToString(conv3);
                        Decimal conv4 = Math.Round(acdDetails.ACD_TotalAdj, 2);
                        acdDetails.TotAdjAmt = "$" + Convert.ToString(conv4);
                        Decimal conv5 = Math.Round(acdDetails.ACD_AmtDiffNAA, 2);
                        acdDetails.convAllAmtDiff = "$" + Convert.ToString(conv5);
                        Decimal conv6 = Math.Round(acdDetails.ACD_AmtDiffPayor, 2);
                        acdDetails.convAllAmtDiffPyr = "$" + Convert.ToString(conv6);
                        Decimal conv7 = Math.Round(acdDetails.ACD_ExpAmt, 2);
                        acdDetails.convExpAmtDiff = "$" + Convert.ToString(conv7);

                        if (linkName == "underPay")
                        {
                            acdDetails.link = 1;
                        }
                        else
                        {
                            acdDetails.link = 2;
                        }
                        //acdDetails.userName = underPaymentsListByID[0].Biller;
                        //if (!string.IsNullOrEmpty(underPaymentsListByID[0].Billing_Provider))
                        //if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                        //{
                        //    acdDetails.userName = underPaymentsListByID[0].Biller;
                        //    acdDetails.userFlag = 1;
                        //}
                        //else
                        //{
                        //    acdDetails.userFlag = 2;
                        //}
                        acdDetails.userFlag = 2;
                        acdDetails.userID = acdDetails.ACD_Owner;

                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Comments = underPaymentsListByID[0].Brief_Summary;
                        }


                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Status = 5;
                        }


                        //CASE DETAILS HISTORY
                        ACDH = getCaseDetailsHistoryList(HospitalAccountID);
                        if (ACDH.Count > 0)
                        {
                            for (int j = 0; j < ACDH.Count; j++)
                            {
                                ACDH[j].ACDH_Status = ACDH[j].ACDH_Status;
                                ACDH[j].ACDH_PayerReason = ACDH[j].ACDH_PayerReason;
                                Double amt = Convert.ToDouble(ACDH[j].ACDH_Amount);
                                amt = Math.Round(amt, 2);
                                ACDH[j].ACDH_Amount = "$" + Convert.ToString(amt);
                                ACDH[j].ACDH_Owner = get_Owner_dropDownText(ACDH[j].ACDH_Owner);
                                ACDH[j].convertDate = ACDH[j].ACDH_CreatedDate.ToShortDateString();
                                ACDH[j].ACDH_Priority = getPrioritydropDownText(Convert.ToInt32(ACDH[j].ACDH_Priority));
                                ACDH[j].convertFollowUpDate = ACDH[j].ACDH_FollowUpDate.Value.ToShortDateString();
                                ACDH[j].ACDH_TaskFollowUp = ACDH[j].ACDH_TaskFollowUp;
                                Decimal amt1 = Math.Round(ACDH[j].ACDH_TotalCharges, 2);
                                ACDH[j].totalCharges = "$" + Convert.ToString(amt1);
                                Decimal amt2 = Math.Round(ACDH[j].ACDH_TotalPay, 2);
                                ACDH[j].totalPay = "$" + Convert.ToString(amt2);
                                Decimal amt3 = Math.Round(ACDH[j].ACDH_TotalAdj, 2);
                                ACDH[j].totalAdj = "$" + Convert.ToString(amt3);

                            }

                        }

                        List<Get_Under_Paymnent_Accounts_PB_Result> onePBResult = new List<Get_Under_Paymnent_Accounts_PB_Result>();
                        onePBResult.Add(new Get_Under_Paymnent_Accounts_PB_Result() { DEPARTMENTNAME = underPaymentsListByID[0].DEPARTMENTNAME, Plan_Name = underPaymentsListByID[0].Plan_Name, Payor_Name = underPaymentsListByID[0].Payor_Name, Diagnosis_Codes = underPaymentsListByID[0].Diagnosis_Codes });
                        ViewBag.underPaymentsLinkData = onePBResult;
                        ViewBag.caseDetailsHistory = ACDH;
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return View(acdDetails);
            }
        }

        [HttpGet]
        public ActionResult PBeditCaseDetails(string id, string linkName)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = id;

                openCaseID = id;
                string HospitalAccountID = openCaseID;
                int new_Case_Value = 0;
                Decimal testAmt1, testAmt2, testAmt3, testAmt4, AmtAllowedNAA, AmtAllowedPay, ExpAmt;
                string ownerId = string.Empty;


                TCG_WLM = new TCG_WorklistModel();
                Account_Case_Details acdDetails = new Account_Case_Details();
                Account_Case_Detials_History acdhDetails = new Account_Case_Detials_History();
                List<Account_Case_Details> ACD = new List<Account_Case_Details>();
                List<Account_Case_Detials_History> ACDH = new List<Account_Case_Detials_History>();

                List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();
                List<Get_Under_Paymnent_Accounts_PB_Result> underPaymentsListByID = getPBUnderPaymentsDetailsList(openCaseID);
                if (underPaymentsListByID.Count > 0)
                {
                    underPaymentsListByID[0].DischargeDate = underPaymentsListByID[0].Disch_Date.Value.ToShortDateString();
                    string[] cptcode;                    
                    if (underPaymentsListByID[0].CPTCODES.Contains(','))
                    {
                        cptcode = underPaymentsListByID[0].CPTCODES.Split(',');
                        List<string> key = new List<string>();
                        for (int i = 0; i < cptcode.Length; i++)
                        {
                            key.Add(cptcode[i]);
                        }
                        ViewBag.cptvalues = new SelectList(key);

                    }
                    else
                    {
                        ViewBag.cptvalues = new SelectList(underPaymentsListByID[0].CPTCODES);
                    }
                }

                if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                {
                    TCG_DataEntities tcguserdata = new TCG_DataEntities();
                    Users_Data tcglogin = new Users_Data();
                    tcglogin = tcguserdata.Users_Data.Where(m => m.user_first_name.ToString() == underPaymentsListByID[0].Biller).FirstOrDefault();
                    ownerId = tcglogin.user_ID.ToString();
                }
                else
                {
                    ownerId = get_Owner_dropDownValue("Select Owner");
                }


                try
                {
                    using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                    {
                        taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                        if (taskDetails.Count > 0 && taskDetails != null)
                        {

                            //if (taskDetails[0].Total_Charge_Amount.HasValue)
                            if (underPaymentsListByID[0].TOTALCHARGES.HasValue)
                            {
                                //testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                                testAmt1 = Convert.ToDecimal(underPaymentsListByID[0].TOTALCHARGES);
                                testAmt1 = Math.Round(testAmt1, 2);
                                taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                            }
                            else
                            {
                                testAmt1 = 0;
                                taskDetails[0].convTotChrgAmt = "$0.00";
                            }


                            if (underPaymentsListByID[0].TOTALPAYMENTS.HasValue)
                            {
                                testAmt2 = Convert.ToDecimal(underPaymentsListByID[0].TOTALPAYMENTS);
                                testAmt2 = Math.Round(testAmt2, 2);
                                taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotPayAmt = "$0.00";
                                testAmt2 = 0;
                            }


                            if (underPaymentsListByID[0].TOTALADJUSTMENTS.HasValue)
                            {
                                testAmt3 = Convert.ToDecimal(underPaymentsListByID[0].TOTALADJUSTMENTS);
                                testAmt3 = Math.Round(testAmt3, 2);
                                taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                            }
                            else
                            {
                                taskDetails[0].convAdjAmt = "$0.00";
                                testAmt3 = 0;
                            }


                            if (taskDetails[0].Total_Account_Balance.HasValue)
                            {
                                testAmt4 = Convert.ToDecimal(taskDetails[0].Total_Account_Balance);
                                testAmt4 = Math.Round(testAmt4, 2);
                                taskDetails[0].convTotAccBal = "$" + testAmt4.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotAccBal = "$0.00";
                                testAmt4 = 0;
                            }


                            if (underPaymentsListByID[0].Allowed_Amount_Difference != null || underPaymentsListByID[0].Allowed_Amount_Difference != 0)
                            {
                                AmtAllowedNAA = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference);
                                AmtAllowedNAA = Math.Round(AmtAllowedNAA, 2);
                                acdDetails.convAllAmtDiff = "$" + AmtAllowedNAA.ToString();
                            }
                            else
                            {
                                acdDetails.convAllAmtDiff = "$" + "0.00";
                                AmtAllowedNAA = 0;
                            }

                            if (underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != null || underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != 0)
                            {
                                AmtAllowedPay = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified);
                                AmtAllowedPay = Math.Round(AmtAllowedPay, 2);
                                acdDetails.convAllAmtDiffPyr = "$" + AmtAllowedPay.ToString();
                            }
                            else
                            {
                                AmtAllowedPay = 0;
                                acdDetails.convAllAmtDiffPyr = "$" + "0.00";
                            }


                            if (underPaymentsListByID[0].Expected_Allowed_Amount != null || Convert.ToInt32(underPaymentsListByID[0].Expected_Allowed_Amount) != 0)
                            {
                                ExpAmt = Convert.ToDecimal(underPaymentsListByID[0].Expected_Allowed_Amount);
                                ExpAmt = Math.Round(ExpAmt, 2);
                                acdDetails.convExpAmtDiff = "$" + ExpAmt.ToString();
                            }
                            else
                            {
                                acdDetails.convExpAmtDiff = "$" + "0.00";
                                ExpAmt = 0;
                            }


                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

                            if (taskDetails[0].Admission_Date.HasValue)
                                taskDetails[0].convAdmDate = taskDetails[0].Admission_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convAdmDate = " ";


                            if (taskDetails[0].Discharge_Date.HasValue)
                                taskDetails[0].convDischDate = taskDetails[0].Discharge_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convDischDate = " ";


                            if (taskDetails[0].First_Billed_Date.HasValue)
                                taskDetails[0].convFirstBillDate = taskDetails[0].First_Billed_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convFirstBillDate = " ";


                            if (!string.IsNullOrEmpty(taskDetails[0].Last_Payor_Rcvd_Dt))
                            {
                                DateTime testDate = Convert.ToDateTime(taskDetails[0].Last_Payor_Rcvd_Dt);
                                taskDetails[0].convLastPayDate = testDate.ToShortDateString();
                            }
                            else
                                taskDetails[0].convLastPayDate = " ";


                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();
                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type;
                                    if (underPaymentsListByID[0].Acct_Status != null)
                                    {
                                        string test = Convert.ToString(underPaymentsListByID[0].Acct_Status);
                                        if (test == "New" || test == "Zero Balance" || test == "Credit Balance")
                                        {
                                            test = "Select";
                                        }
                                        billStatus_type = get_BillStatus_dropDownValue(test.ToString());
                                    }
                                    else
                                        billStatus_type = 5;

                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }

                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "2",
                                       strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(), underPay, " ", "22",
                                       "49", "49", Cmnt, "2", "1", "", 40,
                                        underPaymentsListByID[0].Disch_Date, DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                                else if (linkName == "Other")
                                {
                                    int billStatus_type;
                                    if (string.IsNullOrEmpty(taskDetails[0].Account_Bill_Status))
                                        billStatus_type = 5;
                                    else
                                        billStatus_type = get_BillStatus_dropDownValue(taskDetails[0].Account_Bill_Status);

                                    int accClass_subTpe;
                                    if (string.IsNullOrEmpty(taskDetails[0].Acct_Class))
                                        accClass_subTpe = 11;
                                    else
                                        accClass_subTpe = getAccClassDDLValue(taskDetails[0].Acct_Class); //Acct_Class -Denial , Account_Source - AR => Account_Source[Table in DB]

                                    int rootCause = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Root_Cause))
                                        rootCause = 1;
                                    else
                                        rootCause = getRootCauseDDLValue(taskDetails[0].Root_Cause);

                                    int denialReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Denial_Cat))
                                        denialReason = 1;
                                    else
                                        denialReason = getDenialReason(taskDetails[0].Denial_Cat);

                                    int denialStatusReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Status))
                                        denialStatusReason = 1;
                                    else
                                        denialStatusReason = getDenialStatusReason(taskDetails[0].Status);


                                    DateTime date;
                                    if (string.IsNullOrEmpty(taskDetails[0].Rcvd_Dt))
                                        date = DateTime.Now;
                                    else
                                        date = DateTime.Parse(taskDetails[0].Rcvd_Dt);

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    TCG_WLM.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "2", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                       rootCause, denialStatusReason.ToString(), denialReason.ToString(), "49", "49", "",
                                       "2", "1", "", 40, date,
                                       DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                       "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 2;


                                }
                            }

                        }

                        else if (taskDetails.Count == 0 || taskDetails == null)
                        {

                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


                            Decimal testAmount = Convert.ToDecimal(underPaymentsListByID[0].Acct_Bal);
                            testAmount = Math.Round(testAmount, 2);

                            taskDetails.Add(new Get_Account_Info_for_ARandDenial_Result() { Hospital_Account_ID = Convert.ToString(underPaymentsListByID[0].Account), Account_Patient_Name = underPaymentsListByID[0].Account_Name, convAdmDate = "", Encounter_Type = "", Account_Financial_Class = "", convFirstBillDate = "", convLastPayDate = "", convTotChrgAmt = "$0.00", convTotPayAmt = "$0.00", convAdjAmt = "$0.00", convTotAccBal = "$" + testAmount.ToString() });


                            //testAmt1 = 0;
                            //taskDetails[0].convTotChrgAmt = "$0.00";

                            //taskDetails[0].convTotPayAmt = "$0.00";
                            //testAmt2 = 0;

                            //taskDetails[0].convAdjAmt = "$0.00";
                            //testAmt3 = 0;

                            //if (taskDetails[0].Total_Charge_Amount.HasValue)
                            if (underPaymentsListByID[0].TOTALCHARGES.HasValue)
                            {
                                //testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                                testAmt1 = Convert.ToDecimal(underPaymentsListByID[0].TOTALCHARGES);
                                testAmt1 = Math.Round(testAmt1, 2);
                                taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                            }
                            else
                            {
                                testAmt1 = 0;
                                taskDetails[0].convTotChrgAmt = "$0.00";
                            }


                            if (underPaymentsListByID[0].TOTALPAYMENTS.HasValue)
                            {
                                testAmt2 = Convert.ToDecimal(underPaymentsListByID[0].TOTALPAYMENTS);
                                testAmt2 = Math.Round(testAmt2, 2);
                                taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotPayAmt = "$0.00";
                                testAmt2 = 0;
                            }


                            if (underPaymentsListByID[0].TOTALADJUSTMENTS.HasValue)
                            {
                                testAmt3 = Convert.ToDecimal(underPaymentsListByID[0].TOTALADJUSTMENTS);
                                testAmt3 = Math.Round(testAmt3, 2);
                                taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                            }
                            else
                            {
                                taskDetails[0].convAdjAmt = "$0.00";
                                testAmt3 = 0;
                            }


                            taskDetails[0].convTotAccBal = "$0.00";
                            testAmt4 = 0;

                            acdDetails.convAllAmtDiff = "$" + "0.00";
                            AmtAllowedNAA = 0;

                            AmtAllowedPay = 0;
                            acdDetails.convAllAmtDiffPyr = "$" + "0.00";

                            acdDetails.convExpAmtDiff = "$" + "0.00";
                            ExpAmt = 0;

                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();

                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type;
                                    if (underPaymentsListByID[0].Acct_Status != null)
                                    {
                                        string test = Convert.ToString(underPaymentsListByID[0].Acct_Status);
                                        if(test == "New" || test == "Zero Balance" || test == "Credit Balance")
                                        {
                                            test = "Select";
                                        }
                                        billStatus_type = get_BillStatus_dropDownValue(test.ToString());
                                    }
                                    else
                                        billStatus_type = 5;

                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }
                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }
                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                                    //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                                    //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                                    //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "2", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                        underPay, " ", "22", "49", "49", Cmnt,
                                        "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                        DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                            }
                        }


                        TCG_WorklistModel context = new TCG_WorklistModel();
                        TCG_DataEntities context_tcg = new TCG_DataEntities();


                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                        ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                        ViewBag.OriginalData = taskDetails;
                        ACD = get_CaseDetailsList(HospitalAccountID);
                        ACD[0].dateConvert = ACD[0].ACD_DueDate.Value.ToShortDateString();

                        //CASE DETAILS
                        acdDetails = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                        acdDetails.dateConvert = acdDetails.ACD_DueDate.Value.ToShortDateString();
                        acdDetails.dateFollowUp = acdDetails.ACD_FollowUpDate.Value.ToShortDateString();
                        Decimal AmtConvert = Convert.ToDecimal(acdDetails.ACD_Amount);
                        acdDetails.convertAmount = Math.Round(AmtConvert, 2);
                        acdDetails.convAmt = acdDetails.convertAmount.ToString();
                        Decimal conv1 = Convert.ToDecimal(acdDetails.ACD_Amount);
                        conv1 = Math.Round(conv1, 2);
                        acdDetails.TotAccBal = "$" + Convert.ToString(conv1);
                        Decimal conv2 = Math.Round(acdDetails.ACD_TotalCharges, 2);
                        acdDetails.TotChrgAmt = "$" + Convert.ToString(conv2);
                        Decimal conv3 = Math.Round(acdDetails.ACD_TotalPay, 2);
                        acdDetails.TotPayAmt = "$" + Convert.ToString(conv3);
                        Decimal conv4 = Math.Round(acdDetails.ACD_TotalAdj, 2);
                        acdDetails.TotAdjAmt = "$" + Convert.ToString(conv4);
                        Decimal conv5 = Math.Round(acdDetails.ACD_AmtDiffNAA, 2);
                        acdDetails.convAllAmtDiff = "$" + Convert.ToString(conv5);
                        Decimal conv6 = Math.Round(acdDetails.ACD_AmtDiffPayor, 2);
                        acdDetails.convAllAmtDiffPyr = "$" + Convert.ToString(conv6);
                        Decimal conv7 = Math.Round(acdDetails.ACD_ExpAmt, 2);
                        acdDetails.convExpAmtDiff = "$" + Convert.ToString(conv7);

                        if (linkName == "underPay")
                        {
                            acdDetails.link = 1;
                        }
                        else
                        {
                            acdDetails.link = 2;
                        }
                        //acdDetails.userName = underPaymentsListByID[0].Biller;
                        //if (!string.IsNullOrEmpty(underPaymentsListByID[0].Billing_Provider))
                        //if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                        //{
                        //    acdDetails.userName = underPaymentsListByID[0].Biller;
                        //    acdDetails.userFlag = 1;
                        //}
                        //else
                        //{
                            acdDetails.userFlag = 2;
                        //}
                        acdDetails.userID = acdDetails.ACD_Owner;

                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Comments = underPaymentsListByID[0].Brief_Summary;
                        }


                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Status = 5;
                        }


                        //CASE DETAILS HISTORY
                        ACDH = getCaseDetailsHistoryList(HospitalAccountID);
                        if (ACDH.Count > 0)
                        {
                            for (int j = 0; j < ACDH.Count; j++)
                            {
                                ACDH[j].ACDH_Status = ACDH[j].ACDH_Status;
                                ACDH[j].ACDH_PayerReason = ACDH[j].ACDH_PayerReason;
                                Double amt = Convert.ToDouble(ACDH[j].ACDH_Amount);
                                amt = Math.Round(amt, 2);
                                ACDH[j].ACDH_Amount = "$" + Convert.ToString(amt);
                                ACDH[j].ACDH_Owner = get_Owner_dropDownText(ACDH[j].ACDH_Owner);
                                ACDH[j].convertDate = ACDH[j].ACDH_CreatedDate.ToShortDateString();
                                ACDH[j].ACDH_Priority = getPrioritydropDownText(Convert.ToInt32(ACDH[j].ACDH_Priority));
                                ACDH[j].convertFollowUpDate = ACDH[j].ACDH_FollowUpDate.Value.ToShortDateString();
                                ACDH[j].ACDH_TaskFollowUp = ACDH[j].ACDH_TaskFollowUp;
                                Decimal amt1 = Math.Round(ACDH[j].ACDH_TotalCharges, 2);
                                ACDH[j].totalCharges = "$" + Convert.ToString(amt1);
                                Decimal amt2 = Math.Round(ACDH[j].ACDH_TotalPay, 2);
                                ACDH[j].totalPay = "$" + Convert.ToString(amt2);
                                Decimal amt3 = Math.Round(ACDH[j].ACDH_TotalAdj, 2);
                                ACDH[j].totalAdj = "$" + Convert.ToString(amt3);

                            }

                        }

                        List<Get_Under_Paymnent_Accounts_PB_Result> onePBResult = new List<Get_Under_Paymnent_Accounts_PB_Result>();
                        onePBResult.Add(new Get_Under_Paymnent_Accounts_PB_Result() { DEPARTMENTNAME = underPaymentsListByID[0].DEPARTMENTNAME, Plan_Name = underPaymentsListByID[0].Plan_Name, Payor_Name = underPaymentsListByID[0].Payor_Name, Diagnosis_Codes = underPaymentsListByID[0].Diagnosis_Codes });
                        ViewBag.underPaymentsLinkData = onePBResult;
                        ViewBag.caseDetailsHistory = ACDH;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return View(acdDetails);
            }
        }

        [HttpPost]
        public ActionResult PBeditCaseDetails(Account_Case_Details ACD)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = ACD.ACD_HspAccID;

                openCaseID = ACD.ACD_HspAccID;
                string HospitalAccountID = openCaseID;

                TCG_WLM = new TCG_WorklistModel();
                string linkName = string.Empty;
                if (ACD.link == 1)
                {
                    linkName = "underPay";
                }
                else
                {
                    linkName = "Other";
                }

                try
                {
                    using (TCG_WorklistModel tcg_CaseDetails = new TCG_WorklistModel())
                    {
                        var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));

                        string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                        string[] test = ACD.ACD_Amount.Split('$');
                        ACD.ACD_Amount = test[1];
                        string[] test2 = ACD.TotChrgAmt.Split('$');
                        ACD.ACD_TotalCharges = Convert.ToDecimal(test2[1]);
                        string[] test3 = ACD.TotPayAmt.Split('$');
                        ACD.ACD_TotalPay = Convert.ToDecimal(test3[1]);
                        string[] test4 = ACD.TotAdjAmt.Split('$');
                        ACD.ACD_TotalAdj = Convert.ToDecimal(test4[1]);


                        //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                        //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                        //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                        //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                        //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                        //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                        string desc = string.Empty;
                        if (string.IsNullOrEmpty(ACD.ACD_Description))
                        { desc = " "; }
                        else
                        { desc = ACD.ACD_Description; }
                        TCG_WLM.Case_InsUpd(ACD.ACD_ID, ACD.ACD_HspAccID, ACD.ACD_Amount, ACD.ACD_TotalCharges, ACD.ACD_TotalPay, ACD.ACD_TotalAdj,
                            ACD.ACD_AmtDiffNAA, ACD.ACD_AmtDiffPayor, ACD.ACD_ExpAmt, "1", "1", "2",
                            ACD.ACD_Status, ACD.ACD_Owner, ACD.ACD_Type, ACD.ACD_SubType,
                            ACD.ACD_PayerReason, ACD.ACD_PrimaryReason, "22", "49", "49", ACD.ACD_Comments,
                            "2", ACD.ACD_Priority, desc, ACD.ACD_TaskFollowUp, ACD.ACD_DueDate,
                            ACD.ACD_FollowUpDate, false, Session["username"].ToString(), DateTime.Now,
                            "", DateTime.Now, "", case_idParameter);


                        //new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return RedirectToAction("PBviewCaseDetails", new { id = ACD.ACD_HspAccID, linkName });
            }
        }

        [HttpGet]
        public ActionResult APDViewCaseDetails(string id, string linkName)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = id;

                openCaseID = id;
                string HospitalAccountID = openCaseID;
                int new_Case_Value = 0;
                Decimal testAmt1, testAmt2, testAmt3, testAmt4, AmtAllowedNAA, AmtAllowedPay, ExpAmt;
                string ownerId = string.Empty;


                TCG_WLM = new TCG_WorklistModel();
                Account_Case_Details acdDetails = new Account_Case_Details();
                Account_Case_Detials_History acdhDetails = new Account_Case_Detials_History();
                List<Account_Case_Details> ACD = new List<Account_Case_Details>();
                List<Account_Case_Detials_History> ACDH = new List<Account_Case_Detials_History>();

                List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();
                List<Get_Under_Paymnent_Accounts_APD_Result> underPaymentsListByID = getAPDUnderPaymentsDetailsList(openCaseID);
                if (underPaymentsListByID.Count > 0)
                {
                    underPaymentsListByID[0].DischargeDate = underPaymentsListByID[0].Disch_Date.Value.ToShortDateString();
                }


                if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                {
                    TCG_DataEntities tcguserdata = new TCG_DataEntities();
                    Users_Data tcglogin = new Users_Data();
                    tcglogin = tcguserdata.Users_Data.Where(m => m.user_first_name.ToString() == underPaymentsListByID[0].Biller).FirstOrDefault();
                    ownerId = tcglogin.user_ID.ToString();
                }
                else
                {
                    ownerId = get_Owner_dropDownValue("Select Owner");
                }

                try
                {
                    using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                    {
                        taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                        if (taskDetails.Count > 0 && taskDetails != null)
                        {
                            if (taskDetails[0].Total_Charge_Amount.HasValue)
                            {
                                testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                                testAmt1 = Math.Round(testAmt1, 2);
                                taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                            }
                            else
                            {
                                testAmt1 = 0;
                                taskDetails[0].convTotChrgAmt = "$0.00";
                            }


                            if (taskDetails[0].Total_Payment_Amount.HasValue)
                            {
                                testAmt2 = Convert.ToDecimal(taskDetails[0].Total_Payment_Amount);
                                testAmt2 = Math.Round(testAmt2, 2);
                                taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotPayAmt = "$0.00";
                                testAmt2 = 0;
                            }


                            if (taskDetails[0].Total_Adjustment_Amount.HasValue)
                            {
                                testAmt3 = Convert.ToDecimal(taskDetails[0].Total_Adjustment_Amount);
                                testAmt3 = Math.Round(testAmt3, 2);
                                taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                            }
                            else
                            {
                                taskDetails[0].convAdjAmt = "$0.00";
                                testAmt3 = 0;
                            }


                            if (taskDetails[0].Total_Account_Balance.HasValue)
                            {
                                testAmt4 = Convert.ToDecimal(taskDetails[0].Total_Account_Balance);
                                testAmt4 = Math.Round(testAmt4, 2);
                                taskDetails[0].convTotAccBal = "$" + testAmt4.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotAccBal = "$0.00";
                                testAmt4 = 0;
                            }


                            if (underPaymentsListByID[0].Allowed_Amount_Difference != null || underPaymentsListByID[0].Allowed_Amount_Difference != 0)
                            {
                                AmtAllowedNAA = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference);
                                AmtAllowedNAA = Math.Round(AmtAllowedNAA, 2);
                                acdDetails.convAllAmtDiff = "$" + AmtAllowedNAA.ToString();
                            }
                            else
                            {
                                acdDetails.convAllAmtDiff = "$" + "0.00";
                                AmtAllowedNAA = 0;
                            }

                            if (underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != null || underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != 0)
                            {
                                AmtAllowedPay = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified);
                                AmtAllowedPay = Math.Round(AmtAllowedPay, 2);
                                acdDetails.convAllAmtDiffPyr = "$" + AmtAllowedPay.ToString();
                            }
                            else
                            {
                                AmtAllowedPay = 0;
                                acdDetails.convAllAmtDiffPyr = "$" + "0.00";
                            }


                            if (!string.IsNullOrEmpty(underPaymentsListByID[0].Expected_Allowed_Amount) || underPaymentsListByID[0].Expected_Allowed_Amount != "0")
                            {
                                ExpAmt = Convert.ToDecimal(underPaymentsListByID[0].Expected_Allowed_Amount);
                                ExpAmt = Math.Round(ExpAmt, 2);
                                acdDetails.convExpAmtDiff = "$" + ExpAmt.ToString();
                            }
                            else
                            {
                                acdDetails.convExpAmtDiff = "$" + "0.00";
                                ExpAmt = 0;
                            }


                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

                            if (taskDetails[0].Admission_Date.HasValue)
                                taskDetails[0].convAdmDate = taskDetails[0].Admission_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convAdmDate = " ";


                            if (taskDetails[0].Discharge_Date.HasValue)
                                taskDetails[0].convDischDate = taskDetails[0].Discharge_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convDischDate = " ";


                            if (taskDetails[0].First_Billed_Date.HasValue)
                                taskDetails[0].convFirstBillDate = taskDetails[0].First_Billed_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convFirstBillDate = " ";


                            if (!string.IsNullOrEmpty(taskDetails[0].Last_Payor_Rcvd_Dt))
                            {
                                DateTime testDate = Convert.ToDateTime(taskDetails[0].Last_Payor_Rcvd_Dt);
                                taskDetails[0].convLastPayDate = testDate.ToShortDateString();
                            }
                            else
                                taskDetails[0].convLastPayDate = " ";


                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();
                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());

                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }

                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "3",
                                       strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(), underPay, " ", "22",
                                       "49", "49", Cmnt, "2", "1", "", 40,
                                        underPaymentsListByID[0].Disch_Date, DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                                else if (linkName == "Other")
                                {
                                    int billStatus_type;
                                    if (string.IsNullOrEmpty(taskDetails[0].Account_Bill_Status))
                                        billStatus_type = 5;
                                    else
                                        billStatus_type = get_BillStatus_dropDownValue(taskDetails[0].Account_Bill_Status);

                                    int accClass_subTpe;
                                    if (string.IsNullOrEmpty(taskDetails[0].Acct_Class))
                                        accClass_subTpe = 11;
                                    else
                                        accClass_subTpe = getAccClassDDLValue(taskDetails[0].Acct_Class); //Acct_Class -Denial , Account_Source - AR => Account_Source[Table in DB]

                                    int rootCause = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Root_Cause))
                                        rootCause = 1;
                                    else
                                        rootCause = getRootCauseDDLValue(taskDetails[0].Root_Cause);

                                    int denialReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Denial_Cat))
                                        denialReason = 1;
                                    else
                                        denialReason = getDenialReason(taskDetails[0].Denial_Cat);

                                    int denialStatusReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Status))
                                        denialStatusReason = 1;
                                    else
                                        denialStatusReason = getDenialStatusReason(taskDetails[0].Status);


                                    DateTime date;
                                    if (string.IsNullOrEmpty(taskDetails[0].Rcvd_Dt))
                                        date = DateTime.Now;
                                    else
                                        date = DateTime.Parse(taskDetails[0].Rcvd_Dt);

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    TCG_WLM.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "1", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                       rootCause, denialStatusReason.ToString(), denialReason.ToString(), "49", "49", "",
                                       "2", "1", "", 40, date,
                                       DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                       "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 2;


                                }
                            }

                        }

                        else if (taskDetails.Count == 0 || taskDetails == null)
                        {

                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


                            Decimal testAmount = Convert.ToDecimal(underPaymentsListByID[0].Acct_Bal);
                            testAmount = Math.Round(testAmount, 2);

                            taskDetails.Add(new Get_Account_Info_for_ARandDenial_Result() { Hospital_Account_ID = Convert.ToString(underPaymentsListByID[0].Account), Account_Patient_Name = underPaymentsListByID[0].Account_Name, convAdmDate = "", Encounter_Type = "", Account_Financial_Class = "", convFirstBillDate = "", convLastPayDate = "", convTotChrgAmt = "$0.00", convTotPayAmt = "$0.00", convAdjAmt = "$0.00", convTotAccBal = "$" + testAmount.ToString() });

                            testAmt1 = 0;
                            taskDetails[0].convTotChrgAmt = "$0.00";

                            taskDetails[0].convTotPayAmt = "$0.00";
                            testAmt2 = 0;

                            taskDetails[0].convAdjAmt = "$0.00";
                            testAmt3 = 0;

                            taskDetails[0].convTotAccBal = "$0.00";
                            testAmt4 = 0;

                            acdDetails.convAllAmtDiff = "$" + "0.00";
                            AmtAllowedNAA = 0;

                            AmtAllowedPay = 0;
                            acdDetails.convAllAmtDiffPyr = "$" + "0.00";

                            acdDetails.convExpAmtDiff = "$" + "0.00";
                            ExpAmt = 0;

                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();

                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }

                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }
                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "3",
                                       strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),  underPay, 
                                       " ", "22", "49", "49", Cmnt, "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                        DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                            }
                        }


                        TCG_WorklistModel context = new TCG_WorklistModel();
                        TCG_DataEntities context_tcg = new TCG_DataEntities();


                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                        ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                        ViewBag.OriginalData = taskDetails;
                        ACD = get_CaseDetailsList(HospitalAccountID);
                        ACD[0].dateConvert = ACD[0].ACD_DueDate.Value.ToShortDateString();

                        //CASE DETAILS
                        acdDetails = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                        acdDetails.dateConvert = acdDetails.ACD_DueDate.Value.ToShortDateString();
                        acdDetails.dateFollowUp = acdDetails.ACD_FollowUpDate.Value.ToShortDateString();
                        Decimal AmtConvert = Convert.ToDecimal(acdDetails.ACD_Amount);
                        acdDetails.convertAmount = Math.Round(AmtConvert, 2);
                        acdDetails.convAmt = acdDetails.convertAmount.ToString();
                        Decimal conv1 = Convert.ToDecimal(acdDetails.ACD_Amount);
                        conv1 = Math.Round(conv1, 2);
                        acdDetails.TotAccBal = "$" + Convert.ToString(conv1);
                        Decimal conv2 = Math.Round(acdDetails.ACD_TotalCharges, 2);
                        acdDetails.TotChrgAmt = "$" + Convert.ToString(conv2);
                        Decimal conv3 = Math.Round(acdDetails.ACD_TotalPay, 2);
                        acdDetails.TotPayAmt = "$" + Convert.ToString(conv3);
                        Decimal conv4 = Math.Round(acdDetails.ACD_TotalAdj, 2);
                        acdDetails.TotAdjAmt = "$" + Convert.ToString(conv4);
                        Decimal conv5 = Math.Round(acdDetails.ACD_AmtDiffNAA, 2);
                        acdDetails.convAllAmtDiff = "$" + Convert.ToString(conv5);
                        Decimal conv6 = Math.Round(acdDetails.ACD_AmtDiffPayor, 2);
                        acdDetails.convAllAmtDiffPyr = "$" + Convert.ToString(conv6);
                        Decimal conv7 = Math.Round(acdDetails.ACD_ExpAmt, 2);
                        acdDetails.convExpAmtDiff = "$" + Convert.ToString(conv7);

                        if (linkName == "underPay")
                        {
                            acdDetails.link = 1;
                        }
                        else
                        {
                            acdDetails.link = 2;
                        }
                        //acdDetails.userName = underPaymentsListByID[0].Biller;
                        //if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                        //{
                        //    acdDetails.userName = underPaymentsListByID[0].Biller;
                        //    acdDetails.userFlag = 1;
                        //}
                        //else
                        //{
                        //    acdDetails.userFlag = 2;
                        //}
                        acdDetails.userFlag = 2;
                        acdDetails.userID = acdDetails.ACD_Owner;

                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Comments = underPaymentsListByID[0].Brief_Summary;
                        }


                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Status = 5;
                        }


                        //CASE DETAILS HISTORY
                        ACDH = getCaseDetailsHistoryList(HospitalAccountID);
                        if (ACDH.Count > 0)
                        {
                            for (int j = 0; j < ACDH.Count; j++)
                            {
                                ACDH[j].ACDH_Status = ACDH[j].ACDH_Status;
                                ACDH[j].ACDH_PayerReason = ACDH[j].ACDH_PayerReason;
                                Double amt = Convert.ToDouble(ACDH[j].ACDH_Amount);
                                amt = Math.Round(amt, 2);
                                ACDH[j].ACDH_Amount = "$" + Convert.ToString(amt);
                                ACDH[j].ACDH_Owner = get_Owner_dropDownText(ACDH[j].ACDH_Owner);
                                ACDH[j].convertDate = ACDH[j].ACDH_CreatedDate.ToShortDateString();
                                ACDH[j].ACDH_Priority = getPrioritydropDownText(Convert.ToInt32(ACDH[j].ACDH_Priority));
                                ACDH[j].convertFollowUpDate = ACDH[j].ACDH_FollowUpDate.Value.ToShortDateString();
                                ACDH[j].ACDH_TaskFollowUp = ACDH[j].ACDH_TaskFollowUp;
                                Decimal amt1 = Math.Round(ACDH[j].ACDH_TotalCharges, 2);
                                ACDH[j].totalCharges = "$" + Convert.ToString(amt1);
                                Decimal amt2 = Math.Round(ACDH[j].ACDH_TotalPay, 2);
                                ACDH[j].totalPay = "$" + Convert.ToString(amt2);
                                Decimal amt3 = Math.Round(ACDH[j].ACDH_TotalAdj, 2);
                                ACDH[j].totalAdj = "$" + Convert.ToString(amt3);

                            }

                        }

                        ViewBag.underPaymentsLinkData = underPaymentsListByID;
                        ViewBag.caseDetailsHistory = ACDH;
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return View(acdDetails);
            }
        }

        [HttpGet]
        public ActionResult APDEditCaseDetails(string id, string linkName)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = id;

                openCaseID = id;
                string HospitalAccountID = openCaseID;
                int new_Case_Value = 0;
                Decimal testAmt1, testAmt2, testAmt3, testAmt4, AmtAllowedNAA, AmtAllowedPay, ExpAmt;
                string ownerId = string.Empty;


                TCG_WLM = new TCG_WorklistModel();
                Account_Case_Details acdDetails = new Account_Case_Details();
                Account_Case_Detials_History acdhDetails = new Account_Case_Detials_History();
                List<Account_Case_Details> ACD = new List<Account_Case_Details>();
                List<Account_Case_Detials_History> ACDH = new List<Account_Case_Detials_History>();

                List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();
                List<Get_Under_Paymnent_Accounts_APD_Result> underPaymentsListByID = getAPDUnderPaymentsDetailsList(openCaseID);
                if (underPaymentsListByID.Count > 0)
                {
                    underPaymentsListByID[0].DischargeDate = underPaymentsListByID[0].Disch_Date.Value.ToShortDateString();
                }


                if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                {
                    TCG_DataEntities tcguserdata = new TCG_DataEntities();
                    Users_Data tcglogin = new Users_Data();
                    tcglogin = tcguserdata.Users_Data.Where(m => m.user_first_name.ToString() == underPaymentsListByID[0].Biller).FirstOrDefault();
                    ownerId = tcglogin.user_ID.ToString();
                }
                else
                {
                    ownerId = get_Owner_dropDownValue("Select Owner");
                }

                try
                {
                    using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                    {
                        taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                        if (taskDetails.Count > 0 && taskDetails != null)
                        {
                            if (taskDetails[0].Total_Charge_Amount.HasValue)
                            {
                                testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                                testAmt1 = Math.Round(testAmt1, 2);
                                taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                            }
                            else
                            {
                                testAmt1 = 0;
                                taskDetails[0].convTotChrgAmt = "$0.00";
                            }


                            if (taskDetails[0].Total_Payment_Amount.HasValue)
                            {
                                testAmt2 = Convert.ToDecimal(taskDetails[0].Total_Payment_Amount);
                                testAmt2 = Math.Round(testAmt2, 2);
                                taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotPayAmt = "$0.00";
                                testAmt2 = 0;
                            }


                            if (taskDetails[0].Total_Adjustment_Amount.HasValue)
                            {
                                testAmt3 = Convert.ToDecimal(taskDetails[0].Total_Adjustment_Amount);
                                testAmt3 = Math.Round(testAmt3, 2);
                                taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                            }
                            else
                            {
                                taskDetails[0].convAdjAmt = "$0.00";
                                testAmt3 = 0;
                            }


                            if (taskDetails[0].Total_Account_Balance.HasValue)
                            {
                                testAmt4 = Convert.ToDecimal(taskDetails[0].Total_Account_Balance);
                                testAmt4 = Math.Round(testAmt4, 2);
                                taskDetails[0].convTotAccBal = "$" + testAmt4.ToString();
                            }
                            else
                            {
                                taskDetails[0].convTotAccBal = "$0.00";
                                testAmt4 = 0;
                            }


                            if (underPaymentsListByID[0].Allowed_Amount_Difference != null || underPaymentsListByID[0].Allowed_Amount_Difference != 0)
                            {
                                AmtAllowedNAA = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference);
                                AmtAllowedNAA = Math.Round(AmtAllowedNAA, 2);
                                acdDetails.convAllAmtDiff = "$" + AmtAllowedNAA.ToString();
                            }
                            else
                            {
                                acdDetails.convAllAmtDiff = "$" + "0.00";
                                AmtAllowedNAA = 0;
                            }

                            if (underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != null || underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified != 0)
                            {
                                AmtAllowedPay = Convert.ToDecimal(underPaymentsListByID[0].Allowed_Amount_Difference_Payor_Specified);
                                AmtAllowedPay = Math.Round(AmtAllowedPay, 2);
                                acdDetails.convAllAmtDiffPyr = "$" + AmtAllowedPay.ToString();
                            }
                            else
                            {
                                AmtAllowedPay = 0;
                                acdDetails.convAllAmtDiffPyr = "$" + "0.00";
                            }


                            if (!string.IsNullOrEmpty(underPaymentsListByID[0].Expected_Allowed_Amount) || underPaymentsListByID[0].Expected_Allowed_Amount != "0")
                            {
                                ExpAmt = Convert.ToDecimal(underPaymentsListByID[0].Expected_Allowed_Amount);
                                ExpAmt = Math.Round(ExpAmt, 2);
                                acdDetails.convExpAmtDiff = "$" + ExpAmt.ToString();
                            }
                            else
                            {
                                acdDetails.convExpAmtDiff = "$" + "0.00";
                                ExpAmt = 0;
                            }


                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

                            if (taskDetails[0].Admission_Date.HasValue)
                                taskDetails[0].convAdmDate = taskDetails[0].Admission_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convAdmDate = " ";


                            if (taskDetails[0].Discharge_Date.HasValue)
                                taskDetails[0].convDischDate = taskDetails[0].Discharge_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convDischDate = " ";


                            if (taskDetails[0].First_Billed_Date.HasValue)
                                taskDetails[0].convFirstBillDate = taskDetails[0].First_Billed_Date.Value.ToShortDateString();
                            else
                                taskDetails[0].convFirstBillDate = " ";


                            if (!string.IsNullOrEmpty(taskDetails[0].Last_Payor_Rcvd_Dt))
                            {
                                DateTime testDate = Convert.ToDateTime(taskDetails[0].Last_Payor_Rcvd_Dt);
                                taskDetails[0].convLastPayDate = testDate.ToShortDateString();
                            }
                            else
                                taskDetails[0].convLastPayDate = " ";


                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();
                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            //string ownerId = get_Owner_dropDownValue(Session["username"].ToString());

                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }

                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "3",
                                       strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(), underPay, " ", "22",
                                       "49", "49", Cmnt, "2", "1", "", 40,
                                        underPaymentsListByID[0].Disch_Date, DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                                else if (linkName == "Other")
                                {
                                    int billStatus_type;
                                    if (string.IsNullOrEmpty(taskDetails[0].Account_Bill_Status))
                                        billStatus_type = 5;
                                    else
                                        billStatus_type = get_BillStatus_dropDownValue(taskDetails[0].Account_Bill_Status);

                                    int accClass_subTpe;
                                    if (string.IsNullOrEmpty(taskDetails[0].Acct_Class))
                                        accClass_subTpe = 11;
                                    else
                                        accClass_subTpe = getAccClassDDLValue(taskDetails[0].Acct_Class); //Acct_Class -Denial , Account_Source - AR => Account_Source[Table in DB]

                                    int rootCause = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Root_Cause))
                                        rootCause = 1;
                                    else
                                        rootCause = getRootCauseDDLValue(taskDetails[0].Root_Cause);

                                    int denialReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Denial_Cat))
                                        denialReason = 1;
                                    else
                                        denialReason = getDenialReason(taskDetails[0].Denial_Cat);

                                    int denialStatusReason = 0;
                                    if (string.IsNullOrEmpty(taskDetails[0].Status))
                                        denialStatusReason = 1;
                                    else
                                        denialStatusReason = getDenialStatusReason(taskDetails[0].Status);


                                    DateTime date;
                                    if (string.IsNullOrEmpty(taskDetails[0].Rcvd_Dt))
                                        date = DateTime.Now;
                                    else
                                        date = DateTime.Parse(taskDetails[0].Rcvd_Dt);

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    TCG_WLM.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "1", strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                       rootCause, denialStatusReason.ToString(), denialReason.ToString(), "49", "49", "",
                                       "2", "1", "", 40, date,
                                       DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                       "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 2;


                                }
                            }

                        }

                        else if (taskDetails.Count == 0 || taskDetails == null)
                        {

                            var Case_checkHospitalAccID = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


                            Decimal testAmount = Convert.ToDecimal(underPaymentsListByID[0].Acct_Bal);
                            testAmount = Math.Round(testAmount, 2);

                            taskDetails.Add(new Get_Account_Info_for_ARandDenial_Result() { Hospital_Account_ID = Convert.ToString(underPaymentsListByID[0].Account), Account_Patient_Name = underPaymentsListByID[0].Account_Name, convAdmDate = "", Encounter_Type = "", Account_Financial_Class = "", convFirstBillDate = "", convLastPayDate = "", convTotChrgAmt = "$0.00", convTotPayAmt = "$0.00", convAdjAmt = "$0.00", convTotAccBal = "$" + testAmount.ToString() });

                            testAmt1 = 0;
                            taskDetails[0].convTotChrgAmt = "$0.00";

                            taskDetails[0].convTotPayAmt = "$0.00";
                            testAmt2 = 0;

                            taskDetails[0].convAdjAmt = "$0.00";
                            testAmt3 = 0;

                            taskDetails[0].convTotAccBal = "$0.00";
                            testAmt4 = 0;

                            acdDetails.convAllAmtDiff = "$" + "0.00";
                            AmtAllowedNAA = 0;

                            AmtAllowedPay = 0;
                            acdDetails.convAllAmtDiffPyr = "$" + "0.00";

                            acdDetails.convExpAmtDiff = "$" + "0.00";
                            ExpAmt = 0;

                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_WorklistModel();

                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }

                            if (Case_checkHospitalAccID == null)
                            {
                                if (linkName == "underPay")
                                {
                                    int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                    int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                    int underPay;
                                    if (underPaymentsListByID[0].Underpayment_Reason_Code != null)
                                    {
                                        underPay = underPaymentsListByID[0].Underpayment_Reason_Code.Value;
                                    }
                                    else
                                    {
                                        underPay = 0;
                                    }
                                    string Cmnt = string.Empty;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                        Cmnt = underPaymentsListByID[0].Brief_Summary;
                                    else
                                        Cmnt = "";

                                    int strStatus = 0;
                                    if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                                    {
                                        strStatus = 5;
                                    }
                                    else { strStatus = 9; }

                                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                                    //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                                    //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                                    //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                                    //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                    TCG_WLM.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), testAmt1, testAmt2, testAmt3,
                                       AmtAllowedNAA, AmtAllowedPay, ExpAmt, "1", "1", "3",
                                       strStatus, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(), underPay,
                                       " ", "22", "49", "49", Cmnt, "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                        DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                        "", DateTime.Now, "", case_idParameter);


                                    new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                    acdDetails.link = 1;
                                }
                            }
                        }


                        TCG_WorklistModel context = new TCG_WorklistModel();
                        TCG_DataEntities context_tcg = new TCG_DataEntities();


                        ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                        ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                        ViewBag.OriginalData = taskDetails;
                        ACD = get_CaseDetailsList(HospitalAccountID);
                        ACD[0].dateConvert = ACD[0].ACD_DueDate.Value.ToShortDateString();

                        //CASE DETAILS
                        acdDetails = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                        acdDetails.dateConvert = acdDetails.ACD_DueDate.Value.ToShortDateString();
                        acdDetails.dateFollowUp = acdDetails.ACD_FollowUpDate.Value.ToShortDateString();
                        Decimal AmtConvert = Convert.ToDecimal(acdDetails.ACD_Amount);
                        acdDetails.convertAmount = Math.Round(AmtConvert, 2);
                        acdDetails.convAmt = acdDetails.convertAmount.ToString();
                        Decimal conv1 = Convert.ToDecimal(acdDetails.ACD_Amount);
                        conv1 = Math.Round(conv1, 2);
                        acdDetails.TotAccBal = "$" + Convert.ToString(conv1);
                        Decimal conv2 = Math.Round(acdDetails.ACD_TotalCharges, 2);
                        acdDetails.TotChrgAmt = "$" + Convert.ToString(conv2);
                        Decimal conv3 = Math.Round(acdDetails.ACD_TotalPay, 2);
                        acdDetails.TotPayAmt = "$" + Convert.ToString(conv3);
                        Decimal conv4 = Math.Round(acdDetails.ACD_TotalAdj, 2);
                        acdDetails.TotAdjAmt = "$" + Convert.ToString(conv4);
                        Decimal conv5 = Math.Round(acdDetails.ACD_AmtDiffNAA, 2);
                        acdDetails.convAllAmtDiff = "$" + Convert.ToString(conv5);
                        Decimal conv6 = Math.Round(acdDetails.ACD_AmtDiffPayor, 2);
                        acdDetails.convAllAmtDiffPyr = "$" + Convert.ToString(conv6);
                        Decimal conv7 = Math.Round(acdDetails.ACD_ExpAmt, 2);
                        acdDetails.convExpAmtDiff = "$" + Convert.ToString(conv7);

                        if (linkName == "underPay")
                        {
                            acdDetails.link = 1;
                        }
                        else
                        {
                            acdDetails.link = 2;
                        }
                        //acdDetails.userName = underPaymentsListByID[0].Biller;
                        //if (!string.IsNullOrEmpty(underPaymentsListByID[0].Biller))
                        //{
                        //    acdDetails.userName = underPaymentsListByID[0].Biller;
                        //    acdDetails.userFlag = 1;
                        //}
                        //else
                        //{
                        //    acdDetails.userFlag = 2;
                        //}
                        acdDetails.userFlag = 2;
                        acdDetails.userID = acdDetails.ACD_Owner;

                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Comments = underPaymentsListByID[0].Brief_Summary;
                        }


                        if (!string.IsNullOrEmpty(underPaymentsListByID[0].Brief_Summary))
                        {
                            acdDetails.ACD_Status = 5;
                        }


                        //CASE DETAILS HISTORY
                        ACDH = getCaseDetailsHistoryList(HospitalAccountID);
                        if (ACDH.Count > 0)
                        {
                            for (int j = 0; j < ACDH.Count; j++)
                            {
                                ACDH[j].ACDH_Status = ACDH[j].ACDH_Status;
                                ACDH[j].ACDH_PayerReason = ACDH[j].ACDH_PayerReason;
                                Double amt = Convert.ToDouble(ACDH[j].ACDH_Amount);
                                amt = Math.Round(amt, 2);
                                ACDH[j].ACDH_Amount = "$" + Convert.ToString(amt);
                                ACDH[j].ACDH_Owner = get_Owner_dropDownText(ACDH[j].ACDH_Owner);
                                ACDH[j].convertDate = ACDH[j].ACDH_CreatedDate.ToShortDateString();
                                ACDH[j].ACDH_Priority = getPrioritydropDownText(Convert.ToInt32(ACDH[j].ACDH_Priority));
                                ACDH[j].convertFollowUpDate = ACDH[j].ACDH_FollowUpDate.Value.ToShortDateString();
                                ACDH[j].ACDH_TaskFollowUp = ACDH[j].ACDH_TaskFollowUp;
                                Decimal amt1 = Math.Round(ACDH[j].ACDH_TotalCharges, 2);
                                ACDH[j].totalCharges = "$" + Convert.ToString(amt1);
                                Decimal amt2 = Math.Round(ACDH[j].ACDH_TotalPay, 2);
                                ACDH[j].totalPay = "$" + Convert.ToString(amt2);
                                Decimal amt3 = Math.Round(ACDH[j].ACDH_TotalAdj, 2);
                                ACDH[j].totalAdj = "$" + Convert.ToString(amt3);

                            }

                        }

                        ViewBag.underPaymentsLinkData = underPaymentsListByID;
                        ViewBag.caseDetailsHistory = ACDH;
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return View(acdDetails);
            }
        }

        [HttpPost]
        public ActionResult APDEditCaseDetails(Account_Case_Details ACD)
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.SM = populateStatus();
                ViewBag.AARS = populateAccount_ARStatus();
                ViewBag.ABS = populate_BillStatus();
                ViewBag.AS = populate_AccountSource();
                ViewBag.ECT = populate_EncounterType();
                ViewBag.ICN = populate_Insurance();
                ViewBag.PFC = populate_PayorFC();
                ViewBag.PRM = populate_PrimaryReason();
                ViewBag.TM = populate_Task();
                ViewBag.UL = populate_UserLogin();
                ViewBag.PM = populate_Priority();
                ViewBag.RC = populateRootCause();
                ViewBag.DC = populateDenialCategory();
                ViewBag.UP = populateUnderPayReason();
                ViewBag.CC = populateCompleted();
                ViewBag.DS = populateDenialStatusMaster();

                Session["AccountID"] = ACD.ACD_HspAccID;

                openCaseID = ACD.ACD_HspAccID;
                string HospitalAccountID = openCaseID;

                TCG_WLM = new TCG_WorklistModel();
                string linkName = string.Empty;
                if (ACD.link == 1)
                {
                    linkName = "underPay";
                }
                else
                {
                    linkName = "Other";
                }

                try
                {
                    using (TCG_WorklistModel tcg_CaseDetails = new TCG_WorklistModel())
                    {
                        var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));

                        string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                        string[] test = ACD.ACD_Amount.Split('$');
                        ACD.ACD_Amount = test[1];
                        string[] test2 = ACD.TotChrgAmt.Split('$');
                        ACD.ACD_TotalCharges = Convert.ToDecimal(test2[1]);
                        string[] test3 = ACD.TotPayAmt.Split('$');
                        ACD.ACD_TotalPay = Convert.ToDecimal(test3[1]);
                        string[] test4 = ACD.TotAdjAmt.Split('$');
                        ACD.ACD_TotalAdj = Convert.ToDecimal(test4[1]);


                        //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj,
                        //Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, 
                        //Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, 
                        //string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, 
                        //Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                        //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                        string desc = string.Empty;
                        if (string.IsNullOrEmpty(ACD.ACD_Description))
                        { desc = " "; }
                        else
                        { desc = ACD.ACD_Description; }
                        TCG_WLM.Case_InsUpd(ACD.ACD_ID, ACD.ACD_HspAccID, ACD.ACD_Amount, ACD.ACD_TotalCharges, ACD.ACD_TotalPay, ACD.ACD_TotalAdj,
                            ACD.ACD_AmtDiffNAA, ACD.ACD_AmtDiffPayor, ACD.ACD_ExpAmt, "1", "1", "3",
                            ACD.ACD_Status, ACD.ACD_Owner, ACD.ACD_Type, ACD.ACD_SubType,
                            ACD.ACD_PayerReason, ACD.ACD_PrimaryReason, "22", "49", "49", ACD.ACD_Comments,
                            "2", ACD.ACD_Priority, desc, ACD.ACD_TaskFollowUp, ACD.ACD_DueDate,
                            ACD.ACD_FollowUpDate, false, Session["username"].ToString(), DateTime.Now,
                            "", DateTime.Now, "", case_idParameter);


                        //new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return RedirectToAction("APDViewCaseDetails", new { id = ACD.ACD_HspAccID, linkName });
            }
        }


        public int getRootCauseDDLValue(string x)
        {

            RootCause_Master ABS = new RootCause_Master();

            ABS = TCG_WLM.RootCause_Master.Where(m => m.RC_Name == x).FirstOrDefault();

            int ddlID = ABS.RC_ID;
            return ddlID;

        }

        public int getDenialStatusReason(string x)
        {

            DenialStatus_Master ABS = new DenialStatus_Master();

            ABS = TCG_WLM.DenialStatus_Master.Where(m => m.DS_Name == x).FirstOrDefault();

            int ddlID = ABS.DS_ID;
            return ddlID;

        }

        public int getDenialReason(string x)
        {

            DenialCat_Master ABS = new DenialCat_Master();

            ABS = TCG_WLM.DenialCat_Master.Where(m => m.DC_Name == x).FirstOrDefault();

            int ddlID = ABS.DC_ID;
            return ddlID;

        }

        public int get_BillStatus_dropDownValue(string x)
        {

            Account_Bill_Status ABS = new Account_Bill_Status();

            ABS = TCG_WLM.Account_Bill_Status.Where(m => m.BillSts_Name == x).FirstOrDefault();

            int ddlID = ABS.BillSts_ID;
            return ddlID;

        }

        public int getAccClassDDLValue(string x)
        {

            Account_Source ABS = new Account_Source();

            ABS = TCG_WLM.Account_Source.Where(m => m.AccSrc_Name == x).FirstOrDefault();

            int ddlID = ABS.AccSrc_ID;
            return ddlID;

        }

        public string get_Status_dropDownValue(int x)
        {

            Status_Master ACD = new Status_Master();

            ACD = TCG_WLM.Status_Master.Where(m => m.SM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.SM_Name;
            return DDL_Name;

        }

        public string getTaskdropDownText(int x)
        {

            Task_Master ACD = new Task_Master();

            ACD = TCG_WLM.Task_Master.Where(m => m.TM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.TM_Name;
            return DDL_Name;

        }

        public string getPrioritydropDownText(int x)
        {

            Priority_Master ACD = new Priority_Master();

            ACD = TCG_WLM.Priority_Master.Where(m => m.PM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.PM_Name;
            return DDL_Name;

        }

        public string get_Type_dropDownValue(int x)
        {

            Encounter_Type ACD = new Encounter_Type();

            ACD = TCG_WLM.Encounter_Type.Where(m => m.EncType_ID == x).FirstOrDefault();

            string DDL_Name = ACD.EncType_Name;
            return DDL_Name;

        }

        public string get_PrimaryRsn_dropDownValue(int x)
        {

            PrimaryReason_Master ACD = new PrimaryReason_Master();

            ACD = TCG_WLM.PrimaryReason_Master.Where(m => m.PRM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.PRM_Name;
            return DDL_Name;

        }

        public string get_Owner_dropDownText(string x)
        {
            TCG_DataEntities context_tcg = new TCG_DataEntities();
            Users_Data ACD = new Users_Data();

            ACD = context_tcg.Users_Data.Where(m => m.user_ID.ToString() == x).FirstOrDefault();

            string DDL_Name = ACD.user_web_login;
            return DDL_Name;

        }

        public string get_Owner_dropDownValue(string x)
        {
            TCG_DataEntities context_tcg = new TCG_DataEntities();
            Users_Data ACD = new Users_Data();

            ACD = context_tcg.Users_Data.Where(m => m.user_web_login.ToString() == x).FirstOrDefault();

            string DDL_Name_Value = ACD.user_ID.ToString();
            return DDL_Name_Value;

        }

        public List<Account_Case_Details> get_CaseDetailsList(string HospitalAccountID)
        {

            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            using (var db = new TCG_WorklistModel())
            {
                return (from c in db.Account_Case_Details
                        where c.ACD_HspAccID == HospitalAccountID
                        select c).ToList();
            }
        }


        public List<Account_Case_Detials_History> getCaseDetailsHistoryList(string HospitalAccountID)
        {

            List<Account_Case_Detials_History> ACD = new List<Account_Case_Detials_History>();
            using (var db = new TCG_WorklistModel())
            {
                return (from c in db.Account_Case_Detials_History
                        where c.ACDH_HspAccID == HospitalAccountID
                        orderby c.ACDH_CreatedDate descending
                        select c).ToList();
            }
        }


        public List<Account_Case_Details> get_CaseDetails(string HospitalAccountID, int case_ID)
        {

            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            using (var db = new TCG_WorklistModel())
            {
                return (from c in db.Account_Case_Details
                        where c.ACD_HspAccID == HospitalAccountID && c.ACD_ID == case_ID
                        select c).ToList();
            }
        }


        public List<Account_Case_Task> get_CaseTaskDetails(string HospitalAccountID, int case_ID)
        {

            List<Account_Case_Task> ACDT = new List<Account_Case_Task>();
            using (var db = new TCG_WorklistModel())
            {
                ACDT = (from c in db.Account_Case_Task
                        where c.ACT_HspAccID == HospitalAccountID && c.ACT_ACD_ID == case_ID && c.ACT_DeleteFlag == 0
                        select c).ToList();
            }
            return ACDT;

        }


        public List<Get_Under_Paymnent_Accounts_Result> getUnderPaymentsDetailsList(string HospitalAccountID)
        {

            List<Get_Under_Paymnent_Accounts_Result> UPD = new List<Get_Under_Paymnent_Accounts_Result>();
            using (var db = new TCG_WorklistModel())
            {
                return (from c in db.Get_Under_Paymnent_Accounts()
                        where c.Account == HospitalAccountID
                        select c).ToList();
            }
        }

        public List<Get_Under_Paymnent_Accounts_PB_Result> getPBUnderPaymentsDetailsList(string HospitalAccountID)
        {

            List<Get_Under_Paymnent_Accounts_PB_Result> UPD = new List<Get_Under_Paymnent_Accounts_PB_Result>();
            using (var db = new TCG_WorklistModel())
            {
                return (from c in db.Get_Under_Paymnent_Accounts_PB()
                        where c.Account == HospitalAccountID
                        select c).ToList();
            }
        }


        public List<Get_Under_Paymnent_Accounts_APD_Result> getAPDUnderPaymentsDetailsList(string HospitalAccountID)
        {

            List<Get_Under_Paymnent_Accounts_APD_Result> UPD = new List<Get_Under_Paymnent_Accounts_APD_Result>();
            using (var db = new TCG_WorklistModel())
            {
                return (from c in db.Get_Under_Paymnent_Accounts_APD()
                        where c.Account == HospitalAccountID
                        select c).ToList();
            }
        }

        public string get_OnlyOneCaseDetails(string HospitalAccountID)
        {

            Account_Case_Details ACD = new Account_Case_Details();

            ACD = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


            return ACD.ACD_HspAccID;
        }

        public string getOnlyOneCaseDetailsID(string HospitalAccountID, int underReason)
        {
            string ans = string.Empty;
            Account_Case_Details ACD = new Account_Case_Details();

            ACD = TCG_WLM.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID && m.ACD_PayerReason == underReason).FirstOrDefault();

            if (ACD == null)
                ans = "NULL";
            else
                ans = "NOT NULL";

            return ans;
        }

       

        public ActionResult TrendGraph()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                log.Debug("[User:" + Session["first"] + "]  " + "Loading Denials Management  Page..");
                return View();
            }
        }

        public ActionResult PBTrendGraph()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                return View();
            }

        }

        public ActionResult HBUnderpayTrend()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                return View();
            }

        }


        public ActionResult PBUnderpayTrends()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                return View();
            }

        }


        public ActionResult APDTrendGraph()
        {
            if (Session["username"] == null)
            {
                return Redirect("~/Home/Login");
            }
            else
            {
                ViewBag.UserFirst = Session["first"];
                ViewBag.UserLast = Session["last"];
                ViewBag.Message = GetTableauToken();

                return View();
            }

        }

        private String GetTableauToken()
        {

            log.Debug("Retrieving Tableau token ");
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
        public void DispatchPdf(String pageName)
        {

            log.Debug("[User:" + Session["first"] + "]  " + "[Home][dispatchPdf]: Dispatching PDF  with parameter: " + pageName);
            string WatermarkLocation = Server.MapPath("~/Images/hennepin healthcare new logo.png");
            string imageUrl = "http://34.194.103.217:8111/api/GeneratePDF?url=" + pageName;
            string saveLocation = Server.MapPath("~") + "\\Images\\temp.pdf";

            byte[] imageBytes;

            log.Debug("[Home][dispatchPdf]: Creating post request with url : " + imageUrl);
            HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
            imageRequest.Method = "POST";
            var postData = "url=" + pageName;

            imageRequest.ContentType = "application/x-www-form-urlencoded";
            imageRequest.ContentLength = 0;

            WebResponse imageResponse = imageRequest.GetResponse();

            Stream responseStream = imageResponse.GetResponseStream();
            log.Debug("[Home][dispatchPdf]: Retrieving Response");
            using (BinaryReader br = new BinaryReader(responseStream))
            {
                imageBytes = br.ReadBytes(500000);
                br.Close();
            }
            responseStream.Close();
            imageResponse.Close();
            log.Debug("[Home][dispatchPdf]: Closing Stream!");
            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            log.Debug("[Home][dispatchPdf]: Writing to file at " + saveLocation);
            try
            {
                bw.Write(imageBytes);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }
            log.Debug("[Home][dispatchPdf]: Closed File Stream  ");
            //downloaded the document
            Document document = new Document();
            PdfReader pdfReader = new PdfReader(saveLocation);
            PdfStamper stamp = new PdfStamper(pdfReader, new FileStream(saveLocation.Replace("temp.pdf", "[temp][file].pdf"), FileMode.Create));

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(WatermarkLocation);
            img.ScaleToFit(120f, 155.25f);
            img.SetAbsolutePosition(document.Left, document.Top - 50); // set the position in the document where you want the watermark to appear (0,0 = bottom left corner of the page)


            log.Debug("[Home][dispatchPdf]: Added Image to PDF  ");
            PdfContentByte waterMark;
            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                waterMark = stamp.GetOverContent(page);
                waterMark.AddImage(img);
            }
            stamp.FormFlattening = true;
            stamp.Close();

            pdfReader.Close();
            stamp.Close();
            document.Close();
            // now delete the original file and rename the temp file to the original file
            System.IO.File.Delete(saveLocation);
            log.Debug("[Home][dispatchPdf]: Saving Image with PDF");
            System.IO.File.Move(saveLocation.Replace("temp.pdf", "[temp][file].pdf"), saveLocation);
            //File.Delete(url);
            //File.Move(url.Replace(".pdf", "[temp][file].pdf"), url);


            //client side download 
            log.Debug("[Home][dispatchPdf]: Sending the PDF to Client Browser ");
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename='" + pageName + ".pdf'");
            Response.WriteFile(saveLocation);
            Response.End();
            //byte[] fileBytes = System.IO.File.ReadAllBytes(saveLocation);
            //string fileName = "AR Management.pdf";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            log.Debug("[Home][dispatchPdf]: Deleting the temporary PDF File ");
            System.IO.File.Delete(saveLocation);


        }


    }
}