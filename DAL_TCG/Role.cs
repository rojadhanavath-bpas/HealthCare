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
    
    public partial class Role
    {
        public System.Guid role_key { get; set; }
        public string role_code { get; set; }
        public string role_code_short { get; set; }
        public string role_designation { get; set; }
        public string role_createdBy_user { get; set; }
        public System.DateTime role_created_date { get; set; }
        public byte role_delete_flag { get; set; }
    }
}
