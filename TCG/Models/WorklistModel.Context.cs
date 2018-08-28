﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TCG_WorklistModel : DbContext
    {
        public TCG_WorklistModel()
            : base("name=TCG_WorklistModel")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account_AR_Status> Account_AR_Status { get; set; }
        public virtual DbSet<Account_Bill_Status> Account_Bill_Status { get; set; }
        public virtual DbSet<Account_Case_Details> Account_Case_Details { get; set; }
        public virtual DbSet<Account_Case_Detials_History> Account_Case_Detials_History { get; set; }
        public virtual DbSet<Account_Case_Task> Account_Case_Task { get; set; }
        public virtual DbSet<Account_Case_Task_History> Account_Case_Task_History { get; set; }
        public virtual DbSet<Account_Source> Account_Source { get; set; }
        public virtual DbSet<CaseCompleted_Master> CaseCompleted_Master { get; set; }
        public virtual DbSet<DenialCat_Master> DenialCat_Master { get; set; }
        public virtual DbSet<DenialStatus_Master> DenialStatus_Master { get; set; }
        public virtual DbSet<Encounter_Type> Encounter_Type { get; set; }
        public virtual DbSet<Insurance_Company_Name> Insurance_Company_Name { get; set; }
        public virtual DbSet<Payor_Financial_Class> Payor_Financial_Class { get; set; }
        public virtual DbSet<PrimaryReason_Master> PrimaryReason_Master { get; set; }
        public virtual DbSet<Priority_Master> Priority_Master { get; set; }
        public virtual DbSet<RootCause_Master> RootCause_Master { get; set; }
        public virtual DbSet<Status_Master> Status_Master { get; set; }
        public virtual DbSet<Task_Master> Task_Master { get; set; }
        public virtual DbSet<Underpayment> Underpayments { get; set; }
        public virtual DbSet<Underpayment_ReasonCode> Underpayment_ReasonCode { get; set; }
        public virtual DbSet<HBorPB_Master> HBorPB_Master { get; set; }
    
        public virtual int Case_InsUpd(Nullable<int> aCD_ID, string aCD_HspAccID, string aCD_Amount, Nullable<decimal> aCD_TotalCharges, Nullable<decimal> aCD_TotalPay, Nullable<decimal> aCD_TotalAdj, Nullable<decimal> aCD_AmtDiffNAA, Nullable<decimal> aCD_AmtDiffPayor, Nullable<decimal> aCD_ExpAmt, string aCD_BillProvider, string aCD_Department, string aCD_HBorPB, Nullable<int> aCD_Status, string aCD_Owner, string aCD_Type, string aCD_SubType, Nullable<int> aCD_PayerReason, string aCD_PrimaryReason, string aCD_SecondaryReason, string aCD_PrinDiag, string aCD_PrinProc, string aCD_Comments, string aCD_Completed, string aCD_Priority, string aCD_Description, Nullable<int> aCD_TaskFollowUp, Nullable<System.DateTime> aCD_DueDate, Nullable<System.DateTime> aCD_FollowUpDate, Nullable<bool> aCD_DeleteFlag, string aCD_CreatedBy, Nullable<System.DateTime> aCD_CreatedDate, string aCD_UpdatedBy, Nullable<System.DateTime> aCD_Updateddate, string aCTD_UpdatedBy_DB, ObjectParameter new_recordNumber)
        {
            var aCD_IDParameter = aCD_ID.HasValue ?
                new ObjectParameter("ACD_ID", aCD_ID) :
                new ObjectParameter("ACD_ID", typeof(int));
    
            var aCD_HspAccIDParameter = aCD_HspAccID != null ?
                new ObjectParameter("ACD_HspAccID", aCD_HspAccID) :
                new ObjectParameter("ACD_HspAccID", typeof(string));
    
            var aCD_AmountParameter = aCD_Amount != null ?
                new ObjectParameter("ACD_Amount", aCD_Amount) :
                new ObjectParameter("ACD_Amount", typeof(string));
    
            var aCD_TotalChargesParameter = aCD_TotalCharges.HasValue ?
                new ObjectParameter("ACD_TotalCharges", aCD_TotalCharges) :
                new ObjectParameter("ACD_TotalCharges", typeof(decimal));
    
            var aCD_TotalPayParameter = aCD_TotalPay.HasValue ?
                new ObjectParameter("ACD_TotalPay", aCD_TotalPay) :
                new ObjectParameter("ACD_TotalPay", typeof(decimal));
    
            var aCD_TotalAdjParameter = aCD_TotalAdj.HasValue ?
                new ObjectParameter("ACD_TotalAdj", aCD_TotalAdj) :
                new ObjectParameter("ACD_TotalAdj", typeof(decimal));
    
            var aCD_AmtDiffNAAParameter = aCD_AmtDiffNAA.HasValue ?
                new ObjectParameter("ACD_AmtDiffNAA", aCD_AmtDiffNAA) :
                new ObjectParameter("ACD_AmtDiffNAA", typeof(decimal));
    
            var aCD_AmtDiffPayorParameter = aCD_AmtDiffPayor.HasValue ?
                new ObjectParameter("ACD_AmtDiffPayor", aCD_AmtDiffPayor) :
                new ObjectParameter("ACD_AmtDiffPayor", typeof(decimal));
    
            var aCD_ExpAmtParameter = aCD_ExpAmt.HasValue ?
                new ObjectParameter("ACD_ExpAmt", aCD_ExpAmt) :
                new ObjectParameter("ACD_ExpAmt", typeof(decimal));
    
            var aCD_BillProviderParameter = aCD_BillProvider != null ?
                new ObjectParameter("ACD_BillProvider", aCD_BillProvider) :
                new ObjectParameter("ACD_BillProvider", typeof(string));
    
            var aCD_DepartmentParameter = aCD_Department != null ?
                new ObjectParameter("ACD_Department", aCD_Department) :
                new ObjectParameter("ACD_Department", typeof(string));
    
            var aCD_HBorPBParameter = aCD_HBorPB != null ?
                new ObjectParameter("ACD_HBorPB", aCD_HBorPB) :
                new ObjectParameter("ACD_HBorPB", typeof(string));
    
            var aCD_StatusParameter = aCD_Status.HasValue ?
                new ObjectParameter("ACD_Status", aCD_Status) :
                new ObjectParameter("ACD_Status", typeof(int));
    
            var aCD_OwnerParameter = aCD_Owner != null ?
                new ObjectParameter("ACD_Owner", aCD_Owner) :
                new ObjectParameter("ACD_Owner", typeof(string));
    
            var aCD_TypeParameter = aCD_Type != null ?
                new ObjectParameter("ACD_Type", aCD_Type) :
                new ObjectParameter("ACD_Type", typeof(string));
    
            var aCD_SubTypeParameter = aCD_SubType != null ?
                new ObjectParameter("ACD_SubType", aCD_SubType) :
                new ObjectParameter("ACD_SubType", typeof(string));
    
            var aCD_PayerReasonParameter = aCD_PayerReason.HasValue ?
                new ObjectParameter("ACD_PayerReason", aCD_PayerReason) :
                new ObjectParameter("ACD_PayerReason", typeof(int));
    
            var aCD_PrimaryReasonParameter = aCD_PrimaryReason != null ?
                new ObjectParameter("ACD_PrimaryReason", aCD_PrimaryReason) :
                new ObjectParameter("ACD_PrimaryReason", typeof(string));
    
            var aCD_SecondaryReasonParameter = aCD_SecondaryReason != null ?
                new ObjectParameter("ACD_SecondaryReason", aCD_SecondaryReason) :
                new ObjectParameter("ACD_SecondaryReason", typeof(string));
    
            var aCD_PrinDiagParameter = aCD_PrinDiag != null ?
                new ObjectParameter("ACD_PrinDiag", aCD_PrinDiag) :
                new ObjectParameter("ACD_PrinDiag", typeof(string));
    
            var aCD_PrinProcParameter = aCD_PrinProc != null ?
                new ObjectParameter("ACD_PrinProc", aCD_PrinProc) :
                new ObjectParameter("ACD_PrinProc", typeof(string));
    
            var aCD_CommentsParameter = aCD_Comments != null ?
                new ObjectParameter("ACD_Comments", aCD_Comments) :
                new ObjectParameter("ACD_Comments", typeof(string));
    
            var aCD_CompletedParameter = aCD_Completed != null ?
                new ObjectParameter("ACD_Completed", aCD_Completed) :
                new ObjectParameter("ACD_Completed", typeof(string));
    
            var aCD_PriorityParameter = aCD_Priority != null ?
                new ObjectParameter("ACD_Priority", aCD_Priority) :
                new ObjectParameter("ACD_Priority", typeof(string));
    
            var aCD_DescriptionParameter = aCD_Description != null ?
                new ObjectParameter("ACD_Description", aCD_Description) :
                new ObjectParameter("ACD_Description", typeof(string));
    
            var aCD_TaskFollowUpParameter = aCD_TaskFollowUp.HasValue ?
                new ObjectParameter("ACD_TaskFollowUp", aCD_TaskFollowUp) :
                new ObjectParameter("ACD_TaskFollowUp", typeof(int));
    
            var aCD_DueDateParameter = aCD_DueDate.HasValue ?
                new ObjectParameter("ACD_DueDate", aCD_DueDate) :
                new ObjectParameter("ACD_DueDate", typeof(System.DateTime));
    
            var aCD_FollowUpDateParameter = aCD_FollowUpDate.HasValue ?
                new ObjectParameter("ACD_FollowUpDate", aCD_FollowUpDate) :
                new ObjectParameter("ACD_FollowUpDate", typeof(System.DateTime));
    
            var aCD_DeleteFlagParameter = aCD_DeleteFlag.HasValue ?
                new ObjectParameter("ACD_DeleteFlag", aCD_DeleteFlag) :
                new ObjectParameter("ACD_DeleteFlag", typeof(bool));
    
            var aCD_CreatedByParameter = aCD_CreatedBy != null ?
                new ObjectParameter("ACD_CreatedBy", aCD_CreatedBy) :
                new ObjectParameter("ACD_CreatedBy", typeof(string));
    
            var aCD_CreatedDateParameter = aCD_CreatedDate.HasValue ?
                new ObjectParameter("ACD_CreatedDate", aCD_CreatedDate) :
                new ObjectParameter("ACD_CreatedDate", typeof(System.DateTime));
    
            var aCD_UpdatedByParameter = aCD_UpdatedBy != null ?
                new ObjectParameter("ACD_UpdatedBy", aCD_UpdatedBy) :
                new ObjectParameter("ACD_UpdatedBy", typeof(string));
    
            var aCD_UpdateddateParameter = aCD_Updateddate.HasValue ?
                new ObjectParameter("ACD_Updateddate", aCD_Updateddate) :
                new ObjectParameter("ACD_Updateddate", typeof(System.DateTime));
    
            var aCTD_UpdatedBy_DBParameter = aCTD_UpdatedBy_DB != null ?
                new ObjectParameter("ACTD_UpdatedBy_DB", aCTD_UpdatedBy_DB) :
                new ObjectParameter("ACTD_UpdatedBy_DB", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Case_InsUpd", aCD_IDParameter, aCD_HspAccIDParameter, aCD_AmountParameter, aCD_TotalChargesParameter, aCD_TotalPayParameter, aCD_TotalAdjParameter, aCD_AmtDiffNAAParameter, aCD_AmtDiffPayorParameter, aCD_ExpAmtParameter, aCD_BillProviderParameter, aCD_DepartmentParameter, aCD_HBorPBParameter, aCD_StatusParameter, aCD_OwnerParameter, aCD_TypeParameter, aCD_SubTypeParameter, aCD_PayerReasonParameter, aCD_PrimaryReasonParameter, aCD_SecondaryReasonParameter, aCD_PrinDiagParameter, aCD_PrinProcParameter, aCD_CommentsParameter, aCD_CompletedParameter, aCD_PriorityParameter, aCD_DescriptionParameter, aCD_TaskFollowUpParameter, aCD_DueDateParameter, aCD_FollowUpDateParameter, aCD_DeleteFlagParameter, aCD_CreatedByParameter, aCD_CreatedDateParameter, aCD_UpdatedByParameter, aCD_UpdateddateParameter, aCTD_UpdatedBy_DBParameter, new_recordNumber);
        }
    
        public virtual int Case_Task_InsUpd(Nullable<int> aCT_ID, string aCT_HspAccID, Nullable<int> aCT_ACD_ID, Nullable<bool> aCT_Completed, string aCT_Priority, string aCT_Description, string aCT_Owner, string aCT_Comment, Nullable<System.DateTime> aCT_DueDate, Nullable<int> aCT_DeleteFlag, string aCT_CreatedBy, Nullable<System.DateTime> aCT_CreatedDate, string aCT_UpdatedBy, Nullable<System.DateTime> aCT_Updateddate, string aCT_UpdatedBy_DB, ObjectParameter new_recordNumber)
        {
            var aCT_IDParameter = aCT_ID.HasValue ?
                new ObjectParameter("ACT_ID", aCT_ID) :
                new ObjectParameter("ACT_ID", typeof(int));
    
            var aCT_HspAccIDParameter = aCT_HspAccID != null ?
                new ObjectParameter("ACT_HspAccID", aCT_HspAccID) :
                new ObjectParameter("ACT_HspAccID", typeof(string));
    
            var aCT_ACD_IDParameter = aCT_ACD_ID.HasValue ?
                new ObjectParameter("ACT_ACD_ID", aCT_ACD_ID) :
                new ObjectParameter("ACT_ACD_ID", typeof(int));
    
            var aCT_CompletedParameter = aCT_Completed.HasValue ?
                new ObjectParameter("ACT_Completed", aCT_Completed) :
                new ObjectParameter("ACT_Completed", typeof(bool));
    
            var aCT_PriorityParameter = aCT_Priority != null ?
                new ObjectParameter("ACT_Priority", aCT_Priority) :
                new ObjectParameter("ACT_Priority", typeof(string));
    
            var aCT_DescriptionParameter = aCT_Description != null ?
                new ObjectParameter("ACT_Description", aCT_Description) :
                new ObjectParameter("ACT_Description", typeof(string));
    
            var aCT_OwnerParameter = aCT_Owner != null ?
                new ObjectParameter("ACT_Owner", aCT_Owner) :
                new ObjectParameter("ACT_Owner", typeof(string));
    
            var aCT_CommentParameter = aCT_Comment != null ?
                new ObjectParameter("ACT_Comment", aCT_Comment) :
                new ObjectParameter("ACT_Comment", typeof(string));
    
            var aCT_DueDateParameter = aCT_DueDate.HasValue ?
                new ObjectParameter("ACT_DueDate", aCT_DueDate) :
                new ObjectParameter("ACT_DueDate", typeof(System.DateTime));
    
            var aCT_DeleteFlagParameter = aCT_DeleteFlag.HasValue ?
                new ObjectParameter("ACT_DeleteFlag", aCT_DeleteFlag) :
                new ObjectParameter("ACT_DeleteFlag", typeof(int));
    
            var aCT_CreatedByParameter = aCT_CreatedBy != null ?
                new ObjectParameter("ACT_CreatedBy", aCT_CreatedBy) :
                new ObjectParameter("ACT_CreatedBy", typeof(string));
    
            var aCT_CreatedDateParameter = aCT_CreatedDate.HasValue ?
                new ObjectParameter("ACT_CreatedDate", aCT_CreatedDate) :
                new ObjectParameter("ACT_CreatedDate", typeof(System.DateTime));
    
            var aCT_UpdatedByParameter = aCT_UpdatedBy != null ?
                new ObjectParameter("ACT_UpdatedBy", aCT_UpdatedBy) :
                new ObjectParameter("ACT_UpdatedBy", typeof(string));
    
            var aCT_UpdateddateParameter = aCT_Updateddate.HasValue ?
                new ObjectParameter("ACT_Updateddate", aCT_Updateddate) :
                new ObjectParameter("ACT_Updateddate", typeof(System.DateTime));
    
            var aCT_UpdatedBy_DBParameter = aCT_UpdatedBy_DB != null ?
                new ObjectParameter("ACT_UpdatedBy_DB", aCT_UpdatedBy_DB) :
                new ObjectParameter("ACT_UpdatedBy_DB", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Case_Task_InsUpd", aCT_IDParameter, aCT_HspAccIDParameter, aCT_ACD_IDParameter, aCT_CompletedParameter, aCT_PriorityParameter, aCT_DescriptionParameter, aCT_OwnerParameter, aCT_CommentParameter, aCT_DueDateParameter, aCT_DeleteFlagParameter, aCT_CreatedByParameter, aCT_CreatedDateParameter, aCT_UpdatedByParameter, aCT_UpdateddateParameter, aCT_UpdatedBy_DBParameter, new_recordNumber);
        }
    
        public virtual int Get_Under_Paymnent_Accounts_HB()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Get_Under_Paymnent_Accounts_HB");
        }
    
        public virtual ObjectResult<Get_Under_Paymnent_Accounts_PB_Result> Get_Under_Paymnent_Accounts_PB()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Get_Under_Paymnent_Accounts_PB_Result>("Get_Under_Paymnent_Accounts_PB");
        }
    
        public virtual ObjectResult<Get_Under_Paymnent_Accounts_Result> Get_Under_Paymnent_Accounts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Get_Under_Paymnent_Accounts_Result>("Get_Under_Paymnent_Accounts");
        }
    }
}
