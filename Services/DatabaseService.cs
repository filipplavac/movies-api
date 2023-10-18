using movies_api.Interfaces;

namespace movies_api.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public DatabaseService(
            IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("postgres");
        }

        public string ConnectionString()
        {
            return _connectionString;
        }
    }
}
