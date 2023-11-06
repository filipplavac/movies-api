using movies_api.Contracts.DTOs;
using Npgsql;

namespace movies_api.Models
{
    public class Title 
    {
        public required string Id { get; set; }
        public int? ContentTypeId { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? OriginalTitle { get; set; }
        public int? IsAdult { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? RuntimeMinutes { get; set; }

        public static Title FromRecord(NpgsqlDataReader data)
        {
            Title title = new Title
            {
                Id = data["title_id"].ToString(),
                ContentTypeId = data["content_type_id"].Equals(DBNull.Value) ? null : Convert.ToInt32(data["content_type_id"]),
                PrimaryTitle = data["primary_title"].Equals(DBNull.Value) ? null : data["primary_title"].ToString(),
                OriginalTitle = data["original_title"].Equals(DBNull.Value) ? null : data["original_title"].ToString(),
                IsAdult = data["is_adult"].Equals(DBNull.Value) ? null : Convert.ToInt32(data["is_adult"]),
                StartYear = data["start_year"].Equals(DBNull.Value) ? null : Convert.ToInt32(data["start_year"]),
                EndYear = data["end_year"].Equals(DBNull.Value) ? null : Convert.ToInt32(data["end_year"]),
                RuntimeMinutes = data["runtime_minutes"].Equals(DBNull.Value) ? null : Convert.ToInt32(data["runtime_minutes"])
            };
            return title;
        }

        public TitleDto ToDto()
        {
            return new TitleDto 
                (
                    Id,
                    ContentTypeId,
                    PrimaryTitle,
                    OriginalTitle,
                    IsAdult,
                    StartYear,
                    EndYear,
                    RuntimeMinutes
                );
        }
    }
}
