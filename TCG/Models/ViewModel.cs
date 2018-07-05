using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HealthcareAnalytics.Models
{
    public class ViewModel
    {
        public Users_Data user_info { get; set; }
        public Role User_roles { get; set; }

        public string SelectedRole { get; set; }
        public IEnumerable<SelectListItem> role_name { get; set; }

        public System.Guid user_ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        public string first_name { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [DataType(DataType.Text)]
        public string last_name { get; set; }

        [Display(Name = "Full Name")]
        [DataType(DataType.Text)]
        public string full_name { get; set; }


        [Display(Name = "UserName")]
        [DataType(DataType.Text)]
        public string username { get; set; }

        [Display(Name = "Role")]
        [DataType(DataType.Text)]
        public System.Guid user_role { get; set; }


        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string phone_number { get; set; }


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string email_id { get; set; }


        [Display(Name = "User Added by")]
        [DataType(DataType.Text)]
        public string user_added_by { get; set; }

        [Display(Name = "Added Date")]
        [DataType(DataType.Text)]
        public System.DateTime user_add_date { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

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
        [System.ComponentModel.DataAnnotations.Compare("user_web_pwd", ErrorMessage = "Password mismatch")]
        [DataType(DataType.Password)]
        public string confirm_pwd { get; set; }




    }
}