using System;

namespace web_api_base.Models.FEViewModel
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImageUrl { get; set; }
        public object Doctor { get; set; }
        public object Patient { get; set; }
    }
}