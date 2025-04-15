using Chat_App_Database.Entities;
using Chat_App_Database.Repositories;
using Chat_App_Shared.DTOs.MassageDTO;
using Chat_App_Shared.Services;

namespace Chat_App_Core.Services
{
	public class MessageService : ServiceBase<Message>
	{
		private readonly MessageRepository _messageRepository;
		private readonly UserRepository _userRepository;
		private readonly GroupRepository _groupRepository;
		private readonly ConversationRepository _conversationRepository;

		public MessageService(MessageRepository messageRepository,UserRepository userRepository,GroupRepository groupRepository,ConversationRepository conversationRepository) : base(messageRepository)
		{
			_messageRepository = messageRepository;
			_userRepository = userRepository;
			_groupRepository = groupRepository;
			_conversationRepository = conversationRepository;
		}
		public async Task<List<ResponeMessageDTO>> GetMessagesByGroupId(long groupId,string userEmail)
		{
			var messages = await _messageRepository.GetMessagesByGroupId(groupId);
			if (messages == null)
			{
				return null;
			}
			var messageDTOs = new List<ResponeMessageDTO>();
			foreach (var message in messages)
			{
				string Content;
				var user = await _userRepository.GetByIdAsync(message.UserId.Value);
				if (user != null)
				{
					if (message.IsDelete)
					{
						Content = null;
					}
					else
					{
						Content = message.Content;
					}
					var messageDTO = new ResponeMessageDTO
					{
						Id = message.Id,
						GroupId = message.GroupId.Value,
						userEmail = user.Email,
						Content = Content,
						Type = message.Type,
						sentdAt = message.CreatedAt,
						IsDeleted = message.IsDelete,
						userName = user.FirstName + " " + user.LastName,
						ProfilePiture = user.ProfilePicture
					};
					messageDTOs.Add(messageDTO);
				}
			}
			var myUser = _userRepository.GetByEmail(userEmail);
			await _conversationRepository.ResetConversationNotification(groupId,myUser.Id);
			return messageDTOs.OrderBy(m => m.sentdAt).ToList();

		}
		public async Task<ResponeMessageDTO> AddMessage(long groupId,  MessageDTO messageDTO)
		{
			var user = await _userRepository.GetByEmailAsync(messageDTO.EmailUser);
			if (user == null)
			{
				return null;
			}
			var group = await _groupRepository.GetByIdAsync(groupId);
			if (group == null)
			{
				return null;
			}
			var message = new Message
			{
				GroupId = groupId,
				UserId = user.Id,
				Content = messageDTO.Content,
				Type = messageDTO.Type,
				CreatedAt = DateTime.Now,
				IsDelete = false
			};
			var message1 = await _messageRepository.AddAsync(message);
			//Gửi đi bằng SignalR
			var responseMessage = new ResponeMessageDTO
			{
				Id = message1.Id,
				GroupId = groupId,
				userEmail = user.Email,
				Content = message1.Content,
				Type = message1.Type,
				sentdAt = message1.CreatedAt,
				IsDeleted = message1.IsDelete,
				userName = user.FirstName + " " + user.LastName,
				ProfilePiture = user.ProfilePicture
			};
			
			var conversationExit = await _conversationRepository.GetConversationByUserIdAndGroupId(user.Id, groupId);
			if (conversationExit == null)
			{
				var conversation = new Conversation
				{
					UserId = user.Id,
					GroupId = groupId,
					MessageId = message.Id,
					CreatedAt = message.CreatedAt,
				};
				await _conversationRepository.AddConversation(conversation);
			}
			else
			{
				_conversationRepository.Delete(conversationExit);
				var conversation = new Conversation
				{
					UserId = user.Id,
					GroupId = groupId,
					MessageId = message.Id,
					CreatedAt = message.CreatedAt,
				};
				await _conversationRepository.AddConversation(conversation);
			}
				
			return responseMessage;
		}
		public async Task<Message> DeleteMessage(long messageId)
		{
			var message = await _messageRepository.GetByIdAsync(messageId);
			if (message == null)
			{
				return null;
			}
			message.IsDelete = true;
			_messageRepository.Update(message);
			return message;
		}
	}
}
