using Microsoft.AspNetCore.Mvc;
using movies_api.Interfaces;
using movies_api.Models;
using movies_api.Results;
using Npgsql;

namespace movies_api.Controllers
{
    [Route("api/titles")]
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
        public ActionResult<TitleListResult> GetTitleList([FromQuery] string? cursor = null, [FromQuery] int pageSize = 10)
        {
            List<Title> titles = new ();
            string nextCursor;

            // using - naredba koja osigurava uništavanje objekta danog kao argument
            // Ne želimo biti stalno spojeni na bazu, niti želimo memory leakove.
            using (NpgsqlConnection connection = new NpgsqlConnection(_databaseService.ConnectionString()))
            {
                string query = "SELECT * FROM \"GetTitleList_Cursor\"(@limit, @cursor);";

                // Stvaramo naredbu koju ćemo okinuti o bazu i dodajemo vrijednost @limit parametru u queryString-u
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@limit", pageSize + 1);
                command.Parameters.AddWithValue("@cursor", cursor != null ? cursor : DBNull.Value);

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

                    // Izbaci zadnji redak i pošalji ga kao cursor.
                    nextCursor = titles.Last().Id;
                    titles.RemoveAt(titles.Count - 1);

                    return Ok(new TitleListResult(titles, nextCursor));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500);
                }
            }
        }
    }
}