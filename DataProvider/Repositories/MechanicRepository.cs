using Dapper;
using DataProvider.DataModels;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataProvider.Repositories
{
    public class MechanicRepository : DBContext, IMechanicRepository
    {
        public MechanicRepository(IConfiguration configuration) : base(configuration) { }

        public async Task Post(Mechanic mechanic)
        {
            string query = @"INSERT INTO Mechanic (name, description, distance, ranking, createdAt, modifiedAt) 
                                         VALUES(@name, @description, @distance, @ranking, GETDATE(), GETDATE());";

            var param = new
            {
                mechanic.Name,
                mechanic.Description,
                mechanic.Distance,
                mechanic.Ranking
            };

            using var con = new SqlConnection(GetConnection());

            await con.OpenAsync();

            await con.ExecuteAsync(query, param, commandType: System.Data.CommandType.Text);

            await con.CloseAsync();
        }

        public List<Mechanic> GetAll()
        {
            List<Mechanic> mechanics = new List<Mechanic>(0);

            using (SqlConnection sqlConnection = new SqlConnection(GetConnection()))
            {
                string query = @"SELECT mc.id as mechanicId,
                                    mc.name as mechanicName, 
                                    mc.image as mechanicImage,
                                    mc.ranking as mechanicRanking,
                                    mc.description as mechanicDescription,
                                    sv.name as serviceName,
									sv.price as servicePrice,
									sv.image as serviceImage,
									ad.latitude as mechanicLatitude,
									ad.longitude as mechanicLongitude
                            FROM Mechanic mc
							LEFT JOIN (SELECT mechanic_address_id, latitude, longitude FROM [dbo].[Address]) AS ad
							ON mc.id = ad.mechanic_address_id
							LEFT JOIN (SELECT mechanic_service_id, name, price, image FROM [dbo].[Service]) AS sv
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

                        //TODO: mechanic needs to have a name (fluent validator)
                        mechanic.Id = oReader["mechanicId"].ToString();
                        mechanic.Name = oReader["mechanicName"].ToString();
                        mechanic.Image = oReader["mechanicImage"] is DBNull ? null : oReader["mechanicImage"].ToString();
                        mechanic.Ranking = oReader["mechanicRanking"] is DBNull ? null : Convert.ToDouble(oReader["mechanicRanking"]);
                        mechanic.Description = oReader["mechanicDescription"] is DBNull ? null : oReader["mechanicDescription"].ToString();

                        mechanic.Services[0].Name = oReader["serviceName"] is DBNull ? null : oReader["serviceName"].ToString();
                        mechanic.Services[0].Price = oReader["servicePrice"] is DBNull ? null : Convert.ToDouble(oReader["servicePrice"]);
                        mechanic.Services[0].Image = oReader["serviceImage"] is DBNull ? null : oReader["serviceImage"].ToString();

                        //TODO: mechanic NEEDS to have a latitude and longitude (fluent validator)
                        mechanic.Address.Latitude = oReader["mechanicLatitude"] is DBNull ? 0 : Convert.ToDouble(oReader["mechanicLatitude"]); //remove this condition once the rule validator for address is applied
                        mechanic.Address.Longitude = oReader["mechanicLongitude"] is DBNull ? 0 : Convert.ToDouble(oReader["mechanicLongitude"]); //remove this condition once the rule validator for address is applied

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
        Task Post(Mechanic mechanic);
        List<Mechanic> GetAll();
    }
}
