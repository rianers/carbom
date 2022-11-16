using CarBom.Mappers;
using CarBom.Requests;
using CarBom.Responses;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] OrderedServiceRequest orderedServiceRequest)
        {
            if (orderedServiceRequest is not null)
            {
                try
                {
                    _orderedServiceRepository.Post(orderedServiceRequest.ServiceId, orderedServiceRequest.UserId, orderedServiceRequest.MechanicId);
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
