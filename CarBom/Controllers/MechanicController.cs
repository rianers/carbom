using CarBom.DTO;
using CarBom.Mappers;
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
        public MechanicController(IMechanicRepository mechanicRepository)
        {
            _mechanicRepository = mechanicRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Mechanic>> Get([FromQuery] MechanicListDTO mechanicListDTO, [FromServices] IMechanicMapper mechanicMapper)
        {
            List<Mechanic>? mechanicsResponse = _mechanicRepository.GetAll();

            var mechanics = mechanicMapper.MapMechanics(mechanicsResponse, mechanicListDTO.UserLatitude, mechanicListDTO.UserLongitude).AsQueryable();

            if (mechanicListDTO.Id is not null)
                mechanics = mechanics.Where(m => m.Id.Contains(mechanicListDTO.Id));

            if (mechanicListDTO.Name is not null)
                mechanics = mechanics.Where(m => m.Name.ToUpper().Contains(mechanicListDTO.Name.ToUpper()));

            if (mechanicListDTO.Services is not null)
                mechanics = mechanics.Where(m => m.Services.Where(s => mechanicListDTO.Services.Contains(s.Name)).Any());

            return mechanics.Any() ? Ok(mechanics.OrderBy(m => m.Distance)) : NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody] MechanicDTO mechanicDTO)
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

                    _mechanicRepository.Post(mechanic);
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
