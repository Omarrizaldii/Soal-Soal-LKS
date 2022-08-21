using System.ComponentModel.DataAnnotations;
using static EsemkaLaundry.Helper.EnumCollection;

namespace EsemkaLaundry.ViewModel
{
    public class ServiceViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public ServiceCategory Category { get; set; }
        public ServiceUnit Unit { get; set; }

        [Required]
        [MaxLength(1000000)]
        [MinLength(1)]
        public double Price { get; set; }

        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        public int EstimationDuration { get; set; }
    }
}