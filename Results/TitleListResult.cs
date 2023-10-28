using movies_api.Models;

namespace movies_api.Results
{
    public class TitleListResult
    {
        public string? Cursor { get; }
        public List<Title>? Titles { get; }
        public TitleListResult(List<Title>? titles, string? cursor) 
        { 
            Cursor = cursor;
            Titles = titles;
        }
    }
}
