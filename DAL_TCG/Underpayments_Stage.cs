//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL_TCG
{
    using System;
    using System.Collections.Generic;
    
    public partial class Underpayments_Stage
    {
        public string Account { get; set; }
        public string Acct_Class { get; set; }
        public string Acct_Status { get; set; }
        public string Account_Name { get; set; }
        public Nullable<System.DateTime> Disch_Date { get; set; }
        public Nullable<double> Acct_Bal { get; set; }
        public string Message { get; set; }
        public string Diagnosis_Codes { get; set; }
        public string Plan_Name { get; set; }
        public string Payor_Name { get; set; }
        public Nullable<double> Allowed_Amount_Difference { get; set; }
        public string Expected_Allowed_Amount { get; set; }
        public Nullable<double> Allowed_Amount_Difference__Payor_Specified { get; set; }
        public Nullable<int> Underpayment_Reason_Code { get; set; }
        public string Brief_Summary { get; set; }
        public Nullable<System.DateTime> DateOfUpdate { get; set; }
        public string Biller { get; set; }
    }
}
