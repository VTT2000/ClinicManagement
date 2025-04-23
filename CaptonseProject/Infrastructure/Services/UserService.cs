using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Helper;
using web_api_base.Models.ClinicManagement;
using web_api_base.Models.ViewModel;

public interface IUserService
{
  Task<HTTPResponseClient<IEnumerable<GetAllUsersVM>>> GetAllUserAsync();
  Task<HTTPResponseClient<dynamic>> Register(UserRegisterVM newUser);

  Task<dynamic> AdminCreateUser(AdminRegisterUserVM newUser, string imageUrl);

  Task<HTTPResponseClient<UserLoginResultVM>> Login(UserLoginVM userLogin);

  public Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsStaffAsync();
  public Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsPatientAsync();


  Task<User> GetUserById(int id);

  Task<dynamic> GetProfileUser(string authorization);
  Task<dynamic> ChangePassword(ChangePasswordVM model, string authorization);


}

public class UserService : IUserService
{

  public UnitOfWork _unitOfWork;
  public JwtAuthService _JwtAuthService;
  private readonly ClinicContext _context;
  public UserService(UnitOfWork unitOfWork, JwtAuthService JwtAuthService, ClinicContext context)
  {
    _unitOfWork = unitOfWork;
    _JwtAuthService = JwtAuthService;
    _context = context;
  }

  public async Task<dynamic> AdminCreateUser(AdminRegisterUserVM newUser,  string imageUrl = null)
  {
    //Kiểm tra role của người dùng
    if (newUser.Role != RoleConstant.Admin &&
    newUser.Role != RoleConstant.Technician &&
    newUser.Role != RoleConstant.Patient &&
    newUser.Role != RoleConstant.Doctor &&
    newUser.Role != RoleConstant.Receptionist)
    {

      return new HTTPResponseClient<UserLoginResultVM>
      {
        Message = @$"Role không hợp lệ. Role chỉ được phép là {RoleConstant.Admin}, {RoleConstant.Technician}, {RoleConstant.Patient}, {RoleConstant.Doctor}, {RoleConstant.Receptionist}",
        Data = null,
        StatusCode = StatusCodes.Status400BadRequest,
        DateTime = DateTime.Now
      };

    }

    //Kiểm tra email đã tồn tại trong db chưa
    var userCheckEmail = _unitOfWork._userRepository.GetAllAsync().Result.FirstOrDefault(x => x.Email == newUser.Email);
    if (userCheckEmail != null)
    {
      return new HTTPResponseClient<UserLoginResultVM>
      {
        Message = "Email đã tồn tại trong hệ thống",
        Data = null,
        StatusCode = StatusCodes.Status400BadRequest,
        DateTime = DateTime.Now
      };
    }

    var user = new User()
    {
      FullName = newUser.FullName,
      Email = newUser.Email,
      PasswordHash = PasswordHelper.HashPassword(newUser.Password),
      Role = newUser.Role,
      ImageUrl = imageUrl
    };
    try
    {
      //Commit transaction
      await _unitOfWork.BeginTransaction();
      //Lưu thông tin người dùng bảng User
      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
      //Nếu là bác sĩ thì lưu thông tin vào bảng Doctor
      if (user.Role == RoleConstant.Doctor)
      {
        var doctor = new Doctor()
        {
          UserId = user.UserId,
        };
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();
      }

      //Nếu là bệnh nhân thì lưu thông tin vào bảng Patient
      if (user.Role == RoleConstant.Patient)
      {
        var patient = new Patient()
        {
          UserId = user.UserId,
        };
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
      }

      //Commit transaction
      await _unitOfWork.CommitTransaction();
    }
    catch (Exception ex)
    {
      //Rollback transaction
      await _unitOfWork.RollBack();
       // Nếu có lỗi và đã upload ảnh, xóa file ảnh để không rác hệ thống
        if (!string.IsNullOrEmpty(imageUrl))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
      return new HTTPResponseClient<UserLoginResultVM>
      {
        Message = ex.Message,
        Data = null,
        StatusCode = StatusCodes.Status500InternalServerError,
        DateTime = DateTime.Now
      };
    }
    var userDTO = new UserDTO
    {
      UserId = user.UserId,
      FullName = user.FullName,
      Email = user.Email,
      Role = user.Role,
      CreatedAt = user.CreatedAt,
      ImageUrl = user?.ImageUrl ?? string.Empty
    };
    return new HTTPResponseClient<dynamic>
    {
      Message = "User registered successfully",
      Data = new
      {
        User = userDTO,
      },
      StatusCode = StatusCodes.Status200OK
    };
  }
  public async Task<HTTPResponseClient<IEnumerable<GetAllUsersVM>>> GetAllUserAsync()
  {
    var result = new HTTPResponseClient<IEnumerable<GetAllUsersVM>>();
    try
    {
      var data = await _unitOfWork._userRepository.GetAllAsync();
      //Bỏ feld PasswordHash ra khỏi dữ liệu trả về
      var userDtos = data.Select(item => new GetAllUsersVM()
      {
        UserID = item.UserId,
        FullName = item.FullName,
        Email = item.Email,
        Role = item.Role,
        CreatedAt = item.CreatedAt,
        ImageUrl = item.ImageUrl ?? "data:image/png;base64,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",
      });
      result.Data = userDtos;
    }
    catch (Exception ex)
    {
      result.Message = ex.Message;
      result.Data = null;
      result.StatusCode = StatusCodes.Status500InternalServerError;
    }
    result.DateTime = DateTime.Now;


    return result;
  }

