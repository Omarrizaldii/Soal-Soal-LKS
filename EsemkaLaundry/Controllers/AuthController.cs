using EsemkaLaundry.Services;
using EsemkaLaundry.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EsemkaLaundry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        EsemkaLaundryContext db = new EsemkaLaundryContext();
        IConfiguration config;
        AuthService service;
        public AuthController(EsemkaLaundryContext db,IConfiguration configuration, IHttpContextAccessor http)
        {
            var header = http.HttpContext.Request.Headers["Authorization"];
            var email = header == string.Empty ? string.Empty : db.getUser(header);
            service = new AuthService(db, configuration, email);
        }
        [HttpPost]
        public async Task<IActionResult> Auth(AuthViewModel request)
        {
            try
            {
                var result = await service.Auth(request);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500,result);
                return Ok(result.Data);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }
    }
}
