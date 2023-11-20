using Npgsql;

namespace movies_api.Contracts.ServiceInterfaces
{
    public interface IModelMapper<TModel, TDto, TFilter> where TModel : class where TDto : class
    {
        TModel RecordToModel(NpgsqlDataReader data);
        TDto ModelToDto(TModel model);
        TModel DtoToModel(TDto dto);
        TFilter QueryToFilter(string query);
    }
}
