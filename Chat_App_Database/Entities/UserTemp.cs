using Chat_App_Shared.Database.EntittyBase;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat_App_Database.Entities
{
	[Table("UserTemps")]
	[Index(nameof(Email), IsUnique = true)]
	public class UserTemp : EntityBase
	{
		[Column(TypeName = "nvarchar(250)")]
		public string FirstName { get; set; } = null!;

		[Column(TypeName = "nvarchar(250)")]
		public string LastName { get; set; } = null!;
		public DateOnly? BirthDay { get; set; }
		public string? Gender { get; set; }

		[Column(TypeName = "nvarchar(250)")]
		public string Email { get; set; } = null!;

		public string? ProfilePicture { get; set; } = null!;

		[Column(TypeName = "nvarchar(200)")]
		public string PasswordHash { get; set; } = null!;

		public string? Phone { get; set; }

		public string Role { get; set; } = null!;

		public bool? OnlineStatus { get; set; }
		public int OTP { get; set; }
		public DateTime OTPTime { get; set; }

	}
}
