using System.ComponentModel.DataAnnotations;

public class ChangePasswordVM
{
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới")]
    [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp với mật khẩu mới")]
    public string ConfirmPassword { get; set; }
}