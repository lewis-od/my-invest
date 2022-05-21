using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyInvest.ComponentTests;

public class RestClient : IDisposable
{
    private static readonly JsonSerializerOptions JsonDeserializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly HttpClient _httpClient;

    public RestClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetObjectAsync<T>(string endpoint)
    {
        var result = await _httpClient.GetStreamAsync(endpoint);
        return await DeserializeJsonAsync<T>(result);
    }

    public async Task<TResponse?> PostObjectAsync<TPayload, TResponse>(string endpoint, TPayload payload)
    {
        var payloadJson = new StringContent(JsonSerializer.Serialize(payload), Encoding.Default, MediaTypeNames.Application.Json);
        var result = await _httpClient.PostAsync(endpoint, payloadJson);
        return await DeserializeJsonAsync<TResponse>(await result.Content.ReadAsStreamAsync());
    }

    private static async Task<T?> DeserializeJsonAsync<T>(Stream response) =>
        await JsonSerializer.DeserializeAsync<T>(response, JsonDeserializerOptions);

    public async Task PutAsync(string endpoint)
    {
        await _httpClient.PutAsync(endpoint, null);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
