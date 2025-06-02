using System.Net.Http.Headers;
using Blazored.LocalStorage;

public class TechnicianService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public bool? IsLoaded;
    public TechnicianConditionFilterParaclinical condition = new TechnicianConditionFilterParaclinical();
    public HTTPResponseClient<PagedResponse<List<TechnicianParaclinical>>> list = new HTTPResponseClient<PagedResponse<List<TechnicianParaclinical>>>();

    public int? SelectedServiceIdTechnician;

    public TechnicianService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        condition.SearchNamePatient = string.Empty;
        condition.SearchDate = DateOnly.FromDateTime(DateTime.Now);
        condition.IsTested = null;
        condition.ServiceID = null;

        list.Data = new PagedResponse<List<TechnicianParaclinical>>()
        {
            Data = new List<TechnicianParaclinical>(),
            PageSize = 10,
            PageNumber = 1
        };
    }

    public async Task<HTTPResponseClient<TechnicianServiceVM>> GetServiceVMByIDAsync2(int serviceID)
    {
        string query = $"api/Service/GetServiceVMByIDAsync2";
        HTTPResponseClient<TechnicianServiceVM> kq = new HTTPResponseClient<TechnicianServiceVM>();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, serviceID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<TechnicianServiceVM>>();

                if (result == null)
                {
                    // ErrorMessage6 = "Lỗi dữ liệu!";
                    kq.Message = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage6 = result.Message;
                    kq = result;
                }
            }
            else
            {
                // ErrorMessage6 = response.StatusCode.ToString();
                kq.Message = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage6 = "Thất bại!";
            kq.Message = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return kq;
    }

    public async Task GetAllDiagnosisServiceForTechcian()
    {
        IsLoaded = false;
        string query = $"api/DiagnosisService/GetAllDiagnosisServiceForTechcian";
        PagedResponse<TechnicianConditionFilterParaclinical> conditionFilter = new PagedResponse<TechnicianConditionFilterParaclinical>();
        conditionFilter.PageNumber = list.Data!.PageNumber;
        conditionFilter.PageSize = list.Data.PageSize;
        conditionFilter.Data = condition;
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, conditionFilter);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<TechnicianParaclinical>>>>();

                if (result == null)
                {
                    list.Message = "Lỗi dữ liệu!";
                }
                else
                {
                    list = result;
                }
            }
            else
            {
                list.Message = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            list.Message = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        IsLoaded = true;
        NotifyStateChanged();
    }

    public List<int> GetPageNumbersToDisplay()
    {
        const int range = 2; // Số trang hiển thị trước và sau trang hiện tại
        var pages = new List<int>();
        if (list.Data!.TotalPages <= 7)
        {
            for (int i = 1; i <= list.Data!.TotalPages; i++) pages.Add(i);
        }
        else
        {
            pages.Add(1);
            if (list.Data!.PageNumber > range + 2) pages.Add(-1); // dấu "..."
            for (int i = Math.Max(2, list.Data!.PageNumber - range); i <= Math.Min(list.Data!.TotalPages - 1, list.Data!.PageNumber + range); i++)
            {
                pages.Add(i);
            }
            if (list.Data!.PageNumber < list.Data!.TotalPages - range - 1) pages.Add(-1); // dấu "..."
            pages.Add(list.Data!.TotalPages);
        }
        return pages;
    }

    public async Task SelectPage(int page)
    {
        if (page >= 1 && page <= list.Data!.TotalPages && page != list.Data!.PageNumber)
        {
            list.Data!.PageNumber = page;
            await GetAllDiagnosisServiceForTechcian();
        }
    }
    
    public async Task<dynamic> GetAllServiceParaclinicalAsync2(PagedResponse<string> condition)
    {
        string query = $"api/Service/GetAllServiceParaclinicalAsync2";
        var kq = new HTTPResponseClient<PagedResponse<List<TechnicianServiceVM>>>();
        kq.Data = new PagedResponse<List<TechnicianServiceVM>>();
        kq.Data.PageSize = condition.PageSize;
        kq.Data.PageNumber = condition.PageNumber;
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, condition);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<TechnicianServiceVM>>>>();

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