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
    
    public partial class Account_Case_Task_History
    {
        public int ACTH_ID { get; set; }
        public int ACTH_ACT_ID { get; set; }
        public int ACTH_ACD_ID { get; set; }
        public string ACTH_HspAccID { get; set; }
        public string ACTH_Completed { get; set; }
        public string ACTH_Priority { get; set; }
        public string ACTH_Description { get; set; }
        public string ACTH_Owner { get; set; }
        public string ACTH_Comment { get; set; }
        public System.DateTime ACTH_DueDate { get; set; }
        public string ACTH_CreatedBy { get; set; }
        public System.DateTime ACTH_CreatedDate { get; set; }
        public string ACTH_UpdatedBy_DB { get; set; }
    
        public virtual Account_Case_Details Account_Case_Details { get; set; }
        public virtual Account_Case_Task Account_Case_Task { get; set; }
    }
}
