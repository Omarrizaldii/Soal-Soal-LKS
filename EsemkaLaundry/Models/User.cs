using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static EsemkaLaundry.Helper.EnumCollection;

namespace EsemkaLaundry.Models
{
    public partial class User
    {
        public User()
        {
            HeaderDepositCustomerEmailNavigations = new HashSet<HeaderDeposit>();
            HeaderDepositEmployeeEmailNavigations = new HashSet<HeaderDeposit>();
            PackageTransactions = new HashSet<PackageTransaction>();
        }

        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserGender? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public UserRole? Role { get; set; }

        [JsonIgnore]
        public string? PhotoPath { get; set; }

        [JsonIgnore]
        public virtual ICollection<HeaderDeposit> HeaderDepositCustomerEmailNavigations { get; set; }

        [JsonIgnore]
        public virtual ICollection<HeaderDeposit> HeaderDepositEmployeeEmailNavigations { get; set; }

        [JsonIgnore]
        public virtual ICollection<PackageTransaction> PackageTransactions { get; set; }
    }
}