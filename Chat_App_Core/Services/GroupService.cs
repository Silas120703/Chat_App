using Chat_App_Database.Entities;
using Chat_App_Database.Repositories;
using Chat_App_Shared.DTOs.GroupDTO;
using Chat_App_Shared.Services;

namespace Chat_App_Core.Services
{
	public class GroupService : ServiceBase<Group>
	{
		private readonly GroupRepository _groupRepository;
		private readonly GroupMemberRepository _groupMemberRepository;
		public GroupService(GroupRepository groupRepository, GroupMemberRepository groupMemberRepository): base(groupRepository)
		{
			_groupRepository = groupRepository;
			_groupMemberRepository = groupMemberRepository;
		}
		public async Task<Group> AddGroupAsync(GroupDTO groupDto)
		{
			var group = new Group
			{
				Name = groupDto.Name,
				Description = groupDto.Description,
				IsGroup = groupDto.IsGroup,
				Image = groupDto.GroupPicture,
				
			};
			return await _groupRepository.AddAsync(group);
		}
		public async Task<Group> GetGroupByIDAsync(long id)
		{
			return await _groupRepository.GetByIdAsync(id);
		}
		public async Task<Group> PutGroupByIDAsync( long id, GroupDTO groupDto)
		{
			var ExistingGroup = await _groupRepository.GetByIdAsync(id);
			if (ExistingGroup == null)
			{
				return null;
			}
			ExistingGroup.Name = groupDto.Name;
			ExistingGroup.Description = groupDto.Description;
			ExistingGroup.IsGroup = groupDto.IsGroup;
			return  _groupRepository.Update(ExistingGroup);
		}
		public async Task<Group> DeleteGroupByIDAsync(long id)
		{
			var ExistingGroup = await _groupRepository.GetByIdAsync(id);
			if (ExistingGroup == null)
			{
				return null;
			}
			 _groupRepository.Delete(ExistingGroup);
			return ExistingGroup;
		}
		
		public async Task<Group> CheckUsersInNonGroupAsync(long userId1,long userId2)
		{
			var groups = await _groupRepository.GetGroupByIsGroupEqualFalse();
			foreach (var group in groups) {
				var members = await _groupMemberRepository.GetByGroupId(group.Id);
				var tmp = 0;
				foreach (var member in members)
				{
					if (member.UserId == userId1 || member.UserId == userId2)
					{
						tmp++;
					}
				}
				if (tmp == 2)
				{
					return group;
				}
			}
			return null;
		}



	}
}
