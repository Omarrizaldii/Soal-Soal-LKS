using EsemkaLaundry.Services;
using EsemkaLaundry.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EsemkaLaundry.Helper.EnumCollection;

namespace EsemkaLaundry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private EsemkaLaundryContext db = new EsemkaLaundryContext();
        private UserService service;

        public UserController(EsemkaLaundryContext db, IConfiguration configuration, IHttpContextAccessor http)
        {
            var header = http.HttpContext.Request.Headers["Authorization"];
            var email = header == string.Empty ? string.Empty : db.getUser(header);
            service = new UserService(db, configuration, email);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(int page, string? keyword, UserGender? gender = null)
        {
            try
            {
                var result = await service.GetUser(keyword, page, gender);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status401Unauthorized) return Unauthorized(result);
                return Ok(result);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [Authorize]
        [HttpGet("{email}")]
        public async Task<IActionResult> GetEmail(string email)
        {
            try
            {
                var result = await service.GetUserbyEmail(email);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status401Unauthorized) return Unauthorized(result);
                return Ok(result);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(UserViewModel request)
        {
            try
            {
                var result = await service.PostUser(request);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                if (result.Code == StatusCodes.Status409Conflict) return StatusCode(409, result);
                if (result.Code == StatusCodes.Status403Forbidden) return StatusCode(403, result);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result); ;
                if (result.Code == StatusCodes.Status401Unauthorized) return Unauthorized(result);
                return Created("User", result.Data);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [Authorize]
        [HttpPut("{email}")]
        public async Task<IActionResult> Put(string email, UserViewModel request)
        {
            try
            {
                var result = await service.PutUser(email, request);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                if (result.Code == StatusCodes.Status409Conflict) return Conflict(result);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status401Unauthorized) return Unauthorized(result);
                if (result.Code == StatusCodes.Status403Forbidden) return StatusCode(403, result);
                return Ok("Success");
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [Authorize]
        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                var result = await service.DeleteUser(email);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status401Unauthorized) return Unauthorized(result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                return Ok("Success");
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }
    }
}