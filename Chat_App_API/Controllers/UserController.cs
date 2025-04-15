using Chat_App_Core.Services;
using Microsoft.AspNetCore.Mvc;
using Chat_App_Shared.DTOs.UserDTO;
using Chat_App_Core.Mappers;
using Chat_App_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace Chat_App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[EnableCors("AllowReactApp")]
	public class UserController : ControllerBase
    {
		private readonly UserService _userService;
		private readonly JwtService _jwtService;

		public UserController( UserService userService , JwtService jwtService)
		{
			_userService = userService;
			_jwtService = jwtService;
		}
		[HttpPost("ForgotPassword")]
		public async Task<IActionResult> ForgotPassword([FromBody] string email)
		{
			var user = await _userService.ForgotPassword(email);
			if (user == null)
			{
				return BadRequest("User not found");
			}
			return Ok("OTP was send to your Email: " + email);
		}
		[HttpPost("VerifyForgotOTP")]
		public IActionResult VerifyForgotOTP([FromBody] UserVirifyDTO userDto)
		{
			var user = _userService.VerifyForgotPassword(userDto.Email, userDto.OTP);
			if (user == null)
			{
				return BadRequest("Invalid OTP");
			}
			return Ok(user);
		}
		[HttpPost("ResetPassword")]
		public IActionResult ResetPassword([FromBody] UserResetPasswordDTO userDto)
		{
			var user = _userService.ResetPassword(userDto.Email, userDto.OTP, userDto.NewPassword);
			if (user == null)
			{
				return BadRequest("Invalid OTP");
			}
			return Ok(user.MapToDTO());
		}


		[HttpPost("Register")]
		public IActionResult Rigister([FromBody] UserRegisterDTO userDto)
		{
			var userExist = _userService.GetByEmail(userDto.Email);
			if (userExist != null)
			{
				return BadRequest("User already exist");
			}
			var user = _userService.Register(userDto);
			return Ok("OTP was send to your Email: " + userDto.Email);
		}

		[HttpPost("RegisterVerifyOTP")]
		public IActionResult VerifyOTP([FromBody] UserVirifyDTO userDto)
		{
			var user = _userService.Virify(userDto.Email, userDto.OTP);
			if (user == null)
			{
				return BadRequest("Invalid OTP");
			}
			return Ok(user.MapToDTO());
		}


		[HttpPost("Login")]
		public IActionResult Login([FromBody] UserLoginDTO userDto)
		{
			var user = _userService.Login(userDto);
			if (user == null)
			{
				return BadRequest("Invalid email or password");
			}
			var token = _jwtService.GenerateToken(user);
			return Ok(token);
		}
		[Authorize]
		[HttpPut("Update")]
		public IActionResult Update([FromBody] UserUpdateDTO userDto)
		{
			var user = _userService.Update(userDto);
			if (user == null)
			{
				return NotFound("User not found");
			}
			return Ok(user.MapToDTO());
		}

		[HttpDelete("byEmail")]
		public async Task<IActionResult> Delete(string email)
		{
			var user = await _userService.GetByEmailAsync(email);
			if (user == null)
			{
				return NotFound("User not found");
			}
			_userService.Delete(user);
			return Ok("Delete successfully");
		}


		[HttpGet("byEmail")]
		public async Task<IActionResult> GetByEmail( string Email)
		{
			var user =await _userService.GetByEmailAsync(Email);
			if (user == null)
			{
				return NotFound("User not found");
			}
			return Ok(user.MapToDTO());
		}
		[HttpGet("byId")]
		public async Task<IActionResult> GetById(long id)
		{
			var user = _userService.GetById(id);
			return Ok(user.MapToDTO());
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

		[HttpGet("CheckEmail")]
		public async Task<IActionResult> CheckEmail(string email)
		{
			var user = await _userService.CheckBlock(email);
			if (user == null)
			{
				return Ok("Block");
			}
			return Ok("NoBlock");
		}

	}
}
