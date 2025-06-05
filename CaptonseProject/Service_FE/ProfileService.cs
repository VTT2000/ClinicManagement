
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
// Duplicate removed
using Blazored.LocalStorage;
using web_api_base.ViewModel;
using System.Text.Json;
using web_api_base.Models.Configuration;


namespace web_api_base.Service_FE
{
  public interface IProfileService
  {
    // State của Profile
    ProfilePatientVM CurrentProfile { get; }

    // Event để thông báo khi profile thay đổi
    event Action OnChange;

    // Lấy thông tin profile của người dùng hiện tại
    Task<bool> LoadUserProfile();

    // Cập nhật thông tin profile
    Task<bool> UpdateProfile(UpdateProfilePatientVM updateModel, IBrowserFile file = null);
  
    
  }

  public class ProfileService : IProfileService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly IJSRuntime _jsRuntime;



    // State của Profile
    public ProfilePatientVM CurrentProfile { get; private set; }

    public string imageUrl { get; private set; } = ""; 

    // Event để thông báo khi profile thay đổi
    public event Action OnChange;

    // Constructor
    public ProfileService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage, IJSRuntime jsRuntime)
    {
      _httpClientFactory = httpClientFactory;
      _localStorage = localStorage;
      _jsRuntime = jsRuntime;
      CurrentProfile = new ProfilePatientVM();
    }



    // Lấy thông tin profile của người dùng hiện tại
    public async Task<bool> LoadUserProfile()
    {

      var client = _httpClientFactory.CreateClient("LocalApi");

      try
      {
        // Lấy token từ localStorage
        var token = await _localStorage.GetItemAsStringAsync("token");
        if (string.IsNullOrEmpty(token))
        {
          return false;
        }

        // Cấu hình authorization header
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //Lấy user từ localStorage
        var user = await _localStorage.GetItemAsStringAsync("userInfo");

        // Lấy ID người dùng từ token hoặc localStorage
        var userInfo = JsonSerializer.Deserialize<UserInfo>(user);
        var userId = userInfo.UserId;


        // Gọi API để lấy thông tin profile
        var response = await client.GetAsync($"api/Patient/GetProfilePatient?id={userId.ToString()}");

        if (response.IsSuccessStatusCode)
        {
          var apiResponse = await response.Content.ReadFromJsonAsync<HTTPResponseClient<ProfilePatientVM>>();
          if (apiResponse != null && apiResponse.Data != null)
          {
            CurrentProfile = apiResponse.Data;
            this.imageUrl = apiResponse.Data.ImageUrl;
            NotifyStateChanged();
            return true;
          }
        }

        return false;
      }
      catch (Exception ex)
      {
        await _jsRuntime.InvokeVoidAsync("console.error", $"Error loading profile: {ex.Message}");
        return false;
      }
    }

    // Cập nhật thông tin profile
    public async Task<bool> UpdateProfile(UpdateProfilePatientVM updateModel, IBrowserFile file = null)
    {
      var client = _httpClientFactory.CreateClient("LocalApi");
      try
      {
        // Lấy token từ localStorage
        var token = await _localStorage.GetItemAsStringAsync("token");
        if (string.IsNullOrEmpty(token))
        {
          return false;
        }

        // Cấu hình authorization header
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Lấy ID người dùng từ token hoặc localStorage
        var userId = await _localStorage.GetItemAsync<int>("userId");

        // Tạo MultipartFormDataContent để gửi cả dữ liệu và file
        var content = new MultipartFormDataContent();

        // Thêm các trường dữ liệu
        content.Add(new StringContent(updateModel.FullName), "FullName");
        content.Add(new StringContent(updateModel.Email), "Email");
        content.Add(new StringContent(updateModel.Role ?? ""), "Role");
        content.Add(new StringContent(updateModel.DOB ?? ""), "DOB");
        content.Add(new StringContent(updateModel.Phone ?? ""), "Phone");
        content.Add(new StringContent(updateModel.Address ?? ""), "Address");

        // Thêm file nếu có
        if (file != null)
        {
          var fileContent = new StreamContent(file.OpenReadStream(2 * 1024 * 1024)); // Giới hạn 2MB
          fileContent.Headers.ContentType =
              new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
          content.Add(fileContent, "file", file.Name);
        }

        // Gọi API để cập nhật profile
        var response = await client.PutAsync($"api/Patient/UpdatePatient/{userId}", content);

        if (response.IsSuccessStatusCode)
        {
          var apiResponse = await response.Content.ReadFromJsonAsync<HTTPResponseClient<ProfilePatientVM>>();
          if (apiResponse != null && apiResponse.Data != null)
          {
            // Cập nhật state
            CurrentProfile = apiResponse.Data;
            NotifyStateChanged();
            return true;
          }
        }

        // Hiển thị thông báo lỗi
        var errorResponse = await response.Content.ReadAsStringAsync();
        await _jsRuntime.InvokeVoidAsync("console.error", $"Error updating profile: {errorResponse}");
        return false;
      }
      catch (Exception ex)
      {
        await _jsRuntime.InvokeVoidAsync("console.error", $"Error updating profile: {ex.Message}");
        return false;
      }
    }

    // Tải lên ảnh đại diện mới
    public async Task<bool> UploadProfileImage(IBrowserFile file)
    {
      if (file == null) return false;
      var client = _httpClientFactory.CreateClient("LocalApi");

      try
      {
        // Lấy token từ localStorage
        var token = await _localStorage.GetItemAsStringAsync("token");
        if (string.IsNullOrEmpty(token))
        {
          return false;
        }

        // Cấu hình authorization header
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Lấy ID người dùng từ token hoặc localStorage
        var userId = await _localStorage.GetItemAsync<int>("userId");

        // Tạo content để upload file
        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream(2 * 1024 * 1024)); // Giới hạn 2MB
        fileContent.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        content.Add(fileContent, "file", file.Name);

        // Gọi API để upload ảnh
        var response = await client.PostAsync("api/User/UploadProfileImage", content);

        if (response.IsSuccessStatusCode)
        {
          var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<string>>();
          if (result != null && !string.IsNullOrEmpty(result.Data))
          {
            // Cập nhật ImageUrl trong state
            CurrentProfile.ImageUrl = result.Data;
            NotifyStateChanged();
            return true;
          }
        }

        return false;
      }
      catch (Exception ex)
      {
        await _jsRuntime.InvokeVoidAsync("console.error", $"Error uploading image: {ex.Message}");
        return false;
      }
    }


    // Xóa profile đã lưu (thường dùng khi logout)
    public void ClearProfile()
    {
      CurrentProfile = new ProfilePatientVM();
      NotifyStateChanged();
    }


    // Thông báo state đã thay đổi
    private void NotifyStateChanged() => OnChange?.Invoke();

  }

}