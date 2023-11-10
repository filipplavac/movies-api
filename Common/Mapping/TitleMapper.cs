using movies_api.Contracts.DTOs;
using movies_api.Contracts.ServiceInterfaces;
using movies_api.Models;
using Npgsql;

namespace movies_api.Common.Mapping
{
    public class TitleMapper : IModelMapper<Title, TitleDto>
    {
        public Title RecordToModel(NpgsqlDataReader data)
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
        public TitleDto ModelToDto(Title model)
        {
            return new TitleDto
            (
                model.Id,
                model.ContentTypeId,
                model.PrimaryTitle,
                model.OriginalTitle,
                model.IsAdult,
                model.StartYear,
                model.EndYear,
                model.RuntimeMinutes
            );
        }

        public Title DtoToModel(TitleDto dto)
        {
            return new Title
            {
                Id = dto.Id,
                ContentTypeId = dto.ContentTypeId,
                PrimaryTitle = dto.PrimaryTitle,
                OriginalTitle = dto.OriginalTitle,
                IsAdult = dto.IsAdult,
                StartYear = dto.StartYear,
                EndYear = dto.EndYear,
                RuntimeMinutes = dto.RuntimeMinutes
            };
        }
    }
}
