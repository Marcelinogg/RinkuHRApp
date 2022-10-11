using System;
using System.Collections.Generic;

namespace RinkuHRApp.Data
{
    public partial class Transaction
    {
        public int PayrollId { get; set; }
        public int PeriodId { get; set; }
        public int ConceptId { get; set; }
        public int EmployeeId { get; set; }
        public int Sequence { get; set; }
        public int Times { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StatusId { get; set; }

        public virtual Concept Concept { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Period P { get; set; }
        public virtual Payroll Payroll { get; set; }
    }
}
