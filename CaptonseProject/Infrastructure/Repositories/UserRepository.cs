using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;
using web_api_base.ViewModel;
public interface IUserRepository : IRepository<User>
{
    // Add custom methods for User here if needed
    Task<ProfilePatientVM> GetPatientById(int id);

    Task<ProfilePatientVM> UpdatePatientById(int id, UpdateProfilePatientVM newPatient, IFormFile file);


}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ClinicContext context) : base(context)
    {
    }

    public async Task<ProfilePatientVM> GetPatientById(int id)
    {

        var patient = await _context?.Users
            .Include(p => p.Patient) // Include related User entity
            .Select(p => new ProfilePatientVM
            {
                Id = p.UserId,
                FullName = p.FullName != null ? p.FullName : string.Empty,
                Email = p.Email != null ? p.Email : string.Empty,
                Role = p.Role != null ? p.Role : string.Empty,
                CreatedAt = p.CreatedAt != null ? (p.CreatedAt ?? DateTime.MinValue) : DateTime.MinValue,
                ImageUrl = p.ImageUrl != null ? p.ImageUrl : string.Empty,
                DOB = p.Patient.Dob.HasValue ? p.Patient.Dob.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                Phone = p.Patient.Phone ?? string.Empty,
                Address = p.Patient.Address ?? string.Empty
            })
            .FirstOrDefaultAsync(p => p.Id == id);

        return patient;
    }

    public async Task<ProfilePatientVM> UpdatePatientById(int id, UpdateProfilePatientVM newPatient, IFormFile file)
    {
        var patient = _context.Users.Include(p => p.Patient).FirstOrDefault(p => p.UserId == id);

        if (patient != null)
        {
            patient.FullName = newPatient.FullName;
            patient.Email = newPatient.Email;
            patient.Role = newPatient.Role;
            patient.CreatedAt = DateTime.Now;

            // Xử lý file ảnh nếu có
            if (file != null && file.Length > 0)
            {
                // Thư mục lưu ảnh đại diện
                string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");

                // Đảm bảo thư mục tồn tại
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // Xóa file ảnh cũ nếu có
                if (!string.IsNullOrEmpty(patient.ImageUrl))
                {
                    // Lấy tên file từ đường dẫn, ví dụ "images/avatars/filename.jpg" -> "filename.jpg"
                    string oldFileName = Path.GetFileName(patient.ImageUrl);
                    string oldFilePath = Path.Combine(uploadDirectory, oldFileName);

                    if (File.Exists(oldFilePath))
                    {
                        try
                        {
                            File.Delete(oldFilePath);
                        }
                        catch (Exception ex)
                        {
                            // Log lỗi nếu không xóa được file cũ
                            Console.WriteLine($"Error deleting old avatar file: {ex.Message}");
                        }
                    }
                }

                // Tạo tên file mới độc nhất để tránh trùng lặp
                string fileExtension = Path.GetExtension(file.FileName);
                string newFileName = $"{Guid.NewGuid()}{fileExtension}";
                string filePath = Path.Combine(uploadDirectory, newFileName);

                // Lưu file mới
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Cập nhật đường dẫn ảnh trong database
                patient.ImageUrl = $"/images/avatars/{newFileName}";
            }
            else
            {
                // Nếu không có file mới và newPatient.ImageUrl khác với patient.ImageUrl hiện tại
                // thì giữ nguyên ImageUrl cũ
                if (string.IsNullOrEmpty(newPatient.ImageUrl))
                {
                    // Không làm gì, giữ nguyên ImageUrl cũ
                }
                else
                {
                    patient.ImageUrl = newPatient.ImageUrl;
                }
            }

            // Cập nhật thông tin patient
            if (patient.Patient != null)
            {
                patient.Patient.Dob = string.IsNullOrWhiteSpace(newPatient.DOB)
                    ? null
                    : DateOnly.FromDateTime(DateTime.Parse(newPatient.DOB));
                patient.Patient.Phone = newPatient.Phone;
                patient.Patient.Address = newPatient.Address;
            }

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            // Trả về thông tin patient đã cập nhật
            return new ProfilePatientVM
            {
                Id = patient.UserId,
                FullName = patient.FullName,
                Email = patient.Email,
                Role = patient.Role,
                CreatedAt = patient.CreatedAt ?? DateTime.MinValue,
                ImageUrl = patient.ImageUrl,
                DOB = patient.Patient?.Dob.HasValue == true ? patient.Patient.Dob.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                Phone = patient.Patient?.Phone ?? string.Empty,
                Address = patient.Patient?.Address ?? string.Empty
            };
        }

        // Trả về null nếu không tìm thấy patient
        return null;
    }
}