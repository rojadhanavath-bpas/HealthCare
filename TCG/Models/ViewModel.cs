using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareAnalytics.Models
{
    public class ViewModel
    {
        public Users_Data user_info { get; set; }
        public Role User_roles { get; set; }
    }
}