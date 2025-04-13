using web_api_base.Models.dbebay;

public interface IRoleRepository : IRepository<Role>
{
    // Add custom methods for Role here if needed
}

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(EbayContext context) : base(context)
    {
    }
}