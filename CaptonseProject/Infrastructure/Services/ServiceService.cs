using web_api_base.Models.ClinicManagement;

public interface IServiceService
{
    public Task<dynamic> GetAllServiceVMByIDAsync(PagedResponse<ConditionParaClinicalServiceInfo> pageList);
    public Task<dynamic> GetServiceVMByIDAsync(int serviceID);
    public Task<dynamic> GetAllServiceClinicalAsync(PagedResponse<string> pagedResponseSearchText);
    public Task<dynamic> GetAllServiceParaclinicalAsync(PagedResponse<ConditionFilterParaclinicalServiceSelected> condition);
}

public class ServiceService : IServiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtAuthService _jwtAuthService;

    public ServiceService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _jwtAuthService = jwtAuthService;
    }

    // Implement methods for admin functionalities here
    public async Task<dynamic> GetAllServiceVMByIDAsync(PagedResponse<ConditionParaClinicalServiceInfo> pageList)
    {
        var result = new HTTPResponseClient<PagedResponse<List<ParaClinicalServiceInfoForDoctorVM>>>();
        result.Data = new PagedResponse<List<ParaClinicalServiceInfoForDoctorVM>>();
        result.Data.Data = new List<ParaClinicalServiceInfoForDoctorVM>();
        result.Data.PageSize = pageList.PageSize;
        result.Data.PageNumber = pageList.PageNumber;
        try
        {
            var listTemp = await _unitOfWork._serviceRepository.WhereAsync(p => pageList.Data!.listServiceParaclinical.Contains(p.ServiceId));
            var listTemp2 = await _unitOfWork._diagnosisServiceRepository.WhereAsync(p => p.DiagnosisId == pageList.Data!.DiagnosisID && pageList.Data.listServiceParaclinical.Contains(p.ServiceId));
            var data = pageList.Data!.listServiceParaclinical.Select(p =>
            {
                ParaClinicalServiceInfoForDoctorVM kq = new ParaClinicalServiceInfoForDoctorVM();
                kq.ServiceId = p;
                var temp = listTemp.SingleOrDefault(x=>x.ServiceId == p);
                kq.ServiceName = temp == null ? "" : temp.ServiceName;
                var temp2 = listTemp2.SingleOrDefault(x => x.ServiceId == p);
                if (temp2 != null)
                {
                    kq.CreatedAt = temp2.CreatedAt;
                    kq.ServiceResultReport = temp2.ServiceResultReport;
                    kq.FullNameUserperformed = temp2.UserIdperformedNavigation.FullName;
                    kq.RoomName = temp2.Room.RoomName;
                }
                return kq;
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

    public async Task<dynamic> GetServiceVMByIDAsync(int serviceID)
    {
        var result = new HTTPResponseClient<ServiceVM>();
        try
        {
            var temp = await _unitOfWork._serviceRepository.GetByIdAsync(serviceID);
            var data = new ServiceVM()
            {
                ServiceId = temp!.ServiceId,
                ServiceName = temp.ServiceName
            };
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
    public async Task<dynamic> GetAllServiceClinicalAsync(PagedResponse<string> pagedResponseSearchText)
    {
        var result = new HTTPResponseClient<PagedResponse<List<SearchServiceClinicalSelectedVM>>>();
        result.Data = new PagedResponse<List<SearchServiceClinicalSelectedVM>>();
        result.Data.Data = new List<SearchServiceClinicalSelectedVM>();
        result.Data.PageSize = pagedResponseSearchText.PageSize;
        result.Data.PageNumber = pagedResponseSearchText.PageNumber;
        try
        {
            var list = await _unitOfWork._serviceRepository.WhereAsync(p =>
                p.Type == TypeServiceConstant.Clinical
            );
            list = list.Where(p => string.IsNullOrWhiteSpace(pagedResponseSearchText.Data) || StringHelper.IsMatchSearchKey(pagedResponseSearchText.Data!, p.ServiceName));
            var data = list.Select(p => new SearchServiceClinicalSelectedVM()
            {
                ServiceID = p.ServiceId,
                ServiceName = p.ServiceName
            })
            .ToList();

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

    public async Task<dynamic> GetAllServiceParaclinicalAsync(PagedResponse<ConditionFilterParaclinicalServiceSelected> condition)
    {
        var result = new HTTPResponseClient<PagedResponse<List<SearchServiceParaclinicalSelectedVM>>>();
        result.Data = new PagedResponse<List<SearchServiceParaclinicalSelectedVM>>();
        result.Data.Data = new List<SearchServiceParaclinicalSelectedVM>();
        result.Data.PageSize = condition.PageSize;
        result.Data.PageNumber = condition.PageNumber;
        try
        {
            var list = await _unitOfWork._serviceRepository.GetAllService_Service(p =>
                p.Type == TypeServiceConstant.Paraclinical
            );
            var list2 = list.Where(p => p.ServiceParentId != null).Select(p => p.ServiceParentId!).Distinct().ToList();
            if (!condition.Data!.IsPackageService)
            {
                list = list.Where(p => !list2.Any(q=> q == p.ServiceId) && (StringHelper.IsMatchSearchKey(condition.Data!.SearchText, p.ServiceName) || string.IsNullOrWhiteSpace(condition.Data!.SearchText)))
                .Skip(result.Data.PageSize * (result.Data.PageNumber - 1))
                .Take(result.Data.PageSize).ToList();
                result.Data.TotalRecords = list.Count;
                result.Data.TotalPages = (int)Math.Ceiling((double)list.Count / result.Data.PageSize);
            }
            else
            {
                list = list.Where(p => list2.Any(q=> q == p.ServiceId) && (StringHelper.IsMatchSearchKey(condition.Data!.SearchText, p.ServiceName) || string.IsNullOrWhiteSpace(condition.Data!.SearchText)))
                .Skip(result.Data.PageSize * (result.Data.PageNumber - 1))
                .Take(result.Data.PageSize).ToList();
                result.Data.TotalRecords = list.Count;
                result.Data.TotalPages = (int)Math.Ceiling((double)list.Count / result.Data.PageSize);
            }

            result.Data.Data = list.Select(p => new SearchServiceParaclinicalSelectedVM()
            {
                ServiceID = p.ServiceId,
                ServiceName = p.ServiceName,
                ServiceChildren = p.InverseServiceParent.Select(q => new SearchServiceParaclinicalSelectedVM()
                {
                    ServiceID = q.ServiceId,
                    ServiceName = q.ServiceName
                }).ToList()
            })
            .ToList();

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