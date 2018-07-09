//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HealthcareAnalytics.Models
{
    using System;
    
    public partial class Get_AR_Info_for_Balance_Range_Result
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


        public string New_DischargeDate { get; set; }
        public decimal convertAmount { get; set; }
        public string convertBal { get; set; }
    }
}
