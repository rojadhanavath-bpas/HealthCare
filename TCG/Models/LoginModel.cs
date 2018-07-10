using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareAnalytics.Models
{
    public class LoginModel
    {
        public string id { get; set; }



        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required")]
        [DataType(DataType.Text)]
        public string username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }

       
        public string email { get; set; }


    }
}