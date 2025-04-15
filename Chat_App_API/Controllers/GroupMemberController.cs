using Chat_App_Core.Services;
using Chat_App_Database.Entities;
using Chat_App_Shared.DTOs.GroupMemberDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat_App_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class GroupMemberController : ControllerBase
	{
		private readonly GroupMemberService _groupMemberService;
		private readonly ConversationService _conversationService;
		public GroupMemberController(GroupMemberService groupMemberService,ConversationService conversationService)
		{
			_groupMemberService = groupMemberService;
			_conversationService = conversationService;
		}
		[HttpPost("addMember")]
		public async Task<ActionResult<GroupMember>> AddMember(long groupId, string UserEmail)
		{
			var groupMember = await _groupMemberService.AddMemberNoRole(groupId, UserEmail);
			var conversation = await _conversationService.ReCreateConversation(groupId);
			if (conversation == null) {
				return BadRequest();
			}
			if (groupMember == null)
			{
				return NotFound();
			}
			return groupMember;
		}
		[HttpDelete("deleteMember")]
		public async Task<ActionResult<GroupMember>> RemoveMember(long groupId, string UserEmail)
		{
			var groupMember = await _groupMemberService.RemoveMember(groupId, UserEmail);
			var conversation = await _conversationService.DeleteConversation(groupId, UserEmail);
			if (conversation == null)
			{
				return BadRequest();
			}
			if (groupMember == null)
			{
				return NotFound();
			}
			return groupMember;
		}
		[HttpPut("UpdateMemberRole")]
		public async Task<ActionResult<GroupMember>> UpdateRole(long groupId, string UserEmail, string role)
		{
			var groupMember = await _groupMemberService.UpdateRole(groupId, UserEmail, role);
			if (groupMember == null)
			{
				return NotFound();
			}
			return groupMember;
		}

		[HttpGet("GetGroupMembers")]
		public async Task<ActionResult<List<ResponeGroupMemberDTO>>> GetGroupMembers(long groupId)
		{
			var groupMembers = await _groupMemberService.GetbyGroupId(groupId);
			if (groupMembers == null)
			{
				return NotFound();
			}
			return groupMembers;
		}
		[HttpGet("GetGroupMemberRole")]
		public async Task<ActionResult<string>> GetGroupMemberRole(long groupId, string userEmail)
		{
			var groupMember = await _groupMemberService.GetGroupMemberRole(groupId, userEmail);
			if (groupMember == null)
			{
				return NotFound();
			}
			return groupMember;
		}
	}
}
