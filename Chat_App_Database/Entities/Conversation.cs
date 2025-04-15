using Chat_App_Shared.Database.EntittyBase;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat_App_Database.Entities
{
	[Table("Conversations")]
	public class Conversation:EntityBase
	{
		public long? GroupId { get; set; }
		public long? UserId { get; set; }
		public long? MessageId { get; set; }
		public int CountNotifications { get; set; }
		public bool IsRemove { get; set; }
		public virtual User User { get; set; }
		public virtual Group Group { get; set; }
		public virtual Message Message { get; set; }
	}
}
