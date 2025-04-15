using Chat_App_Shared.Database.EntittyBase;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat_App_Database.Entities
{
	[Table("Configs")]
	[Index(nameof(Key), IsUnique = true)]
	public class Config : EntityBase
	{
		[Column(TypeName = "nvarchar(250)")]
		public string Key { get; set; }
		public string Value { get; set; }
	}
}
