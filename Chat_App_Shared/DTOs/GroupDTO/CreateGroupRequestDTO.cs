namespace Chat_App_Shared.DTOs.GroupDTO
{
	public class CreateGroupRequestDTO
	{
		public GroupDTO Group { get; set; }
		public List<string> UserEmails { get; set; }
	}
}
