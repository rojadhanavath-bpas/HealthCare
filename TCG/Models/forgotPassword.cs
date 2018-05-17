using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareAnalytics.Models
{
    public class forgotPassword
    {
        [Required(ErrorMessage = "OTP is required.")]
        [Display(Name = "One Time Password")]
        public string otp_text { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Required(ErrorMessage = "Password is required.")]
        [Display(Name ="Password")]
        public string password { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Display(Name ="Confirm password")]
        [Compare("password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string confirm_pwd { get; set; }

        [DataType(DataType.EmailAddress)]
        public string email { get; set; }


    }
}