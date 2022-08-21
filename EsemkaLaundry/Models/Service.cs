using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static EsemkaLaundry.Helper.EnumCollection;

namespace EsemkaLaundry.Models
{
    public partial class Service
    {
        public Service()
        {
            DetailDeposits = new HashSet<DetailDeposit>();
            Packages = new HashSet<Package>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ServiceCategory? Category { get; set; }
        public ServiceUnit? Unit { get; set; }
        public double Price { get; set; }
        public int EstimationDuration { get; set; }

        [JsonIgnore]
        public virtual ICollection<DetailDeposit> DetailDeposits { get; set; }

        [JsonIgnore]
        public virtual ICollection<Package> Packages { get; set; }
    }
}