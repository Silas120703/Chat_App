using Microsoft.EntityFrameworkCore;
using Chat_App_Database.Entities;
using Chat_App_Shared.Verification;
using Chat_App_Shared.Extensions;
namespace Chat_App_Database;

public class ChatAppDBContext : DbContext
{
	public ChatAppDBContext()
	{
	}
	public ChatAppDBContext(DbContextOptions<ChatAppDBContext> options)
		: base(options)
	{
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.RegisterDatabaseEntities("Chat_App");
		

	}
}


