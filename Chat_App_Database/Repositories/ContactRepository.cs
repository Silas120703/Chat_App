using Chat_App_Database.Entities;
using Chat_App_Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace Chat_App_Database.Repositories
{
	public class ContactRepository : RepositoryBase<Contact>
	{
		public ContactRepository(ChatAppDBContext context) : base(context)
		{
		}
		public async Task<Contact> AddContact(Contact contact)
		{
			await base.AddAsync(contact);
			return contact;
		}
		public async Task<Contact> GetContactByIDAsync(long id)
		{
			return await base.GetAll().FirstOrDefaultAsync(e => e.Id == id);
		}
		public async Task<Contact> PutContactByUserID(long id, Contact contact)
		{
			var entity = await GetContactByIDAsync(id);
			if (entity == null)
			{
				return null;
			}
			entity.Status = contact.Status;
			entity.UpdatedAt = DateTime.Now;
			base.Update(entity);
			return entity;
		}
		public async Task<Contact> GetFriendByIDAsync(long userID, long friendID)
		{
			return await base.GetAll().FirstOrDefaultAsync(e => e.UserId == userID && e.FriendId==friendID && e.Status== "Accepted");
		}

		public async Task<Contact> GetFriendStatusByIDAsync(long userID, long friendID)
		{
			return await base.GetAll().FirstOrDefaultAsync(e => e.UserId == userID && e.FriendId == friendID );
		}

		public async Task<List<object>> GetContactsByUserIDAsync(long userID)
		{
			return await base.GetAll().Where(c => c.UserId == userID)
			.Select(c => (object)new
			{
				ContactId = c.Id,
				ContactStatus = c.Status,
				Friend = new
				{
					c.Friend.Id,
					c.Friend.FirstName,
					c.Friend.LastName,
					c.Friend.BirthDay,
					c.Friend.Gender,
					c.Friend.Email,
					c.Friend.ProfilePicture,
					c.Friend.Phone,
					c.Friend.Role,
					c.Friend.OnlineStatus
				}
			})
			.ToListAsync();
		}


		public async Task<List<Contact>> GetContactsSentByUserIDAsync(long userID)
		{
			return await base.GetAll().Where(e => e.UserId == userID && e.Status == "Sent").ToListAsync();
		}
		public async Task<Contact> AddFriendAsync(User friend , User user)
		{
			var contact = new Contact
			{
				User = user,
				Friend = friend,
				UserId = user.Id,
				FriendId = friend.Id,
				Status = "Sent",
				CreatedAt = DateTime.Now
			};

			await AddAsync(contact);
			return contact;
		}
		public async Task<Contact> AcceptFriendAsync(User friend, User user)
		{
			var contact = await GetFriendStatusByIDAsync(friend.Id, user.Id);
			if (contact == null)
			{
				return null;
			}
			contact.Status = "Accepted";
			 base.Update(contact);
			return contact;
		}

		public async Task<Contact> RejectFriendAsync(long friendID, long userID)
		{
			var contact = await GetFriendByIDAsync(friendID, userID);
			if (contact == null)
			{
				return null;
			}
			contact.Status = "Rejected";
			base.Update(contact);
			return contact;
		}
		
	}
}
