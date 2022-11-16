using CarBom.Requests;
using DataProvider.DataModels;
using DataProvider.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] ServiceRequest serviceRequest, string mechanicId)
        {
            if (serviceRequest is not null && mechanicId is not null)
            {
                try
                {
                    Service service = new Service
                    {
                        Name = serviceRequest.Name,
                        Image = serviceRequest.Image,
                        Price = serviceRequest.Price
                    };

                    _serviceRepository.Post(service, mechanicId);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Service>> Get([FromQuery] string mechanicId)
        {
            var services = _serviceRepository.Get(mechanicId);

            if (services is not null)
                return Ok(services);
            else
                return NotFound();
        }
    }
}
