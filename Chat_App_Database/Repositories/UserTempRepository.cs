using Chat_App_Database.Entities;
using Chat_App_Shared.Services;

namespace Chat_App_Database.Repositories
{
	public class UserTempRepository: RepositoryBase<UserTemp>
	{
		public UserTempRepository(ChatAppDBContext context) : base(context)
		{

		}
		public UserTemp GetByEmail(string mail)
		{
			return base.GetAll().FirstOrDefault(e => e.Email == mail);
		}
		
	}
}
