using web_api_base.Models.ClinicManagement;

public interface IUserRepository : IRepository<User>
{
    // Add custom methods for User here if needed
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ClinicContext context) : base(context)
    {
    }
}