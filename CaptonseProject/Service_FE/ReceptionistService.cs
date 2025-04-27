using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.OpenApi.MicrosoftExtensions;


public interface IReceptionistService
{
    public Task<List<AppointmentPatientVM>> GetAllAppointmentPatientAsync(string date = "");
}
public class ReceptionistService : IReceptionistService
{
    public string ErrorMessage = string.Empty;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;

    public ReceptionistService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage, NavigationManager navigationManager)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }

    public async Task<List<AppointmentPatientVM>> GetAllAppointmentPatientAsync(string date = "")
    {
        string query = "api/Appointment/GetAllAppointmentPatientAsync";
        try
        {
            if (!string.IsNullOrEmpty(date))
            {
                if (DateTime.TryParse(date, out DateTime result))
                {
                    //Console.WriteLine($"Giá trị hợp lệ: {result}");
                    query += $"/{date}";
                }
                else
                {
                    //Console.WriteLine($"Giá trị không hợp lệ, không thể chuyển sang DateTime.");
                    ErrorMessage = "Giá trị không hợp lệ!";
                    return new List<AppointmentPatientVM>();
                }
            }
            var client = _httpClientFactory.CreateClient("LocalApi");
            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<AppointmentPatientVM>>>();
                if(result == null){
                    ErrorMessage = "Lỗi dữ liệu!";
                    return new List<AppointmentPatientVM>();
                }
                else{
                    ErrorMessage = result.Message;
                    return result.Data ?? new List<AppointmentPatientVM>();
                }
            }
            else
            {
                ErrorMessage = response.StatusCode.ToString();
                return new List<AppointmentPatientVM>();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Thất bại!";
            Console.WriteLine(ex.Message);
            return new List<AppointmentPatientVM>();
        }
    }
}