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
    
    public partial class Users_Data
    {
        public System.Guid user_ID { get; set; }
        public string user_first_name { get; set; }
        public string user_last_name { get; set; }
        public string user_full_name { get; set; }
        public System.Guid user_role_key { get; set; }
        public string user_phone_number { get; set; }
        public string user_email_id { get; set; }
        public string user_added_by { get; set; }
        public System.DateTime user_add_date { get; set; }
        public string user_updated_by { get; set; }
        public Nullable<System.DateTime> user_updated_date { get; set; }
        public byte user_delete_flag { get; set; }
        public string user_middle_name { get; set; }
        public string user_web_pwd { get; set; }
        public string otp_key { get; set; }
        public Nullable<System.DateTime> otp_time { get; set; }
        public byte user_active_flag { get; set; }
    }
}
