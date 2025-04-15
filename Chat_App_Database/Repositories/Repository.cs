using Chat_App_Shared.Database.EntittyBase;
using Chat_App_Shared.Services;

namespace Chat_App_Database.Repositories
{
	public class Repository<T> : RepositoryBase<T> where T : EntityBase
	{
		public Repository(ChatAppDBContext dbContext) : base(dbContext)
		{
		}
	}	
}
