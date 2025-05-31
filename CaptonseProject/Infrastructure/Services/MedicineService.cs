public interface IMedicineService
{
    public Task<dynamic> GetAllMedicineForDoctorByIdAsync(List<int> listID);
    public Task<dynamic> GetAllMedicineForSearchDoctor(PagedResponse<string> pageSearch);
}

public class MedicineService : IMedicineService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtAuthService _jwtAuthService;

    public MedicineService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _jwtAuthService = jwtAuthService;
    }

    // Implement methods for admin functionalities here
    public async Task<dynamic> GetAllMedicineForDoctorByIdAsync(List<int> listID)
    {
        var result = new HTTPResponseClient<List<MedicineForDiagnosisDoctorVM>>();
        result.Data = new List<MedicineForDiagnosisDoctorVM>();
        try
        {
            var list = await _unitOfWork._medicineRepository.WhereAsync(p => listID.Contains(p.MedicineId));
            var data = list.Select(p => new MedicineForDiagnosisDoctorVM()
            {
                MedicineId = p.MedicineId,
                MedicineName = p.MedicineName,
                Unit = p.Unit
            }).ToList();

            result.Data = data;
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

    public async Task<dynamic> GetAllMedicineForSearchDoctor(PagedResponse<string> pageSearch)
    {
        var result = new HTTPResponseClient<PagedResponse<List<MedicineForDiagnosisDoctorVM>>>();
        result.Data = new PagedResponse<List<MedicineForDiagnosisDoctorVM>>();
        result.Data.PageSize = pageSearch.PageSize;
        result.Data.PageNumber = pageSearch.PageNumber;
        result.Data.Data = new List<MedicineForDiagnosisDoctorVM>();
        try
        {
            var list = await _unitOfWork._medicineRepository.GetAllAsync();
            list = list.Where(p => string.IsNullOrWhiteSpace(pageSearch.Data) || StringHelper.IsMatchSearchKey(pageSearch.Data!, p.MedicineName));
            var data = list.Select(p => new MedicineForDiagnosisDoctorVM()
            {
                MedicineId = p.MedicineId,
                MedicineName = p.MedicineName,
                Unit = p.Unit
            }).ToList();

            result.Data.TotalRecords = data.Count;
            result.Data.TotalPages = (int)Math.Ceiling((double)data.Count / result.Data.PageSize);
            result.Data.Data = data
            // lấy theo page
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
}