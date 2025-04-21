using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using web_api_base.Models.ClinicManagement;
using web_api_base.Models.ViewModel;

public class JwtAuthService
{
    private readonly string? _key;
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly ClinicContext _context;
    public JwtAuthService(IConfiguration Configuration, ClinicContext db)
    {
        _key = Configuration["jwt:Serect-Key"];
        _issuer = Configuration["jwt:Issuer"];
        _audience = Configuration["jwt:Audience"];
        _context = db;
    }

    public string GenerateToken(web_api_base.Models.ClinicManagement.User userLogin)
    {
        // Khóa bí mật để ký token
        var key = Encoding.ASCII.GetBytes(_key);
        // Kiểm tra role từ database và ánh xạ bằng RoleConstant
        string role = userLogin.Role switch
        {
            RoleConstant.Admin => RoleConstant.Admin,
            RoleConstant.User => RoleConstant.User,
            RoleConstant.Doctor => RoleConstant.Doctor,
            RoleConstant.Technician => RoleConstant.Technician,
            RoleConstant.Receptionist => RoleConstant.Receptionist,
            _ => throw new Exception("Invalid role") // Xử lý nếu role không hợp lệ
        };
        // Tạo danh sách các claims cho token
        var claims = new List<Claim>
        {
            new Claim("Email", userLogin.Email),               // Claim mặc định cho username
            // new Claim(ClaimTypes.Role, userLogin.Role),                   // Claim mặc định cho Role
            new Claim(JwtRegisteredClaimNames.Sub, userLogin.FullName),   // Subject của token
            
            new Claim("Name", userLogin.FullName),   // Subject của token          
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Thời gian tạo token
            //Add role vào claims
            new Claim(ClaimTypes.Role, role) //Thêm role vào claims

        };


        // Tạo khóa bí mật để ký token
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
        // Thiết lập thông tin cho token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(30), // Token hết hạn sau 1 giờ
            SigningCredentials = credentials,
            Issuer = _issuer,                 // Thêm Issuer vào token
            Audience = _audience,              // Thêm Audience vào token
        };
        // Tạo token bằng JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Trả về chuỗi token đã mã hóa
        return tokenHandler.WriteToken(token);
    }

    public string DecodePayloadToken(string token)
    {
        try
        {
            // Kiểm tra token có null hoặc rỗng không
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token không được để trống", nameof(token));
            }

            // Tạo handler và đọc token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Lấy username từ claims (thường nằm trong claim "sub" hoặc "name")
            var usernameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "Name"); // Common in some identity providers

            if (usernameClaim == null)
            {
                throw new InvalidOperationException("Không tìm thấy Name trong payload");
            }

            return usernameClaim.Value;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi (có thể log lỗi ở đây)
            throw new InvalidOperationException($"Lỗi khi decode token: {ex.Message}", ex);
        }
    }

}