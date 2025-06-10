using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using web_api_base.ViewModel;

namespace web_api_base.Service_FE.Services
{
    public interface IGetDoctorService
    {
        // State properties
        List<DoctorDTO> Doctors { get; }
        bool IsLoading { get; }
        string ErrorMessage { get; }
        int CurrentPage { get; }
        int TotalDoctors { get; }

        // Events
        event Action OnChange;
        event Action<string> OnError;

        // Methods
        Task<HTTPResponseClient<List<DoctorDTO>>> LoadDoctorsAsync(int pageNumber = 1, int pageSize = 12);
        Task LoadMoreDoctorsAsync();
        Task RefreshDoctorsAsync();
        void ClearDoctors();
    }

    public class GetDoctorService : IGetDoctorService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJSRuntime _jsRuntime;


        // State fields
        private List<DoctorDTO> _doctors = new List<DoctorDTO>();
        private bool _isLoading = false;
        private string _errorMessage = string.Empty;
        private int _currentPage = 1;
        private int _totalDoctors = 0;
        private int _pageSize = 12;

        // Public properties
        public List<DoctorDTO> Doctors => _doctors;
        public bool IsLoading => _isLoading;
        public string ErrorMessage => _errorMessage;
        public int CurrentPage => _currentPage;
        public int TotalDoctors => _totalDoctors;

        // Events
        public event Action OnChange;
        public event Action<string> OnError;
        private readonly ILocalStorageService _localStorage;

        public GetDoctorService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _jsRuntime = jsRuntime;
            _localStorage = localStorage;
        }

        public async Task<HTTPResponseClient<List<DoctorDTO>>> LoadDoctorsAsync(int pageNumber = 1, int pageSize = 12)
        {
            var result = new HTTPResponseClient<List<DoctorDTO>>
            {
                Data = new List<DoctorDTO>(),
                Message = string.Empty,
            };
            try
            {

                // // Lấy token từ localStorage
                // var token = await _localStorage.GetItemAsStringAsync("token");
                var request = new HttpRequestMessage(HttpMethod.Get, $"api/User/GetAllDoctors?pageNumber={pageNumber}&pageSize={pageSize}");
                // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                SetLoading(true);
                ClearError();

                _currentPage = pageNumber;
                _pageSize = pageSize;



                var client = _httpClientFactory.CreateClient("LocalApi");
                var response = await client.SendAsync(request);

                // Log thông tin response để debug
                await LogToConsole($"Response Status: {response.StatusCode}");
                await LogToConsole($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<HTTPResponseClient<List<DoctorDTO>>>(
                        jsonResponse,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    result.Data = apiResponse?.Data ?? new List<DoctorDTO>();
                    result.Message = apiResponse?.Message ?? string.Empty;

                    if (apiResponse?.Data != null)
                    {
                        if (pageNumber == 1)
                        {
                            _doctors = apiResponse.Data;
                        }
                        else
                        {
                            _doctors.AddRange(apiResponse.Data);
                        }

                        ParsePaginationInfo(apiResponse.Message);
                        await LogToConsole($"Loaded {apiResponse.Data.Count} doctors successfully");
                    }
                    else
                    {
                        SetError("Không nhận được dữ liệu từ server");
                    }
                }
                else
                {
                    SetError($"Lỗi API: {response.StatusCode} - {response.ReasonPhrase}");
                }

            }
            catch (Exception ex)
            {
                SetError($"Lỗi khi tải danh sách bác sĩ: {ex.Message}");
                await LogToConsole($"Error loading doctors: {ex.Message}");
                result.Message = $"Lỗi khi tải danh sách bác sĩ: {ex.Message}";
                result.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                result.Data = null;
            }
            finally
            {
                SetLoading(false);

            }
            
            return result;
        }

        public async Task LoadMoreDoctorsAsync()
        {
            if (_isLoading) return;

            var nextPage = _currentPage + 1;
            await LoadDoctorsAsync(nextPage, _pageSize);
        }

        public async Task RefreshDoctorsAsync()
        {
            await LoadDoctorsAsync(1, _pageSize);
        }

        public void ClearDoctors()
        {
            _doctors.Clear();
            _currentPage = 1;
            _totalDoctors = 0;
            ClearError();
            NotifyStateChanged();
        }

        private void SetLoading(bool isLoading)
        {
            _isLoading = isLoading;
            NotifyStateChanged();
        }

        private void SetError(string error)
        {
            _errorMessage = error;
            OnError?.Invoke(error);
            NotifyStateChanged();
        }

        private void ClearError()
        {
            _errorMessage = string.Empty;
        }

        private void ParsePaginationInfo(string message)
        {
            if (!string.IsNullOrEmpty(message) && message.Contains("Tổng"))
            {
                var parts = message.Split("Tổng");
                if (parts.Length > 1)
                {
                    var totalPart = parts[1].Trim().Split(' ')[0];
                    if (int.TryParse(totalPart, out int total))
                    {
                        _totalDoctors = total;
                    }
                }
            }
        }

        private async Task LogToConsole(string message)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("console.log", $"GetDoctorService: {message}");
            }
            catch
            {
                // Ignore console errors
            }
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}