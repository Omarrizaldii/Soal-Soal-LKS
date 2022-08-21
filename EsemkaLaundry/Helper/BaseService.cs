using EsemkaLaundry.Models;

namespace EsemkaLaundry.Helper
{
    public class BaseService
    {
        private EsemkaLaundryContext db = new EsemkaLaundryContext();
        public User user;

        public BaseService(EsemkaLaundryContext db, string email)
        {
            this.db = db;
            user = db.Users.Where(s => s.Email == email).FirstOrDefault();
        }

        public Response response(int code, string? message = null, object? data = null)
        {
            try
            {
                return new Response
                {
                    Code = code,
                    Message = message,
                    Data = data
                };
            }
            catch (Exception x)
            {
                return new Response
                {
                    Code = 500,
                    Message = x.Message,
                    Data = x.Data
                };
            }
        }
    }
}