using Microsoft.AspNetCore.Mvc;
using movies_api.Contracts.ServiceInterfaces;
using movies_api.Models;
using movies_api.Contracts.Results;
using Npgsql;
using movies_api.Contracts.Dtos;

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
            List<TitleDto> titles = new ();
            string? nextCursor;

            // using - the using statement defines a scope at the end of which an object is disposed.
            // The database connection must be disposed in order to prevent memory leaks.
            using (NpgsqlConnection connection = new NpgsqlConnection(_databaseService.ConnectionString))
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
                        Title title = Title.FromRecord(reader);
                        titles.Add(title.ToDto());
                    }
                    // Close the reader after there are no more results.
                    reader.Close();

                    // Set the cursor to the id of the last result in titles. Set it to null if last page was recieved from the database.
                    nextCursor = titles.Count > pageSize ? titles.Last().Id : null;
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