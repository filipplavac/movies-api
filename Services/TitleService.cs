using movies_api.Contracts.Dtos;
using movies_api.Contracts.Results;
using movies_api.Contracts.ServiceInterfaces;
using movies_api.Models;
using Npgsql;

namespace movies_api.Services
{
    public class TitleService : IDatabaseService<TitleDto>
    {
        public string ConnectionString => _connectionString;

        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        private NpgsqlDataSource _dataSource;

        public TitleService(
            IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("postgres");
            _dataSource = NpgsqlDataSource.Create(_connectionString);
        }

        public async Task<List<TitleDto>> GetList(string cursor, int pageSize)
        {
            List<TitleDto> titles = new();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string query = "SELECT * FROM \"GetTitleList\"(@limit, @cursor);";

                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@limit", pageSize + 1);
                command.Parameters.AddWithValue("@cursor", cursor != null ? cursor : DBNull.Value);

                connection.Open();

                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Title title = Title.FromRecord(reader);
                    titles.Add(title.ToDto());
                }

                reader.Close();

                return titles;
            }
        }

        //public async Task<TitleDto> Get(string id)
        //{
    
        //}

        //public async Task<TitleDto> Create(TitleDto title)
        //{

        //}

        //public async void Update(TitleDto title)
        //{

        //}

        //public async void Delete(string id)
        //{

        //}
    }
}
