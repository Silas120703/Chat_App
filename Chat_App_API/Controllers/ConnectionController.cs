using Chat_App_Core.Services;
using Chat_App_Shared.DTOs.UserConnectionDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat_App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
		private readonly UserConnectionService _userConnectionService;
		public ConnectionController(UserConnectionService userConnectionService)
		{
			_userConnectionService = userConnectionService;
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserConnectionsAsync(string userEmail)
		{
			var userConnections = await _userConnectionService.GetUserConnectionsbyEmailAsync(userEmail);
			if (userConnections == null)
			{
				return NotFound();
			}
			return Ok(userConnections);
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddConnection( string userEmail, string connectionId,string browserId)
		{
		
			var userConnection = await _userConnectionService.AddConnection(userEmail, connectionId, browserId);
			if (userConnection == null)
			{
				return NotFound();
			}
			return Ok(userConnection);
		}
		[HttpDelete]
		[Authorize]
		public async Task<IActionResult> RemoveConnection(UserConnectionRequestDTO requestDTO)
		{
			var userConnection = await _userConnectionService.RemoveConnection(requestDTO.UserEmail,requestDTO.BrowserId);
			if (userConnection == null)
			{
				return NotFound();
			}
			return Ok(userConnection);
		}
	}
}
