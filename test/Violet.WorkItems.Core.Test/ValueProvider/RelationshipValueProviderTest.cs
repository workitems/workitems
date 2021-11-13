using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types.CommonSdlc;
using Violet.WorkItems.Validation;
using Xunit;

namespace Violet.WorkItems.ValueProvider;

//TODO: Test empty field (CommonSdlc has mandatory validator applied)

public class RelationshipValueProviderTest
{
    [Fact]
    public async Task RelationshipValueProvider_ValueExists_Success()
    {
        // arrange
        var manager = BuildManager();
        var epic = await manager.CreateTemplateAsync("Foo", "Epic");
        epic["Title"].Value = "Epic1";
        epic["State"].Value = "Open";
        var result1 = await manager.CreateAsync("Foo", "Epic", epic.Properties);
        Assert.True(result1.Success);

        var feature = await manager.CreateTemplateAsync("Foo", "Feature");
        feature["Title"].Value = "Feature1";
        feature["Epic"].Value = RelationshipValueProvider.EncodeRelationship("realize", "Foo", result1.Id);
        feature["State"].Value = "Open";
        feature["AcceptanceCriteria"].Value = "Should be featuritis";
        var result = await manager.CreateAsync("Foo", "Feature", feature.Properties);

        Assert.True(result.Success);
        Assert.Collection(result.Errors);
    }

    [Fact]
    public async Task RelationshipValueProvider_ValueExists_WrongId()
    {
        // arrange
        var manager = BuildManager();
        var epic = await manager.CreateTemplateAsync("Foo", "Epic");
        epic["Title"].Value = "Epic1";
        epic["State"].Value = "Open";
        var result1 = await manager.CreateAsync("Foo", "Epic", epic.Properties);
        Assert.True(result1.Success);

        var feature = await manager.CreateTemplateAsync("Foo", "Feature");
        feature["Title"].Value = "Feature1";
        feature["Epic"].Value = RelationshipValueProvider.EncodeRelationship("realize", "Foo", "999");
        feature["State"].Value = "Open";
        feature["AcceptanceCriteria"].Value = "Should be featuritis";
        var result = await manager.CreateAsync("Foo", "Feature", feature.Properties);

        Assert.False(result.Success);
        Assert.Collection(result.Errors,
            e =>
            {
                Assert.Equal(nameof(ValueProviderValidator), e.Source);
            });
    }

    [Fact]
    public async Task RelationshipValueProvider_ValueExists_InvalidReference()
    {
        // arrange
        var manager = BuildManager();
        var epic = await manager.CreateTemplateAsync("Foo", "Epic");
        epic["Title"].Value = "Epic1";
        epic["State"].Value = "Open";
        var result1 = await manager.CreateAsync("Foo", "Epic", epic.Properties);
        Assert.True(result1.Success);

        var feature = await manager.CreateTemplateAsync("Foo", "Feature");
        feature["Title"].Value = "Feature1";
        feature["Epic"].Value = "adsfasdfasf";
        feature["State"].Value = "Open";
        feature["AcceptanceCriteria"].Value = "Should be featuritis";
        var result = await manager.CreateAsync("Foo", "Feature", feature.Properties);

        Assert.False(result.Success);
        Assert.Collection(result.Errors,
            e =>
            {
                Assert.Equal(nameof(ValueProviderValidator), e.Source);
            });
    }

    private static WorkItemManager BuildManager()
    {
        return new WorkItemManager(new InMemoryDataProvider(), new CommonSdlcDescriptorProvider());
    }
}
