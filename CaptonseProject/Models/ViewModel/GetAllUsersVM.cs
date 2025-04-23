namespace web_api_base.Models.ViewModel;

public class GetAllUsersVM
{
    public int UserID { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } 
    public DateTime? CreatedAt { get; set; } 
    public string ImageUrl { get; set; }
}    