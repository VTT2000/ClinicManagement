using web_api_base.Models.dbebay;

public interface IUserRepository : IRepository<User>
{
    // Add custom methods for User here if needed
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(EbayContext context) : base(context)
    {
    }
}