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
    
    public partial class Underpayments_APD
    {
        public string Account { get; set; }
        public string Acct_Class { get; set; }
        public string Acct_Status { get; set; }
        public string Account_Name { get; set; }
        public Nullable<System.DateTime> Disch_Date { get; set; }
        public Nullable<double> Acct_Bal { get; set; }
        public string Message { get; set; }
        public string Diagnosis_Codes__Mapped___CMS_ICD_10_CM___ICD_9_CM_ { get; set; }
        public string Plan_Name { get; set; }
        public string Payor_Name { get; set; }
        public Nullable<double> Allowed_Amount_Difference__Current_NAA_Posted_To_Bucket_ { get; set; }
        public string Expected_Allowed_Amount { get; set; }
        public Nullable<double> Allowed_Amount_Difference__Payor_Specified_ { get; set; }
        public Nullable<int> Underpayment_Reason_Code { get; set; }
        public string Brief_Summary { get; set; }
        public Nullable<System.DateTime> DateOfUpdate { get; set; }
        public string Biller { get; set; }
        public string BillingProvider { get; set; }
        public string CaseStatus { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string SSN { get; set; }
        public Nullable<System.DateTime> Adm_Ser_Date { get; set; }
        public Nullable<System.DateTime> Bill_Date { get; set; }
        public Nullable<System.DateTime> Claim_Date { get; set; }
        public Nullable<double> Total_Charges { get; set; }
        public string Financial_Class { get; set; }
        public string Ins1_ID__ { get; set; }
        public Nullable<double> INS1_Balance { get; set; }
        public string Ins2_ID__ { get; set; }
        public string INS2_Mnemonic { get; set; }
        public string Ins2_Payer { get; set; }
        public Nullable<double> INS2_Balance { get; set; }
        public string Ins3_ID { get; set; }
        public string INS3_Mnemonic { get; set; }
        public string Ins3_Payer { get; set; }
        public string INS3_Balance { get; set; }
        public Nullable<double> Pt_Balance { get; set; }
    }
}
