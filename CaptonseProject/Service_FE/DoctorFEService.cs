using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Blazored.LocalStorage;

public class DoctorFEService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public string currentTab = string.Empty;
    public bool? isLoaded;
    public ConditionFilterPatientForAppointmentDoctor conditionFilter = new ConditionFilterPatientForAppointmentDoctor();
    public ConditionFilterPatientForAppointmentDoctor conditionFilter2 = new ConditionFilterPatientForAppointmentDoctor();
    public ConditionFilterPatientForAppointmentDoctor conditionFilter3 = new ConditionFilterPatientForAppointmentDoctor();
    public ConditionFilterPatientForAppointmentDoctor conditionFilter4 = new ConditionFilterPatientForAppointmentDoctor();
    public HTTPResponseClient<PagedResponse<List<AppointmentPatientForDoctorVM>>> list = new HTTPResponseClient<PagedResponse<List<AppointmentPatientForDoctorVM>>>();


    public bool isLoaded5 = false;
    public string ErrorMessage5 = string.Empty;
    public List<DiagnosisDoctorVM> listDiagnosisDoctorVM = new List<DiagnosisDoctorVM>();

    public DoctorFEService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;

        currentTab = StatusConstant.AppointmentStatus.Waiting;
        conditionFilter.dateAppointment = DateOnly.FromDateTime(DateTime.Now);
        conditionFilter.status = StatusConstant.AppointmentStatus.Waiting;
        conditionFilter2.dateAppointment = DateOnly.FromDateTime(DateTime.Now);
        conditionFilter2.status = StatusConstant.AppointmentStatus.Turned;
        conditionFilter3.dateAppointment = DateOnly.FromDateTime(DateTime.Now);
        conditionFilter3.status = StatusConstant.AppointmentStatus.Processing;
        conditionFilter4.dateAppointment = DateOnly.FromDateTime(DateTime.Now);
        conditionFilter4.status = StatusConstant.AppointmentStatus.Diagnosed;

        list.Data = new PagedResponse<List<AppointmentPatientForDoctorVM>>()
        {
            Data = new List<AppointmentPatientForDoctorVM>(),
            PageNumber = 1,
            PageSize = 10
        };
    }

    public async Task<HTTPResponseClient<bool>> IsChangeStatusAppointmentToDiagnosedAsync(int appointmentID)
    {
        string query = $"api/Appointment/IsChangeStatusAppointmentToDiagnosedAsync";
        HTTPResponseClient<bool> kq = new HTTPResponseClient<bool>();
        kq.Data = false;
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, appointmentID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<bool>>();

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

    public async Task<HTTPResponseClient<string>> GetStatusAppointmentForDoctorAsync(int appointmentID)
    {
        string query = $"api/Appointment/GetStatusAppointmentForDoctorAsync";
        HTTPResponseClient<string> kq = new HTTPResponseClient<string>();
        kq.Data = string.Empty;
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, appointmentID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<string>>();

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

    public async Task<dynamic> GetAllMedicineForSearchDoctor(PagedResponse<string> pageSearch)
    {
        string query = $"api/Medicine/GetAllMedicineForSearchDoctor";
        HTTPResponseClient<PagedResponse<List<MedicineForDiagnosisDoctorVM>>> kq = new HTTPResponseClient<PagedResponse<List<MedicineForDiagnosisDoctorVM>>>();
        kq.Data = new PagedResponse<List<MedicineForDiagnosisDoctorVM>>();
        kq.Data.PageSize = pageSearch.PageSize;
        kq.Data.PageNumber = pageSearch.PageNumber;
        kq.Data.Data = new List<MedicineForDiagnosisDoctorVM>();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, pageSearch);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<MedicineForDiagnosisDoctorVM>>>>();

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

    public async Task<dynamic> GetAllMedicineForDoctorByIdAsync(List<int> list)
    {
        string query = $"api/Medicine/GetAllMedicineForDoctorByIdAsync";
        HTTPResponseClient<List<MedicineForDiagnosisDoctorVM>> kq = new HTTPResponseClient<List<MedicineForDiagnosisDoctorVM>>();
        kq.Data = new List<MedicineForDiagnosisDoctorVM>();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, list);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<MedicineForDiagnosisDoctorVM>>>();

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

    public async Task<HTTPResponseClient<DetailSaveDiagnosisDoctorVM>> GetDiagnosisDoctorByIDAsync(int diagnosisID)
    {
        string query = $"api/Diagnosis/GetDiagnosisDoctorByIDAsync";
        HTTPResponseClient<DetailSaveDiagnosisDoctorVM> kq = new HTTPResponseClient<DetailSaveDiagnosisDoctorVM>();
        kq.Data = new DetailSaveDiagnosisDoctorVM();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, diagnosisID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<DetailSaveDiagnosisDoctorVM>>();

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

    public async Task<dynamic> GetAllServiceClinicalAsync(PagedResponse<string> pagedResponseSearchText)
    {
        string query = $"api/Service/GetAllServiceClinicalAsync";
        var kq = new HTTPResponseClient<PagedResponse<List<SearchServiceClinicalSelectedVM>>>();
        kq.Data = new PagedResponse<List<SearchServiceClinicalSelectedVM>>();
        kq.Data.PageSize = pagedResponseSearchText.PageSize;
        kq.Data.PageNumber = pagedResponseSearchText.PageNumber;
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, pagedResponseSearchText);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<SearchServiceClinicalSelectedVM>>>>();

                if (result == null)
                {
                    // ErrorMessage6 = "Lỗi dữ liệu!";
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
                // ErrorMessage6 = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage6 = "Thất bại!";
            kq.Message = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return kq;
        // isLoaded6 = true;
        // NotifyStateChanged();
    }

    public async Task<HTTPResponseClient<PagedResponse<List<SearchServiceParaclinicalSelectedVM>>>> GetAllServiceParaclinicalAsync(PagedResponse<ConditionFilterParaclinicalServiceSelected> condition)
    {
        string query = $"api/Service/GetAllServiceParaclinicalAsync";
        var kq = new HTTPResponseClient<PagedResponse<List<SearchServiceParaclinicalSelectedVM>>>();
        kq.Data = new PagedResponse<List<SearchServiceParaclinicalSelectedVM>>();
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
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<SearchServiceParaclinicalSelectedVM>>>>();

                if (result == null)
                {
                    // ErrorMessage6 = "Lỗi dữ liệu!";
                    kq.Message = "Lỗi dữ liệu!";
                }
                else
                {
                    kq = result;
                    //ErrorMessage6 = result.Message;

                }
            }
            else
            {
                kq.Message = response.StatusCode.ToString();
                // ErrorMessage6 = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage6 = "Thất bại!";
            kq.Message = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return kq;
        // isLoaded6 = true;
        // NotifyStateChanged();
    }

    public async Task<HTTPResponseClient<bool>> DeleteDiagnosisDoctorAsync(int diagnosisID)
    {
        string query = $"api/Diagnosis/DeleteDiagnosisDoctorAsync/{diagnosisID}";
        HTTPResponseClient<bool> kq = new HTTPResponseClient<bool>();
        kq.Data = false;
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<bool>>();

                if (result == null)
                {
                    kq.Message = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage = result.Message;
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

    public async Task<HTTPResponseClient<bool>> SaveDiagnosisDoctorAsync(DetailSaveDiagnosisDoctorVM detailSaveDiagnosisDoctorVM)
    {
        string query = $"api/Diagnosis/SaveDiagnosisDoctorAsync";
        HTTPResponseClient<bool> kq = new HTTPResponseClient<bool>();
        kq.Data = false;
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync(query, detailSaveDiagnosisDoctorVM);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<bool>>();

                if (result == null)
                {
                    kq.Message = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage = result.Message;
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
    
    public async Task<HTTPResponseClient<List<ParaClinicalServiceInfoForDoctorVM>>> GetAllServiceVMByIDAsync(ConditionParaClinicalServiceInfo condition)
    {
        string query = $"api/Service/GetAllServiceVMByIDAsync";
        HTTPResponseClient<List<ParaClinicalServiceInfoForDoctorVM>> kq = new HTTPResponseClient<List<ParaClinicalServiceInfoForDoctorVM>>();
        kq.Data = new List<ParaClinicalServiceInfoForDoctorVM>();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, condition);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<ParaClinicalServiceInfoForDoctorVM>>>();

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

    public async Task<HTTPResponseClient<ServiceVM>> GetServiceVMByIDAsync(int serviceID)
    {
        string query = $"api/Service/GetServiceVMByIDAsync";
        HTTPResponseClient<ServiceVM> kq = new HTTPResponseClient<ServiceVM>();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, serviceID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<ServiceVM>>();

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

    public async Task<HTTPResponseClient<List<ParaClinicalServiceInfoForDoctorVM>>> GetAllServiceVMByIDAsync(List<int> listServiceID)
    {
        string query = $"api/Service/GetAllServiceVMByIDAsync";
        HTTPResponseClient<List<ParaClinicalServiceInfoForDoctorVM>> kq = new HTTPResponseClient<List<ParaClinicalServiceInfoForDoctorVM>>();
        kq.Data = new List<ParaClinicalServiceInfoForDoctorVM>();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, listServiceID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<ParaClinicalServiceInfoForDoctorVM>>>();

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

    public async Task GetAllDiagnosisByAppointmentIDAsync(int appointmentID)
    {
        string query = $"api/Diagnosis/GetAllDiagnosisByAppointmentIDAsync";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, appointmentID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<DiagnosisDoctorVM>>>();

                if (result == null)
                {
                    ErrorMessage5 = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage5 = result.Message;
                    listDiagnosisDoctorVM = result.Data ?? new List<DiagnosisDoctorVM>();
                }
            }
            else
            {
                ErrorMessage5 = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage5 = "Thất bại!";
            Console.WriteLine(ex.Message);
        }

        isLoaded5 = true;
        NotifyStateChanged();
    }

    public async Task<dynamic> UpdateStatusAppointmentForDoctor(int appointmentId, string status)
    {
        string query = $"api/Appointment/UpdateStatusAppointmentForDoctor/{appointmentId}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.PutAsJsonAsync(query, status);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<bool>>();

                if (result == null)
                {
                    // ErrorMessage = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage = result.Message;
                    return result.Data;
                }
            }
            else
            {
                Console.WriteLine(response.StatusCode + "/" + response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return false;
    }


    public async Task GetAllListPatientForDocTorAsync2()
    {
        isLoaded = false;
        string query = $"api/Appointment/GetAllListPatientForDocTorAsync2";
        PagedResponse<ConditionFilterPatientForAppointmentDoctor> condition = new PagedResponse<ConditionFilterPatientForAppointmentDoctor>()
        {
            Data = new ConditionFilterPatientForAppointmentDoctor(),
            PageNumber = list.Data.PageNumber,
            PageSize = list.Data.PageSize
        };
        if (currentTab.Equals(StatusConstant.AppointmentStatus.Waiting))
        {
            condition.Data = conditionFilter;
        }
        if (currentTab.Equals(StatusConstant.AppointmentStatus.Turned))
        {
            condition.Data = conditionFilter2;
        }
        if (currentTab.Equals(StatusConstant.AppointmentStatus.Processing))
        {
            condition.Data = conditionFilter3;
        }
        if (currentTab.Equals(StatusConstant.AppointmentStatus.Diagnosed))
        {
            condition.Data = conditionFilter4;
        }

        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, condition);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<AppointmentPatientForDoctorVM>>>>();

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
        isLoaded = true;
        NotifyStateChanged();
    }
}