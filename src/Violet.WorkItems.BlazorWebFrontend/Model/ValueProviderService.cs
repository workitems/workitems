using System.Net.Http.Json;
using Violet.WorkItems.Service.Messages;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.BlazorWebFrontend;

public class ValueProviderService : BaseService
{
    public ValueProviderService(HttpClient httpClient)
        : base(httpClient)
    { }

    public async Task<IEnumerable<ProvidedValue>> GetValuesForTemplateProperty(string projectCode, string workItemType, string propertyName)
    {
        var uri = $"{_baseUri}/projects/{projectCode}/types/{workItemType}/properties/{propertyName}/provider";
        var result = await _httpClient.GetFromJsonAsync<ValueProviderApiResponse>(uri, _jsonOptions);

        return result.Values;
    }
}