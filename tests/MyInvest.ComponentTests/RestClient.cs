using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyInvest.ComponentTests;

public class RestResponse<TBody>
{
    public HttpStatusCode StatusCode { get; }
    public TBody? Body { get; }

    public RestResponse(HttpStatusCode statusCode, TBody? body)
    {
        StatusCode = statusCode;
        Body = body;
    }
}

public class RestResponse
{
    public HttpStatusCode StatusCode { get; }
    
    public RestResponse(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
}

public static class HttpStatusCodeExtensions
{
    public static bool IsSuccess(this HttpStatusCode code) => (int) code >= 200 && (int) code < 300;
}

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

    public async Task<RestResponse<T>> GetObjectAsync<T>(string endpoint)
    {
        var result = await _httpClient.GetAsync(endpoint);
        return await ToRestResponseWithBodyAsync<T>(result);
    }

    public async Task<RestResponse<TResponse>> PostObjectAsync<TPayload, TResponse>(string endpoint, TPayload payload)
    {
        var payloadJson = new StringContent(JsonSerializer.Serialize(payload), Encoding.Default, MediaTypeNames.Application.Json);
        var result = await _httpClient.PostAsync(endpoint, payloadJson);
        return await ToRestResponseWithBodyAsync<TResponse>(result);
    }
    
    public async Task<RestResponse> PatchObjectAsync<TPayload>(string endpoint, TPayload payload)
    {
        var payloadJson = new StringContent(JsonSerializer.Serialize(payload), Encoding.Default, MediaTypeNames.Application.Json);
        var result = await _httpClient.PatchAsync(endpoint, payloadJson);
        return new RestResponse(result.StatusCode);
    }

    public async Task<RestResponse> PutAsync(string endpoint)
    {
        var result = await _httpClient.PutAsync(endpoint, null);
        return new RestResponse(result.StatusCode);
    }
    
    private static async Task<RestResponse<T>> ToRestResponseWithBodyAsync<T>(HttpResponseMessage httpResponse)
    {
        var contentStream = await httpResponse.Content.ReadAsStreamAsync();
        var body = await DeserializeJsonAsync<T>(contentStream);
        return new RestResponse<T>(httpResponse.StatusCode, body);
    }
    
    private static async Task<T?> DeserializeJsonAsync<T>(Stream response) =>
        await JsonSerializer.DeserializeAsync<T>(response, JsonDeserializerOptions);

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
