using Microsoft.AspNetCore.Mvc;
using movies_api.Interfaces;
using movies_api.Models;
using Npgsql;

namespace movies_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private IDatabaseService _databaseService;
        public TitlesController(
            IDatabaseService databaseService) 
        { 
            _databaseService = databaseService;
        }

        [Route("list")]
        [HttpGet]
        public ActionResult<List<Title>> GetTitleList()
        {
            string query = "SELECT * FROM \"GetTitleList\"(@limit);";

            List<Title> titles = new List<Title>();
            ActionResult result;

            // using - naredba koja osigurava uništavanje objekta danog kao argument
            // Ne želimo biti stalno spojeni na bazu, niti želimo memory leakove.
            using (NpgsqlConnection connection = new NpgsqlConnection(_databaseService.ConnectionString()))
            {
                // Stvaramo naredbu koju ćemo okinuti o bazu i dodajemo vrijednost @limit parametru u queryString-u
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@limit", 5);

                try
                {
                    connection.Open();

                    // ExecuteReader stvara reader kako bismo iščitali redove iz tablice i daje mu query iz naredbe.
                    NpgsqlDataReader reader = command.ExecuteReader();
                    // Read metoda pomiče reader na sljedeći red.
                    while (reader.Read())
                    {
                        Title title = new Title
                        {
                            Id = reader["title_id"].ToString(),
                            ContentTypeId = reader["content_type_id"].Equals(DBNull.Value) ? null : Convert.ToInt32(reader["content_type_id"]),
                            PrimaryTitle = reader["primary_title"].Equals(DBNull.Value) ? null : reader["primary_title"].ToString(),
                            OriginalTitle = reader["original_title"].Equals(DBNull.Value) ? null : reader["original_title"].ToString(),
                            IsAdult = reader["is_adult"].Equals(DBNull.Value) ? null : Convert.ToInt32(reader["is_adult"]),
                            StartYear = reader["start_year"].Equals(DBNull.Value) ? null : Convert.ToInt32(reader["start_year"]),
                            EndYear = reader["end_year"].Equals(DBNull.Value) ? null : Convert.ToInt32(reader["end_year"]),
                            RuntimeMinutes = reader["runtime_minutes"].Equals(DBNull.Value) ? null : Convert.ToInt32(reader["runtime_minutes"])
                        };
                        titles.Add(title);
                    }
                    // Nakon što više nema redova zatvaramo reader.
                    reader.Close();

                    result = Ok(titles);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    result = StatusCode(500);
                }
            }

            return result;
        }
    }
}