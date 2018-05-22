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
    
    public partial class Account_Case_Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account_Case_Task()
        {
            this.Account_Case_Task_History = new HashSet<Account_Case_Task_History>();
        }
    
        public int ACT_ID { get; set; }
        public string ACT_HspAccID { get; set; }
        public int ACT_ACD_ID { get; set; }
        public string ACT_Completed { get; set; }
        public string ACT_Priority { get; set; }
        public string ACT_Description { get; set; }
        public string ACT_Owner { get; set; }
        public string ACT_Comment { get; set; }
        public System.DateTime ACT_DueDate { get; set; }
        public string ACT_CreatedBy { get; set; }
        public System.DateTime ACT_CreatedDate { get; set; }
        public string ACT_UpdatedBy { get; set; }
        public System.DateTime ACT_Updateddate { get; set; }
        public string ACT_UpdatedBy_DB { get; set; }
    
        public virtual Account_Case_Details Account_Case_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account_Case_Task_History> Account_Case_Task_History { get; set; }
    }
}
