public class DoctorFEService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public bool isLoaded = false;
    public string ErrorMessage = string.Empty;
    public List<AppointmentPatientForDoctorVM> listAppointmentPatientForDoctorToDay = new List<AppointmentPatientForDoctorVM>();

    public DoctorFEService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task GetAllListPatientForDocTor(DateOnly? date = null)
    {
        if(date == null){
            date = DateOnly.FromDateTime(DateTime.Now);
        }
        Console.WriteLine("Chay 111");
        string query = $"api/Appointment/GetAllListPatientForDocTor/{date}";
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<AppointmentPatientForDoctorVM>>>();
                if (result == null)
                {
                    ErrorMessage = "Lỗi dữ liệu!";
                }
                else
                {
                    //ErrorMessage = result.Message;
                    listAppointmentPatientForDoctorToDay = result.Data ?? new List<AppointmentPatientForDoctorVM>();
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