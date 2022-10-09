using System;
using System.Collections.Generic;

namespace RinkuHRApp.Data
{
    public partial class VwPayrollConcept
    {
        public int? EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public int PayrollId { get; set; }
        public int PeriodId { get; set; }
        public int ConceptId { get; set; }
        public string ConceptName { get; set; }
        public decimal Amount { get; set; }
        public string TypeConcept { get; set; }
    }
}
