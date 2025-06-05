
namespace web_api_base.ViewModel;
public class ProfilePatientVM
{
  public int Id { get; set; }
  public string FullName { get; set; }
  public string Email { get; set; }
  public string Role { get; set; }
  public DateTime CreatedAt { get; set; }
  public string ImageUrl { get; set; }

  public DateTime DOB { get; set; }

  public string Phone { get; set; }

  public string Address { get; set; }
}

public class UpdateProfilePatientVM
{

  public string FullName { get; set; }
  public string Email { get; set; }
  public string Role { get; set; }
  public string? ImageUrl { get; set; }

  public string DOB { get; set; }

  public string Phone { get; set; }

  public string Address { get; set; }
}

