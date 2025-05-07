public interface IDoctorService
{
    public Task<dynamic> GetByNameForReceptionistAsync(string searchKey);
}

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
    }

    // Implement methods for admin functionalities here
    public async Task<dynamic> GetByNameForReceptionistAsync(string searchKey)
    {
        var result = new HTTPResponseClient<List<DoctorSearchedForCreateAppointmentVM>>();
        try
        {
            if(string.IsNullOrWhiteSpace(searchKey)){
                result.Data = new List<DoctorSearchedForCreateAppointmentVM>();
            }
            else{
                // Chuẩn hóa keyword: bỏ dấu, lowercase, tách từ
                var keywords = StringHelper.RemoveDiacritics(searchKey).ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var patientList = await _unitOfWork._doctorRepository.GetAllDoctorUserAsync();
                var data = patientList
                .Where(p=>{
                    var normalizedName = StringHelper.RemoveDiacritics(p.User!.FullName).ToLower();

                    // Đảm bảo tất cả từ khóa đều xuất hiện trong tên
                    return keywords.All(word => normalizedName.Contains(word));
                })
                .Select(x=> new DoctorSearchedForCreateAppointmentVM(){
                    DoctorId = x.DoctorId,
                    FullName = x.User!.FullName,
                    Specialization = x.Specialization
                }).ToList();
                result.Data = data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }
}