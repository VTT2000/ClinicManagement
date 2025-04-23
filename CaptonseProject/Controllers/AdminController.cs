using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Models.ViewModel;

namespace CaptonseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/admin
        [Authorize(Roles = RoleConstant.Admin)]
        [HttpPost("AdminCreateUser")]
        public async Task<IActionResult> AdminCreateUser([FromForm] AdminRegisterUserVM newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data.");
            }

            // Xử lý file ảnh nếu có
            string imageUrl = null;
            if (newUser.ImageFile != null && newUser.ImageFile.Length > 0)
            {
                try
                {
                    // Tạo tên file duy nhất để tránh trùng lặp
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newUser.ImageFile.FileName);

                    // Đường dẫn lưu file
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Lưu file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newUser.ImageFile.CopyToAsync(fileStream);
                    }

                    // Đường dẫn tương đối để lưu vào database
                    imageUrl = "/images/avatars/" + fileName;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = $"Lỗi khi upload ảnh: {ex.Message}" });
                }
            }

            // Gọi service với imageUrl đã xử lý
            var result = await _userService.AdminCreateUser(newUser, imageUrl);
            return Ok(result);
        }
    }
}