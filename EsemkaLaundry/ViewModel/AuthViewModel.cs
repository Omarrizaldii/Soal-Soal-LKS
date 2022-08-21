using System.ComponentModel.DataAnnotations;

namespace EsemkaLaundry.ViewModel
{
    public class AuthViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
