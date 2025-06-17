using System.Net.Http.Headers;
using System.Reflection.Metadata;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ChatLocalServiceFE
{
    private readonly HubConnection _hubConnection;
    public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    public ChatLocalServiceFE(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage, IConfiguration configuration, NavigationManager navigationManager)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        var apiBaseUrl = configuration.GetSection("ApiSettings").GetValue<string>("ApiBaseUrl");
        _hubConnection = new HubConnectionBuilder()
        .WithUrl(new Uri(new Uri(apiBaseUrl ?? ""), "hub/chatlocal").ToString(), options =>
        {
            options.AccessTokenProvider = async () =>
            {
                var token = await _localStorage.GetItemAsStringAsync("token");
                return token;
            };
        })
        .WithAutomaticReconnect()
        .Build();
    }

    public async Task<HTTPResponseClient<bool>> SendMessageChatRoom(MemberChatLocalSendVM item)
    {
        string query = $"api/ChatLocal/SendMessageChatRoom";
        HTTPResponseClient<bool> kq = new HTTPResponseClient<bool>
        {
            Data = false
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, item);

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

    public async Task<HTTPResponseClient<bool>> CreateChatRoom(ChatBoxLocalCreateVM item)
    {
        string query = $"api/ChatLocal/CreateChatRoom";
        HTTPResponseClient<bool> kq = new HTTPResponseClient<bool>
        {
            Data = false
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, item);

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

    public async Task<HTTPResponseClient<bool>> DeleteChatRoom(Guid chatID)
    {
        string query = $"api/ChatLocal/DeleteChatRoom/{chatID}";
        HTTPResponseClient<bool> kq = new HTTPResponseClient<bool>
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

    public async Task<HTTPResponseClient<PagedResponse<List<UserSelectdVMForChatBoxLocal>>>> GetAllUserForChatBoxLocal(PagedResponse<ConditionFilterUserSelectedForChatBoxLocal> pagedResponse)
    {
        string query = $"api/ChatLocal/GetAllUserForChatBoxLocal";
        HTTPResponseClient<PagedResponse<List<UserSelectdVMForChatBoxLocal>>> kq = new HTTPResponseClient<PagedResponse<List<UserSelectdVMForChatBoxLocal>>>()
        {
            Data = new PagedResponse<List<UserSelectdVMForChatBoxLocal>>()
            {
                Data = new List<UserSelectdVMForChatBoxLocal>(),
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
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<PagedResponse<List<UserSelectdVMForChatBoxLocal>>>>();

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

    public async Task<HTTPResponseClient<List<ChatBoxLocalVM>>> GetAllChatRoom()
    {
        string query = $"api/ChatLocal/GetAllChatRoom";
        HTTPResponseClient<List<ChatBoxLocalVM>> kq = new HTTPResponseClient<List<ChatBoxLocalVM>>()
        {
            Data = new List<ChatBoxLocalVM>()
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<ChatBoxLocalVM>>>();

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

    public async Task<HTTPResponseClient<List<MemberChatLocalVM>>> GetAllMessageChatFromChatRoom(Guid chatID)
    {
        string query = $"api/ChatLocal/GetAllMessageChatFromChatRoom";
        HTTPResponseClient<List<MemberChatLocalVM>> kq = new HTTPResponseClient<List<MemberChatLocalVM>>()
        {
            Data = new List<MemberChatLocalVM>()
        };
        try
        {
            var client = _httpClientFactory.CreateClient("LocalApi");
            var token = await _localStorage.GetItemAsStringAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(query, chatID);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HTTPResponseClient<List<MemberChatLocalVM>>>();

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

    /// <summary>
    /// Kết nối tới SignalR Hub
    /// </summary>
    public async Task ConnectAsync()
    {
        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            await _hubConnection.StartAsync();
        }
    }

    /// <summary>
    /// Ngắt kết nối SignalR Hub
    /// </summary>
    public async Task DisconnectAsync()
    {
        if (_hubConnection.State != HubConnectionState.Disconnected)
        {
            await _hubConnection.StopAsync();
        }
    }

    /// <summary>
    /// Gửi tin nhắn đến server
    /// </summary>
    public async Task SendMessageAsync1(string methodName)
    {
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.InvokeAsync(methodName);
        }
    }
    /// <summary>
    /// Gửi tin nhắn đến server
    /// </summary>
    public async Task SendMessageAsync2<T>(string methodName, T input)
    {
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.InvokeAsync(methodName, input);
        }
    }

    /// <summary>
    /// Đăng ký lắng nghe một sự kiện SignalR kiểu tổng quát
    /// </summary>
    public void OnEvent1(string methodName, Action handler)
    {
        _hubConnection.On(methodName, () =>
        {
            handler();
        });
    }
    /// <summary>
    /// Đăng ký lắng nghe một sự kiện SignalR kiểu tổng quát
    /// </summary>
    public void OnEvent2<T>(string methodName, Action<T> handler)
    {
        _hubConnection.On<T>(methodName, (data) =>
        {
            handler(data);
        });
    }
}