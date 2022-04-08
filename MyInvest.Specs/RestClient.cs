using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MyInvest.Specs;

public class RestClient
{
    private const string JsonContentType = "application/json";

    private static readonly JsonSerializerOptions JsonDeserializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly string _baseUrl;
    private readonly HttpClient _httpClient = new();

    public RestClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));
    }

    public async Task<T?> GetObject<T>(string endpoint)
    {
        var result = await _httpClient.GetStreamAsync($"{_baseUrl}${endpoint}");
        return await DeserializeJson<T>(result);
    }

    public async Task<TResponse?> PostObject<TPayload, TResponse>(string endpoint, TPayload payload)
    {
        var payloadJson = new StringContent(JsonSerializer.Serialize(payload), Encoding.Default, JsonContentType);
        var result = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", payloadJson);
        return await DeserializeJson<TResponse>(await result.Content.ReadAsStreamAsync());
    }

    private static async Task<T?> DeserializeJson<T>(Stream response) => await JsonSerializer.DeserializeAsync<T>(response, JsonDeserializerOptions);
}
