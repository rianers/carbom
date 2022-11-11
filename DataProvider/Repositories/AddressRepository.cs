using Dapper;
using DataProvider.DataModels;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataProvider.Repositories
{
    public class AddressRepository : DBContext, IAddressRepository
    {
        public AddressRepository(IConfiguration configuration) : base(configuration) { }

        public async Task Post(Address address, string mechanicId)
        {
            string query = @"INSERT INTO Address (state, city, street, number, neighbourhood, zippostalcode, createdAt, modifiedAt, mechanic_address_id, latitude, longitude) 
                                         VALUES(@state, @city, @street, @number, @neighbourhood, @zippostalcode, GETDATE(), GETDATE(), @mechanicId, @latitude, @longitude)";

            var param = new
            {
                address.State,
                address.City,
                address.Street,
                address.Number,
                address.Neighbourhood,
                address.ZipPostalCode,
                mechanicId,
                address.Latitude,
                address.Longitude
            };

            using var con = new SqlConnection(GetConnection());

            await con.OpenAsync();

            await con.ExecuteAsync(query, param, commandType: System.Data.CommandType.Text);

            await con.CloseAsync();
        }
    }

    public interface IAddressRepository
    {
        Task Post(Address address, string mechanicId);
    }
}