  public async Task<HTTPResponseClient<UserLoginResultVM>> Login(UserLoginVM userLogin)
  {
    var result = new HTTPResponseClient<UserLoginResultVM>();
    try
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
        result.Data = usLoginResult;
      }
      //Luu thông tin người dùng vào cookie
      var cookieOption = new CookieOptions()
      {
        HttpOnly = true,
        Secure = true,
        Expires = DateTime.Now.AddDays(1)
      };
      //Lưu thông tin người dùng vào cookie
      var httpContext = new HttpContextAccessor();
      if (httpContext.HttpContext != null)
      {
        var response = httpContext.HttpContext.Response;
        response.Cookies.Append("UserInfo", userLogin.Account, cookieOption);
      }
    }
    catch (Exception ex)
    {
      result.Message = ex.Message;
      result.Data = null;
      result.StatusCode = StatusCodes.Status500InternalServerError;
    }
    return result;

  }

  public async Task<HTTPResponseClient<dynamic>> Register(UserRegisterVM newUser)
  {
    var user = new User()
    {
      FullName = newUser.FullName,
      Email = newUser.Email,
      PasswordHash = PasswordHelper.HashPassword(newUser.Password),
      Role = RoleConstant.Patient,
    };
    //Kiểm tra email đã tồn tại trong db chưa
    var userCheckEmail = _unitOfWork._userRepository.GetAllAsync().Result.FirstOrDefault(x => x.Email == newUser.Email);
    if (userCheckEmail != null)
    {
      return new HTTPResponseClient<dynamic>
      {
        Message = "Email đã tồn tại trong hệ thống",
        Data = null,
        StatusCode = StatusCodes.Status400BadRequest
      };
    }


    //Commit transaction
    try
    {
      await _unitOfWork.BeginTransaction();
      //Lưu thông tin người dùng bảng User
      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
      //lưu thông tin vào bảng Patient
      var patient = new Patient()
      {
        UserId = user.UserId,
      };
      await _context.Patients.AddAsync(patient);
      await _context.SaveChangesAsync();

      //Commit transaction
      await _unitOfWork.CommitTransaction();

      ;
    }
    catch (System.Exception)
    {
      //Rollback transaction
      await _unitOfWork.RollBack();
      throw;
    }

    // Tạo một đối tượng User mới không có tham chiếu đến Patient
    var userDTO = new UserDTO
    {
      UserId = user.UserId,
      FullName = user.FullName,
      Email = user.Email,
      Role = user.Role,
      CreatedAt = user.CreatedAt,
      ImageUrl = user?.ImageUrl ?? string.Empty
    };
    return new HTTPResponseClient<dynamic>
    {
      Message = "User registered successfully",
      Data = new
      {
        User = userDTO,
        AccessToken = _JwtAuthService.GenerateToken(user)
      },
      StatusCode = StatusCodes.Status200OK
    };
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