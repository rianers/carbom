using CarBom.Mappers;
using CarBom.Requests;
using DataProvider.DataModels;
using DataProvider.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MechanicController : ControllerBase
    {
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IAddressRepository _addressRepository;
        public MechanicController(IMechanicRepository mechanicRepository, IAddressRepository addressRepository)
        {
            _mechanicRepository = mechanicRepository;
            _addressRepository = addressRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Mechanic>> Get([FromQuery] MechanicListRequest mechanicListDTO, [FromServices] IMechanicMapper mechanicMapper)
        {
            List<Mechanic>? mechanicsResponse = _mechanicRepository.GetAll();

            var mechanics = mechanicMapper.MapMechanics(mechanicsResponse, mechanicListDTO.UserLatitude, mechanicListDTO.UserLongitude).AsQueryable();

            if (mechanicListDTO.Id is not null)
                mechanics = mechanics.Where(m => m.Id.Contains(mechanicListDTO.Id));

            if (mechanicListDTO.Name is not null)
                mechanics = mechanics.Where(m => m.Name.ToUpper().Contains(mechanicListDTO.Name.ToUpper()));

            if (mechanicListDTO.Services is not null)
                mechanics = mechanics.Where(m => m.Services.Where(s => mechanicListDTO.Services.Contains(s.Name)).Any());

            return mechanics.Any() ? Ok(mechanics) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] MechanicRequest mechanicDTO)
        {
            if (mechanicDTO is not null)
            {
                try
                {
                    Mechanic mechanic = new Mechanic
                    {
                        Name = mechanicDTO.Name,
                        Description = mechanicDTO.Description,
                        Address = mechanicDTO.Address,
                        Image = mechanicDTO.Image,
                        Ranking = mechanicDTO.Ranking
                    };

                    int mechanicId = await _mechanicRepository.Post(mechanic);
                    await _addressRepository.Post(mechanic.Address, mechanicId.ToString());
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
