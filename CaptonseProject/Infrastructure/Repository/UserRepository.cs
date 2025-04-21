using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using web_api_base.Models.ClinicManagement;

public interface IUserRepository : IRepository<User>
{

    //Nếu muốn định nghĩa thêm thì liệt kê
    public string otherFunction();
}

public class UserRepository : Repository<User>, IUserRepository
{
    
    public UserRepository(ClinicContext context): base(context)
    {
        
    }

    public string otherFunction()
    {
        // _dbSet
        return "handle other function";
    }
}