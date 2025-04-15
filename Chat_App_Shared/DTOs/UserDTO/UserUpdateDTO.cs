namespace Chat_App_Shared.DTOs.UserDTO
{
	public class UserUpdateDTO
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateOnly BirthDay { get; set; }
		public string Gender { get; set; }
		public string ProfilePicture { get; set; }
		public string Phone {  get; set; }
	}
}
