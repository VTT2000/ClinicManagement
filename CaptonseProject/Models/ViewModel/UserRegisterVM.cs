namespace web_api_base.Models.ViewModel
{
    public class UserRegisterVM
    {
        public UserRegisterVM() {

        }

        public required string FullName {get; set;} = "";

        public required  string Email { get; set; } ="";
        
        
        public required  string Password { get; set; } = "";
    }


    public class AdminRegisterUserVM : UserRegisterVM
    {
        public  required string Role { get; set; } ="";
       public IFormFile ImageFile { get; set; }
    }

    public class UserRegisterResultVM
    {
        public string Account { get; set; }
        public string AccessToken { get; set; }
    }


}
