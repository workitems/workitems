using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Violet.WorkItems.Service.Models;

namespace Violet.WorkItems.Service.Controllers
{
    [ApiController]
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
                var list = await _workItemManager.DataProvider.ListWorkItemsAsync(projectCode);

                if (list != null)
                {
                    return Ok(new WorkItemListApiResponse()
                    {
                        Success = true,
                        WorkItems = list,
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