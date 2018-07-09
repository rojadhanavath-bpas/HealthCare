using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace HealthcareAnalytics.Models
{
    public class UserLogin
    {
        public string id { get; set; }



        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required")]
        [DataType(DataType.Text)]
        public string username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        //[Display(Name = "Email")]
        //[Required(ErrorMessage = "Enter your email to search for your account.")]
        //[DataType(DataType.EmailAddress)]
        public string email { get; set; }

    }
}