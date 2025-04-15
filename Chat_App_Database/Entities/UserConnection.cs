using Chat_App_Shared.Database.EntittyBase;

namespace Chat_App_Database.Entities
{
	public class UserConnection : EntityBase
	{
		public string ConnectionId { get; set; }
		public long UserId { get; set; }
		public string BrowserId { get; set; }
		public virtual User User { get; set; }
		
	}
}
