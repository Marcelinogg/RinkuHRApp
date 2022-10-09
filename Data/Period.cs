using System;
using System.Collections.Generic;

namespace RinkuHRApp.Data
{
    public partial class Period
    {
        public Period()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int PayrollId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
