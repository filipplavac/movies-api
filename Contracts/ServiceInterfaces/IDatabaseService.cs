namespace movies_api.Contracts.ServiceInterfaces
{
    public interface IDatabaseService<T>
    {
        public string ConnectionString { get; }

        public Task<List<T>> GetList(string cursor, int pageSize);
        //public T Get(string id);
        //public T Create(T entity);
        //public void Update(T entity);
        //public void Delete(string id);
    }
}
