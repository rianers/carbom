using CarBom.DTO;
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
        public IActionResult Post([FromBody] ServiceDTO serviceDTO)
        {
            if (serviceDTO is not null)
            {
                try
                {
                    Service service = new Service
                    {
                        Name = serviceDTO.Name,
                        Image = serviceDTO.Image,
                        Price = serviceDTO.Price
                    };

                    _serviceRepository.Post(service);
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
        public ActionResult<IEnumerable<Service>> Get([FromQuery] string mechanicId)
        {
            var services = _serviceRepository.Get(mechanicId);

            if (services is not null)
                return Ok(services);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("orderservice")]
        public IActionResult Post([FromBody] OrderedServiceDTO orderedServiceDTO)
        {
            //TODO: Create new table in DB (log? ordered table?) and use a new context
            if (orderedServiceDTO is not null)
            {
                return Ok();
            }
            return Ok();
        }
    }
}
