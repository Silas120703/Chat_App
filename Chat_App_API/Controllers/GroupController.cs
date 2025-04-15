using Chat_App_Core.Services;
using Chat_App_Database.Entities;
using Chat_App_Shared.DTOs.GroupDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat_App_API.Controllers
{
    [Route("api/[controller]")]
	[Authorize]
	[ApiController]
    public class GroupController : ControllerBase
    {
		private readonly GroupService _groupService;
		private readonly GroupMemberService _groupMemberService;
		private readonly UserService _userService;
		private readonly ConversationService _conversationService;
		public GroupController(GroupService groupService, GroupMemberService groupMemberService,UserService userService,ConversationService conversationService)
		{
			_groupService = groupService;
			_groupMemberService = groupMemberService;
			_userService = userService;
			_conversationService = conversationService;
		}
		[HttpPost]
		public async Task<ActionResult<Group>> AddGroupAsync(GroupDTO groupDto)
		{
			var group = await _groupService.AddGroupAsync(groupDto);
			if (group == null)
			{
				return BadRequest();
			}
			return Ok(group);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<Group>> GetGroupByIDAsync(long id)
		{
			var group = await _groupService.GetGroupByIDAsync(id);
			if (group == null)
			{
				return NotFound();
			}
			return Ok(group);
		}
		[HttpPut("{id}")]
		public async Task<ActionResult<Group>> PutGroupByIDAsync(long id, GroupDTO groupDto)
		{
			var group = await _groupService.PutGroupByIDAsync(id, groupDto);
			if (group == null)
			{
				return NotFound();
			}
			return Ok(group);
		}
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteGroupByIDAsync(long id)
		{
			var conversation = await _conversationService.DeleteAllConversationbyGroupId(id);
			if (conversation == null)
			{
				return NotFound();
			}
			return Ok(conversation);

		}
		
		[HttpPost("CreateWithMembers")]
		public async Task<ActionResult<Group>> CreateGroupWithMembersAsync(CreateGroupRequestDTO createGroupRequestDTO)
		{
			var user1 = _userService.GetByEmail(createGroupRequestDTO.UserEmails[0]);
			var user2 = _userService.GetByEmail(createGroupRequestDTO.UserEmails[1]);
			var existGroup = await _groupService.CheckUsersInNonGroupAsync(user1.Id, user2.Id);
			if (createGroupRequestDTO.Group.IsGroup == false && existGroup != null)
			{
					existGroup.UpdatedAt = DateTime.UtcNow;
					_groupService.Update(existGroup);
					await _conversationService.ReCreateConversation(existGroup.Id);
				return existGroup;
			}
			else 
			{ 

				var group = await _groupService.AddGroupAsync(createGroupRequestDTO.Group);
			if (group == null)
			{
				return BadRequest("Could not create group.");
			}

			List<GroupMember> addedMembers = new List<GroupMember>();
			foreach (var email in createGroupRequestDTO.UserEmails)
			{
				string role = "Member"; // Default role for new members
				if (createGroupRequestDTO.UserEmails.IndexOf(email) == 0)
				{
					role = "Admin"; // First user is set as Admin
				}
				var user = _userService.GetByEmail(email);
				if (user == null)
				{
					continue; // Skip if user not found
				}
				var member = await _groupMemberService.AddMember(group.Id, email,role);
				
				if (member != null)
				{
					addedMembers.Add(member);
					
				}

			}
			var conservation = await _conversationService.CreateNewConversations(group.Id);

			return Ok(group);
			}
			return BadRequest("Could not create group.");
		}
	}
}
