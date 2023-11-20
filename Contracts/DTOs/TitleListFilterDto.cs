namespace movies_api.Contracts.DTOs
{
    public record class TitleListFilterDto
    (
        string? title_name,
        List<string>? talents,
        int? start_year,
        int? end_year
    );
}
