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
    using System.ComponentModel.DataAnnotations;
    
    public partial class Role
    {
        [Display(Name = "Role Type")]
        public System.Guid role_key { get; set; }

        [DataType(DataType.Text)]
        [Display(Name ="Role Type")]
        public string role_code { get; set; }

        [DataType(DataType.Text)]
        [Display(Name ="Role Type")]
        public string role_code_short { get; set; }

        public string role_designation { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Created By")]
        public string role_createdBy_user { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Role Created Date")]
        public System.DateTime role_created_date { get; set; }

        public byte role_delete_flag { get; set; }
    }
}
