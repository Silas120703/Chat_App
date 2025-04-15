using Chat_App_Database.Entities;
using Chat_App_Database.Repositories;
using Chat_App_Shared.DTOs.ContactDTO;
using Chat_App_Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat_App_Core.Services
{
	public class ContactService:ServiceBase<Contact>
	{
		private readonly ContactRepository _contactRepository;
		private readonly UserRepository _userRepository;
		public ContactService(ContactRepository contactRepository,UserRepository userRepository) : base(contactRepository)
		{
			_contactRepository = contactRepository;
			_userRepository = userRepository;
		}
		public async Task<Contact> AddContactAsync(Contact contact)
		{
			return await _contactRepository.AddAsync(contact);
		}
		public async Task<Contact> GetContactByIDAsync(long id)
		{
			return await _contactRepository.GetContactByIDAsync(id);
		}
		public async Task<Contact> PutContactByUserIDAsync(long id, Contact contact)
		{
			return await _contactRepository.PutContactByUserID(id, contact);
		}
		public async Task<List<object>> GetContactsByUserEmailAsync([FromBody] string Email)
		{
			var user = await _userRepository.GetByEmailAsync(Email);
			if (user == null)
			{
				return null;
			}
			return await _contactRepository.GetContactsByUserIDAsync(user.Id);
		}
		public async Task<Contact> GetFriendByID(ContactRequestDTO contact)
		{
			var user = await _userRepository.GetByEmailAsync(contact.UserEmail);
			var friend = await _userRepository.GetByEmailAsync(contact.FriendEmail);
			if (user == null || friend == null)
			{
				return null;
			}
			var contactExist = await _contactRepository.GetFriendByIDAsync(user.Id, friend.Id);
			if (contactExist == null)
			{
				return null;
			}
				return contactExist;
		}
		public async Task<Contact> AddFriendAsync(ContactRequestDTO contact)
		{
			var user = await _userRepository.GetByEmailAsync(contact.UserEmail);
			var friend = await _userRepository.GetByEmailAsync(contact.FriendEmail);
			if (user == null || friend == null)
			{
				return null;
			}
			var contactExist = await _contactRepository.GetFriendStatusByIDAsync(user.Id, friend.Id);
			if (contactExist != null)
			{
				if (contactExist.Status == "Rejected")
				{
					contactExist.Status = "Sent";
					_contactRepository.Update(contactExist);
				}
				
					return contactExist;
			}


					
			return await _contactRepository.AddFriendAsync(user, friend);

		}
			

		public async Task<Contact> AcceptFriendAsync(ContactRequestDTO contact)
		{
			var user = await _userRepository.GetByEmailAsync(contact.UserEmail);
			var friend = await _userRepository.GetByEmailAsync(contact.FriendEmail);
			if (user == null || friend == null)
			{
				return null;
			}
			var contactExist = await _contactRepository.GetFriendStatusByIDAsync(user.Id, friend.Id);
			if (contactExist == null)
			{
				return null;
			}
			if (contactExist.Status == "Sent")
			{
				await _contactRepository.AddFriendAsync(user, friend);
				await _contactRepository.AcceptFriendAsync(friend, user);
				return await _contactRepository.AcceptFriendAsync(user, friend);
			}
			return null;
		}

		public async Task<Contact> RejectFriendAsync(ContactRequestDTO contact)
		{
			var user = await _userRepository.GetByEmailAsync(contact.UserEmail);
			var friend = await _userRepository.GetByEmailAsync(contact.FriendEmail);
			if (user == null || friend == null)
			{
				return null;
			}
			var contactExist = await _contactRepository.GetFriendStatusByIDAsync(user.Id, friend.Id);
			if (contactExist == null)
			{
				return null;
			}
			if (contactExist.Status == "Sent")
			{

				_contactRepository.Delete(contactExist);
				return contactExist;
			}
			return null;
		}
		public async Task<Contact> DeleteFriendAsync(ContactRequestDTO contact)
		{
			var user = await _userRepository.GetByEmailAsync(contact.UserEmail);
			var friend = await _userRepository.GetByEmailAsync(contact.FriendEmail);
			if (user == null || friend == null)
			{
				return null;
			}
			var contactExist = await _contactRepository.GetFriendStatusByIDAsync(user.Id, friend.Id);
			var contactExist1 = await _contactRepository.GetFriendStatusByIDAsync(friend.Id,user.Id);
			if (contactExist == null)
			{
				return null;
			}

				_contactRepository.Delete(contactExist);
				_contactRepository.Delete(contactExist1);
			return contactExist;


		}
		public async Task<Contact> GetFriendStatusByIDAsync(ContactRequestDTO contact)
		{
			var user = await _userRepository.GetByEmailAsync(contact.UserEmail);
			var friend = await _userRepository.GetByEmailAsync(contact.FriendEmail);
			if (user == null || friend == null)
			{
				return null;
			}
			return await _contactRepository.GetFriendStatusByIDAsync(user.Id, friend.Id);
		}
	}	
	
	
}
