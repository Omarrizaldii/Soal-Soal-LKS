using EsemkaLaundry.Services;
using EsemkaLaundry.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EsemkaLaundry.Controllers
{
    [Route("api/Packages/Transactions")]
    [ApiController]
    public class PackageTransactionController : ControllerBase
    {
        private PackageTransactionService service;

        public PackageTransactionController(EsemkaLaundryContext db, IHttpContextAccessor http)
        {
            var header = http.HttpContext.Response.Headers["Authorization"];
            var email = header == string.Empty ? string.Empty : db.getUser(header);
            service = new PackageTransactionService(db, email);
        }

        [HttpGet]
        public async Task<ActionResult<PackageTransaction>> GetPackageTransaction(int page, string? keyword)
        {
            try
            {
                var result = await service.GetPackageTransaction(page, keyword);
                if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
                return Ok(result.Data);
            }
            catch (Exception x)
            {
                return StatusCode(500, x.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(Guid id)
        {
            var result = await service.GetById(id);
            if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
            if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<PackageTransaction>> PostPackage(PackageTransactionViewModel request)
        {
            var result = await service.PostPackageTransaction(request);
            if (result.Code == StatusCodes.Status500InternalServerError) return StatusCode(500, result);
            if (result.Code == StatusCodes.Status404NotFound) return NotFound(result);
            return Created("EsemkaLaundry", result.Data);
        }
    }
}