using Chat_App_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat_App_API.Controllers
{
    [Route("api/[controller]")]
	[Authorize]
	[ApiController]
    public class ConversationController : ControllerBase
    {
		private readonly ConversationService _conversationService;
		public ConversationController(ConversationService conversationService)
		{
			_conversationService = conversationService;
		}
		[HttpGet("GetAll")]
		public async Task<IActionResult> Get(string userEmails)
		{
			var conversations = await _conversationService.GetAllConversations(userEmails);
			return Ok(conversations);
		}
		[HttpGet("GetByGroupId")]
		public async Task<IActionResult> GetByGroupId(long groupId,string userEmail)
		{
			var conversation = await _conversationService.GetConversationByGroupIdAsync(groupId,userEmail);
			if (conversation == null)
			{
				return NotFound();
			}
			return Ok(conversation);
		}
	}
}
