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

  Task<User> GetUserById(int id);

  Task<dynamic> GetProfileUser(string authorization);
  Task<dynamic> ChangePassword(ChangePasswordVM model, string authorization);


}

public class UserService : IUserService
{
  public IUnitOfWork _unitOfWork;
  public JwtAuthService _JwtAuthService;
  private readonly ClinicContext _context;
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

  public async Task<User> GetUserById(int id)
  {
    var user = await _unitOfWork._userRepository.GetByIdAsync(id);
    return user;
  }

  public async Task<dynamic> ChangePassword(ChangePasswordVM model, string authorization)
  {
    //Lấy thông tin user từ token
    var user = GetProfileUser(authorization);
    //Kiểm tra mật khẩu cũ có đúng không
    if (user == null || !PasswordHelper.VerifyPassword(model.OldPassword, user.Result.PasswordHash))
    {
      return null;
    }
    //Nếu đúng thì cập nhật mật khẩu mới
    user.Result.PasswordHash = PasswordHelper.HashPassword(model.NewPassword);
    //Cập nhật vào db
    var userResult = await user as User;
    _unitOfWork._userRepository.Update(userResult);
    await _unitOfWork.SaveChangesAsync();

    return user;
  }

  public async Task<dynamic> GetProfileUser(string authorization)
  {
      string token = authorization.Replace("Bearer ", "");
      string name = _JwtAuthService.DecodePayloadToken(token);
      var users = await _unitOfWork._userRepository.GetAllAsync();
      var user = users.FirstOrDefault(x => x.FullName == name);
      if (user == null)
      {
          return null;
      }
      return user;
  }
}