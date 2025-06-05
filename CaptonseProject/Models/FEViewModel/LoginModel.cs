// Model classes
  public class LoginResult
  {
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
  }



  public class UserLoginResultVM
  {
    public string AccessToken { get; set; }
    public UserInfo User { get; set; }
  }

  public class UserInfo
  {
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    // Thêm các thuộc tính khác nếu cần
  }