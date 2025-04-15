namespace Chat_App_Shared.Database.EntittyBase
{
	public interface IEntityModel
	{
		long Id { get; set; }
		DateTime CreatedAt { get; set; }	
		DateTime? UpdatedAt { get; set; }
	}
}
