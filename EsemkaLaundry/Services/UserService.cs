using EsemkaLaundry.Helper;
using EsemkaLaundry.ViewModel;
using static EsemkaLaundry.Helper.EnumCollection;

namespace EsemkaLaundry.Services
{
    public class UserService : BaseService
    {
        private EsemkaLaundryContext db = new EsemkaLaundryContext();
        private IConfiguration configuration;

        public UserService(EsemkaLaundryContext db, IConfiguration _configuration, string email) : base(db, email)
        {
            this.db = db;
            this.configuration = _configuration;
        }

        public async Task<Response> GetUser(string? keyword, int page, UserGender? gender = null)
        {
            try
            {
                if (keyword != null && page != 0 && gender != null)
                {
                    var users = await db.Users.Where(s => s.Email.Contains(keyword) ||
                        s.Name.Contains(keyword) ||
                        s.PhoneNumber.Contains(keyword) ||
                        s.Address.Contains(keyword))
                        .Where(s => s.Gender == gender)
                        .Skip((page - 1) * 10).Take(10).ToListAsync();
                    return response(200, data: users);
                }
                if (keyword != null && page != 0)
                {
                    var users = await db.Users.Where(s => s.Email.Contains(keyword) ||
                        s.Name.Contains(keyword) ||
                        s.PhoneNumber.Contains(keyword) ||
                        s.Address.Contains(keyword))
                        .Skip((page - 1) * 10).Take(10).ToListAsync();
                    return response(200, data: users);
                }
                if (keyword != null && gender != null)
                {
                    var users = await db.Users.Where(s => s.Email.Contains(keyword) ||
                        s.Name.Contains(keyword) ||
                        s.PhoneNumber.Contains(keyword) ||
                        s.Address.Contains(keyword))
                        .Where(s => s.Gender == gender)
                        .ToListAsync();
                    return response(200, data: users);
                }
                if (page != 0 && gender != null)
                {
                    var users = await db.Users
                       .Where(s => s.Gender == gender)
                       .Skip((page - 1) * 10).Take(10).ToListAsync();
                    return response(200, data: users);
                }
                if (keyword != null)
                {
                    var users = await db.Users.Where(s => s.Email.Contains(keyword) ||
                        s.Name.Contains(keyword) ||
                        s.PhoneNumber.Contains(keyword) ||
                        s.Address.Contains(keyword)).ToListAsync();
                    return response(200, data: users);
                }
                if (page != 0)
                {
                    var users = await db.Users.Skip((page - 1) * 10).Take(10).ToListAsync();
                    return response(200, data: users);
                }
                if (gender != null)
                {
                    var users = await db.Users
                   .Where(s => s.Gender == gender).ToListAsync();
                    return response(200, data: users);
                }
                var user = await db.Users.ToListAsync();
                return response(200, data: user);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> GetUserbyEmail(string email)
        {
            try
            {
                var checkUser = await db.Users.Where(s => s.Email == email).FirstOrDefaultAsync();
                if (checkUser == null) return response(404, "User not found");
                return response(200, data: checkUser);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> PostUser(UserViewModel request)
        {
            try
            {
                var checkUser = await db.Users.Where(s => s.Email == request.Email).FirstOrDefaultAsync();
                if (checkUser != null) return response(409, "User already exist");
                // if (request.Gender != "Female" || request.Gender != "Male") return response(403, "Gender is not valid");
                var user = new User
                {
                    Email = request.Email,
                    Name = request.Name,
                    Password = request.Password,
                    Gender = request.Gender,
                    DateOfBirth = request.DateOfBirth,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Role = request.Role
                };
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return response(201, data: user);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> PutUser(string email, UserViewModel request)
        {
            try
            {
                var checkUser = await db.Users.Where(s => s.Email == email).FirstOrDefaultAsync();
                if (checkUser == null) return response(404, "User not found");
                if (request.Email != email) return response(400, "Email request must be same email parameter");
                //if (checkUser.Email == request.Email) return response(409, "Email already exist");
                //  if (checkUser.Gender != UserGender.Female || checkUser.Gender != UserGender.Male) return response(403, "Gender is not valid");
                checkUser.Email = request.Email;
                checkUser.Name = request.Name;
                checkUser.Password = request.Password;
                checkUser.Gender = request.Gender;
                checkUser.DateOfBirth = request.DateOfBirth;
                checkUser.PhoneNumber = request.PhoneNumber;
                checkUser.Address = request.Address;
                checkUser.Role = request.Role;
                await db.SaveChangesAsync();
                return response(200);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> DeleteUser(string email)
        {
            try
            {
                var checkUser = await db.Users.Where(s => s.Email == email).FirstOrDefaultAsync();
                if (checkUser == null) return response(404, "User not found");
                db.Users.Remove(checkUser);
                await db.SaveChangesAsync();
                return response(200);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }
    }
}