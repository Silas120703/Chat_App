namespace Chat_App_Shared.DTOs.MassageDTO
{
	public class ResponeMessageDTO
	{
		public long Id { get; set; }
		public long? GroupId { get; set; }
		public string userEmail { get; set; }
		public string userName { get; set; }
		public string Content { get; set; }
		public string Type { get; set; } // text, image
		public string ProfilePiture { get; set; }
		public DateTime sentdAt { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}
