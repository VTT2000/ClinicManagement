public interface IDoctorService
{
    public Task<dynamic> GetByNameForReceptionistAsync(string searchKey);
    public Task<dynamic> GetAllDoctorForSelectedDoctorAsync(PagedResponse<ReceptionistConditionFIlterForSelectedDoctor> pagedResponse);
    public Task<dynamic> GetDoctorByIdAsync(int id);
    public Task<dynamic> GetDoctorSelectedByIdAsync(int id);
}

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
    }

    // Implement methods for admin functionalities here
    public async Task<dynamic> GetDoctorSelectedByIdAsync(int id)
    {
        var result = new HTTPResponseClient<ReceptionistSelectedDoctorVM>()
        {
            Data = new ReceptionistSelectedDoctorVM()
        };
        try
        {
            var item = await _unitOfWork._doctorRepository.GetDoctorUserAsync(id);

            result.Data = new ReceptionistSelectedDoctorVM()
            {
                DoctorId = item!.DoctorId,
                FullName = item.User!.FullName,
                Specialization = item.Specialization
            };
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
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
    public async Task<dynamic> GetDoctorByIdAsync(int id)
    {
        var result = new HTTPResponseClient<DoctorSearchedForCreateAppointmentVM>();
        try
        {
            var itemfinded = await _unitOfWork._doctorRepository.GetDoctorUserAsync(id);
            if (itemfinded == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.Message = StatusCodes.Status400BadRequest.ToString();
            }
            else
            {
                result.Data = new DoctorSearchedForCreateAppointmentVM()
                {
                    DoctorId = itemfinded.DoctorId,
                    FullName = itemfinded.User!.FullName,
                    Specialization = itemfinded.Specialization
                };
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

    public async Task<dynamic> GetAllDoctorForSelectedDoctorAsync(PagedResponse<ReceptionistConditionFIlterForSelectedDoctor> pagedResponse)
    {
        var result = new HTTPResponseClient<PagedResponse<List<ReceptionistSelectedDoctorVM>>>()
        {
            Data = new PagedResponse<List<ReceptionistSelectedDoctorVM>>()
            {
                Data = new List<ReceptionistSelectedDoctorVM>(),
                PageNumber = pagedResponse.PageNumber,
                PageSize = pagedResponse.PageSize
            }
        };
        try
        {
            var list = await _unitOfWork._doctorRepository.GetAllDoctorUserAsync();
            list = list.Where(p =>
                (string.IsNullOrWhiteSpace(pagedResponse.Data!.NameDoctor) || StringHelper.IsMatchSearchKey(pagedResponse.Data.NameDoctor, p.User!.FullName))
                &&
                (string.IsNullOrWhiteSpace(pagedResponse.Data!.NameSpecialization) || StringHelper.IsMatchSearchKey(pagedResponse.Data.NameSpecialization, p.Specialization?? ""))
            ).ToList();

            var data = list.Select(p=> new ReceptionistSelectedDoctorVM()
            {
                DoctorId = p.DoctorId,
                FullName = p.User!.FullName,
                Specialization = p.Specialization
            }).ToList();

            result.Data.TotalRecords = data.Count;
            result.Data.TotalPages = (int)Math.Ceiling((double)data.Count / result.Data.PageSize);
            result.Data.Data = data
            .Skip(result.Data.PageSize * (result.Data.PageNumber - 1))
            .Take(result.Data.PageSize).ToList();

            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
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

    public async Task<dynamic> GetByNameForReceptionistAsync(string searchKey)
    {
        var result = new HTTPResponseClient<List<DoctorSearchedForCreateAppointmentVM>>();
        try
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                result.Data = new List<DoctorSearchedForCreateAppointmentVM>();
            }
            else
            {
                // Chuẩn hóa keyword: bỏ dấu, lowercase, tách từ
                var keywords = StringHelper.RemoveDiacritics(searchKey).ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var patientList = await _unitOfWork._doctorRepository.GetAllDoctorUserAsync();
                var data = patientList
                .Where(p =>
                {
                    var normalizedName = StringHelper.RemoveDiacritics(p.User!.FullName).ToLower();

                    // Đảm bảo tất cả từ khóa đều xuất hiện trong tên
                    return keywords.All(word => normalizedName.Contains(word));
                })
                .Select(x => new DoctorSearchedForCreateAppointmentVM()
                {
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