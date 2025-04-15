using Chat_App_Shared.Database.EntittyBase;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Chat_App_Database.Entities
{
	[Table("Contacts")]
	public class Contact : EntityBase
	{
		public long? UserId { get; set; }
		public long? FriendId { get; set; }
		[Column(TypeName = "nvarchar(50)")]
		public string Status { get; set; }
		[JsonIgnore]
		[InverseProperty("ContactsAsUser")]
		public virtual User User { get; set; }
		[JsonIgnore]
		[InverseProperty("ContactsAsFriend")]
		public virtual User Friend { get; set; }
	}

}
