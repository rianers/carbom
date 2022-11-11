using CarBom.DTO;
using DataProvider.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderedServiceController : ControllerBase
    {
        private readonly IOrderedServiceRepository _orderedServiceRepository;

        public OrderedServiceController(IOrderedServiceRepository orderedServiceRepository)
        {
            _orderedServiceRepository = orderedServiceRepository;
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
    }
}
