using EsemkaLaundry.Helper;
using EsemkaLaundry.ViewModel;
using Microsoft.AspNetCore.Http;

namespace EsemkaLaundry.Services
{
    public class MeService : BaseService
    {
        private EsemkaLaundryContext db = new EsemkaLaundryContext();
        private IWebHostEnvironment webHost;

        public MeService(EsemkaLaundryContext db, IWebHostEnvironment _webHost, string email) : base(db, email)
        {
            webHost = _webHost;
            this.db = db;
        }

        public async Task<Response> GetMe()
        {
            try
            {
                return response(200, data: user);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> PostPhoto(FileUpload file)
        {
            var cekuser = await db.Users.Where(s => s.Email == user.Email).FirstOrDefaultAsync();
            if (file.Photo.Length > 0)
            {
                string path = webHost.WebRootPath + "\\Photo\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "-" + file.Photo.FileName;
                using (FileStream fileStream = File.Create(path + uniqueFileName))
                {
                    file.Photo.CopyTo(fileStream);
                    fileStream.Flush();
                    cekuser.PhotoPath = fileStream.Name;
                    await db.SaveChangesAsync();
                    return response(200);
                }
            }
            else
            {
                return response(400, "File not upload");
            }
        }

        public async Task<Response> PutMe(UserViewModel request)
        {
            try
            {
                var cekuser = await db.Users.Where(s => s.Email == user.Email).FirstOrDefaultAsync();
                if (cekuser == null) return response(404, "User not found");
                cekuser.Email = request.Email;
                cekuser.Name = request.Name;
                cekuser.Password = request.Password;
                cekuser.Gender = request.Gender;
                cekuser.DateOfBirth = request.DateOfBirth;
                cekuser.PhoneNumber = request.PhoneNumber;
                cekuser.Address = request.Address;
                cekuser.Role = request.Role;
                await db.SaveChangesAsync();
                return response(200);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> GetMePhoto()
        {
            try
            {
                var cekuser = await db.Users.Where(s => s.Email == user.Email).FirstOrDefaultAsync();
                if (cekuser.PhotoPath == null) return response(404, "Photo null");
                return response(200, message: cekuser.PhotoPath);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }
    }
}