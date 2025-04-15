using Chat_App_Database.Entities;
using Chat_App_Database.Repositories;
using Chat_App_Shared.DTOs.GroupMemberDTO;
using Chat_App_Shared.Services;

namespace Chat_App_Core.Services
{
	public class GroupMemberService : ServiceBase<GroupMember>
	{
		private readonly GroupMemberRepository _groupMemberReposirory;
		private readonly GroupRepository _groupReposirory;
		private readonly UserRepository _userReposirory;
		public GroupMemberService(GroupMemberRepository groupMemberRepository,GroupRepository groupRepository,UserRepository userRepository) : base(groupMemberRepository)
		{
			_groupMemberReposirory = groupMemberRepository;
			_groupReposirory = groupRepository;
			_userReposirory = userRepository;
		}
		public async Task<GroupMember> AddMember(long groupId, string UserEmail, string role)
		{
			var group = await _groupReposirory.GetByIdAsync(groupId);
			if (group == null)
			{
				return null;
			}
			var user = await _userReposirory.GetByEmailAsync(UserEmail);
			if (user == null)
			{
				return null;
			}
			var groupMemberExist = await _groupMemberReposirory.GetByGroupIdAndUserId(groupId, user.Id);
			if (groupMemberExist != null)
			{
				groupMemberExist.IsDeleted = false;
				groupMemberExist.UpdatedAt = DateTime.UtcNow;
				_groupMemberReposirory.Update(groupMemberExist);
				return groupMemberExist;
			}
			var groupMember = new GroupMember
			{
				GroupId = groupId,
				UserId = user.Id,
				Role = role,
				IsDeleted = false,
			};
			await _groupMemberReposirory.AddAsync(groupMember);
			return groupMember;
		}

		public async Task<GroupMember> AddMemberNoRole(long groupId, string UserEmail)
		{
			var group = await _groupReposirory.GetByIdAsync(groupId);
			if (group == null)
			{
				return null;
			}
			var user = await _userReposirory.GetByEmailAsync(UserEmail);
			if (user == null)
			{
				return null;
			}
			var groupMemberExist = await _groupMemberReposirory.GetByGroupIdAndUserId(groupId, user.Id);
			if (groupMemberExist != null)
			{
				groupMemberExist.IsDeleted = false;
				groupMemberExist.UpdatedAt = DateTime.UtcNow;
				_groupMemberReposirory.Update(groupMemberExist);
				return groupMemberExist;
			}
			var groupMember = new GroupMember
			{
				GroupId = groupId,
				UserId = user.Id,
				Role = "Member",
				IsDeleted = false,
			};
			await _groupMemberReposirory.AddAsync(groupMember);
			return groupMember;
		}
		public async Task<GroupMember> RemoveMember(long groupId, string UserEmail)
		{
			var user = await _userReposirory.GetByEmailAsync(UserEmail);
			if (user == null)
			{
				return null;
			}
			var groupMember = await _groupMemberReposirory.GetByGroupIdAndUserId(groupId, user.Id);
			if (groupMember == null)
			{
				return null;
			}
			groupMember.IsDeleted = true;
			groupMember.UpdatedAt = DateTime.UtcNow;
			_groupMemberReposirory.Update(groupMember);
			return groupMember;
		}
		public async Task<GroupMember> UpdateRole(long groupId, string UserEmail, string role)
		{
			var user = await _userReposirory.GetByEmailAsync(UserEmail);
			if (user == null)
			{
				return null;
			}
			var groupMember = await _groupMemberReposirory.GetByGroupIdAndUserId(groupId, user.Id);
			if (groupMember == null)
			{
				return null;
			}
			var groupMemberExist = await _groupMemberReposirory.GetByGroupIdAndUserId(groupId, user.Id);
			if (groupMemberExist == null)
			{
				return null;
			}
			groupMember.Role = role;
			return _groupMemberReposirory.Update(groupMember);
		}
		public async Task<List<ResponeGroupMemberDTO>> GetbyGroupId(long groupId)
		{
			var groupMembers = await _groupMemberReposirory.GetByGroupIdAndIsDelete(groupId);
			List<ResponeGroupMemberDTO> groupMembersList = new List<ResponeGroupMemberDTO>();
			foreach (var groupMember in groupMembers)
			{
				var user = await _userReposirory.GetByIdAsync(groupMember.UserId.Value);
				if (user == null)
				{
					continue;
				}
				groupMembersList.Add(new ResponeGroupMemberDTO
				{
					GroupId = groupMember.GroupId.Value,
					UserId = groupMember.UserId.Value,
					UserEmail = user.Email,
					UseeName = user.FirstName+" "+user.LastName,
					UserImage = user.ProfilePicture,
					Role = groupMember.Role
				});
			}
			if (groupMembers == null)
			{
				return null;
			}
			return groupMembersList;
		}
		public async Task<string> GetGroupMemberRole(long groupId, string userEmail)
		{
			var user = await _userReposirory.GetByEmailAsync(userEmail);
			if (user == null)
			{
				return null;
			}
			var groupMember = await _groupMemberReposirory.GetByGroupIdAndUserId(groupId, user.Id);
			if (groupMember == null)
			{
				return null;
			}
			return groupMember.Role;
		}
	}
}
