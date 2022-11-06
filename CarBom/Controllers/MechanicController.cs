using CarBom.DTO;
using CarBom.Models;
using CarBom.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CarBom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MechanicController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Mechanic>> Get([FromQuery] MechanicListDTO mechanicListDTO)
        {
            IEnumerable<Mechanic> mechanics = GetDummyMechanics(mechanicListDTO.Latitude, mechanicListDTO.Longitude).AsQueryable();

            if (mechanicListDTO.Id is not null) //Id filtering
                mechanics = mechanics.Where(m => m.Id.Contains(mechanicListDTO.Id));

            if (mechanicListDTO.Name is not null) //name filtering
                mechanics = mechanics.Where(m => m.Name.ToUpper().Contains(mechanicListDTO.Name.ToUpper()));

            if (mechanicListDTO.Services is not null) //service filtering
                mechanics = mechanics.Where(m => m.Services.Where(s => mechanicListDTO.Services.Contains(s.Name)).Any());

            return mechanics.Any() ? Ok(mechanics.OrderBy(m => m.Distance)) : NotFound();
        }

        private List<Mechanic> GetDummyMechanics(double userLat, double userLong)
        {
            double lat1 = -23.494656; //latitude user
            double lon1 = -46.678016; //longitude user

            double lat2 = -23.498350;
            double lon2 = -46.674740;

            var distance = DistanceGeneratorUtil.DistanceTo(userLat, userLong, lat2, lon2);

            List<Mechanic> mechanics = new List<Mechanic>
            {
                new Mechanic
                {
                    Id = "1",
                    Name = "Mecânica do Seu Zé",
                    Ranking = 3.5,
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Id = "1",
                            Name = "Troca de óleo",
                            Price = 50
                        },
                        new Service
                        {
                            Id = "2",
                            Name = "Reparo Simples",
                            Price = 120
                        }
                    },
                    Distance = distance,
                    Description = "Mecânica especializada em serviços gerais e conserto. Entre já em contato!",
                    Address = new Address
                    {
                        State = "São Paulo",
                        City = "São Paulo",
                        Street = "Avenida Paulista",
                        Number = "2345",
                        Neighbourhood = "Aclimação",
                        ZipPostalCode = "02751000"
                    }
                },
                new Mechanic
                {
                    Id = "2",
                    Name = "Mecânica do Wills",
                    Ranking = 3.5,
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Id = "1",
                            Name = "Tinturaria",
                            Price = 50
                        }
                    },
                    Distance = distance,
                    Description = "Mecânica fundada há mais de 10 anos na cidade de São Paulo no bairro Vila Madalena. Somos especializados em serviços gerais e funilaria. Oferecemos um serviço de qualidade e por um ótimo preço. Faça já a sua avaliação!",
                    Address = new Address
                    {
                        State = "São Paulo",
                        City = "São Paulo",
                        Street = "Avenida Brasil",
                        Number = "2345",
                        Neighbourhood = "Aclimação",
                        ZipPostalCode = "02751000"
                    }
                }
            };

            return mechanics;
        }
    }
}
