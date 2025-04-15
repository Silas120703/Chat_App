using Chat_App_Database.Entities;
using Chat_App_Shared.DTOs.UserDTO;

namespace Chat_App_Core.Mappers
{
	public static class UserMapper
	{
		public static UserDTO MapToDTO(this User user)
		{
			return new UserDTO
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				BirthDay = (DateOnly)user.BirthDay,
				Email = user.Email,
				Phone = user.Phone,
				Gender= user.Gender,
				ProfilePicture = user.ProfilePicture
			};
		}
		
		
		
	}
}
