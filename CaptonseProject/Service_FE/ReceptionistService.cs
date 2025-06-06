using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.OpenApi.MicrosoftExtensions;
using Microsoft.VisualBasic;
using StackExchange.Redis;
using web_api_base.Models.ViewModel.Receptionist;

public class ReceptionistService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public ConditionFilterPatientForAppointmentReceptionist condition = new ConditionFilterPatientForAppointmentReceptionist();
    public HTTPResponseClient<PagedResponse<List<AppointmentPatientVM>>> list = new HTTPResponseClient<PagedResponse<List<AppointmentPatientVM>>>();

    public ReceptionistConditionFilterWorkScheduleDoctor condition2 = new ReceptionistConditionFilterWorkScheduleDoctor();
    public HTTPResponseClient<PagedResponse<List<WorkScheduleDoctorVM>>> list2 = new HTTPResponseClient<PagedResponse<List<WorkScheduleDoctorVM>>>();

    public ReceptionistService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        list.Data = new PagedResponse<List<AppointmentPatientVM>>()
        {
            Data = new List<AppointmentPatientVM>(),
            PageNumber = 1,
            PageSize = 10
        };
        condition.dateAppointment = DateOnly.FromDateTime(DateTime.Now);

        list2.Data = new PagedResponse<List<WorkScheduleDoctorVM>>()
        {
            Data = new List<WorkScheduleDoctorVM>(),
            PageNumber = 1,
            PageSize = 10
        };
    }

    public async Task<HTTPResponseClient<ReceptionistPatientInfoVM>> GetPatientForReceptionistAsync2(int patientID)
    {
        string query = $"api/Patient/GetPatientForReceptionistAsync2";
        HTTPResponseClient<ReceptionistPatientInfoVM> kq = new HTTPResponseClient<ReceptionistPatientInfoVM>
        {
            Data = new ReceptionistPatientInfoVM()
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, patientID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<ReceptionistPatientInfoVM>>();

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

    public async Task<HTTPResponseClient<PagedResponse<List<ReceptionistSelectedPatientVM>>>> GetAllPatientForReceptionistAsync(PagedResponse<ReceptionistConditionFilterForSelectedPatient> pagedResponse)
    {
        string query = $"api/Patient/GetAllPatientForReceptionistAsync";
        HTTPResponseClient<PagedResponse<List<ReceptionistSelectedPatientVM>>> kq = new HTTPResponseClient<PagedResponse<List<ReceptionistSelectedPatientVM>>>()
        {
            Data = new PagedResponse<List<ReceptionistSelectedPatientVM>>()
            {
                Data = new List<ReceptionistSelectedPatientVM>(),
                PageNumber = pagedResponse.PageNumber,
                PageSize = pagedResponse.PageSize
            }
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, pagedResponse);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<ReceptionistSelectedPatientVM>>>>();

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

    public async Task GetAllAppointmentPatientAsync2()
    {
        string query = $"api/Appointment/GetAllAppointmentPatientAsync";
        PagedResponse<ConditionFilterPatientForAppointmentReceptionist> conditionFilter = new PagedResponse<ConditionFilterPatientForAppointmentReceptionist>()
        {
            Data = condition,
            PageNumber = list.Data!.PageNumber,
            PageSize = list.Data.PageSize
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, conditionFilter);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<AppointmentPatientVM>>>>();

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
        NotifyStateChanged();
    }

    public async Task<dynamic> ChangeStatusWaitingForPatient(int apppointmentId)
    {
        string query = $"api/Appointment/ChangeStatusWaitingForPatient";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.PostAsJsonAsync(query, apppointmentId);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<bool>>();

                if (result == null)
                {
                    // ErrorMessage = "Lỗi dữ liệu!";
                    Console.WriteLine("Lỗi dữ liệu!");
                }
                else
                {
                    Console.WriteLine(result.Message);
                    //ErrorMessage = result.Message;
                    return result.Data;
                }
            }
            else
            {
                Console.WriteLine(response.StatusCode.ToString());
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return false;
    }

    public async Task<dynamic> GetAllFreeTimeAppointmentForDoctor(DateOnly date, int doctorId)
    {
        string query = $"api/Appointment/GetAllFreeTimeAppointmentForDoctor/{date}/{doctorId}";
        List<TimeOnly> kq = new List<TimeOnly>();
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<TimeOnly>>>();
                if (result == null)
                {
                    // ErrorMessage = "Lỗi dữ liệu!";
                    Console.WriteLine("Lỗi dữ liệu!");
                }
                else
                {
                    //ErrorMessage = result.Message;
                    Console.WriteLine(result.Message);
                    kq = result.Data ?? new List<TimeOnly>();
                }
            }
            else
            {
                Console.WriteLine(response.StatusCode.ToString());
                // ErrorMessage = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return kq;
    }

    public async Task<dynamic> GetDoctorSelectedByIdAsync(int id)
    {
        string query = $"api/Doctor/GetDoctorSelectedByIdAsync/{id}";
        HTTPResponseClient<ReceptionistSelectedDoctorVM> kq = new HTTPResponseClient<ReceptionistSelectedDoctorVM>()
        {
            Data = new ReceptionistSelectedDoctorVM()
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<ReceptionistSelectedDoctorVM>>();
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

    public async Task<dynamic> GetDoctorByIdAsync(int id)
    {
        if (id < 1)
        {
            return new DoctorSearchedForCreateAppointmentVM();
        }
        string query = $"api/Doctor/GetDoctorByIdAsync/{id}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<DoctorSearchedForCreateAppointmentVM>>();
                if (result == null)
                {
                    // ErrorMessage = "Lỗi dữ liệu!";
                    Console.WriteLine("Lỗi dữ liệu!");
                }
                else
                {
                    //ErrorMessage = result.Message;
                    Console.WriteLine(result.Message);
                    return result.Data ?? new DoctorSearchedForCreateAppointmentVM();
                }
            }
            else
            {
                Console.WriteLine(response.StatusCode.ToString());
                // ErrorMessage = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return new DoctorSearchedForCreateAppointmentVM();
    }

    public async Task<HTTPResponseClient<bool>> DeleteWorkScheduleDoctorAsync(int id)
    {
        string query = $"api/WorkSchedule/DeleteWorkScheduleDortorAsync/{id}";
        HTTPResponseClient<bool> kq = new HTTPResponseClient<bool>()
        {
            Data = false
        };
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

    public async Task<HTTPResponseClient<bool>> SaveWorkScheduleDoctorAsync(WorkScheduleDoctorDetailVM item)
    {
        string query = $"api/WorkSchedule/SaveWorkScheduleDoctorAsync";
        HTTPResponseClient<bool> kq = new HTTPResponseClient<bool>()
        {
            Data = false
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync(query, item);

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

    public async Task<dynamic> GetWorkScheduleDoctorAsync(int id)
    {
        if (id < 1)
        {
            return new WorkScheduleDoctorDetailVM();
        }
        string query = $"api/WorkSchedule/GetWorkScheduleDortorAsync/{id}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<WorkScheduleDoctorDetailVM>>();
                if (result == null)
                {
                    // ErrorMessage = "Lỗi dữ liệu!";
                    Console.WriteLine("Lỗi dữ liệu!");
                }
                else
                {
                    //ErrorMessage = result.Message;
                    Console.WriteLine(result.Message);
                    return result.Data ?? new WorkScheduleDoctorDetailVM();
                }
            }
            else
            {
                Console.WriteLine(response.StatusCode.ToString());
                // ErrorMessage = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return new WorkScheduleDoctorDetailVM();
    }

    public async Task GetAllWorkScheduleDoctorAsync2()
    {
        string query = $"api/WorkSchedule/GetAllWorkScheduleDortorAsync2";
        PagedResponse<ReceptionistConditionFilterWorkScheduleDoctor> conditionFilter = new PagedResponse<ReceptionistConditionFilterWorkScheduleDoctor>()
        {
            Data = condition2,
            PageNumber = list2.Data!.PageNumber,
            PageSize = list2.Data.PageSize
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, conditionFilter);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<WorkScheduleDoctorVM>>>>();
                if (result == null)
                {
                    list2.Message = "Lỗi dữ liệu!";
                }
                else
                {
                    list2 = result;
                }
            }
            else
            {
                list2.Message = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            list2.Message = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        NotifyStateChanged();
    }

    public async Task<dynamic> CreateAppointmentAsync(AppointmentReceptionistCreateVM appointmentReceptionistCreateVM)
    {
        string query = $"api/Appointment/CreateAppointmentFromReceptionist";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, appointmentReceptionistCreateVM);

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

    public async Task<dynamic> GetAllDoctorForSelectedDoctorAsync(PagedResponse<ReceptionistConditionFIlterForSelectedDoctor> pagedResponse)
    {
        string query = $"api/Doctor/GetAllDoctorForSelectedDoctorAsync";
        HTTPResponseClient<PagedResponse<List<ReceptionistSelectedDoctorVM>>> kq = new HTTPResponseClient<PagedResponse<List<ReceptionistSelectedDoctorVM>>>()
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
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, pagedResponse);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<ReceptionistSelectedDoctorVM>>>>();
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
