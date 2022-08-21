using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EsemkaLaundry.Models
{
    public partial class PackageTransaction
    {
        public PackageTransaction()
        {
            DetailDeposits = new HashSet<DetailDeposit>();
        }

        public Guid Id { get; set; }
        public string? UserEmail { get; set; }
        public virtual User? User { get; set; }
        public Guid PackageId { get; set; }
        public virtual Package Package { get; set; } = null!;
        public double Price { get; set; }
        public double AvailableUnit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        [JsonIgnore]
        public virtual ICollection<DetailDeposit> DetailDeposits { get; set; }
    }
}