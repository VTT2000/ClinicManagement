public interface IAdminService
{
  
}

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtAuthService _jwtAuthService;

    public AdminService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _jwtAuthService = jwtAuthService;
    }

    // Implement methods for admin functionalities here

}