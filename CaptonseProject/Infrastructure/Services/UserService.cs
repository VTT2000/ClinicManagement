// using Azure;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.AspNetCore.Mvc;
// using web_api_base.Helper;
// using web_api_base.Models.dbebay;
// using web_api_base.Models.ViewModel;

// public interface IUserService
// {

//     Task<dynamic> Register(dynamic newUser);
//     Task<ActionResult> Login(UserLoginVM userLogin);

// }
// public class UserService : IUserService
// {
//     public IUnitOfWork _unitOfWork;
//     public JwtAuthService _JwtAuthService;
//     public UserService(IUnitOfWork unitOfWork, JwtAuthService JwtAuthService)
//     {
//         _unitOfWork = unitOfWork;
//         _JwtAuthService = JwtAuthService;
//     }
//     public async Task<ActionResult> Login(UserLoginVM userLogin)
//     {
        
//         //Kiểm tra user trong database
//         User? userDB = await _unitOfWork._userRepository.SingleOrDefaultAsync(n => n.Username == userLogin.Account || n.Email == userLogin.Account);
//         if (userDB != null && PasswordHelper.VerifyPassword(userLogin.Password, userDB.PasswordHash))
//         {
//             // Đăng nhập thành công
//             //Tạo token trả vào userLoginResult
//             UserLoginResultVM usResult = new UserLoginResultVM();
//             usResult.Account = userLogin.Account;
//             usResult.AccessToken = _JwtAuthService.GenerateToken(userDB);
//             var resOb = new HTTPResponseClient<UserLoginResultVM>()
//             {
//                 StatusCode = 200,
//                 Message = "Successfully",
//                 DateTime = DateTime.Now,
//                 Data = usResult

//             };
         
       
//             return new OkObjectResult(resOb);
//         }
//         var failOb = new HTTPResponseClient<UserLoginResultVM>()
//         {
//             StatusCode = 400,
//             Message = "Login fail",
//             DateTime = DateTime.Now,
//             Data = null
//         };
//         return new BadRequestObjectResult(failOb);
//     }
//     public Task<dynamic> Register(dynamic newUser)
//     {
//         throw new NotImplementedException();
//     }
//     //Generate token 
// }


