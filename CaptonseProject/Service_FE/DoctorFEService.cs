using System.Net.Http.Headers;
using Blazored.LocalStorage;

public class DoctorFEService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public bool isLoaded = false;
    public string ErrorMessage = string.Empty;
    public bool isLoaded2 = false;
    public string ErrorMessage2 = string.Empty;
    public bool isLoaded3 = false;
    public string ErrorMessage3 = string.Empty;
    public bool isLoaded4 = false;
    public string ErrorMessage4 = string.Empty;
    public PagedResponse<List<AppointmentPatientForDoctorVM>> listAppointmentPatientForDoctorWaiting = new PagedResponse<List<AppointmentPatientForDoctorVM>>()
    {
        Data = new List<AppointmentPatientForDoctorVM>(),
        PageSize = 10,
        PageNumber = 1
    };
    public PagedResponse<List<AppointmentPatientForDoctorVM>> listAppointmentPatientForDoctorTurned = new PagedResponse<List<AppointmentPatientForDoctorVM>>()
    {
        Data = new List<AppointmentPatientForDoctorVM>(),
        PageSize = 10,
        PageNumber = 1
    };
    public PagedResponse<List<AppointmentPatientForDoctorVM>> listAppointmentPatientForDoctorProcessing = new PagedResponse<List<AppointmentPatientForDoctorVM>>()
    {
        Data = new List<AppointmentPatientForDoctorVM>(),
        PageSize = 10,
        PageNumber = 1
    };
    public PagedResponse<List<AppointmentPatientForDoctorVM>> listAppointmentPatientForDoctorDiagnosed  = new PagedResponse<List<AppointmentPatientForDoctorVM>>()
    {
        Data = new List<AppointmentPatientForDoctorVM>(),
        PageSize = 10,
        PageNumber = 1
    };

    public bool isLoaded5 = false;
    public string ErrorMessage5 = string.Empty;
    public List<DiagnosisDoctorVM> listDiagnosisDoctorVM = new List<DiagnosisDoctorVM>();

    public DoctorFEService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
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

    
    public async Task GetAllListPatientForDocTorAsync2(ConditionFilterPatientForAppointmentDoctor condition)
    {
        string query = $"api/Appointment/GetAllListPatientForDocTorAsync2";

        PagedResponse<ConditionFilterPatientForAppointmentDoctor> conditionfilter = new PagedResponse<ConditionFilterPatientForAppointmentDoctor>();
        conditionfilter.Data = condition;
        if (condition.status.Equals(StatusConstant.AppointmentStatus.Waiting))
        {
            conditionfilter.PageSize = listAppointmentPatientForDoctorWaiting.PageSize;
            conditionfilter.PageNumber = listAppointmentPatientForDoctorWaiting.PageNumber;
        }
        if (condition.status.Equals(StatusConstant.AppointmentStatus.Turned))
        {
            conditionfilter.PageSize = listAppointmentPatientForDoctorTurned.PageSize;
            conditionfilter.PageNumber = listAppointmentPatientForDoctorTurned.PageNumber;
        }
        if (condition.status.Equals(StatusConstant.AppointmentStatus.Processing))
        {
            conditionfilter.PageSize = listAppointmentPatientForDoctorProcessing.PageSize;
            conditionfilter.PageNumber = listAppointmentPatientForDoctorProcessing.PageNumber;
        }
        if (condition.status.Equals(StatusConstant.AppointmentStatus.Diagnosed))
        {
            conditionfilter.PageSize = listAppointmentPatientForDoctorDiagnosed.PageSize;
            conditionfilter.PageNumber = listAppointmentPatientForDoctorDiagnosed.PageNumber;
        }

        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, conditionfilter);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<AppointmentPatientForDoctorVM>>>>();

                if (result == null)
                {
                    if (condition.status.Equals(StatusConstant.AppointmentStatus.Waiting))
                    {
                        ErrorMessage = "Lỗi dữ liệu!";
                    }
                    if (condition.status.Equals(StatusConstant.AppointmentStatus.Turned))
                    {
                        ErrorMessage2 = "Lỗi dữ liệu!";
                    }
                    if (condition.status.Equals(StatusConstant.AppointmentStatus.Processing))
                    {
                        ErrorMessage3 = "Lỗi dữ liệu!";
                    }
                    if (condition.status.Equals(StatusConstant.AppointmentStatus.Diagnosed))
                    {
                        ErrorMessage4 = "Lỗi dữ liệu!";
                    }
                }
                else
                {
                    //ErrorMessage = result.Message;
                    if (condition.status.Equals(StatusConstant.AppointmentStatus.Waiting))
                    {
                        listAppointmentPatientForDoctorWaiting = result.Data ?? new PagedResponse<List<AppointmentPatientForDoctorVM>>();
                    }
                    if (condition.status.Equals(StatusConstant.AppointmentStatus.Turned))
                    {
                        listAppointmentPatientForDoctorTurned = result.Data ?? new PagedResponse<List<AppointmentPatientForDoctorVM>>();
                    }
                    if (condition.status.Equals(StatusConstant.AppointmentStatus.Processing))
                    {
                        listAppointmentPatientForDoctorProcessing = result.Data ?? new PagedResponse<List<AppointmentPatientForDoctorVM>>();
                    }
                    if (condition.status.Equals(StatusConstant.AppointmentStatus.Diagnosed))
                    {
                        listAppointmentPatientForDoctorDiagnosed = result.Data ?? new PagedResponse<List<AppointmentPatientForDoctorVM>>();
                    }
                }
            }
            else
            {
                if (condition.status.Equals(StatusConstant.AppointmentStatus.Waiting))
                {
                    ErrorMessage = response.StatusCode.ToString();
                }
                if (condition.status.Equals(StatusConstant.AppointmentStatus.Turned))
                {
                    ErrorMessage2 = response.StatusCode.ToString();
                }
                if (condition.status.Equals(StatusConstant.AppointmentStatus.Processing))
                {
                    ErrorMessage3 = response.StatusCode.ToString();
                }
                if (condition.status.Equals(StatusConstant.AppointmentStatus.Diagnosed))
                {
                    ErrorMessage4 = response.StatusCode.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            if (condition.status.Equals(StatusConstant.AppointmentStatus.Waiting))
            {
                ErrorMessage = "Thất bại!";
                Console.WriteLine(ex.Message);
            }
            if (condition.status.Equals(StatusConstant.AppointmentStatus.Turned))
            {
                ErrorMessage2 = "Thất bại!";
                Console.WriteLine(ex.Message);
            }
            if (condition.status.Equals(StatusConstant.AppointmentStatus.Processing))
            {
                ErrorMessage3 = "Thất bại!";
                Console.WriteLine(ex.Message);
            }
            if (condition.status.Equals(StatusConstant.AppointmentStatus.Diagnosed))
            {
                ErrorMessage4 = "Thất bại!";
                Console.WriteLine(ex.Message);
            }
        }
        if (condition.status.Equals(StatusConstant.AppointmentStatus.Waiting))
        {
            isLoaded = true;
            NotifyStateChanged();
        }
        if (condition.status.Equals(StatusConstant.AppointmentStatus.Turned))
        {
            isLoaded2 = true;
            NotifyStateChanged();
        }
        if (condition.status.Equals(StatusConstant.AppointmentStatus.Processing))
        {
            isLoaded3 = true;
            NotifyStateChanged();
        }
        if (condition.status.Equals(StatusConstant.AppointmentStatus.Diagnosed))
        {
            isLoaded4 = true;
            NotifyStateChanged();
        }
        
    }
}