using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EsemkaLaundry.Models
{
    public partial class Package
    {
        public Package()
        {
            PackageTransactions = new HashSet<PackageTransaction>();
        }

        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;
        public double Total { get; set; }
        public double Price { get; set; }

        [JsonIgnore]
        public virtual ICollection<PackageTransaction> PackageTransactions { get; set; }
    }
}