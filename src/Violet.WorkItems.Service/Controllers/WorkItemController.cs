using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Violet.WorkItems.Service.Models;

namespace Violet.WorkItems.Service.Controllers;

[ApiController]
[Authorize("WorkItemPolicy")]
public class WorkItemController : ControllerBase
{
    private readonly WorkItemManager _workItemManager;
    private readonly ILogger<WorkItemController> _logger;

    public WorkItemController(WorkItemManager workItemManager, ILogger<WorkItemController> logger)
    {
        _workItemManager = workItemManager;
        _logger = logger;
    }

    [HttpGet("api/v1/projects/{projectCode}/types/{workItemType}/new")]
    [ProducesResponseType(typeof(WorkItemApiResponse), 200)]
    [ProducesResponseType(typeof(WorkItemBadRequestApiResponse), 400)]
    [ProducesResponseType(typeof(WorkItemApiResponse), 404)]
    public async Task<ActionResult> GetNewFromTemplate(string projectCode, string workItemType)
    {
        try
        {
            var item = await _workItemManager.CreateTemplateAsync(projectCode, workItemType);

            return item is not null
                ? Ok(new WorkItemApiResponse()
                {
                    Success = true,
                    ProjectCode = item.ProjectCode,
                    WorkItemId = null,
                    WorkItem = item,
                })
                : NotFound(new WorkItemApiResponse()
                {
                    Success = false,
                    ProjectCode = projectCode,
                    WorkItemId = null,
                    WorkItem = null,
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception in {nameof(WorkItemController)}.{nameof(GetNewFromTemplate)}");

            return BadRequest(new WorkItemBadRequestApiResponse()
            {
                Success = false,
                ProjectCode = projectCode,
                WorkItemId = null,
            });
        }
    }

    [HttpGet("api/v1/projects/{projectCode}/workitems/{workItemId}")]
    [ProducesResponseType(typeof(WorkItemApiResponse), 200)]
    [ProducesResponseType(typeof(WorkItemBadRequestApiResponse), 400)]
    [ProducesResponseType(typeof(WorkItemApiResponse), 404)]
    public async Task<ActionResult> GetSingleWorkItem(string projectCode, string workItemId)
    {
        try
        {
            var item = await _workItemManager.GetAsync(projectCode, workItemId);

            return item is not null
                ? Ok(new WorkItemApiResponse()
                {
                    Success = true,
                    ProjectCode = projectCode,
                    WorkItemId = workItemId,
                    WorkItem = item,
                })
                : NotFound(new WorkItemApiResponse()
                {
                    Success = false,
                    ProjectCode = projectCode,
                    WorkItemId = workItemId,
                    WorkItem = null,
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception in {nameof(WorkItemController)}.{nameof(GetSingleWorkItem)}");

            return BadRequest(new WorkItemBadRequestApiResponse()
            {
                Success = false,
                ProjectCode = projectCode,
                WorkItemId = workItemId,
            });
        }
    }

    [HttpPost("api/v1/projects/{projectCode}/workitems")]
    [ProducesResponseType(typeof(WorkItemApiResponse), 200)]
    [ProducesResponseType(typeof(WorkItemBadRequestApiResponse), 400)]
    public async Task<ActionResult> CreateSingleWorkItem(string projectCode, [FromBody] CreateWorkItemApiRequest request)
    {
        try
        {
            var result = await _workItemManager.CreateAsync(request.ProjectCode, request.WorkItemType, request.Properties);

            return result is { Success: true }
                ? Ok(new WorkItemApiResponse()
                {
                    Success = true,
                    ProjectCode = result.ChangedWorkItem.ProjectCode,
                    WorkItemId = result.ChangedWorkItem.Id,
                    WorkItem = result.ChangedWorkItem,
                })
                : BadRequest(new WorkItemBadRequestApiResponse()
                {
                    Success = false,
                    ProjectCode = projectCode,
                    Errors = result.Errors,
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception in {nameof(WorkItemController)}.{nameof(GetSingleWorkItem)}");

            return BadRequest(new WorkItemBadRequestApiResponse()
            {
                Success = false,
                ProjectCode = projectCode,
            });
        }
    }

    [HttpPost("api/v1/projects/{projectCode}/workitems/{workItemId}")]
    [ProducesResponseType(typeof(WorkItemApiResponse), 200)]
    [ProducesResponseType(typeof(WorkItemBadRequestApiResponse), 400)]
    public async Task<ActionResult> UpdateSingleWorkItem(string projectCode, string workItemId, [FromBody] UpdateWorkItemApiRequest request)
    {
        try
        {
            var result = await _workItemManager.UpdateAsync(request.ProjectCode, request.WorkItemId, request.Properties, request.Comment);

            return result is { Success: true }
                ? Ok(new WorkItemApiResponse()
                {
                    Success = true,
                    ProjectCode = projectCode,
                    WorkItemId = workItemId,
                    WorkItem = result.ChangedWorkItem,
                })
                : BadRequest(new WorkItemBadRequestApiResponse()
                {
                    Success = false,
                    ProjectCode = projectCode,
                    WorkItemId = workItemId,
                    Errors = result.Errors,
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception in {nameof(WorkItemController)}.{nameof(GetSingleWorkItem)}");

            return BadRequest(new WorkItemBadRequestApiResponse()
            {
                Success = false,
                ProjectCode = projectCode,
                WorkItemId = workItemId,
            });
        }
    }

    [HttpPost("api/v1/projects/{projectCode}/workitems/{workItemId}/commands")]
    [ProducesResponseType(typeof(WorkItemApiResponse), 200)]
    [ProducesResponseType(typeof(WorkItemBadRequestApiResponse), 400)]
    public async Task<ActionResult> ExecuteCommandOnSingleWorkItem(string projectCode, string workItemId, [FromBody] ExecuteCommandWorkItemApiRequest request)
    {
        try
        {
            var result = await _workItemManager.ExecuteCommandAsync(request.ProjectCode, request.WorkItemId, request.Command);

            return result is { Success: true }
                ? Ok(new WorkItemApiResponse()
                {
                    Success = true,
                    ProjectCode = projectCode,
                    WorkItemId = workItemId,
                    WorkItem = result.ChangedWorkItem,
                })
                : BadRequest(new WorkItemBadRequestApiResponse()
                {
                    Success = false,
                    ProjectCode = projectCode,
                    WorkItemId = workItemId,
                    Errors = result.Errors,
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception in {nameof(WorkItemController)}.{nameof(GetSingleWorkItem)}");

            return BadRequest(new WorkItemBadRequestApiResponse()
            {
                Success = false,
                ProjectCode = projectCode,
                WorkItemId = workItemId,
            });
        }
    }
}
