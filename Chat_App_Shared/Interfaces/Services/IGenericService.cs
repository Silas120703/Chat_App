using Chat_App_Shared.Database.EntittyBase;

namespace Chat_App_Shared.Interfaces.Services;
public interface IGenericService<T> : IService where T : IEntityModel
{
	Task<T> AddAsync(T entity);
	T Add(T entity);

	T Update(T entity);
	void Delete(T entity);

	Task<T> GetByIdAsync(long id);
	T GetById(long id);

	void UpdateRange(IEnumerable<T> entities);
	void DeleteRange(IEnumerable<T> entities);

	IQueryable<T> GetAll();
}