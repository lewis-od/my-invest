using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MyInvest.REST.Clients;

namespace MyInvest.Specs.Drivers;

public class ClientDriver
{
    private readonly HttpClient _httpClient = new();
    private readonly string _baseUrl;

    public ClientDriver(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<ClientDto?> SignUp(string username)
    {
        var createClientRequest = new SignUpRequestDto
        {
            Username = username
        };
        var json = new StringContent(JsonSerializer.Serialize(createClientRequest), Encoding.Default, "application/json");
        var result = await _httpClient.PostAsync($"{_baseUrl}/clients/sign-up", json);
        var response = await result.Content.ReadAsStringAsync();
        var opts = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var dto = JsonSerializer.Deserialize<ClientDto>(response, opts);
        return dto;
        // return JsonSerializer.Deserialize<ClientDto>(await result.Content.ReadAsStringAsync());
    }
}
