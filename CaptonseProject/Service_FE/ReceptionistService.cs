using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.OpenApi.MicrosoftExtensions;

public class ReceptionistService
{
    public bool isLoaded = false;
    public string ErrorMessage = string.Empty;
    public List<AppointmentPatientVM> listAppointment = new List<AppointmentPatientVM>();
    private readonly IHttpClientFactory _httpClientFactory;

    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public ReceptionistService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task GetAllAppointmentPatientAsync(string date = "")
    {
        string query = "api/Appointment/GetAllAppointmentPatientAsync";
        try
        {
            if (!string.IsNullOrEmpty(date))
            {
                query += $"/{date}";
            }
            var client = _httpClientFactory.CreateClient("LocalApi");
            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<AppointmentPatientVM>>>();
                if(result == null){
                    ErrorMessage = "Lỗi dữ liệu!";
                }
                else{
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
