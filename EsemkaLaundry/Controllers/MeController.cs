using EsemkaLaundry.Services;
using EsemkaLaundry.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EsemkaLaundry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private MeService service;

        public MeController(EsemkaLaundryContext db, IWebHostEnvironment web, IHttpContextAccessor http)
        {
            var header = http.HttpContext.Request.Headers["Authorization"];
            var email = header == string.Empty ? string.Empty : db.getUser(header);
            service = new MeService(db, web, email);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await service.GetMe();
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                return Ok(result);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PutMe(UserViewModel request)
        {
            try
            {
                var result = await service.PutMe(request);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                return Ok();
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [Authorize]
        [HttpPost("Photo")]
        public async Task<IActionResult> PostMe([FromForm] FileUpload request)
        {
            try
            {
                var result = await service.PostPhoto(request);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                return Ok();
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [Authorize]
        [HttpGet("Photo")]
        public async Task<IActionResult> GetPhoto()
        {
            try
            {
                var result = await service.GetMePhoto();
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                Byte[] b = System.IO.File.ReadAllBytes(result.Message);
                return File(b, "image/jpeg");
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }
    }
}