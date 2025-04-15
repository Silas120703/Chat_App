using Chat_App_Shared.Database.EntittyBase;

namespace Chat_App_Shared.Interfaces.Services
{
	public interface IServiceBase<T> : IGenericService<T> where T : IEntityModel
	{
	}
}
