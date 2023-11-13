using movies_api.Contracts.ServiceInterfaces;

namespace movies_api.Common.Mapping
{
    // Single responsibilty: maps the query string filter to the sql where statement.
    public class QueryMapper: IQueryMapper
    {
        public string FilterToSqlWhere(string filter)
        {
            // The filter respects the RHS colon syntax.

            // Split the query 

            return "s";
        }

    }
}
