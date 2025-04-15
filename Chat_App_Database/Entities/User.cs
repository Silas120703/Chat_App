using Chat_App_Shared.Database.EntittyBase;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Chat_App_Database.Entities;

[Table("Users")]
[Index(nameof(Email), IsUnique = true)]
public partial class User : EntityBase
{
    [Column(TypeName = "nvarchar(250)")]
	public string FirstName { get; set; } = null!;

	[Column(TypeName = "nvarchar(250)")]
	public string LastName { get; set; } = null!;
	public DateOnly? BirthDay { get; set; }
	public string? Gender { get; set; }

	[Column(TypeName = "nvarchar(250)")]
	public string Email { get; set; } = null!;

	public string? ProfilePicture { get; set; } = null!;

	[Column(TypeName = "nvarchar(200)")]
	public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public string Role { get; set; } = null!;

    public bool? OnlineStatus { get; set; }
	public bool? IsBlock { get; set; } = false;	
	[IgnoreDataMember]
	[JsonIgnore]
	public virtual ICollection<Contact>? ContactsAsUser { get; set; }
	[IgnoreDataMember]
	[JsonIgnore]
	public virtual ICollection<Contact>? ContactsAsFriend { get; set; }
	[IgnoreDataMember]
	[JsonIgnore]
	public virtual ICollection<GroupMember>? GroupMembers { get; set; }
	[IgnoreDataMember]
	[JsonIgnore]
	public virtual ICollection<Message>? Messages { get; set; }
	[IgnoreDataMember]
	[JsonIgnore]
	public virtual ICollection<Conversation>? Conversations { get; set; }
	[IgnoreDataMember]
	[JsonIgnore]
	public virtual ICollection<UserConnection>? UserConnections { get; set; }
}
