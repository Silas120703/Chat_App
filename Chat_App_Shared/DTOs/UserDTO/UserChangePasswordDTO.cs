using System.ComponentModel.DataAnnotations;

namespace Chat_App_Shared.DTOs.UserDTO
{
	public class UserChangePasswordDTO
	{
		public string Email { get; set; }
		public string NewPassword { get; set; }
	}
}

