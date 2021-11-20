using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Violet.WorkItems.Query;
using Violet.WorkItems.Service.Messages;

namespace Violet.WorkItems.Service.Controllers;

[ApiController]
[Authorize("WorkItemPolicy")]
public class WorkItemListController : ControllerBase
{
    private readonly WorkItemManager _workItemManager;
    private readonly ILogger<WorkItemController> _logger;

    public WorkItemListController(WorkItemManager workItemManager, ILogger<WorkItemController> logger)
    {
        _workItemManager = workItemManager;
        _logger = logger;
    }

    [HttpPost("api/v1/projects/{projectCode}/workitems/query")]
    [ProducesResponseType(typeof(WorkItemListApiResponse), 200)]
    [ProducesResponseType(typeof(WorkItemBadRequestApiResponse), 400)]
    public async Task<ActionResult> GetAllProjectWorkItems(string projectCode, [FromBody] WorkItemListApiRequest queryBody)
    {
        try
        {
            var query = queryBody.Query with
            {
                Clause = queryBody.Query.Clause.EnsureProjectCode(projectCode),
            };

            var queryErrors = await _workItemManager.DataProvider.ValidateQueryAsync(query);

            if (!queryErrors.Any())
            {
                var list = await _workItemManager.DataProvider.ListWorkItemsAsync(query);

                return list != null
                    ? Ok(new WorkItemListApiResponse(true, list, Array.Empty<QueryError>()))
                    : NotFound(new WorkItemApiResponse()
                    {
                        Success = false,
                        ProjectCode = projectCode,
                        WorkItemId = null,
                        WorkItem = null,
                    });
            }
            else
            {
                return BadRequest(new WorkItemListApiResponse(false, Array.Empty<WorkItem>(), queryErrors));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception in {nameof(WorkItemController)}.{nameof(GetAllProjectWorkItems)}");

            return BadRequest(new WorkItemBadRequestApiResponse()
            {
                Success = false,
                ProjectCode = projectCode,
                WorkItemId = null,
            });
        }

    }
}
