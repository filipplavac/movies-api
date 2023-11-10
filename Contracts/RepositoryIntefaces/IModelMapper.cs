using Npgsql;

namespace movies_api.Contracts.RepositoryIntefaces
{
    public interface IModelMapper<TModel, TDto> where TModel : class where TDto : class
    {
        TModel RecordToModel(NpgsqlDataReader data);
        TDto ModelToDto(TModel model);
        TModel DtoToModel(TDto dto);
    }
}
