namespace Chat_App_Shared.DTOs.GroupMemberDTO
{
	public class ResponeGroupMemberDTO
	{
		public long GroupId { get; set; }
		public long UserId { get; set; }
		public string Role { get; set; }
		public string UseeName { get; set; }
		public string UserImage { get; set; }
		public string UserEmail { get; set; }
	}
}
