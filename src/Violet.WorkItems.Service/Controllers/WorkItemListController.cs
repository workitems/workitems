using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Violet.WorkItems.Service.Models;
using Violet.WorkItems.Provider;

namespace Violet.WorkItems.Service.Controllers
{
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

        [HttpGet("api/v1/projects/{projectCode}/workitems")]
        [ProducesResponseType(typeof(WorkItemListApiResponse), 200)]
        [ProducesResponseType(typeof(WorkItemBadRequestApiResponse), 400)]
        public async Task<ActionResult> GetAllProjectWorkItems(string projectCode)
        {
            try
            {
                var result = await _workItemManager.DataProvider.QueryWorkItemsAsync(new ListQuery(new ProjectCodeEqualityClause(projectCode))) as ListQueryResult;

                if (result != null)
                {
                    return Ok(new WorkItemListApiResponse()
                    {
                        Success = true,
                        WorkItems = result.WorkItems,
                    });
                }
                else
                {
                    return NotFound(new WorkItemApiResponse()
                    {
                        Success = false,
                        ProjectCode = projectCode,
                        WorkItemId = null,
                        WorkItem = null,
                    });
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
}