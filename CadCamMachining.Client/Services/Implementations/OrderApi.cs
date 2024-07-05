using System.Net.Http;

namespace CadCamMachining.Client.Services.Implementations;

public class OrderApi
{
    private readonly HttpClient _httpClient;

    public OrderApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}