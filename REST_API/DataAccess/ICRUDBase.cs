namespace REST_API.DataAccess;

public interface ICRUDBase<T>
{
	Task<IList<T>> ReadAll();
	Task<T> ReadById(Guid id);
	Task Create(T model);
	Task Update(T model);
	Task Delete(Guid id);
}