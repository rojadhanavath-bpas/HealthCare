using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareAnalytics.Models
{
    public class UserContext
    {
        public static UserContext CurrentUser { get; set; }
        public string Role { get; set; }
        public string IsAdmin { get; set; }
    }
}