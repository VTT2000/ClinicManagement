using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;


public interface IReceptionistService
{

}
public class ReceptionistService : IReceptionistService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;

    public ReceptionistService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage, NavigationManager navigationManager)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }

    
}