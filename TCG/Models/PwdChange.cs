using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareAnalytics.Models
{
    public class PwdChange
    {
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        
        [DataType(DataType.Password)]
        public string current_pwd { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        
        [DataType(DataType.Password)]
        public string new_pwd { get; set; }

        [Display(Name = "Re-enter Password")]
        [Required(ErrorMessage = "renter password is required")]
        [Compare("new_pwd", ErrorMessage = "Password and Confirmation Password must match.")]
        [DataType(DataType.Password)]
        public string retype_pwd { get; set; }

    }

}