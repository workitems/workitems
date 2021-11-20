using System.Text.Json;
using System.Text.Json.Serialization;
using Violet.WorkItems.Query;

namespace Violet.WorkItems.BlazorWebFrontend;

public abstract class BaseService
{
    public BaseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected string _baseUri = "https://localhost:5001/api/v1";
    protected readonly HttpClient _httpClient;
    protected JsonSerializerOptions _jsonOptions = QuerySerialization.GetOptions();
}
