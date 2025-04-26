using Blazored.LocalStorage;

public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    // Cấu hình tự động gắn access token lên header authorization
    // dùng thì gọi như dưới
    // builder.Services.AddTransient<AuthenticatedHttpClientHandler>();
    // builder.Services.AddHttpClient("ApiClient", client =>
    // {
    //     client.BaseAddress = new Uri("https://yourapi.com/");
    // })
    // .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
    
    private readonly ILocalStorageService _localStorage;

    public AuthenticatedHttpClientHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
