using movies_api.Contracts.DTOs;
using movies_api.Contracts.ServiceInterfaces;
using movies_api.Models;
using Npgsql;

namespace movies_api.DAL
{
    // Single responsibility: interact with the database.
    public class TitleRepository : Db, IRepository<TitleDto>
    {

        // Dependencies
        private readonly IModelMapper<Title, TitleDto> _titleMapper;
        // Queries
        private const string GetListQuery = "SELECT * FROM \"GetTitleList\"(@limit, @cursor);";

        public TitleRepository(
            IConfiguration configuration,
            IModelMapper<Title,TitleDto> titleMapper) : base(configuration)
        {
            _titleMapper = titleMapper;
        }
  
        public async Task<List<TitleDto>> GetList(string? cursor, int pageSize)
        {
            List<TitleDto> titles = new ();

            using NpgsqlConnection connection = CreatePostgresConnection();
            NpgsqlCommand command = CreatePostgresCommand(GetListQuery, connection);

            command.Parameters.AddWithValue("@limit", pageSize + 1);
            command.Parameters.AddWithValue("@cursor", cursor != null ? cursor : DBNull.Value);

            connection.Open();
            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Title title = _titleMapper.RecordToModel(reader);
                titles.Add(_titleMapper.ModelToDto(title));
            }
            reader.Close();

            return titles;
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
