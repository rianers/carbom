using Microsoft.Extensions.Configuration;

namespace DataProvider
{
    public class DBContext
    {
        private readonly IConfiguration _configuration;

        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected string GetConnection() => _configuration.GetSection("ConnectionStrings").GetSection("CarBom_DB").Value;
    }
}