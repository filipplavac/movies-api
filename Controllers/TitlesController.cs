using Microsoft.AspNetCore.Mvc;
using movies_api.Models;
using Npgsql;

namespace movies_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        [Route("list")]
        [HttpGet]
        public ActionResult<List<Title>> GetTitlesList()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=FFFfff1#;Database=imdb";
            string queryString = "SELECT * FROM titles" +
                "LIMIT @limit;";
            int limit = 5;


            List<Title> titles = new List<Title>();
            ActionResult result;

            // using - naredba koja osigurava uništavanje objekta danog kao argument
            // Ne želimo biti stalno spojeni na bazu, niti želimo memory leakove.
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                // Stvaramo naredbu koju ćemo okinuti o bazu i dodajemo vrijednost @limit parametru u queryString-u
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@limit", limit);

                try
                {
                    connection.Open();

                    // ExecuteReader stvara reader kako bismo iščitali redove iz tablice i daje mu queryString iz naredbe.
                    NpgsqlDataReader reader = command.ExecuteReader();
                    // Read metoda pomiče reader na sljedeći red.
                    while (reader.Read())
                    {
                        Title title = new Title
                        {
                            Id = reader["Id"].ToString(),
                            ContentTypeId = Convert.ToInt32(reader["content_type_id"]),
                            PrimaryTitle = reader["primary_title"].ToString(),
                            OriginalTitle = reader["original_title"].ToString(),
                            IsAdult = Convert.ToInt32(reader["is_adult"]),
                            StartYear = Convert.ToInt32(reader["start_year"]),
                            EndYear = Convert.ToInt32(reader["end_year"]),
                            RuntimeMinutes = Convert.ToInt32(reader["runtime_minutes"])
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