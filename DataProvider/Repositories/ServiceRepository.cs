using Dapper;
using DataProvider.DataModels;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataProvider.Repositories
{
    public class ServiceRepository : DBContext, IServiceRepository
    {
        public ServiceRepository(IConfiguration configuration) : base(configuration) { }

        public async Task Post(Service service, string mechanicId)
        {
            string query = @"INSERT INTO Service (name, price, image, createdAt, modifiedAt, mechanic_service_id)
                                VALUES (@name, @price, @image, GETDATE(), GETDATE(), @mechanicId)";

            var param = new
            {
                service.Name,
                service.Price,
                service.Image,
                mechanicId
            };

            using var con = new SqlConnection(GetConnection());

            await con.OpenAsync();

            await con.ExecuteAsync(query, param, commandType: System.Data.CommandType.Text);

            await con.CloseAsync();
        }

        public List<Service> Get(string mechanicId)
        {
            List<Service> services = new List<Service>(0);

            using (SqlConnection sqlConnection = new SqlConnection(GetConnection()))
            {
                string query = @"SELECT id,
                                        name,
                                        price,
                                        image
                                 FROM service
                                WHERE mechanic_service_id = @mechanicId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlParameter param = new SqlParameter
                {
                    ParameterName = "@mechanicId",
                    Value = mechanicId
                };
                sqlCommand.Parameters.Add(param);

                sqlConnection.Open();
                using (SqlDataReader oReader = sqlCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Service service = new Service
                        {
                            Id = oReader["id"].ToString(),
                            Name = oReader["name"].ToString(),
                            Price = oReader["price"] is DBNull ? null : Convert.ToDouble(oReader["price"]),
                            Image = oReader["image"].ToString()
                        };

                        services.Add(service);
                    }
                    sqlConnection.Close();
                }
            }
            return services;
        }
    }

    public interface IServiceRepository
    {
        Task Post(Service service, string mechanicId);
        List<Service> Get(string mechanicId);
    }
}
