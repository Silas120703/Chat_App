using Chat_App_Shared.Database.EntittyBase;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat_App_Database.Entities
{
	[Table("GroupMembers")]
	public class GroupMember : EntityBase
	{
		public long? UserId { get; set; }
		public long? GroupId { get; set; }
		[Column(TypeName = "nvarchar(50)")]
		public string Role { get; set; }
		public bool IsDeleted { get; set; } = false;
		public virtual User User { get; set; }
		public virtual Group Group { get; set; }

	}
}
