using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareAnalytics.Models
{
    public class Registration
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
        public string user_full_name { get; set; }
        public System.Guid user_role_key { get; set; }

        [Display(Name = "Phone")]
        // [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [DataType(DataType.PhoneNumber)]
        public string user_phone_number { get; set; }


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string user_email_id { get; set; }

        public string user_added_by { get; set; }
        public System.DateTime user_add_date { get; set; }
        public string user_updated_by { get; set; }
        public Nullable<System.DateTime> user_updated_date { get; set; }
        public byte user_delete_flag { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Middle Name is required")]
        [DataType(DataType.Text)]
        public string user_middle_name { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string user_web_pwd { get; set; }


        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("user_web_pwd", ErrorMessage = "Password and Confirmation Password must match.")]
        [DataType(DataType.Password)]
        public string confirm_pwd { get; set; }
    }
}




    