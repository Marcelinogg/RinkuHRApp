using System;
using System.Collections.Generic;

namespace RinkuHRApp.Data
{
    public partial class Payroll
    {
        public Payroll()
        {
            Employees = new HashSet<Employee>();
            Periods = new HashSet<Period>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Weeks { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Period> Periods { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
