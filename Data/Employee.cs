using System;
using System.Collections.Generic;

namespace RinkuHRApp.Data
{
    public partial class Employee
    {
        public Employee()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public int PositionId { get; set; }
        public decimal SalaryPerHour { get; set; }
        public int HoursPerDay { get; set; }
        public int DaysPerWeek { get; set; }
        public int PayrollId { get; set; }
        public int StatusId { get; set; }

        public virtual Payroll Payroll { get; set; }
        public virtual Position Position { get; set; }
        public virtual EmployeeStatus Status { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
