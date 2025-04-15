using Chat_App_Shared.Database.EntittyBase;

namespace Chat_App_Shared.Interfaces.Repositories
{
	public interface IRepositoryBase<T> : IGenericRepository<T> where T : IEntityModel
	{
		
	}
}
