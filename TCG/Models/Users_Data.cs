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
    
    public partial class Users_Data
    {
        public System.Guid user_ID { get; set; }


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        public string user_first_name { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [DataType(DataType.Text)]
        public string user_last_name { get; set; }


        [Display(Name = "Full Name")]
        [DataType(DataType.Text)]
        public string user_full_name { get; set; }

        public System.Guid user_role_key { get; set; }

        [Display(Name ="Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string user_phone_number { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string user_email_id { get; set; }

        [Display(Name = "Added By")]
        [DataType(DataType.Text)]
        public string user_added_by { get; set; }

        [Display(Name = "User Added Date")]
        [DataType(DataType.DateTime)]
        public System.DateTime user_add_date { get; set; }

        public string user_updated_by { get; set; }


        public Nullable<System.DateTime> user_updated_date { get; set; }

        public byte user_delete_flag { get; set; }

        [Display(Name = "Middle Name")]
        [DataType(DataType.Text)]
        public string user_middle_name { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string user_web_pwd { get; set; }


        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and confirm password didn't match")]
        public string confirm_pwd { get; set; }


        public string otp_key { get; set; }

        public Nullable<System.DateTime> otp_time { get; set; }


        public byte user_active_flag { get; set; }
    }
}
