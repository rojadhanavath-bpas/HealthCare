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

        public string openCaseID;  //editOpenTask_id

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


        public ActionResult Underpayemnts_UserAcc_List(string sortOrder, string currentFilter, string searchString, int? page)
        {
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



            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

           
            using (TCG_DataEntities db2 = new TCG_DataEntities())
            {
                List<Get_Under_Paymnent_Accounts_Result> result = db2.Get_Under_Paymnent_Accounts().ToList();
                


                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i].Account.HasValue)
                    {
                        result[i].AccId = Convert.ToString(result[i].Account);                        
                    }
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

                
                int pageSize = 13;
                int pageNumber = (page ?? 1);
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


        private static List<SelectListItem> populateStatus() // 9 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Status_Master.Select(x => new SelectListItem { Text = x.SM_Name, Value = x.SM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateAccount_ARStatus() // 3 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Account_AR_Status.Select(x => new SelectListItem { Text = x.ARSts_Name, Value = x.ARSts_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_BillStatus() // 5 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Account_Bill_Status.Select(x => new SelectListItem { Text = x.BillSts_Name, Value = x.BillSts_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_AccountSource()  // 11 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Account_Source.Select(x => new SelectListItem { Text = x.AccSrc_Name, Value = x.AccSrc_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_EncounterType() // 22 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Encounter_Type.Select(x => new SelectListItem { Text = x.EncType_Name, Value = x.EncType_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_Insurance()  // 151 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Insurance_Company_Name.Select(x => new SelectListItem { Text = x.InsCmp_Name, Value = x.InsCmp_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_PayorFC() // 136 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Payor_Financial_Class.Select(x => new SelectListItem { Text = x.PyrFC_Name, Value = x.PyrFC_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_PrimaryReason() // 49 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.PrimaryReason_Master.Select(x => new SelectListItem { Text = x.PRM_Name, Value = x.PRM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_Task() // 40 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Task_Master.Select(x => new SelectListItem { Text = x.TM_Name, Value = x.TM_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_UserLogin()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_DataEntities context = new TCG_DataEntities();
            return context.User_Login.Select(x => new SelectListItem { Text = x.user_web_login, Value = x.user_Id.ToString() }).ToList();

        }

        private static List<SelectListItem> populate_Priority()  // 1 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Priority_Master.Select(x => new SelectListItem { Text = x.PM_Name, Value = x.PM_ID.ToString() }).ToList();

        }


        private static List<SelectListItem> populateUnderPayReason() // 
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.Underpayment_Reason.Select(x => new SelectListItem { Text = x.Description, Value = x.Reason_Code.ToString() }).ToList();

        }


        private static List<SelectListItem> populateRootCause() // 1 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.RootCause_Master.Select(x => new SelectListItem { Text = x.RC_Name, Value = x.RC_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateDenialCategory() //1 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.RootCause_Master.Select(x => new SelectListItem { Text = x.RC_Name, Value = x.RC_ID.ToString() }).ToList();

        }


        private static List<SelectListItem> populateCompleted() //2 - No
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.CaseCompleted_Master.Select(x => new SelectListItem { Text = x.CC_Name, Value = x.CC_ID.ToString() }).ToList();

        }

        private static List<SelectListItem> populateDenialStatusMaster() //1 - Select
        {
            List<SelectListItem> items = new List<SelectListItem>();
            TCG_Worklist context = new TCG_Worklist();
            return context.DenialStatus_Master.Select(x => new SelectListItem { Text = x.DS_Name, Value = x.DS_ID.ToString() }).ToList();

        }

        public ActionResult TestPage_CaseDetails(string id, string linkName)
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

            Session["AccountID"] = id;

            openCaseID = id;
            string HospitalAccountID = openCaseID;

            TCG_WL = new TCG_Worklist();
            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            Get_Account_Info_for_ARandDenial_Result GAIFADR = new Get_Account_Info_for_ARandDenial_Result();
            List<Get_Account_Info_for_ARandDenial_Result> taskDetails = new List<Get_Account_Info_for_ARandDenial_Result>();
            List<Get_Under_Paymnent_Accounts_Result> result = db2.Get_Under_Paymnent_Accounts().ToList();
            List<Get_Under_Paymnent_Accounts_Result> underPaymentsListByID = getUnderPaymentsDetailsList(openCaseID);

            try
            {
                using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                {
                    taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();
                    
                    if (taskDetails.Count > 0 || taskDetails != null)
                    {

                        if (linkName == "Other")
                        {

                            GAIFADR.isLinkOther = 1;
                            var Case_checkHospitalAccID = TCG_WL.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

                            var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                            var model = new TCG_Worklist();
                            int new_Case_Value;
                            if (Case_checkHospitalAccID != null)
                            {
                                new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                            }
                            else
                            {
                                new_Case_Value = 0;
                            }
                            string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                            if (Case_checkHospitalAccID == null)
                            {

                                TCG_WL.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), 1, ownerId, "1",
                               "2", 2, "1", "2", "3", "2", (taskDetails[0].Reporting_Rsn_Code_w__Desc == null) ? "None" : taskDetails[0].Reporting_Rsn_Code_w__Desc, "", "", "", 0, DateTime.Now, DateTime.Now, false, Session["username"].ToString(), DateTime.Now, "", DateTime.Now, "", case_idParameter);


                                new_Case_Value = Convert.ToInt32(case_idParameter.Value);
                            }


                            TCG_Worklist context = new TCG_Worklist();
                            TCG_DataEntities context_tcg = new TCG_DataEntities();


                            ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                            ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                            ViewBag.OriginalData = taskDetails;
                            ACD = get_CaseDetailsList(HospitalAccountID);


                            int a = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_Status);
                            ViewBag.SM = new SelectList(context.Status_Master.Select(x => new { Value = x.SM_ID.ToString(), Text = x.SM_Name }), "Value", "Text", a);

                            string b = ViewBag.ACD_Data[0].ACD_Owner;
                            ViewBag.UL = new SelectList(context_tcg.User_Login.Select(x => new { Value = x.user_Id.ToString(), Text = x.user_web_login }), "Value", "Text", b);

                            int c = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_Type);
                            ViewBag.ECT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", c);

                            int d = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_SubType);
                            ViewBag.SbT = new SelectList(context.Encounter_Type.Select(x => new { Value = x.EncType_ID.ToString(), Text = x.EncType_Name }), "Value", "Text", d);

                            int e = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrimaryReason);
                            ViewBag.PRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", e);

                            int f = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_SecondaryReason);
                            ViewBag.SRM = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", f);

                            int g = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PayerReason);
                            ViewBag.PR = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", g);

                            int h = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrinDiag);
                            ViewBag.PD = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", h);

                            int i = Convert.ToInt32(ViewBag.ACD_Data[0].ACD_PrinProc);
                            ViewBag.PP = new SelectList(context.PrimaryReason_Master.Select(x => new { Value = x.PRM_ID.ToString(), Text = x.PRM_Name }), "Value", "Text", i);



                            if (ACD.Count > 0)
                            {

                                for (int x = 0; x < ACD.Count; x++)
                                {
                                    int aa = Convert.ToInt32(ACD[x].ACD_Status);
                                    int bb = Convert.ToInt32(ACD[x].ACD_Type);
                                    int cc = Convert.ToInt32(ACD[x].ACD_PrimaryReason);
                                    int dd = Convert.ToInt32(ACD[x].ACD_PrinDiag);
                                    int ee = Convert.ToInt32(ACD[x].ACD_PrinProc);

                                    ACD[x].ACD_Status = aa;
                                    ACD[x].ACD_Type = get_Type_dropDownValue(bb);
                                    ACD[x].ACD_PrimaryReason = get_PrimaryRsn_dropDownValue(cc);
                                    ACD[x].ACD_PrinDiag = get_PrimaryRsn_dropDownValue(dd);
                                    ACD[x].ACD_PrinProc = get_PrimaryRsn_dropDownValue(ee);
                                }
                            }

                            ViewBag.OtherLinkData = ACD;

                        }
                        else if (linkName == "underPayments")
                        {
                            GAIFADR.isLinkOther = 2;

                            ViewBag.underPaymentsLinkData = underPaymentsListByID;

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(taskDetails);

        }

        [HttpGet]
        public ActionResult editCaseDetails(string id, string linkName)
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

            TCG_WL = new TCG_Worklist();
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

            try
            {
                using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                {
                    taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                    if (taskDetails.Count > 0 && taskDetails != null)
                    {

                        var Case_checkHospitalAccID = TCG_WL.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

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



                        if (taskDetails[0].Total_Charge_Amount.HasValue)
                        {
                            Decimal testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                            testAmt1 = Math.Round(testAmt1, 2);
                            taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                        }
                        else
                            taskDetails[0].convTotChrgAmt = "$0.00";


                        if (taskDetails[0].Total_Payment_Amount.HasValue)
                        {
                            Decimal testAmt2 = Convert.ToDecimal(taskDetails[0].Total_Payment_Amount);
                            testAmt2 = Math.Round(testAmt2, 2);
                            taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                        }
                        else
                            taskDetails[0].convTotPayAmt = "$0.00";


                        if (taskDetails[0].Total_Adjustment_Amount.HasValue)
                        {
                            Decimal testAmt3 = Convert.ToDecimal(taskDetails[0].Total_Adjustment_Amount);
                            testAmt3 = Math.Round(testAmt3, 2);
                            taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                        }
                        else
                            taskDetails[0].convAdjAmt = "$0.00";


                        if (taskDetails[0].Total_Account_Balance.HasValue)
                        {
                            Decimal testAmt4 = Convert.ToDecimal(taskDetails[0].Total_Account_Balance);
                            testAmt4 = Math.Round(testAmt4, 2);
                            taskDetails[0].convTotAccBal = "$" + testAmt4.ToString();
                        }
                        else
                            taskDetails[0].convTotAccBal = "$0.00";



                        var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                        var model = new TCG_Worklist();
                        if (Case_checkHospitalAccID != null)
                        {
                            new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                        }
                        else
                        {
                            new_Case_Value = 0;
                        }
                        string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                        if (Case_checkHospitalAccID == null)
                        {
                            if (linkName == "underPay")
                            {
                                int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                                //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                                //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                                //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                TCG_WL.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), 9, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                    0, " ", "22", "49", "49", "",
                                    "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                    DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
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

                                //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                                //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                                //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                                //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                TCG_WL.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), 9, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
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

                        var Case_checkHospitalAccID = TCG_WL.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


                        Decimal testAmount = Convert.ToDecimal(underPaymentsListByID[0].Acct_Bal);
                        testAmount = Math.Round(testAmount, 2);
                        
                        taskDetails.Add(new Get_Account_Info_for_ARandDenial_Result() { Hospital_Account_ID = Convert.ToString(underPaymentsListByID[0].Account), Account_Patient_Name = underPaymentsListByID[0].Account_Name, convAdmDate = "", Encounter_Type = "", Account_Financial_Class = "", convFirstBillDate = "", convLastPayDate = "", convTotChrgAmt = "$0.00", convTotPayAmt = "$0.00", convAdjAmt = "$0.00", convTotAccBal = "$"+ testAmount.ToString() });


                        var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                        var model = new TCG_Worklist();

                        if (Case_checkHospitalAccID != null)
                        {
                            new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                        }
                        else
                        {
                            new_Case_Value = 0;
                        }
                        string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                        if (Case_checkHospitalAccID == null)
                        {
                            if (linkName == "underPay")
                            {
                                int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                                //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                                //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                                //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                TCG_WL.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), 9, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                    0, " ", "22", "49", "49", "",
                                    "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                    DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                    "", DateTime.Now, "", case_idParameter);


                                new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                acdDetails.link = 1;
                            }
                        }
                    }

                    TCG_Worklist context = new TCG_Worklist();
                    TCG_DataEntities context_tcg = new TCG_DataEntities();


                    ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                    ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                    ViewBag.OriginalData = taskDetails;
                    ACD = get_CaseDetailsList(HospitalAccountID);
                    ACD[0].dateConvert = ACD[0].ACD_DueDate.Value.ToShortDateString();

                    //CASE DETAILS
                    acdDetails = TCG_WL.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                    acdDetails.dateConvert = acdDetails.ACD_DueDate.Value.ToShortDateString();
                    acdDetails.dateFollowUp = acdDetails.ACD_FollowUpDate.Value.ToShortDateString();
                    Decimal AmtConvert = Convert.ToDecimal(acdDetails.ACD_Amount);
                    acdDetails.convertAmount = Math.Round(AmtConvert, 2);
                    acdDetails.convAmt = acdDetails.convertAmount.ToString();
                    if (linkName == "underPay")
                    {
                        acdDetails.link = 1;
                    }
                    else
                    {
                        acdDetails.link = 2;
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

        [HttpPost]
        public ActionResult editCaseDetails(Account_Case_Details ACD)
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

            TCG_WL = new TCG_Worklist();
            string linkName = string.Empty;
            if(ACD.link == 1)
            {
                linkName = "underPay";
            }
            else
            {
                linkName = "Other";
            }
                       
            try
            {
                using (TCG_Worklist tcg_CaseDetails = new TCG_Worklist())
                {
                    int new_Case_Value=0;
                    var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));

                    string ownerId = get_Owner_dropDownValue(Session["username"].ToString());

                    //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                    //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                    //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                    //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                    //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                    string desc = string.Empty;
                    if (string.IsNullOrEmpty(ACD.ACD_Description))
                    {    desc = " ";     }
                    else
                    { desc= ACD.ACD_Description; }
                    TCG_WL.Case_InsUpd(ACD.ACD_ID, ACD.ACD_HspAccID, ACD.ACD_Amount, ACD.ACD_Status, ACD.ACD_Owner, ACD.ACD_Type, ACD.ACD_SubType,
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

            return RedirectToAction("viewCaseDetails", new { id = ACD.ACD_HspAccID , linkName  });

        }

        [HttpGet]
        public ActionResult viewCaseDetails(string id, string linkName)
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

            TCG_WL = new TCG_Worklist();
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

            try
            {
                using (TCG_DataEntities tcg_CaseDetails = new TCG_DataEntities())
                {
                    taskDetails = tcg_CaseDetails.Get_Account_Info_for_ARandDenial(id).ToList();

                    if (taskDetails.Count > 0 && taskDetails != null)
                    {

                        var Case_checkHospitalAccID = TCG_WL.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();

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



                        if (taskDetails[0].Total_Charge_Amount.HasValue)
                        {
                            Decimal testAmt1 = Convert.ToDecimal(taskDetails[0].Total_Charge_Amount);
                            testAmt1 = Math.Round(testAmt1, 2);
                            taskDetails[0].convTotChrgAmt = "$" + testAmt1.ToString();
                        }
                        else
                            taskDetails[0].convTotChrgAmt = "$0.00";


                        if (taskDetails[0].Total_Payment_Amount.HasValue)
                        {
                            Decimal testAmt2 = Convert.ToDecimal(taskDetails[0].Total_Payment_Amount);
                            testAmt2 = Math.Round(testAmt2, 2);
                            taskDetails[0].convTotPayAmt = "$" + testAmt2.ToString();
                        }
                        else
                            taskDetails[0].convTotPayAmt = "$0.00";


                        if (taskDetails[0].Total_Adjustment_Amount.HasValue)
                        {
                            Decimal testAmt3 = Convert.ToDecimal(taskDetails[0].Total_Adjustment_Amount);
                            testAmt3 = Math.Round(testAmt3, 2);
                            taskDetails[0].convAdjAmt = "$" + testAmt3.ToString();
                        }
                        else
                            taskDetails[0].convAdjAmt = "$0.00";


                        if (taskDetails[0].Total_Account_Balance.HasValue)
                        {
                            Decimal testAmt4 = Convert.ToDecimal(taskDetails[0].Total_Account_Balance);
                            testAmt4 = Math.Round(testAmt4, 2);
                            taskDetails[0].convTotAccBal = "$" + testAmt4.ToString();
                        }
                        else
                            taskDetails[0].convTotAccBal = "$0.00";



                        var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                        var model = new TCG_Worklist();
                        if (Case_checkHospitalAccID != null)
                        {
                            new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                        }
                        else
                        {
                            new_Case_Value = 0;
                        }
                        string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                        if (Case_checkHospitalAccID == null)
                        {
                            if (linkName == "underPay")
                            {
                                int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                                //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                                //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                                //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                TCG_WL.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), 9, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                    0, " ", "22", "49", "49", "",
                                    "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                    DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
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

                                //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                                //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                                //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                                //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                TCG_WL.Case_InsUpd(0, taskDetails[0].Hospital_Account_ID, System.Convert.ToString(taskDetails[0].Total_Account_Balance), 9, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
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

                        var Case_checkHospitalAccID = TCG_WL.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


                        Decimal testAmount = Convert.ToDecimal(underPaymentsListByID[0].Acct_Bal);
                        testAmount = Math.Round(testAmount, 2);

                        taskDetails.Add(new Get_Account_Info_for_ARandDenial_Result() { Hospital_Account_ID = Convert.ToString(underPaymentsListByID[0].Account), Account_Patient_Name = underPaymentsListByID[0].Account_Name, convAdmDate = "", Encounter_Type = "", Account_Financial_Class = "", convFirstBillDate = "", convLastPayDate = "", convTotChrgAmt = "$0.00", convTotPayAmt = "$0.00", convAdjAmt = "$0.00", convTotAccBal = "$" + testAmount.ToString() });


                        var case_idParameter = new ObjectParameter("new_recordNumber", typeof(int));
                        var model = new TCG_Worklist();

                        if (Case_checkHospitalAccID != null)
                        {
                            new_Case_Value = Case_checkHospitalAccID.ACD_ID;
                        }
                        else
                        {
                            new_Case_Value = 0;
                        }
                        string ownerId = get_Owner_dropDownValue(Session["username"].ToString());
                        if (Case_checkHospitalAccID == null)
                        {
                            if (linkName == "underPay")
                            {
                                int billStatus_type = get_BillStatus_dropDownValue(underPaymentsListByID[0].Acct_Status);
                                int accClass_subTpe = getAccClassDDLValue(underPaymentsListByID[0].Acct_Class);
                                //Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, string aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, 
                                //string aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, 
                                //string aCD_Completed, string aCD_Priority, string aCD_Description, string aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, 
                                //Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, 
                                //string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber
                                TCG_WL.Case_InsUpd(0, System.Convert.ToString(underPaymentsListByID[0].Account), System.Convert.ToString(underPaymentsListByID[0].Acct_Bal), 9, ownerId, billStatus_type.ToString(), accClass_subTpe.ToString(),
                                    0, " ", "22", "49", "49", "",
                                    "2", "1", "", 40, underPaymentsListByID[0].Disch_Date,
                                    DateTime.Now, false, Session["username"].ToString(), DateTime.Now,
                                    "", DateTime.Now, "", case_idParameter);


                                new_Case_Value = Convert.ToInt32(case_idParameter.Value);

                                acdDetails.link = 1;
                            }
                        }
                    }

                    TCG_Worklist context = new TCG_Worklist();
                    TCG_DataEntities context_tcg = new TCG_DataEntities();


                    ViewBag.ACD_data = get_CaseDetails(HospitalAccountID, new_Case_Value);
                    ViewBag.One_ACD_data = get_OnlyOneCaseDetails(HospitalAccountID);
                    ViewBag.OriginalData = taskDetails;
                    ACD = get_CaseDetailsList(HospitalAccountID);
                    ACD[0].dateConvert = ACD[0].ACD_DueDate.Value.ToShortDateString();

                    //CASE DETAILS
                    acdDetails = TCG_WL.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();
                    acdDetails.dateConvert = acdDetails.ACD_DueDate.Value.ToShortDateString();
                    acdDetails.dateFollowUp = acdDetails.ACD_FollowUpDate.Value.ToShortDateString();
                    Decimal AmtConvert = Convert.ToDecimal(acdDetails.ACD_Amount);
                    acdDetails.convertAmount = Math.Round(AmtConvert, 2);
                    acdDetails.convAmt =  acdDetails.convertAmount.ToString();
                    if (linkName == "underPay")
                    {
                        acdDetails.link = 1;
                    }
                    else
                    {
                        acdDetails.link = 2;
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



        public int getRootCauseDDLValue(string x)
        {

            RootCause_Master ABS = new RootCause_Master();

            ABS = TCG_WL.RootCause_Master.Where(m => m.RC_Name == x).FirstOrDefault();

            int ddlID = ABS.RC_ID;
            return ddlID;

        }


        public int getDenialStatusReason(string x)
        {

            DenialStatus_Master ABS = new DenialStatus_Master();

            ABS = TCG_WL.DenialStatus_Master.Where(m => m.DS_Name == x).FirstOrDefault();

            int ddlID = ABS.DS_ID;
            return ddlID;

        }

        public int getDenialReason(string x)
        {

            DenialCat_Master ABS = new DenialCat_Master();

            ABS = TCG_WL.DenialCat_Master.Where(m => m.DC_Name == x).FirstOrDefault();

            int ddlID = ABS.DC_ID;
            return ddlID;

        }

        public int get_BillStatus_dropDownValue(string x)
        {

            Account_Bill_Status ABS = new Account_Bill_Status();

            ABS = TCG_WL.Account_Bill_Status.Where(m => m.BillSts_Name == x).FirstOrDefault();

            int ddlID = ABS.BillSts_ID;
            return ddlID;

        }

        public int getAccClassDDLValue(string x)
        {

            Account_Source ABS = new Account_Source();

            ABS = TCG_WL.Account_Source.Where(m => m.AccSrc_Name == x).FirstOrDefault();

            int ddlID = ABS.AccSrc_ID;
            return ddlID;

        }

        public string get_Status_dropDownValue(int x)
        {

            Status_Master ACD = new Status_Master();

            ACD = TCG_WL.Status_Master.Where(m => m.SM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.SM_Name;
            return DDL_Name;

        }

        public string getTaskdropDownText(int x)
        {

            Task_Master ACD = new Task_Master();

            ACD = TCG_WL.Task_Master.Where(m => m.TM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.TM_Name;
            return DDL_Name;

        }

        public string getPrioritydropDownText(int x)
        {

            Priority_Master ACD = new Priority_Master();

            ACD = TCG_WL.Priority_Master.Where(m => m.PM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.PM_Name;
            return DDL_Name;

        }

        public string get_Type_dropDownValue(int x)
        {

            Encounter_Type ACD = new Encounter_Type();

            ACD = TCG_WL.Encounter_Type.Where(m => m.EncType_ID == x).FirstOrDefault();

            string DDL_Name = ACD.EncType_Name;
            return DDL_Name;

        }

        public string get_PrimaryRsn_dropDownValue(int x)
        {

            PrimaryReason_Master ACD = new PrimaryReason_Master();

            ACD = TCG_WL.PrimaryReason_Master.Where(m => m.PRM_ID == x).FirstOrDefault();

            string DDL_Name = ACD.PRM_Name;
            return DDL_Name;

        }

        public string get_Owner_dropDownText(string x)
        {
            TCG_DataEntities context_tcg = new TCG_DataEntities();
            User_Login ACD = new User_Login();

            ACD = context_tcg.User_Login.Where(m => m.user_Id.ToString() == x).FirstOrDefault();

            string DDL_Name = ACD.user_web_login;
            return DDL_Name;

        }

        public string get_Owner_dropDownValue(string x)
        {
            TCG_DataEntities context_tcg = new TCG_DataEntities();
            User_Login ACD = new User_Login();

            ACD = context_tcg.User_Login.Where(m => m.user_web_login.ToString() == x).FirstOrDefault();

            string DDL_Name_Value = ACD.user_Id.ToString();
            return DDL_Name_Value;

        }


        public List<Account_Case_Details> get_CaseDetailsList(string HospitalAccountID)
        {

            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            using (var db = new TCG_Worklist())
            {
                return (from c in db.Account_Case_Details
                        where c.ACD_HspAccID == HospitalAccountID
                        select c).ToList();
            }
        }


        public List<Account_Case_Detials_History> getCaseDetailsHistoryList(string HospitalAccountID)
        {

            List<Account_Case_Detials_History> ACD = new List<Account_Case_Detials_History>();
            using (var db = new TCG_Worklist())
            {
                return (from c in db.Account_Case_Detials_History
                        where c.ACDH_HspAccID == HospitalAccountID orderby c.ACDH_CreatedDate descending
                        select c).ToList();
            }
        }


        public List<Account_Case_Details> get_CaseDetails(string HospitalAccountID, int case_ID)
        {

            List<Account_Case_Details> ACD = new List<Account_Case_Details>();
            using (var db = new TCG_Worklist())
            {
                return (from c in db.Account_Case_Details
                        where c.ACD_HspAccID == HospitalAccountID && c.ACD_ID == case_ID
                        select c).ToList();
            }
        }


        public List<Account_Case_Task> get_CaseTaskDetails(string HospitalAccountID, int case_ID)
        {

            List<Account_Case_Task> ACDT = new List<Account_Case_Task>();
            using (var db = new TCG_Worklist())
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
            using (var db = new TCG_DataEntities())
            {
                return (from c in db.Get_Under_Paymnent_Accounts()
                        where c.Account == Convert.ToDouble(HospitalAccountID)
                        select c).ToList();
            }
        }

        public string get_OnlyOneCaseDetails(string HospitalAccountID)
        {

            Account_Case_Details ACD = new Account_Case_Details();

            ACD = TCG_WL.Account_Case_Details.Where(m => m.ACD_HspAccID == HospitalAccountID).FirstOrDefault();


            return ACD.ACD_HspAccID;
        }

    }
}