using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.Types.CommonSdlc
{
    public class CommonSdlcDescriptorProvider : InMemoryDescriptorProvider, IDescriptorProvider
    {
        public CommonSdlcDescriptorProvider()
            : base(
                BugTaskModel.Bug,
                BugTaskModel.Task,
                EpicFeatureUserStoryModel.Epic,
                EpicFeatureUserStoryModel.Feature,
                EpicFeatureUserStoryModel.UserStory
            )
        { }
    }
}