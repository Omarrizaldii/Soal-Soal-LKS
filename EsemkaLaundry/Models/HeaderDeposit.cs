using System;
using System.Collections.Generic;

namespace EsemkaLaundry.Models
{
    public partial class HeaderDeposit
    {
        public HeaderDeposit()
        {
            DetailDeposits = new HashSet<DetailDeposit>();
        }

        public Guid Id { get; set; }
        public string CustomerEmail { get; set; } = null!;
        public string EmployeeEmail { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime EstimationAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public virtual User CustomerEmailNavigation { get; set; } = null!;
        public virtual User EmployeeEmailNavigation { get; set; } = null!;
        public virtual ICollection<DetailDeposit> DetailDeposits { get; set; }
    }
}
