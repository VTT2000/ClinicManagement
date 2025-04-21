using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Helper;
using web_api_base.Models.ClinicManagement;
using web_api_base.Models.ViewModel;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<dynamic> Register(UserRegisterVM newUser);

    Task<dynamic> AdminCreateUser(AdminRegisterUserVM newUser);

    UserLoginResultVM Login(UserLoginVM userLogin);

    public Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsStaffAsync();
    public Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsPatientAsync();
}

public class UserService : IUserService
{
    public IUnitOfWork _unitOfWork;
    public JwtAuthService _JwtAuthService;

    public UserService(IUnitOfWork unitOfWork, JwtAuthService JwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _JwtAuthService = JwtAuthService;
    }

    public async Task<dynamic> AdminCreateUser(AdminRegisterUserVM newUser)
    {
        var user = new User()
        {
            FullName = newUser.FullName,
            Email = newUser.Email,
            PasswordHash = PasswordHelper.HashPassword(newUser.Password),
            Role = newUser.Role,
        };
        await _unitOfWork._userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _unitOfWork._userRepository.GetAllAsync();
        return users;
    }

    public UserLoginResultVM Login(UserLoginVM userLogin)
    {
        //Tìm user trong db có  email
        var userCheckLogin = _unitOfWork._userRepository.GetAllAsync().Result.FirstOrDefault(x => x.Email == userLogin.Account);
        //Nếu account tồn tại trong db
        if (userCheckLogin != null && PasswordHelper.VerifyPassword(userLogin.Password, userCheckLogin.PasswordHash))
        {
            //Tạo token cho user
            var token = _JwtAuthService.GenerateToken(userCheckLogin);
            UserLoginResultVM usLoginResult = new UserLoginResultVM();
            usLoginResult.AccessToken = token;
            usLoginResult.User = userCheckLogin;
            return usLoginResult;
        }
        return null;
    }

    public async Task<dynamic> Register(UserRegisterVM newUser)
    {
        var user = new User()
        {
            FullName = newUser.FullName,
            Email = newUser.Email,
            PasswordHash = PasswordHelper.HashPassword(newUser.Password),
            Role = RoleConstant.User,
        };
        await _unitOfWork._userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user;
    }

    public async Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsStaffAsync()
    {
        var result = new HTTPResponseClient<IEnumerable<User>>();
        try
        {
            var data = await _unitOfWork._userRepository.WhereAsync(p => !p.Role.Equals(RoleUser.ADMIN) && !p.Role.Equals(RoleUser.BENH_NHAN));
            result.Data = data;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }

        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsPatientAsync()
    {
        var result = new HTTPResponseClient<IEnumerable<User>>();
        try
        {
            var data = await _unitOfWork._userRepository.WhereAsync(p => p.Role.Equals(RoleUser.BENH_NHAN));
            result.Data = data;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }

        result.DateTime = DateTime.Now;
        return result;
    }
}