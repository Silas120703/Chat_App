namespace Chat_App_Shared.DTOs.ContactDTO
{
	public class ContactInfoDTO
	{

			public long Id { get; set; }
			public long? UserId { get; set; }
			public long? FriendId { get; set; }
			public string Status { get; set; }
			public string NameFriend { get; set; }

	}
}
