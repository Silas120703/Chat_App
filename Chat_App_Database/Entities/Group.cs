using Chat_App_Shared.Database.EntittyBase;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Chat_App_Database.Entities
{
	[Table("Groups")]
	public class Group : EntityBase
	{
		public string Name { get; set; }
		[Column(TypeName = "nvarchar(250)")]
		public string? Description { get; set; }
		[Column(TypeName = "nvarchar(200)")]
		public string? Image { get; set; }
		public bool IsGroup { get; set; }
		[JsonIgnore]
		public virtual ICollection<GroupMember?> GroupMembers { get; set; }
		[JsonIgnore]
		public virtual ICollection<Message?> Messages { get; set; }
		[JsonIgnore]
		public virtual ICollection<Conversation?> Conversations { get; set; }
	}
}
