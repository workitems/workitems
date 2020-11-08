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
    public class WorkItemDescriptorController : ControllerBase
    {
        private readonly WorkItemManager _workItemManager;
        private readonly ILogger<WorkItemDescriptorController> _logger;

        public WorkItemDescriptorController(WorkItemManager workItemManager, ILogger<WorkItemDescriptorController> logger)
        {
            _workItemManager = workItemManager;
            _logger = logger;
        }

        [HttpGet("api/v1/projects/{projectCode}/workitems/{workItemId}/descriptor")]
        public async Task<ActionResult> GetDescriptorForWorkItem(string projectCode, string workItemId)
            => InternalGetDescriptorForWorkItem(await _workItemManager.GetAsync(projectCode, workItemId));

        [HttpGet("api/v1/projects/{projectCode}/types/{workItemType}/descriptor")]
        public async Task<ActionResult> GetDescriptorForTemplate(string projectCode, string workItemType)
            => InternalGetDescriptorForWorkItem(await _workItemManager.CreateTemplateAsync(projectCode, workItemType));

        private ActionResult InternalGetDescriptorForWorkItem(WorkItem item)
        {
            var properties = _workItemManager.DescriptorManager.GetCurrentPropertyDescriptors(item);
            var commands = _workItemManager.DescriptorManager.GetCurrentCommands(item);

            return Ok(new WorkItemDescriptorApiResponse()
            {
                Success = true,
                ProjectCode = item.ProjectCode,
                WorkItemId = item.Id,
                Properties = properties,
                Commands = commands,
            });
        }
    }
}