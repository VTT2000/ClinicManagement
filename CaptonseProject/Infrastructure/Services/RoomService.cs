public interface IRoomService
{
    public Task<dynamic> GetAllRoomVMAsync(PagedResponse<string> condition);
    public Task<dynamic> GetRoomVMByIDAsync(int roomID);
}

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtAuthService _jwtAuthService;

    public RoomService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _jwtAuthService = jwtAuthService;
    }

    // Implement methods for admin functionalities here
    public async Task<dynamic> GetAllRoomVMAsync(PagedResponse<string> condition)
    {
        var result = new HTTPResponseClient<PagedResponse<List<RoomVM>>>();
        result.Data = new PagedResponse<List<RoomVM>>();
        result.Data.PageNumber = condition.PageNumber;
        result.Data.PageSize = condition.PageSize;
        result.Data.Data = new List<RoomVM>();
        try
        {
            var list = await _unitOfWork._roomRepository.GetAllAsync();
            list = list.Where(p => string.IsNullOrWhiteSpace(condition.Data) || StringHelper.IsMatchSearchKey(condition.Data, p.RoomName));
            var data = list.Select(x => new RoomVM()
            {
                RoomId = x.RoomId,
                RoomName = x.RoomName
            }).ToList();

            result.Data.TotalRecords = data.Count;
            result.Data.TotalPages = (int)Math.Ceiling((double)data.Count / result.Data.PageSize);
            result.Data.Data = data
            // lấy theo page
            .Skip(condition.PageSize * (condition.PageNumber - 1))
            .Take(condition.PageSize).ToList();
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

    public async Task<dynamic> GetRoomVMByIDAsync(int roomID)
    {
        var result = new HTTPResponseClient<RoomVM>();
        result.Data = new RoomVM();
        try
        {
            var temp = await _unitOfWork._roomRepository.SingleOrDefaultAsync(p => p.RoomId == roomID);
            var data = temp == null ? new RoomVM() : new RoomVM()
            {
                RoomId = temp.RoomId,
                RoomName = temp.RoomName
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
}