using Dapper;
using DataProvider.DataModels;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataProvider.Repositories
{
    public class LoginRepository : DBContext, ILoginRepository
    {
        public LoginRepository(IConfiguration configuration) : base(configuration) { }

        public async Task Post(User user)
        {
            string query = @"INSERT INTO Customer (name, email, password, createdAt, modifiedAt) 
                                         VALUES(@name, @email, @password, GETDATE(), GETDATE())";

            var param = new
            {
                user.Name,
                user.Email,
                user.Password
            };

            using var con = new SqlConnection(GetConnection());

            await con.OpenAsync();

            await con.ExecuteAsync(query, param, commandType: System.Data.CommandType.Text);

            await con.CloseAsync();
        }


        public async Task<string> Get(string email, string password)
        {
            string? userId = null;

            using (SqlConnection sqlConnection = new SqlConnection(GetConnection()))
            {
                string query = @"SELECT CAST (id as VARCHAR) as id FROM Customer WHERE email = @email and password = @password";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlParameter emailParam = new SqlParameter
                {
                    ParameterName = "@email",
                    Value = email
                };
                sqlCommand.Parameters.Add(emailParam);

                SqlParameter passwordParam = new SqlParameter
                {
                    ParameterName = "@password",
                    Value = password
                };
                sqlCommand.Parameters.Add(passwordParam);

                sqlConnection.Open();
                using (SqlDataReader oReader = sqlCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        userId = oReader["id"] is DBNull ? null : oReader["id"].ToString();
                    }
                    sqlConnection.Close();
                }
            }
            return userId;
        }

    }

    public interface ILoginRepository
    {
        Task Post(User user);
        Task<string> Get(string email, string password);
    }
}
