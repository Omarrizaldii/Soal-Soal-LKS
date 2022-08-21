using EsemkaLaundry.Helper;
using EsemkaLaundry.Models;
using EsemkaLaundry.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EsemkaLaundry.Services
{
    public class AuthService : BaseService
    {
        private EsemkaLaundryContext db;
        private IConfiguration configuration;

        public AuthService(EsemkaLaundryContext db, IConfiguration _configuration, string email) : base(db, email)
        {
            this.db = db;
            this.configuration = _configuration;
        }

        public async Task<Response> Auth(AuthViewModel request)
        {
            try
            {
                var user = await db.Users.Where(s => s.Email == request.Email && s.Password == request.Password).FirstOrDefaultAsync();
                if (user == null) return response(404, "User not found!");
                var Claims = new[] { new Claim("Email", request.Email) };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
                var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    claims: Claims,
                    audience: configuration["JWT:Audience"],
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: sign);
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return response(200, data: jwt);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }
    }
}