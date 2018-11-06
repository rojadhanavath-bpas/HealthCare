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
    
    public partial class Get_Account_Info_for_ARandDenial_Result
    {
        public Nullable<System.DateTime> Account_Date { get; set; }
        public Nullable<System.DateTime> Admission_Date { get; set; }
        public Nullable<System.DateTime> Discharge_Date { get; set; }
        public Nullable<System.DateTime> Coding_Date { get; set; }
        public Nullable<System.DateTime> First_Billed_Date { get; set; }
        public Nullable<System.DateTime> First_Claim_Date { get; set; }
        public Nullable<System.DateTime> Most_Recent_Claim_Date { get; set; }
        public Nullable<System.DateTime> Most_Recent_Insurance_Payment_Received_Date { get; set; }
        public Nullable<System.DateTime> Most_Recent_SelfPay_Payment_Received_Date { get; set; }
        public Nullable<System.DateTime> Account_Closed_Date { get; set; }
        public Nullable<System.DateTime> Zero_Balance_Date { get; set; }
        public string Primary_Coverage_Payor_Name { get; set; }
        public string Primary_Coverage_Payor_Financial_Class { get; set; }
        public string Primary_Coverage_Benefit_Plan_Name { get; set; }
        public string Second_Coverage_Financial_Class { get; set; }
        public string Second_Coverage_Benefit_Plan_Name { get; set; }
        public string Second_Coverage_Payor_Name { get; set; }
        public string Admitting_Provider_Name { get; set; }
        public string Account_Department_Name { get; set; }
        public string Account_Department_Specialty { get; set; }
        public string Account_Location_Name { get; set; }
        public string Account_Service_Area_Name { get; set; }
        public string Billing_DRG_Code { get; set; }
        public string Billing_DRG_Code_Set { get; set; }
        public string MS_DRG_Code { get; set; }
        public string Guarantor_Epic_ID { get; set; }
        public string Guarantor_Name { get; set; }
        public string Encounter_Epic_CSN { get; set; }
        public string Encounter_Type { get; set; }
        public string Account_Patient_Epic_ID { get; set; }
        public string Account_Patient_Name { get; set; }
        public string Hospital_Account_ID { get; set; }
        public string Account_Financial_Class { get; set; }
        public string Account_Source { get; set; }
        public string Account_Bill_Status { get; set; }
        public string Account_AR_Status { get; set; }
        public Nullable<decimal> Total_Account_Balance { get; set; }
        public Nullable<decimal> Total_Self_Pay_Balance { get; set; }
        public Nullable<decimal> Total_Primary_Insurance_Balance { get; set; }
        public Nullable<decimal> Total_Pre_Billed_Balance { get; set; }
        public Nullable<decimal> Total_Non_Primary_Insurance_Balance { get; set; }
        public Nullable<decimal> Total_Undistributed_Balance { get; set; }
        public Nullable<decimal> Total_Bad_Debt_Balance { get; set; }
        public Nullable<decimal> Total_Adjustment_Amount { get; set; }
        public Nullable<decimal> Total_Self_Pay_Adjustment_Amount { get; set; }
        public Nullable<decimal> Total_Primary_Insurance_Adjustment_Amount { get; set; }
        public Nullable<decimal> Total_Non_Primary_Insurance_Adjustment_Amount { get; set; }
        public Nullable<decimal> Total_Undistributed_Adjustment_Amount { get; set; }
        public Nullable<decimal> Total_Bad_Debt_Adjustment_Amount { get; set; }
        public Nullable<decimal> Total_Payment_Amount { get; set; }
        public Nullable<decimal> Total_Self_Pay_Payment_Amount { get; set; }
        public Nullable<decimal> Total_Primary_Insurance_Payment_Amount { get; set; }
        public Nullable<decimal> Total_Non_Primary_Insurance_Payment_Amount { get; set; }
        public Nullable<decimal> Total_Undistributed_Payment_Amount { get; set; }
        public Nullable<decimal> Total_Bad_Debt_Payment_Amount { get; set; }
        public Nullable<decimal> Total_Settlement_Payments { get; set; }
        public Nullable<decimal> Total_Charge_Amount { get; set; }
        public string Invoice { get; set; }
        public string Ext_Rsn_Code { get; set; }
        public string Ln_on_EOB { get; set; }
        public string Rev_Code { get; set; }
        public string CPT_R__Code { get; set; }
        public string Allowed_Amt { get; set; }
        public string Denied_Amount { get; set; }
        public string Billed_Amount { get; set; }
        public string Bucket_Payor { get; set; }
        public string Bucket_Plan { get; set; }
        public string Aging_Bkt { get; set; }
        public string Owning_Area { get; set; }
        public string Source_Area { get; set; }
        public string Status { get; set; }
        public string Root_Cause { get; set; }
        public string Preventable_ { get; set; }
        public string Follow_up_Dt { get; set; }
        public string Resolution_or_Resp_Cat { get; set; }
        public string Rcvd_Dt { get; set; }
        public string Acct_Class { get; set; }
        public string Account_ID { get; set; }
        public string Account_Name { get; set; }
        public string Account_Location { get; set; }
        public string Follow_up_ID { get; set; }
        public string Age { get; set; }
        public string First_Payor_Rcvd_Dt { get; set; }
        public string Last_Payor_Rcvd_Dt { get; set; }
        public string Clinical_Root_Cause { get; set; }
        public string Days_Denied { get; set; }
        public string Denial_Cat { get; set; }
        public string Ext_Rsn_Code_w__Desc { get; set; }
        public string Financial_Class { get; set; }
        public string First_Claim_Dt { get; set; }
        public string First_Ext_Claim_Sent_Dt { get; set; }
        public string HB_Acct_Bal { get; set; }
        public string Acct_Status { get; set; }
        public string Last_Completed_Dt { get; set; }
        public string Last_Reopened_Dt { get; set; }
        public string Last_User_for_Closed_Rec { get; set; }
        public string Paid_Amount { get; set; }
        public string Rsn_Code { get; set; }
        public string Reporting_Rsn_Code_w__Desc { get; set; }
        public string Rev_Code_Desc { get; set; }
        public string Service_Area { get; set; }
        public string Source { get; set; }
        public string Source_Dept { get; set; }
        public string Src_Pmt_Tx { get; set; }
        public string Stage { get; set; }

        public int isLinkOther { get; set; }
        public string convAdmDate { get; set; }
        public string convDischDate { get; set; }
        public string convFirstBillDate { get; set; }
        public string convLastPayDate { get; set; }
        public string convTotChrgAmt { get; set; }
        public string convTotPayAmt { get; set; }
        public string convAdjAmt { get; set; }
        public string convTotAccBal { get; set; }


    }
}
