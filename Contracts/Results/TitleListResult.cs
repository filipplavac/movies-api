using movies_api.Contracts.Dtos;

namespace movies_api.Contracts.Results
{
    public class TitleListResult
    {
        public string? Cursor { get; }
        public List<TitleDto>? Titles { get; }
        public TitleListResult(List<TitleDto>? titles, string? cursor) 
        { 
            Cursor = cursor;
            Titles = titles;
        }
    }
}
