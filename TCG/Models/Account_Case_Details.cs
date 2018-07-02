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
    using System.Collections.Generic;
    
    public partial class Account_Case_Details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account_Case_Details()
        {
            this.Account_Case_Detials_History = new HashSet<Account_Case_Detials_History>();
            this.Account_Case_Task = new HashSet<Account_Case_Task>();
            this.Account_Case_Task_History = new HashSet<Account_Case_Task_History>();
        }
    
        public int ACD_ID { get; set; }
        public string ACD_HspAccID { get; set; }
        public string ACD_Amount { get; set; }
        public int ACD_Status { get; set; }
        public string ACD_Owner { get; set; }
        public string ACD_Type { get; set; }
        public string ACD_SubType { get; set; }
        public int ACD_PayerReason { get; set; }
        public string ACD_PrimaryReason { get; set; }
        public string ACD_SecondaryReason { get; set; }
        public string ACD_PrinDiag { get; set; }
        public string ACD_PrinProc { get; set; }
        public string ACD_Comments { get; set; }
        public string ACD_CreatedBy { get; set; }
        public System.DateTime ACD_CreatedDate { get; set; }
        public string ACD_UpdatedBy { get; set; }
        public System.DateTime ACD_Updateddate { get; set; }
        public string ACD_UpdatedBy_DB { get; set; }
        public string ACD_Completed { get; set; }
        public string ACD_Priority { get; set; }
        public string ACD_Description { get; set; }
        public int ACD_TaskFollowUp { get; set; }
        public Nullable<System.DateTime> ACD_DueDate { get; set; }
        public Nullable<System.DateTime> ACD_FollowUpDate { get; set; }
        public Nullable<bool> ACD_DeleteFlag { get; set; }

        public string dateConvert { get; set; }
        public string dateFollowUp { get; set; }
        public decimal convertAmount { get; set; }
        public string convAmt { get; set; }
        public int link { get; set; }
        public string userName { get; set; }
        public string userID { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account_Case_Detials_History> Account_Case_Detials_History { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account_Case_Task> Account_Case_Task { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account_Case_Task_History> Account_Case_Task_History { get; set; }
    }
}
