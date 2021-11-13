using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Violet.WorkItems.Service.Models;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.Service.Controllers;

[ApiController]
[Authorize("WorkItemPolicy")]
public class ValueProviderController : ControllerBase
{
    private readonly WorkItemManager _workItemManager;

    public ValueProviderController(WorkItemManager workItemManager)
    {
        _workItemManager = workItemManager;
    }

    [HttpGet("api/v1/projects/{projectCode}/types/{workItemType}/properties/{propertyName}/provider")]
    public async Task<ValueProviderApiResponse> GetValuesForTemplateProperty(string projectCode, string workItemType, string propertyName, [FromQuery] string query)
        => await InternalGetValuesForWorkItemProperty(await _workItemManager.CreateTemplateAsync(projectCode, workItemType), propertyName, query);

    [HttpGet("api/v1/projects/{projectCode}/workitems/{workItemId}/properties/{propertyName}/provider")]
    public async Task<ValueProviderApiResponse> GetValuesForWorkItemProperty(string projectCode, string workItemId, string propertyName, [FromQuery] string query)
        => await InternalGetValuesForWorkItemProperty(await _workItemManager.GetAsync(projectCode, workItemId), propertyName, query);

    private async Task<ValueProviderApiResponse> InternalGetValuesForWorkItemProperty(WorkItem item, string propertyName, [FromQuery] string query)
    {
        var descriptor = _workItemManager.DescriptorManager.GetCurrentPropertyDescriptor(item, propertyName);

        if (descriptor.ValueProvider is null)
        {
            return new ValueProviderApiResponse()
            {
                ProjectCode = item.ProjectCode,
                WorkItemId = item.Id,
                PropertyName = propertyName,
                Values = Array.Empty<ProvidedValue>(),
            };
        }
        else
        {
            var provider = _workItemManager.ValidationManager.CreateValueProvider(item, descriptor.ValueProvider);

            var suggestions = await provider.SuggestionsAsync(query ?? string.Empty);

            return new ValueProviderApiResponse()
            {
                Success = true,
                ProjectCode = item.ProjectCode,
                WorkItemId = item.Id,
                PropertyName = propertyName,
                Values = suggestions,
            };
        }
    }
}
