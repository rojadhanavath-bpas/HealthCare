using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace HealthcareAnalytics.Models
{
    public class RegistrationModel
    {
        public System.Guid user_ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        public string first_name { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [DataType(DataType.Text)]
        public string last_name { get; set; }
       
        

        [Display(Name = "Phone")]
        // [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [DataType(DataType.PhoneNumber)]
        public string phone_number { get; set; }


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        [System.Web.Mvc.Remote("CheckEmail", "Email", ErrorMessage = "This email already exist")]
        public string email_id { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Middle Name is required")]
        [DataType(DataType.Text)]
        public string middle_name { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please select a UserName")]
        [DataType(DataType.Text)]
        public string username { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }


        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("user_web_pwd", ErrorMessage = "Password and Confirmation Password must match.")]
        [DataType(DataType.Password)]
        public string confirm_password { get; set; }


    }

}




