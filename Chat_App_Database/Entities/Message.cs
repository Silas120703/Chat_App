using Chat_App_Shared.Database.EntittyBase;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Chat_App_Database.Entities
{
	[Table("Messages")]
	public class Message: EntityBase
	{
		public long? GroupId { get; set; }
		public long? UserId { get; set; }
		[Column(TypeName = "nvarchar(50)")]
		public string Type { get; set; }
		public string Content { get; set; }
		public DateTime SentAt { get; set; }
		public bool IsDelete { get; set; }
		public virtual User User { get; set; }
		public virtual Group Group { get; set; }
		[IgnoreDataMember]
		[JsonIgnore]
		public virtual ICollection<Conversation> Conversations { get; set; }

	}
}
