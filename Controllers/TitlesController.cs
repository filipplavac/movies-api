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
            string? nextCursor;

            // using - the using statement defines a scope at the end of which an object is disposed.
            // The database connection must be disposed in order to prevent memory leaks.
            using (NpgsqlConnection connection = new NpgsqlConnection(_databaseService.ConnectionString()))
            {
                string query = "SELECT * FROM \"GetTitleList\"(@limit, @cursor);";

                // First, declare and instantiate the command object, which will interact with the database.
                // Afterwards, we provide it with the values for parameters in the query.
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@limit", pageSize + 1);
                command.Parameters.AddWithValue("@cursor", cursor != null ? cursor : DBNull.Value);

                try
                {
                    connection.Open();

                    // ExecuteReader method executes the command text (query) against the connection.
                    // It also returns a reader object.
                    NpgsqlDataReader reader = command.ExecuteReader();
                    // The Read method advances the reader to the next record in the result set.
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
                    // Close the reader after there are no more results.
                    reader.Close();

                    // Set the cursor to the id of the last result in titles. Set it to null if last page was recieved.
                    nextCursor = titles.Count == pageSize ? titles.Last().Id : null;
                    // Remove the last result from titles if there are more pages left.
                    if (nextCursor != null)
                    {
                        titles.RemoveAt(titles.Count - 1);
                    }

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