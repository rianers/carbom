using Dapper;
using DataProvider.DataModels;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataProvider.Repositories
{
    public class MechanicRepository : DBContext, IMechanicRepository
    {
        public MechanicRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<int> Post(Mechanic mechanic)
        {
            string query = @"INSERT INTO Mechanic (name, description, distance, ranking, createdAt, modifiedAt) 
                                         VALUES(@name, @description, @distance, @ranking, GETDATE(), GETDATE());
                             SELECT CAST(SCOPE_IDENTITY() as int)";

            var param = new
            {
                mechanic.Name,
                mechanic.Description,
                mechanic.Distance,
                mechanic.Ranking
            };

            using var con = new SqlConnection(GetConnection());

            await con.OpenAsync();

            var result = await con.QueryAsync<int>(query, param);

            await con.CloseAsync();

            return result.Single();
        }

        public List<Mechanic> GetAll()
        {
            List<Mechanic> mechanics = new List<Mechanic>(0);

            using (SqlConnection sqlConnection = new SqlConnection(GetConnection()))
            {
                string query = @"SELECT DISTINCT mc.id as mechanicId,
                                    mc.name as mechanicName, 
                                    mc.image as mechanicImage,
                                    mc.ranking as mechanicRanking,
                                    mc.description as mechanicDescription,
									sv.id as serviceId,
                                    sv.name as serviceName,
									sv.price as servicePrice,
									sv.image as serviceImage,
									ad.latitude as mechanicLatitude,
									ad.longitude as mechanicLongitude,
                                    ad.state as state,
                                    ad.city as city,
                                    ad.street as street,
                                    ad.number as number,
                                    ad.neighbourhood as neighbourhood,
                                    ad.zippostalcode as zippostalcode
                            FROM Mechanic mc
							LEFT JOIN (SELECT * FROM Address) AS ad
							ON mc.id = ad.mechanic_address_id
							LEFT JOIN (SELECT id, mechanic_service_id, name, price, image FROM Service) AS sv
							ON mc.id = mechanic_service_id";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();

                using (SqlDataReader oReader = sqlCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Mechanic mechanic = new Mechanic()
                        {
                            Address = new Address(),
                            Services = new List<Service>() { new Service() }
                        };

                        mechanic.Id = oReader["mechanicId"].ToString();
                        mechanic.Name = oReader["mechanicName"].ToString();
                        mechanic.Image = oReader["mechanicImage"] is DBNull ? null : oReader["mechanicImage"].ToString();
                        mechanic.Ranking = oReader["mechanicRanking"] is DBNull ? null : Convert.ToDouble(oReader["mechanicRanking"]);
                        mechanic.Description = oReader["mechanicDescription"] is DBNull ? null : oReader["mechanicDescription"].ToString();

                        mechanic.Services[0].Id = oReader["serviceId"] is DBNull ? null : oReader["serviceId"].ToString();
                        mechanic.Services[0].Name = oReader["serviceName"] is DBNull ? null : oReader["serviceName"].ToString();
                        mechanic.Services[0].Price = oReader["servicePrice"] is DBNull ? null : Convert.ToDouble(oReader["servicePrice"]);
                        mechanic.Services[0].Image = oReader["serviceImage"] is DBNull ? null : oReader["serviceImage"].ToString();

                        mechanic.Address.Latitude = oReader["mechanicLatitude"] is DBNull ? 0 : Convert.ToDouble(oReader["mechanicLatitude"]);
                        mechanic.Address.Longitude = oReader["mechanicLongitude"] is DBNull ? 0 : Convert.ToDouble(oReader["mechanicLongitude"]);
                        mechanic.Address.State = oReader["state"] is DBNull ? null : oReader["state"].ToString();
                        mechanic.Address.City = oReader["city"] is DBNull ? null : oReader["city"].ToString();
                        mechanic.Address.Street = oReader["street"] is DBNull ? null : oReader["street"].ToString();
                        mechanic.Address.Number = oReader["number"] is DBNull ? null : oReader["number"].ToString();
                        mechanic.Address.Neighbourhood = oReader["neighbourhood"] is DBNull ? null : oReader["neighbourhood"].ToString();
                        mechanic.Address.ZipPostalCode = oReader["zippostalcode"] is DBNull ? null : oReader["zippostalcode"].ToString();

                        mechanics.Add(mechanic);
                    }
                    sqlConnection.Close();
                }
            }
            return mechanics;
        }
    }

    public interface IMechanicRepository
    {
        Task<int> Post(Mechanic mechanic);
        List<Mechanic> GetAll();
    }
}
