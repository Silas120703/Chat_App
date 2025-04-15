using Chat_App_Database.Repositories;
using Chat_App_Shared.Database.EntittyBase;
using Chat_App_Shared.Services;

namespace Chat_App_Core.Services
{
	public class Service<T> : ServiceBase<T> where T : EntityBase
	{
		public Service(Repository<T> repository) : base(repository)
		{
		}
	}
	
}
