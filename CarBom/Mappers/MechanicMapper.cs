using CarBom.Utils;
using DataProvider.DataModels;

namespace CarBom.Mappers
{
    public class MechanicMapper : IMechanicMapper
    {
        public List<Mechanic> MapMechanics(List<Mechanic> mechanics, double userLat, double userLong)
        {
            List<Mechanic> result = new List<Mechanic>();

            if (mechanics is not null)
            {
                for (int mechIndex = 0; mechIndex < mechanics.Count(); mechIndex++)
                {
                    int existingMechanicIndex = result.FindIndex(m => m.Id == mechanics[mechIndex].Id);

                    if (existingMechanicIndex is -1) //Check if the mechanic is already mapped
                    {
                        var mechanic = new Mechanic()
                        {
                            Id = mechanics[mechIndex].Id,
                            Name = mechanics[mechIndex].Name,
                            Image = mechanics[mechIndex].Image,
                            Ranking = mechanics[mechIndex].Ranking,
                            Description = mechanics[mechIndex].Description,
                            Services = new List<Service>()
                        };
                        MapAddress(ref mechanic, mechIndex, mechanics, userLat, userLong);

                        Service service = MapService(mechIndex, mechanics);
                        mechanic.Services?.Add(service);

                        result.Add(mechanic);
                    }
                    else //If is already mapped, map only the service
                    {
                        var service = MapService(mechIndex, mechanics);
                        result[existingMechanicIndex].Services?.Add(service);
                    }
                }
            }
            return result;
        }

        private void MapAddress(ref Mechanic mechanic, int mechIndex, List<Mechanic> mechanics, double userLat, double userLong)
        {
            var address = mechanics[mechIndex].Address;
            if (address is not null)
            {
                mechanic.Address = new Address();
                mechanic.Address.Latitude = address.Latitude;
                mechanic.Address.Longitude = address.Longitude;
                mechanic.Distance = DistanceGeneratorUtil.DistanceTo(userLat, userLong, address.Latitude, address.Longitude);
            }
        }

        private Service MapService(int mechIndex, List<Mechanic> mechanics)
        {
            var service = new Service()
            {
                Name = mechanics[mechIndex].Services?[0].Name,
                Price = mechanics[mechIndex].Services?[0].Price,
                Image = mechanics[mechIndex].Services?[0].Image
            };
            return service;
        }
    }

    public interface IMechanicMapper
    {
        List<Mechanic> MapMechanics(List<Mechanic> mechanics, double userLat, double userLong);
    }
}
