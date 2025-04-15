using Chat_App_Database.Entities;
using Chat_App_Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace Chat_App_Database.Repositories
{
	public class MessageRepository : RepositoryBase<Message>
	{
		public MessageRepository(ChatAppDBContext dbContext) : base(dbContext)
		{
		}
		public async Task<List<Message>> GetMessagesByGroupId(long groupId)
		{
			return await base.GetAll().Where(x => x.GroupId == groupId).ToListAsync();
		}
		
	}
}
