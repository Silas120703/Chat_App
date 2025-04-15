using Chat_App_Database.Entities;
using Chat_App_Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace Chat_App_Database.Repositories
{
	public class GroupRepository : RepositoryBase<Group>
	{
		public GroupRepository(ChatAppDBContext context) : base(context)
		{
		}
		public async Task<List<Group>> GetByName(string name)
		{
			return await base.GetAll().Where(e => e.Name == name).ToListAsync();
		}
		public async Task<Group> UpdateImageAsync(long id, string Image)
		{
			var group = await base.GetByIdAsync(id);
			if (group == null)
			{
				return null;
			}
			group.Image = Image;
			return base.Update(group);
		}

		public async Task<List<Group>> GetGroupByIsGroupEqualFalse()
		{
			return await base.GetAll().Where(e => e.IsGroup == false).ToListAsync();
		}



	}
	
}
