using Chat_App_Database.Entities;
using Chat_App_Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace Chat_App_Database.Repositories
{
	public class UserConnectionRepository : RepositoryBase<UserConnection>
	{
		public UserConnectionRepository(ChatAppDBContext dbContext) : base(dbContext)
		{

		}
		public async Task<List<UserConnection>> GetUserConnectionsAsync(long userId )
		{
			return await base.GetAll().Where(x => x.UserId == userId).ToListAsync();
		}
		public async Task<UserConnection> GetUserConnectByUserIdAndBrowserIdAsync(long userId,string browserId)
		{
			return await base.GetAll().FirstOrDefaultAsync(x=> x.BrowserId==browserId && x.UserId==userId);
		}
		public async Task<UserConnection> GetUserConnectByBrowserIdAsync(string browserId)
		{
			return await base.GetAll().FirstOrDefaultAsync(x =>  x.BrowserId==browserId);
		}
		
		public async Task RemoveConnectionAsync(string connectionId)
		{
			var userConnection = await base.GetAll().FirstOrDefaultAsync(x => x.ConnectionId == connectionId);
			if (userConnection != null)
			{
				base.Delete(userConnection);
			}
		}

	}
}
