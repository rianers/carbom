using CarBom.DTO;
using CarBom.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] ServiceDTO serviceDTO)
        {
            return Ok(new Service());
        }
    }
}
