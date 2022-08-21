using EsemkaLaundry.Helper;
using EsemkaLaundry.Services;
using EsemkaLaundry.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EsemkaLaundry.Helper.EnumCollection;

namespace EsemkaLaundry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private ServicesService service;

        public ServiceController(EsemkaLaundryContext db, IConfiguration configuration, IHttpContextAccessor http)
        {
            var header = http.HttpContext.Request.Headers["Authorization"];
            var email = header == string.Empty ? string.Empty : db.getUser(header);
            service = new ServicesService(db, configuration, email);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServiceViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int page, string? name, ServiceCategory? category, ServiceUnit? unit, double minPrice, double maxPrice)
        {
            try
            {
                var result = await service.GetService(page, name, category, unit, minPrice, maxPrice);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result.Message);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result.Message);
                return Ok(result);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(Guid id)
        {
            try
            {
                var result = await service.GetById(id);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result.Message);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                return Ok(result);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ServiceViewModel request)
        {
            try
            {
                var result = await service.Post(request);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result.Message);
                if (result.Code == StatusCodes.Status409Conflict) return Conflict(result);
                return Created("Service", result.Data);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ServiceViewModel request)
        {
            try
            {
                var result = await service.Put(id, request);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result.Message);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                if (result.Code == StatusCodes.Status409Conflict) return Conflict(result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                return Ok("Success");
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await service.Delete(id);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result.Message);
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