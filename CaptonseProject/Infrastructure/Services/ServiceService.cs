using web_api_base.Models.ClinicManagement;

public interface IServiceService
{
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
                p.Type == TypeServiceConstant.Clinical
            );
            if (!condition.Data!.IsPackageService)
            {
                list = list.Where(p => p.ServiceParentId.HasValue && StringHelper.IsMatchSearchKey(condition.Data!.SearchText, p.ServiceName))
                .Skip(result.Data.PageSize * (result.Data.PageNumber - 1))
                .Take(result.Data.PageSize).ToList();
                result.Data.TotalRecords = list.Count;
                result.Data.TotalPages = (int)Math.Ceiling((double)list.Count / result.Data.PageSize);
            }
            else
            {
                list = list.Where(p => !p.ServiceParentId.HasValue && StringHelper.IsMatchSearchKey(condition.Data!.SearchText, p.ServiceName))
                .Skip(result.Data.PageSize * (result.Data.PageNumber - 1))
                .Take(result.Data.PageSize).ToList();
                result.Data.TotalRecords = list.Count;
                result.Data.TotalPages = (int)Math.Ceiling((double)list.Count / result.Data.PageSize);

                // Tạo một HashSet để tránh trùng
                var resultSet = new HashSet<Service>(list);

                foreach (var item in list)
                {
                    AddAllChildren(item, resultSet);
                }

                // Cập nhật lại list đã đầy đủ cả cha và con
                list = resultSet.ToList();
            }

            result.Data.Data = list.Select(p => new SearchServiceParaclinicalSelectedVM()
            {
                ServiceID = p.ServiceId,
                ServiceName = p.ServiceName,
                ServiceIdParent = p.ServiceParentId
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
    
    private void AddAllChildren(Service parent, HashSet<Service> resultSet)
    {
        if (parent.InverseServiceParent == null || !parent.InverseServiceParent.Any())
            return;

        foreach (var child in parent.InverseServiceParent)
        {
            if (resultSet.Add(child)) // Add thành công => chưa có
            {
                AddAllChildren(child, resultSet); // Đệ quy xuống con
            }
        }
    }
}