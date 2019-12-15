using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Types.CommonSdlc
{
    public class CommonSdlcDescriptorProvider : IDescriptorProvider
    {
        public Task<IEnumerable<WorkItemDescriptor>> GetAllDescriptorsAsync()
        {
            IEnumerable<WorkItemDescriptor> types = new WorkItemDescriptor[] {
                BugTaskModel.Bug,
                BugTaskModel.Task,
                EpicFeatureUserStoryModel.Epic,
                EpicFeatureUserStoryModel.Feature,
                EpicFeatureUserStoryModel.UserStory,
             };

            return Task.FromResult(types);
        }
    }
}