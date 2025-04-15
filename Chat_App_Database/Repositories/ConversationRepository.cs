using Chat_App_Database.Entities;
using Chat_App_Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace Chat_App_Database.Repositories
{
	public class ConversationRepository : RepositoryBase<Conversation>
	{
		public ConversationRepository(ChatAppDBContext dbContext) : base(dbContext)
		{
		}
		public async Task<List<Conversation>> GetAllConversations(long id)
		{
			return await base.GetAll().Where(u => u.UserId==id && u.IsRemove==false).ToListAsync();
		}
		public async Task<Conversation> GetConversationByUserIdAndGroupId(long userId,long groupId)
		{
			return await base.GetAll().Where(c => c.UserId == userId && c.GroupId == groupId).FirstOrDefaultAsync();
		}
		public async Task<List<Conversation>> GetConversationByGroupId(long groupId)
		{
			return await base.GetAll().Where(c => c.GroupId == groupId).ToListAsync();
		}

		public async Task<Conversation> AddConversation(Conversation conversation)
		{
			await base.AddAsync(conversation);
			return conversation;
		}
		public async Task<Conversation> DeleteConversation(long conversationId)
		{
			var conversation = await base.GetByIdAsync(conversationId);
			if (conversation == null)
			{
				return null;
			}
			conversation.IsRemove = true;
			base.Update(conversation);
			return conversation;
		}

		public async Task<Conversation> GetConversationByGroupIdAndUserIdAsync(long GroupId,long UserId)
		{
			return await base.GetAll().Where(c => c.GroupId == GroupId && c.UserId == UserId).FirstOrDefaultAsync();
		}

		public async Task<Conversation> ResetConversationNotification(long GroupId, long UserId)
		{
			var conversation = await GetConversationByGroupIdAndUserIdAsync(GroupId, UserId);
			if (conversation == null)
			{
				return null;
			}
			conversation.CountNotifications = 0;
			base.Update(conversation);
			return conversation;
		}

	}
}
