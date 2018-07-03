using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareAnalytics.Models
{
    public class UserDataModel
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


        [Display(Name = "Role")]
        [DataType(DataType.Text)]
        public System.Guid user_role_key { get; set; }


        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string user_phone_number { get; set; }


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string user_email_id { get; set; }


        [Display(Name = "User Added by")]
        [DataType(DataType.Text)]
        public string user_added_by { get; set; }

        [Display(Name = "Added Date")]
        [DataType(DataType.Text)]
        public System.DateTime user_add_date { get; set; }

        [Display(Name = "Updated By")]
        [DataType(DataType.Text)]
        public string user_updated_by { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
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
        [CompareAttribute("user_web_pwd", ErrorMessage = "Emails mismatch")]
        [DataType(DataType.Password)]
        public string confirm_pwd { get; set; }


    }
}