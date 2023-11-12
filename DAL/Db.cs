using Npgsql;
using System.Data;
using System.Data.Common;

namespace movies_api.DAL
{
    // Single responsibility: encapsulate common functionality for all data providers.
    public abstract class Db
    {
        protected readonly IConfiguration _configuration;

        public Db (IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public NpgsqlConnection CreatePostgresConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("postgres"));
        }

        public NpgsqlCommand CreatePostgresCommand(string query, NpgsqlConnection connection)
        {
            return new NpgsqlCommand(query, connection);
        }
    }
}
