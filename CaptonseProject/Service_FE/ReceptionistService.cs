using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.OpenApi.MicrosoftExtensions;
using web_api_base.Models.ViewModel.Receptionist;

public class ReceptionistService
{
    public bool isLoaded = false;
    public string ErrorMessage = string.Empty;
    public List<AppointmentPatientVM> listAppointment = new List<AppointmentPatientVM>();

    private readonly IHttpClientFactory _httpClientFactory;
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public bool isLoaded2 = false;
    public string ErrorMessage2 = string.Empty;
    public List<WorkScheduleDoctorVM> listWorkScheduleDoctor = new List<WorkScheduleDoctorVM>();

    public ReceptionistService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<dynamic> GetAllFreeTimeAppointmentForDoctor(DateOnly date, int doctorId)
    {
        string query = $"api/Appointment/GetAllFreeTimeAppointmentForDoctor/{date}/{doctorId}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
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
                    return result.Data ?? new List<TimeOnly>();
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

    public async Task<dynamic> DeleteWorkScheduleDoctorAsync(int id)
    {
        string query = $"api/WorkSchedule/DeleteWorkScheduleDortorAsync/{id}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");

            var response = await client.DeleteAsync(query);

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
            else{
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

    public async Task<dynamic> SaveWorkScheduleDoctorAsync(WorkScheduleDoctorDetailVM item)
    {
        string query = $"api/WorkSchedule/SaveWorkScheduleDortorAsync";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");

            var response = await client.PostAsJsonAsync(query, item);

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
            else{
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

    public async Task<dynamic> GetWorkScheduleDoctorAsync(int id)
    {
        if(id < 1){
            return new WorkScheduleDoctorDetailVM();
        }
        string query = $"api/WorkSchedule/GetWorkScheduleDortorAsync/{id}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
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

    public async Task GetAllWorkScheduleDoctorAsync()
    {
        string query = $"api/WorkSchedule/GetAllWorkScheduleDortorAsync";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<WorkScheduleDoctorVM>>>();
                if (result == null)
                {
                    ErrorMessage2 = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage2 = result.Message;
                    listWorkScheduleDoctor = result.Data ?? new List<WorkScheduleDoctorVM>();
                }
            }
            else
            {
                ErrorMessage2 = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage2 = "Thất bại!";
            Console.WriteLine(ex.Message);
        };
        isLoaded2 = true;
        NotifyStateChanged();
    }

    public async Task<dynamic> CreateAppointmentAsync(AppointmentReceptionistCreateVM appointmentReceptionistCreateVM)
    {
        string query = $"api/Appointment/CreateAppointmentFromReceptionist";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");

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
            else{
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

    public async Task<dynamic> GetDoctorByNameForReceptionistAsync(string searchKey)
    {
        if (string.IsNullOrWhiteSpace(searchKey))
        {
            return new List<DoctorSearchedForCreateAppointmentVM>();
        }
        string query = $"api/Doctor/GetByNameForReceptionistAsync/{searchKey}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<DoctorSearchedForCreateAppointmentVM>>>();
                if (result == null)
                {
                    // ErrorMessage = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage = result.Message;
                    return result.Data ?? new List<DoctorSearchedForCreateAppointmentVM>();
                }
            }
            else
            {
                // ErrorMessage = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return new List<DoctorSearchedForCreateAppointmentVM>();
    }

    public async Task<List<PatientSearchedForCreateAppointmentVM>> GetPatientByNameForReceptionistAsync(string searchKey)
    {
        if (string.IsNullOrWhiteSpace(searchKey))
        {
            return new List<PatientSearchedForCreateAppointmentVM>();
        }
        string query = $"api/Patient/GetByNameForReceptionistAsync/{searchKey}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<PatientSearchedForCreateAppointmentVM>>>();
                if (result == null)
                {
                    // ErrorMessage = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage = result.Message;
                    return result.Data ?? new List<PatientSearchedForCreateAppointmentVM>();
                }
            }
            else
            {
                // ErrorMessage = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            // ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        return new List<PatientSearchedForCreateAppointmentVM>();
    }

    public async Task GetAllAppointmentPatientAsync(string date = "")
    {
        string query = "api/Appointment/GetAllAppointmentPatientAsync";
        try
        {
            if (!string.IsNullOrWhiteSpace(date))
            {
                query += $"/{date}";
            }
            var client = _httpClientFactory.CreateClient("LocalApi");
            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<AppointmentPatientVM>>>();
                if (result == null)
                {
                    ErrorMessage = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage = result.Message;
                    listAppointment = result.Data ?? new List<AppointmentPatientVM>();
                }
            }
            else
            {
                ErrorMessage = response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
        }
        isLoaded = true;
        NotifyStateChanged();
    }
}
