using Chat_App_Database.Entities;
using Chat_App_Database.Repositories;
using Chat_App_Shared.DTOs;
using Chat_App_Shared.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;

namespace Chat_App_Core.Services
{
	public class ConversationService : ServiceBase<Conversation>
	{
		private readonly ConversationRepository _conversationRepository;
		private readonly UserRepository _userRepository;
		private readonly GroupRepository _groupRepository;
		private readonly GroupMemberRepository _groupMemberRepository;
		public ConversationService(ConversationRepository conversationRepository, UserRepository userRepository, GroupRepository groupRepository, GroupMemberRepository groupMemberRepository) : base(conversationRepository)
		{
			_conversationRepository = conversationRepository;
			_userRepository = userRepository;
			_groupRepository = groupRepository;
			_groupMemberRepository = groupMemberRepository;
		}

		public async Task<List<ConversationDTO>> GetAllConversations(string userEmail)
		{

			var user = await _userRepository.GetByEmailAsync(userEmail);
			if (user == null)
			{
				return null;
			}
			List<ConversationDTO> conversationDTOs = new List<ConversationDTO>();
			var conversation = await _conversationRepository.GetAllConversations(user.Id);
			foreach (var item in conversation)
			{
				if (item.GroupId == null)
				{
					continue;
				}
				var group = await _groupRepository.GetByIdAsync(item.GroupId.Value);

				if (group == null)
				{
					continue;
				}


				if (group.IsGroup == true)
				{

					conversationDTOs.Add(new ConversationDTO
					{
						GroupId = group.Id,
						UserId = item.UserId.Value,
						GroupName = group.Name,
						ProfilePiture = group.Image,
						IsGroup = group.IsGroup,
						UserEmail = null,
						CreatedAt = item.CreatedAt,
						CountNotifications = item.CountNotifications
					});
		
				}
				else
				{
					var groupMembers = await _groupMemberRepository.GetByGroupId(item.GroupId.Value);
					var user1 = _userRepository.GetByEmail(userEmail);
					foreach (var groupMember in groupMembers)
					{
						if (groupMember.UserId.Value == user1.Id)
						{
							continue;
						}
						else
						{
							var userOther = _userRepository.GetById(groupMember.UserId.Value);
							conversationDTOs.Add(new ConversationDTO
							{
								GroupId = group.Id,
								UserId = userOther.Id,
								GroupName = userOther.FirstName + " " + userOther.LastName,
								ProfilePiture = userOther.ProfilePicture,
								UserEmail=userOther.Email,
								IsGroup = group.IsGroup,
								CreatedAt = item.CreatedAt,
								CountNotifications = item.CountNotifications
							});
						}
					}
					
				}

			
			}
			return conversationDTOs.OrderByDescending(c=> c.CreatedAt).ToList();
		}
		public async Task<List<Conversation>> CreateConversation(long groupId,string emailUser)
		{
			List<Conversation> conversations = new List<Conversation>();
			var groupMembers = await _groupMemberRepository.GetByGroupId(groupId);
			if (groupMembers == null)
			{
				return null;
			}
			var user =  _userRepository.GetByEmail(emailUser);
			foreach (var item in groupMembers)
			{
				var existingConversation = await _conversationRepository.GetConversationByGroupIdAndUserIdAsync(groupId, item.UserId.Value);
				if (existingConversation == null)
				{
					if (item.UserId.Value == user.Id)
					{
						continue;
					}
					var conversation = new Conversation
					{
						GroupId = groupId,
						UserId = item.UserId,
						CreatedAt = DateTime.Now,
						IsRemove = false,
						CountNotifications = 1
					};
					await _conversationRepository.AddAsync(conversation);
					conversations.Add(conversation);
				}
				else
				{
					if (item.UserId.Value == user.Id)
					{
						continue;
					}
					existingConversation.CreatedAt = DateTime.Now;
					existingConversation.IsRemove = false;
					existingConversation.CountNotifications = existingConversation.CountNotifications + 1;
					_conversationRepository.Update(existingConversation);
				}

			}
			return conversations ;
		}

		public async Task<List<Conversation>> ReCreateConversation(long groupId)
		{
			var existingConversation = await _conversationRepository.GetConversationByGroupId(groupId);
			if (existingConversation != null)
			{
				foreach (var item in existingConversation)
				{
					item.IsRemove = false;
					item.CountNotifications = 1;
					item.CreatedAt = DateTime.Now;
					base.Update(item);
				}
				return existingConversation;
			}
			return null;


		}



		public async Task<List<Conversation>> CreateNewConversations(long groupId)
		{
			List<Conversation> conversations = new List<Conversation>();
			var groupMembers = await _groupMemberRepository.GetByGroupId(groupId);
			if (groupMembers == null)
			{
				return null;
			}
			foreach (var item in groupMembers)
			{
					var conversation = new Conversation
					{
						GroupId = groupId,
						UserId = item.UserId.Value,
						CreatedAt = DateTime.Now,
						IsRemove = false,
						CountNotifications = 1
					};
					await _conversationRepository.AddAsync(conversation);
					conversations.Add(conversation);
					

			}
			return conversations;
		}

		public async Task<ConversationDTO> GetConversationByGroupIdAsync(long groupId,string userEmail)
		{
			var group = await _groupRepository.GetByIdAsync(groupId);

			if (group == null)
			{
				return null;
			}

			if (group.IsGroup == true)
			{
				var conversationDTO = new ConversationDTO
				{
					GroupId = group.Id,
					GroupName = group.Name,
					ProfilePiture = group.Image
				};
				return conversationDTO;
			}
			else
			{
				var groupMembers = await _groupMemberRepository.GetByGroupId(groupId);
				var user1 = _userRepository.GetByEmail(userEmail);
				foreach (var groupMember in groupMembers)
				{
					if (groupMember.UserId.Value == user1.Id)
					{
						continue;
					}
					else
					{
						var userOther = _userRepository.GetById(groupMember.UserId.Value);
						var conversationDTO= new ConversationDTO
						{
							GroupId = group.Id,
							GroupName = userOther.FirstName + " " + userOther.LastName,
							ProfilePiture = userOther.ProfilePicture
						};
						return conversationDTO;
					}
				}
			}
			return null;

		}
		public async Task<Conversation> DeleteConversation(long groupId, string userEmail)
		{
			var user = await _userRepository.GetByEmailAsync(userEmail);
			if (user == null)
			{
				return null;
			}
			var conversation = await _conversationRepository.GetConversationByGroupIdAndUserIdAsync(groupId, user.Id);
			if (conversation == null)
			{
				return null;
			}
			conversation.IsRemove = true;
			base.Update(conversation);
			return conversation;
		}

		public async Task<List<Conversation>> DeleteAllConversationbyGroupId(long groupId)
		{
			var conversations = await _conversationRepository.GetConversationByGroupId(groupId);
			if (conversations == null)
			{
				return null;
			}
			foreach (var item in conversations)
			{
				item.IsRemove = true;
				base.Update(item);
			}
			return conversations;
		}
		
			
	}


	
}
