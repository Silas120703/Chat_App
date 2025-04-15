using Chat_App_API.Hubs;
using Chat_App_Core.Mappers;
using Chat_App_Core.Services;
using Chat_App_Database.Entities;
using Chat_App_Shared.DTOs.MassageDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Chat_App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class MessageController : ControllerBase
    {
		private readonly MessageService _messageService;
		private readonly ConversationService _conversationService;
		private readonly UserConnectionService _userConnectionService;
		private readonly GroupMemberService _groupMemberService;
		private readonly IHubContext<ChatHub> _chatHub;


		public MessageController(MessageService messageService,ConversationService conversationService,UserConnectionService userConnectionService,GroupMemberService groupMemberService, IHubContext<ChatHub> chatHub)
		{
			_messageService = messageService;
			_conversationService = conversationService;
			_userConnectionService = userConnectionService;
			_groupMemberService = groupMemberService;
			_chatHub = chatHub;
		}
		[HttpGet("{groupId}")]
		public async Task<ActionResult<List<ResponeMessageDTO>>> GetMessagesByGroupId(long groupId,string userEmail)
		{
			var messages = await _messageService.GetMessagesByGroupId(groupId,userEmail);
			if (messages == null)
			{
				return NotFound();
			}
			return Ok(messages);
		}
		[HttpPost("AddMessage")]
		public async Task<ActionResult<ResponeMessageDTO>> AddMessageAsync(long groupId,string connectId,MessageDTO message)
		{
			var newMessage = await _messageService.AddMessage(groupId, message);
			var groupMembers = await _groupMemberService.GetbyGroupId(groupId);
			var conversation = await _conversationService.CreateConversation(groupId, message.EmailUser);
			foreach (var groupMember in groupMembers)
			{
				var userConnections = await _userConnectionService.GetUserConnectionByUserIdAsync(groupMember.UserId);
				foreach (var userConnection in userConnections)
				{
					if (userConnection == null)
					{
						continue;
					}
					await _chatHub.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveMessage", newMessage);
				}
			}
			if (newMessage == null)
			{
				return BadRequest();
			}
			return Ok(newMessage);
		}

		[HttpDelete("{messageId}")]
		public async Task<ActionResult<Message>> DeleteMessageAsync(long messageId,long groupId)
		{
			var message = await _messageService.DeleteMessage(messageId);
			var groupMembers = await _groupMemberService.GetbyGroupId(groupId);
			foreach (var groupMember in groupMembers)
			{
				var userConnections = await _userConnectionService.GetUserConnectionByUserIdAsync(groupMember.UserId);
				foreach (var userConnection in userConnections)
				{
					if (userConnection == null)
					{
						continue;
					}
					await _chatHub.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveMessageDelete", message);
				}
			}
			if (message == null)
			{
				return NotFound();
			}
			return Ok(message);
		}
	}
}
