//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL_TCG
{
    using System;
    using System.Collections.Generic;
    
    public partial class Timely_Filing_Limits
    {
        public string Payer { get; set; }
        public string FileAsPrimary { get; set; }
        public int PrimaryInMonths { get; set; }
        public int PrimaryInDays { get; set; }
        public string FileAsSecondary { get; set; }
        public int SecondaryInMonths { get; set; }
        public int SecondaryInDays { get; set; }
        public string AppealTimeFrame { get; set; }
        public double AppealInMonths { get; set; }
        public double AppealInDays { get; set; }
    }
}