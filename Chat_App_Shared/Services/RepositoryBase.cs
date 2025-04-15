using Chat_App_Shared.Database.EntittyBase;
using Chat_App_Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat_App_Shared.Services
{
	public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
	{
		private readonly DbContext _dbContext;
		private readonly DbSet<T> _dbSet;	
		public RepositoryBase(DbContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}
		public T Add(T entity)
		{
			_dbSet.Add(entity);
			_saveChange();
			return entity;
		}

		public async Task<T> AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _saveChangeAsync();	
			return entity;
		}

		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
			_saveChange();	
		}

		public void DeleteRange(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
			_saveChange();
		}

		public void AddRange(IEnumerable<T> entities)
		{
			_dbSet.AddRange(entities);
			_saveChange();
		}

		public IQueryable<T> GetAll()
		{
			return _dbSet.AsQueryable();
		}


		public T GetById(long id)
		{
			return GetAll().FirstOrDefault(e => e.Id == id);
		}

		public async Task<T> GetByIdAsync(long id)
		{
			return await GetAll().FirstOrDefaultAsync(e => e.Id == id);
		}

		public T Update(T entity)
		{
			entity.UpdatedAt = DateTime.Now;
			_dbSet.Update(entity);
			_saveChange();
			return entity;	
		}

		public void UpdateRange(IEnumerable<T> entities)
		{
			_dbSet.UpdateRange(entities);
			_saveChange();
		}

		private void _saveChange() => _dbContext.SaveChanges();
		private async Task _saveChangeAsync() => await _dbContext.SaveChangesAsync();	

	}
}
