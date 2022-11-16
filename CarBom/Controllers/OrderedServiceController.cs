using CarBom.DTO;
using CarBom.Mappers;
using CarBom.Responses;
using DataProvider.DataModels;
using DataProvider.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderedServiceController : ControllerBase
    {
        private readonly IOrderedServiceRepository _orderedServiceRepository;
        private readonly IOrderedServiceMapper _orderedServiceMapper;

        public OrderedServiceController(IOrderedServiceRepository orderedServiceRepository, IOrderedServiceMapper orderedServiceMapper)
        {
            _orderedServiceRepository = orderedServiceRepository;
            _orderedServiceMapper = orderedServiceMapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderedServiceDTO orderedServiceDTO)
        {
            if (orderedServiceDTO is not null)
            {
                try
                {
                    _orderedServiceRepository.Post(orderedServiceDTO.ServiceId, orderedServiceDTO.UserId, orderedServiceDTO.MechanicId);
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
        public ActionResult<IEnumerable<OrderedServiceResponse>> Get([FromQuery] string userId)
        {
            var orderedServices = _orderedServiceRepository.Get(userId);

            if (orderedServices is not null)
            {
                var orderedServicesMapped = _orderedServiceMapper.MapOrderedServices(orderedServices);
                return Ok(orderedServicesMapped);
            }
            else
                return NotFound();
        }
    }
}
