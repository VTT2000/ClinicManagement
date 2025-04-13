namespace web_api_base.Models.ViewModel
{
    public class UserLoginVM
    {
        public UserLoginVM() {

        }
        public required  string Account { get; set; } ="";
        public required  string Password { get; set; } = "";
    }


    public class UserLoginResultVM
    {
        public string Account { get; set; }
        public string AccessToken { get; set; }
    }


}
