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
    
    public partial class Account_Case_Detials_History
    {
        public int ACDH_ID { get; set; }
        public int ACDH_ACD_ID { get; set; }
        public string ACDH_HspAccID { get; set; }
        public string ACDH_Amount { get; set; }
        public int ACDH_Status { get; set; }
        public string ACDH_Owner { get; set; }
        public string ACDH_Type { get; set; }
        public string ACDH_SubType { get; set; }
        public int ACDH_PayerReason { get; set; }
        public string ACDH_PrimaryReason { get; set; }
        public string ACDH_SecondaryReason { get; set; }
        public string ACDH_PrinDiag { get; set; }
        public string ACDH_PrinProc { get; set; }
        public string ACDH_Comments { get; set; }
        public string ACDH_CreatedBy { get; set; }
        public System.DateTime ACDH_CreatedDate { get; set; }
        public string ACDH_UpdatedBy_DB { get; set; }
        public string ACDH_Completed { get; set; }
        public string ACDH_Priority { get; set; }
        public string ACDH_Description { get; set; }
        public int ACDH_TaskFollowUp { get; set; }
        public Nullable<System.DateTime> ACDH_DueDate { get; set; }
        public Nullable<System.DateTime> ACDH_FollowUpDate { get; set; }
        public Nullable<bool> ACDH_DeleteFlag { get; set; }
        public decimal ACDH_TotalCharges { get; set; }
        public decimal ACDH_TotalPay { get; set; }
        public decimal ACDH_TotalAdj { get; set; }
        public decimal ACDH_AmtDiffNAA { get; set; }
        public decimal ACDH_AmtDiffPayor { get; set; }
        public decimal ACDH_ExpAmt { get; set; }
        public string ACDH_BillProvider { get; set; }
        public string ACDH_Department { get; set; }
        public string ACDH_HBorPB { get; set; }

        public string convertDate { get; set; }
        public string convertFollowUpDate { get; set; }
        public string totalCharges { get; set; }
        public string totalPay { get; set; }
        public string totalAdj { get; set; }
        public string totalAccBal { get; set; }

        public virtual Account_Case_Details Account_Case_Details { get; set; }
    }
}
