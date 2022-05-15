using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace MyInvest.Specs;

public class RestClient
{
    private static readonly JsonSerializerOptions JsonDeserializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly HttpClient _httpClient;

    public RestClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetObject<T>(string endpoint)
    {
        var result = await _httpClient.GetStreamAsync(endpoint);
        return await DeserializeJson<T>(result);
    }

    public async Task<TResponse?> PostObject<TPayload, TResponse>(string endpoint, TPayload payload)
    {
        var payloadJson = new StringContent(JsonSerializer.Serialize(payload), Encoding.Default, MediaTypeNames.Application.Json);
        var result = await _httpClient.PostAsync(endpoint, payloadJson);
        return await DeserializeJson<TResponse>(await result.Content.ReadAsStreamAsync());
    }

    private static async Task<T?> DeserializeJson<T>(Stream response) => await JsonSerializer.DeserializeAsync<T>(response, JsonDeserializerOptions);
}
