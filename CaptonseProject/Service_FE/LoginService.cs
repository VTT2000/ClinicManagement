using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace web_api_base.Service_FE.Services
{
  public interface ILoginService
  {
    Task<LoginResult> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<UserInfo> GetUserInfoAsync();
  }

  public class LoginService : ILoginService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;
    private UserInfo _cachedUserInfo;

    public LoginService(
        IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorage,
        NavigationManager navigationManager)
    {
      _httpClientFactory = httpClientFactory;
      _localStorage = localStorage;
      _navigationManager = navigationManager;
    }

    public async Task<LoginResult> LoginAsync(string email, string password)
    {
      try
      {
        var client = _httpClientFactory.CreateClient("LocalApi");
        var loginModel = new { Account = email, Password = password };

        var response = await client.PostAsJsonAsync("/api/User/Login", loginModel);

        if (response.IsSuccessStatusCode)
        {
          var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<UserLoginResultVM>>();

          await _localStorage.SetItemAsStringAsync("token", result.Data.AccessToken);
          await _localStorage.SetItemAsStringAsync("userInfo", JsonSerializer.Serialize(result.Data.User));

          // Lưu cache thông tin người dùng
          _cachedUserInfo = result.Data.User;

          return new LoginResult { IsSuccess = true };
        }

        return new LoginResult
        {
          IsSuccess = false,
          ErrorMessage = "Invalid email or password."
        };
      }
      catch (Exception ex)
      {
        return new LoginResult
        {
          IsSuccess = false,
          ErrorMessage = $"Login failed: {ex.Message}"
        };
      }
    }

    public async Task LogoutAsync()
    {
      await _localStorage.RemoveItemAsync("token");
      await _localStorage.RemoveItemAsync("userInfo");
      _cachedUserInfo = null;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
      var token = await _localStorage.GetItemAsStringAsync("token");
      return !string.IsNullOrEmpty(token);
    }

    public async Task<UserInfo> GetUserInfoAsync()
    {
      if (_cachedUserInfo != null)
        return _cachedUserInfo;

      var userInfoJson = await _localStorage.GetItemAsStringAsync("userInfo");

      if (!string.IsNullOrEmpty(userInfoJson))
      {
        try
        {
          var userInfo = JsonSerializer.Deserialize<UserInfo>(userInfoJson,
              new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
          _cachedUserInfo = userInfo;
          return userInfo;
        }
        catch
        {
          // Nếu không đọc được JSON, xóa thông tin lưu trữ không hợp lệ
          await _localStorage.RemoveItemAsync("userInfo");
        }
      }

      return null;
    }


  }
  // Model classes
  public class LoginResult
  {
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
  }



  public class UserLoginResultVM
  {
    public string AccessToken { get; set; }
    public UserInfo User { get; set; }
  }

  public class UserInfo
  {
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    // Thêm các thuộc tính khác nếu cần
  }
}