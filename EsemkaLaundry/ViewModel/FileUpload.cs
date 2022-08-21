using Microsoft.AspNetCore.Http;

namespace EsemkaLaundry.ViewModel
{
    public class FileUpload
    {
        public IFormFile Photo { get; set; }
    }
}