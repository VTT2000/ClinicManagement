// Thêm class DTO để tránh vòng lặp tham chiếu
public class UserDTO
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string ImageUrl { get; set; }
    // Không bao gồm Doctor và Patient để tránh vòng lặp
}