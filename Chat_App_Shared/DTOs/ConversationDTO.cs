namespace Chat_App_Shared.DTOs
{
	public class ConversationDTO
	{
		public long? GroupId { get; set; }
		public long? UserId { get; set; }
		public bool IsGroup { get; set; }
		public string GroupName { get; set; }
		public string UserEmail { get; set; }
		public string ProfilePiture { get; set; }
		public int CountNotifications { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
