using Chat_App_Database;
using Chat_App_Database.Entities;
using Chat_App_Database.Repositories;
using Chat_App_Shared.Services;
using System.Threading.Tasks;

namespace Chat_App_Core.Services
{
	public class UserConnectionService:ServiceBase<UserConnection>
	{
		private readonly UserConnectionRepository _userConnectionRepository;
		private readonly UserRepository _userRepository;
		public UserConnectionService(UserConnectionRepository userConnectionRepository, UserRepository userRepository) : base(userConnectionRepository)
		{
			_userConnectionRepository = userConnectionRepository;
			_userRepository = userRepository;
		}
		public async Task<List<UserConnection>> GetUserConnectionsbyEmailAsync(string userEmail)
		{
			var user = await  _userRepository.GetByEmailAsync(userEmail);
			if (user == null)
			{
				return null;
			}
			return await _userConnectionRepository.GetUserConnectionsAsync(user.Id);
		}
		public async Task<List<UserConnection>> GetUserConnectionByUserIdAsync(long userId)
		{
			return await _userConnectionRepository.GetUserConnectionsAsync(userId);
		}
		public async Task<UserConnection> AddConnection(string userEmail, string connectionId, string browserId)
		{
			if(connectionId == null || userEmail == null || browserId==null)
			{
				return null;
			}
			var user = await _userRepository.GetByEmailAsync(userEmail);
			if (user == null)
			{
				return null;
			}
			var userConnectionExist = await _userConnectionRepository.GetUserConnectByUserIdAndBrowserIdAsync(user.Id, browserId);
			if (userConnectionExist != null)
			{
				userConnectionExist.ConnectionId = connectionId;
				_userConnectionRepository.Update(userConnectionExist);
				return userConnectionExist;
			}
			else
			{
				var userConnection = new UserConnection
				{
					UserId = user.Id,
					ConnectionId = connectionId,
					BrowserId = browserId
				};
				return await _userConnectionRepository.AddAsync(userConnection);
			}
			
		}
		public async Task<UserConnection> RemoveConnection(string userEmail,string browserId)
		{
			if (browserId == null || userEmail == null )
			{
				return null;
			}
			var user =  _userRepository.GetByEmail(userEmail);
			if (user == null)
			{
				return null;
			}
			var userConnection = await _userConnectionRepository.GetUserConnectByUserIdAndBrowserIdAsync(user.Id,browserId);
			if (userConnection == null)
			{
				return null;
			}
			_userConnectionRepository.Delete(userConnection);
			return userConnection;
		}
	}
}
