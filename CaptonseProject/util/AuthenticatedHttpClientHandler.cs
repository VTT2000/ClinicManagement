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
        // _localStorage chỉ hoạt động ở FE, đây là server; muốn xài cấu hình thêm service bên blazor server tức serviceFE 
        // hiện tại bên service Fe; client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var token = await _localStorage.GetItemAsStringAsync("token");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
