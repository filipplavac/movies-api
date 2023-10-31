namespace movies_api.Contracts.Dtos
{
    public record TitleDto
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
