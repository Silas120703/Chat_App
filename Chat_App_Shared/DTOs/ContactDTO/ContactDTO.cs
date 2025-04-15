namespace Chat_App_Shared.DTOs.ContactDTO
{
	public class ContactDTO
	{
		public long UserID { get; set; }
		public long FriendID { get; set; }
		public string Status { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
