﻿using movies_api.Contracts.DTOs;
using movies_api.Contracts.RepositoryInterfaces;
using movies_api.Models;
using Npgsql;

namespace movies_api.DAL
{
    public class TitleRepository : IRepository<TitleDto>
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private const string GetListQuery = "SELECT * FROM \"GetTitleList\"(@limit, @cursor);";

        public TitleRepository(
            IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("postgres");
        }

        public async Task<List<TitleDto>> GetList(string? cursor, int pageSize)
        {
            List<TitleDto> titles = new ();

            using NpgsqlConnection connection = new(_connectionString);

            NpgsqlCommand command = new(GetListQuery, connection);
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