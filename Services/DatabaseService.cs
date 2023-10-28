using movies_api.Contracts.ServiceInterfaces;

namespace movies_api.Services
{
    public class DatabaseService : IDatabaseService
    {
        public string ConnectionString => _connectionString;

        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public DatabaseService(
            IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("postgres");
        }

    }
}
