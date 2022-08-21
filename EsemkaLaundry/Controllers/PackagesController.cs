using EsemkaLaundry.Services;
using EsemkaLaundry.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EsemkaLaundry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private PackageService service;

        public PackagesController(EsemkaLaundryContext db, IHttpContextAccessor http)
        {
            var header = http.HttpContext.Response.Headers["Authorization"];
            var email = header == string.Empty ? string.Empty : db.getUser(header);
            service = new PackageService(db, email);
        }

        [HttpGet]
        public async Task<ActionResult<Package>> GetPackage([FromQuery] int page, int total, double minPrice, double maxPrice)
        {
            try
            {
                var result = await service.GetPackage(page, total, minPrice, maxPrice);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                return Ok(result.Data);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Package>> GetPackageId(Guid id)
        {
            try
            {
                var result = await service.GetPackageId(id);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                return Ok(result.Data);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Package>> PostPackage(PackageViewModel request)
        {
            try
            {
                var result = await service.PostPackage(request);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                return Created("EsemkaLaundry", result);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPackage(Guid id, PackageViewModel request)
        {
            try
            {
                var result = await service.PutPackage(id, request);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
                if (result.Code == StatusCodes.Status400BadRequest) return BadRequest(result);
                return Ok("Success");
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            try
            {
                var result = await service.DeletePackage(id);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
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