using Chat_App_Core.Services;
using Chat_App_Shared.DTOs.ContactDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chat_App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
		private readonly ContactService _contactService;	
		public ContactsController(ContactService contactService)
		{
			_contactService = contactService;
		}
		[HttpGet("GetContactsByUserEmail")]
		[Authorize]
		public async Task<IActionResult> GetContactsByUserEmail(string Email)
		{
			var contacts = await _contactService.GetContactsByUserEmailAsync(Email);
			if (contacts == null)
			{
				return NotFound("Contacts not found");
			}
			return Ok(contacts);
		}
		[HttpPost("GetFriendByID")]
		[Authorize]
		public async Task<IActionResult> GetFriendByID([FromBody] ContactRequestDTO contact)
		{
			var friend = await _contactService.GetFriendByID(contact);
			if (friend == null)
			{
				return NotFound("Friend not found");
			}
			return Ok(friend);
		}
		[HttpPost("AddFriend")]
		[Authorize]
		public async Task<IActionResult> AddFriend([FromBody] ContactRequestDTO contact)
		{
			var friend = await _contactService.AddFriendAsync(contact);
			if (friend == null)
			{
				return NotFound("Friend not found");
			}
			return Ok(friend);
		}
		[HttpPost("AcceptFriend")]
		[Authorize]
		public async Task<IActionResult> AcceptFriend([FromBody] ContactRequestDTO contact)
		{
			var friend = await _contactService.AcceptFriendAsync(contact);
			if (friend == null)
			{
				return NotFound("Friend not found");
			}
			return Ok(friend);
		}
		[HttpPost("RejectFriend")]
		[Authorize]
		public async Task<IActionResult> RejectFriend([FromBody] ContactRequestDTO contact)
		{
			var friend = await _contactService.RejectFriendAsync(contact);
			if (friend == null)
			{
				return NotFound("Friend not found");
			}
			return Ok("Rejected Successfully") ;
		}
		[HttpPost("DeleteFriend")]
		[Authorize]
		public async Task<IActionResult> DeleteFriend([FromBody] ContactRequestDTO contact)
		{
			var friend = await _contactService.DeleteFriendAsync(contact);
			if (friend == null)
			{
				return NotFound("Friend not found");
			}
			return Ok("Deleted Successfully" + friend);
		}

	}
}
