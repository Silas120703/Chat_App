using Chat_App_Database.Entities;
using Microsoft.EntityFrameworkCore;
using Chat_App_Shared.Services;

namespace Chat_App_Database.Repositories
{
	public class UserRepository : RepositoryBase<User>
	{
		public UserRepository(ChatAppDBContext context): base(context)
		{
			
		}

		public async Task<User> GetByEmailAsync(string mail)
		{
		    return await base.GetAll().FirstOrDefaultAsync(e=>e.Email == mail);
		}

		public User GetByEmail(string mail)
		{
			return base.GetAll().FirstOrDefault(e => e.Email == mail);
		}
		public async Task<User> GetByEmailAndPasswordAsync(string mail, string password)
		{
			return await base.GetAll().FirstOrDefaultAsync(e => e.Email == mail && e.PasswordHash == password);
		}
		public User GetByEmailAndPassword(string mail, string password)
		{
			return base.GetAll().FirstOrDefault(e => e.Email == mail && e.PasswordHash == password);
		}
		public async Task<User> GetByPhone(string phone)
		{
			return await base.GetAll().FirstOrDefaultAsync(e => e.Phone ==phone );
		}
		public async Task<User> GetCheckBlockByEmail(string userEmail)
		{
			return await base.GetAll().FirstOrDefaultAsync(e => e.Email==userEmail&&e.IsBlock==false);
		}



	}
}
