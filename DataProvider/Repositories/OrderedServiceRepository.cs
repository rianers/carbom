using Dapper;
using DataProvider.DataModels;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Globalization;

namespace DataProvider.Repositories
{
    public class OrderedServiceRepository : DBContext, IOrderedServiceRepository
    {
        public OrderedServiceRepository(IConfiguration configuration) : base(configuration) { }

        public async Task Post(string serviceId, string userId, string mechanicId)
        {
            string query = @"INSERT INTO OrderedService (service_id, customer_id, mechanic_id, createdAt, modifiedAt)
                                VALUES (@serviceId, @userId, @mechanicId, GETDATE(), GETDATE())";

            var param = new
            {
                serviceId,
                userId,
                mechanicId
            };

            using var con = new SqlConnection(GetConnection());

            await con.OpenAsync();

            await con.ExecuteAsync(query, param, commandType: System.Data.CommandType.Text);

            await con.CloseAsync();
        }

        public List<OrderedService> Get(string userId)
        {
            List<OrderedService> orderedServices = new List<OrderedService>(0);

            using (SqlConnection sqlConnection = new SqlConnection(GetConnection()))
            {
                string query = @"SELECT mc.name as mechanicName,
                                        sv.name as serviceName,
				                        os.createdAt as createdDate
                                 FROM mechanic mc
                                 INNER JOIN (SELECT service_id, mechanic_id, createdAt 
                                 FROM OrderedService 
                                 WHERE customer_id = @userId) AS os
                                 ON os.mechanic_id = mc.id
                                 LEFT JOIN (SELECT id, name FROM SERVICE) AS sv
                                 ON sv.id = os.service_id";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlParameter param = new SqlParameter
                {
                    ParameterName = "@userId",
                    Value = userId
                };
                sqlCommand.Parameters.Add(param);

                sqlConnection.Open();
                using (SqlDataReader oReader = sqlCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        OrderedService orderedService = new OrderedService
                        {
                            Name = oReader["serviceName"].ToString(),
                            Mechanic = oReader["mechanicName"].ToString(),
                            CreatedDate = Convert.ToDateTime(oReader["createdDate"])
                        };

                        orderedServices.Add(orderedService);
                    }
                    sqlConnection.Close();
                }
            }
            return orderedServices;
        }

    }

    public interface IOrderedServiceRepository
    {
        Task Post(string serviceId, string userId, string mechanicId);
        List<OrderedService> Get(string userId);
    }
}
