using Chat_App_Core.Services;
using Chat_App_Shared.DTOs.UserDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chat_App_Core.Mappers;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Chat_App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize(Roles ="admin")]
    public class AdminController : ControllerBase
    {
		private readonly UserService _userService;
		public AdminController(UserService userService)
		{
			_userService = userService;
		}
		[HttpPut("changePassword")]
		public IActionResult ChangePassword([FromBody] UserChangePasswordDTO changePasswordDTO)
		{
			var user = _userService.ChangePassWord(changePasswordDTO.Email, changePasswordDTO.NewPassword);
			if (user == null)
			{
				return BadRequest("Invalid email or password");
			}
			return Ok(user.MapToDTO());
		}
		[HttpGet("getAllUser")]
		public IActionResult GetAllUser()
		{
			var users = _userService.GetAll();
			if (users == null)
			{
				return NotFound("No user found");
			}
			return Ok(users);
		}
		[HttpGet("getUserByEmail")]
		public IActionResult GetUserByEmail(string email)
		{
			var user = _userService.GetByEmail(email);
			if (user == null)
			{
				return NotFound("No user found");
			}
			return Ok(user.MapToDTO());
		}
		[HttpPut("updateUserRole")]
		public IActionResult UpdateUserRole(UserChangeRoleDTO roleDTO)
		{
			var user = _userService.GetByEmail(roleDTO.Email);
			if (user == null)
			{
				return NotFound("No user found");
			}
			user.Role = roleDTO.Role;
			_userService.Update(user);
			return Ok(user.MapToDTO());
		}
		[HttpPut("blockUser")]
		public async Task<IActionResult> BlockUser(string email)
		{
			var user = await _userService.BlockUser(email);
			if (user == null)
			{
				return NotFound("No user found");
			}
			return Ok(user.MapToDTO());
		}
		[HttpPut("unBlockUser")]
		public async Task<IActionResult> UnBlockUser(string email)
		{
			var user = await _userService.UnBlockUser(email);
			if (user == null)
			{
				return NotFound("No user found");
			}
			return Ok(user.MapToDTO());
		}
	}
}
