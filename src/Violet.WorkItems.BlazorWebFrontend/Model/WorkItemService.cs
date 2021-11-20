using System.Net.Http.Json;
using Violet.WorkItems.Service.Messages;

namespace Violet.WorkItems.BlazorWebFrontend;

public class WorkItemService : BaseService
{
    public WorkItemService(HttpClient httpClient)
        : base(httpClient)
    { }

    public async Task<WorkItem> GetTemplateAsync(string projectCode, string workItemType)
    {
        var uri = $"{_baseUri}/projects/{projectCode}/types/{workItemType}/new";
        var result = await _httpClient.GetFromJsonAsync<WorkItemApiResponse>(uri, _jsonOptions);

        return result.WorkItem;
    }

    public async Task<IEnumerable<WorkItem>> GetAllWorkItems(string projectCode)
    {
        var uri = $"{_baseUri}/projects/{projectCode}/workitems";
        var result = await _httpClient.GetFromJsonAsync<WorkItemListApiResponse>(uri, _jsonOptions);

        return result.WorkItems;
    }

    public async Task<WorkItem> GetWorkItemAsync(string projectCode, string workItemId)
    {
        var uri = $"{_baseUri}/projects/{projectCode}/workitems/{workItemId}";
        var result = await _httpClient.GetFromJsonAsync<WorkItemApiResponse>(uri, _jsonOptions);

        return result.WorkItem;
    }

    public async Task<WorkItem> CreateWorkItemAsync(string projectCode, string workItemType, IEnumerable<Property> properties)
    {
        var uri = $"{_baseUri}/projects/{projectCode}/workitems";

        var response = await _httpClient.PostAsJsonAsync(uri, new CreateWorkItemApiRequest()
        {
            ProjectCode = projectCode,
            WorkItemType = workItemType,
            Properties = properties
        }, _jsonOptions);

        var result = await response.Content.ReadFromJsonAsync<WorkItemApiResponse>();

        return result.WorkItem;
    }

    public async Task<WorkItem> UpdateWorkItemAsync(WorkItem workItem, string comment)
    {

        var uri = $"{_baseUri}/projects/{workItem.ProjectCode}/workitems/{workItem.Id}";

        var response = await _httpClient.PostAsJsonAsync(uri, new UpdateWorkItemApiRequest()
        {
            ProjectCode = workItem.ProjectCode,
            WorkItemId = workItem.Id,
            Comment = comment,
            Properties = workItem.Properties
        }, _jsonOptions);

        var result = await response.Content.ReadFromJsonAsync<WorkItemApiResponse>();

        return result.WorkItem;
    }

    public async Task<WorkItem> ExecuteCommandAsync(string projectCode, string workItemId, string commandName)
    {
        var uri = $"{_baseUri}/projects/{projectCode}/workitems/{workItemId}/commands";

        var response = await _httpClient.PostAsJsonAsync(uri, new ExecuteCommandWorkItemApiRequest()
        {
            ProjectCode = projectCode,
            WorkItemId = workItemId,
            Command = commandName,
        }, _jsonOptions);

        var result = await response.Content.ReadFromJsonAsync<WorkItemApiResponse>();

        return result.WorkItem;
    }
}