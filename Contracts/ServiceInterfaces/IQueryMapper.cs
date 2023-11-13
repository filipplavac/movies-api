namespace movies_api.Contracts.ServiceInterfaces
{
    public interface IQueryMapper
    {
        public string FilterToSqlWhere(string filter);
    }
}
