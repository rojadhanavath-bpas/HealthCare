using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareAnalytics.Models
{
    public class ApplicationRoles
    {

        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Role Description")]
        public string  Role_Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Role")]
        public string Role_Name { get; set; }

        public bool RoleDeleteFlag { get; set; }
    }
}