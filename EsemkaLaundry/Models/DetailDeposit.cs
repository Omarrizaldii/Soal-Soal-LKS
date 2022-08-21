using System;
using System.Collections.Generic;

namespace EsemkaLaundry.Models
{
    public partial class DetailDeposit
    {
        public Guid Id { get; set; }
        public Guid HeaderDepositId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid? PackageTransactionId { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public DateTime? CompletedAt { get; set; }

        public virtual HeaderDeposit HeaderDeposit { get; set; } = null!;
        public virtual PackageTransaction? PackageTransaction { get; set; }
        public virtual Service Service { get; set; } = null!;
    }
}
