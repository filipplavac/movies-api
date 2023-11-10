namespace movies_api.Contracts.ServiceInterfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<List<TEntity>> GetList(string? cursor, int pageSize);
        //public T Get(string id);
        //public T Create(T entity);
        //public void Update(T entity);
        //public void Delete(string id);
    }
}
