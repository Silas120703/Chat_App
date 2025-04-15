namespace Chat_App_Shared.DTOs.UserDTO
{
	public class UserResetPasswordDTO
	{
		public string Email { get; set; }
		public int OTP { get; set; }
		public string NewPassword { get; set; }
	}
}
