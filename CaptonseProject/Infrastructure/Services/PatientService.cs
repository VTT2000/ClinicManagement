using web_api_base.Models.ViewModel.Receptionist;

public interface IPatientService
{
    public Task<dynamic> GetByNameForReceptionistAsync(string searchKey);
}

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
    }
    // Implement methods for admin functionalities here

    public async Task<dynamic> GetByNameForReceptionistAsync(string searchKey){
        var result = new HTTPResponseClient<List<PatientSearchedForCreateAppointmentVM>>();
        try
        {
            if(string.IsNullOrWhiteSpace(searchKey)){
                result.Data = new List<PatientSearchedForCreateAppointmentVM>();
            }
            else{
                // Chuẩn hóa keyword: bỏ dấu, lowercase, tách từ
                var keywords = StringHelper.RemoveDiacritics(searchKey).ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var patientList = await _unitOfWork._patientRepository.GetAllPatientUserAsync();
                var data = patientList
                .Where(p=>{
                    var normalizedName = StringHelper.RemoveDiacritics(p.User!.FullName).ToLower();

                    // Đảm bảo tất cả từ khóa đều xuất hiện trong tên
                    return keywords.All(word => normalizedName.Contains(word));
                })
                .Select(x=> new PatientSearchedForCreateAppointmentVM(){
                    PatientId = x.PatientId,
                    FullName = x.User!.FullName,
                    Dob = x.Dob,
                    Phone = x.Phone,
                    Email = x.User!.Email,
                    Address = x.Address,
                    PasswordHash = x.User.PasswordHash
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