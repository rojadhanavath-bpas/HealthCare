using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareAnalytics.Models
{
    public class VerifyAccount
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Enter your email to search for your account.")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "OTP is required.")]
        [Display(Name = "One Time Password")]
        public string otp_text { get; set; }        

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "please re-enter your password.")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password & confirm password didn't match.")]
        public string confirm_password { get; set; }
    }
}