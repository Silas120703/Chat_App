using Chat_App_Database.Entities;
using Chat_App_Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace Chat_App_Database.Repositories
{
	public class GroupMemberRepository : RepositoryBase<GroupMember>
	{
		public GroupMemberRepository(ChatAppDBContext context) : base(context)
		{
			
		}
		public async Task<GroupMember> GetByGroupIdAndUserId(long groupId, long userId)
		{
			return await base.GetAll().FirstOrDefaultAsync(e => e.GroupId == groupId && e.UserId == userId);
		}
		public async Task<List<GroupMember>> GetByGroupId(long groupId)
		{
			return await base.GetAll().Where(e => e.GroupId == groupId).ToListAsync();
		}
		public async Task<List<GroupMember>> GetByGroupIdAndIsDelete(long groupId)
		{
			return await base.GetAll().Where(e => e.GroupId == groupId && e.IsDeleted == false).ToListAsync();
		}
	}
}
