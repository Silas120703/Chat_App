
namespace Chat_App_Shared.Database.EntittyBase
{
	public abstract class EntityBase : IEntityModel
	{
		public long Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
