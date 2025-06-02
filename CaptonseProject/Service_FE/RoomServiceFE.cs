using System.Net.Http.Headers;
using Blazored.LocalStorage;

public class RoomServiceFE
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    // public event Action? OnChange;
    // private void NotifyStateChanged() => OnChange?.Invoke();

    public int? SelectedRoomID;

    public int? SelectedRoomIdTechnician;

    public RoomServiceFE(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
    }

    public async Task<dynamic> GetAllRoomVMAsync(PagedResponse<string> pagedResponseSearchText)
    {
        string query = $"api/Room/GetAllRoomVMAsync";
        var kq = new HTTPResponseClient<PagedResponse<List<RoomVM>>>();
        kq.Data = new PagedResponse<List<RoomVM>>();
        kq.Data.PageSize = pagedResponseSearchText.PageSize;
        kq.Data.PageNumber = pagedResponseSearchText.PageNumber;
        kq.Data.Data = new List<RoomVM>();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, pagedResponseSearchText);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<RoomVM>>>>();

                if (result == null)
                {
                    kq.Message = "Lỗi dữ liệu!";
                }
                else
                {
                    kq = result;
                }
            }
            else
            {
                kq.Message = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            kq.Message = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return kq;
    }

    public async Task<dynamic> GetRoomVMByIDAsync(int RoomID)
    {
        string query = $"api/Room/GetRoomVMByIDAsync";
        var kq = new HTTPResponseClient<RoomVM>();
        kq.Data = new RoomVM();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, RoomID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<RoomVM>>();

                if (result == null)
                {
                    kq.Message = "Lỗi dữ liệu!";
                }
                else
                {
                    kq = result;
                }
            }
            else
            {
                kq.Message = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            kq.Message = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return kq;
    }
}