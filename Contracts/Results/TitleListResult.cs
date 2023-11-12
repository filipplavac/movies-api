using movies_api.Contracts.DTOs;

namespace movies_api.Contracts.Results
{
    // Single responsibility: encapsulate the GetTitleList result data.
    public class GetTitleListResult
    {
        public string? Cursor { get; }
        public List<TitleDto>? Titles { get; }
        public GetTitleListResult(List<TitleDto>? titles, string? cursor) 
        { 
            Cursor = cursor;
            Titles = titles;
        }
    }
}
