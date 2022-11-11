using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

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
    }

    public interface IOrderedServiceRepository
    {
        Task Post(string serviceId, string userId, string mechanicId);
    }
}
