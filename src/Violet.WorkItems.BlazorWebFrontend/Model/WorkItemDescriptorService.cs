using System.Net.Http.Json;

namespace Violet.WorkItems.BlazorWebFrontend;

public class WorkItemDescriptorService : BaseService
{
    public WorkItemDescriptorService(HttpClient httpClient)
    : base(httpClient)
    { }

    public async Task<WorkItemDescriptorApiResponse> GetWorkItemDescriptorByWorkItemType(string projectCode, string workItemType)
    {
        var uri = $"{_baseUri}/projects/{projectCode}/types/{workItemType}/descriptor";
        var result = await _httpClient.GetFromJsonAsync<WorkItemDescriptorApiResponse>(uri, _jsonOptions);

        return result;
    }

    public async Task<WorkItemDescriptorApiResponse> GetWorkItemDescriptorByWorkItemId(string projectCode, string workItemId)
    {
        var uri = $"{_baseUri}/projects/{projectCode}/workitems/{workItemId}/descriptor";
        var result = await _httpClient.GetFromJsonAsync<WorkItemDescriptorApiResponse>(uri, _jsonOptions);

        return result;
    }
}
