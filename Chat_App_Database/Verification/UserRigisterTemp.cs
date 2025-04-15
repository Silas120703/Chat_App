using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat_App_Shared.Verification
{
	public class UserRigisterTemp
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateOnly BirthDay { get; set; }
		public bool Gender { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public string Phone { get; set; }
		public string ProfilePicture { get; set; }
		public bool OnlineStatus { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime UpdateAt { get; set; }
		public int OTP { get; set; }
		public DateTime TimeOTP { get; set; }
	}
}
