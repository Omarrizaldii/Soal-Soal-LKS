using System.ComponentModel.DataAnnotations;

namespace EsemkaLaundry.ViewModel
{
    public class PackageViewModel
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }

        [Required]
        [MaxLength(1000)]
        [MinLength(1)]
        public double Total { get; set; }

        [Required]
        [MaxLength(1000000)]
        [MinLength(1)]
        public double Price { get; set; }
    }
}