namespace movies_api.Contracts.DTOs
{
    // Single responsibility: encapsulate data of the Title entity.
    public record class TitleDto
    (
        string Id,
        int? ContentTypeId,
        string? PrimaryTitle,
        string? OriginalTitle,
        int? IsAdult,
        int? StartYear,
        int? EndYear,
        int? RuntimeMinutes
    );
}
