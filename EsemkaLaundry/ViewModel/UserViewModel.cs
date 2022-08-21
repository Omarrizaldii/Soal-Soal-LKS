using System.ComponentModel.DataAnnotations;
using static EsemkaLaundry.Helper.EnumCollection;

namespace EsemkaLaundry.ViewModel
{
    public class UserViewModel
    {
        [EmailAddress]
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "The field Password must be special character, uppercase and number")]
        public string Password { get; set; } = null!;

        public UserGender? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = null!;

        public UserRole? Role { get; set; }
    }
}