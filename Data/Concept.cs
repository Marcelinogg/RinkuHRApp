using System;
using System.Collections.Generic;

namespace RinkuHRApp.Data
{
    public partial class Concept
    {
        public Concept()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TypConcepteId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
