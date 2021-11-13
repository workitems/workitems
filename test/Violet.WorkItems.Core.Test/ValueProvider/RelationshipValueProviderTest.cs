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
        var epic = (await manager.CreateTemplateAsync("Foo", "Epic"))
            .WithValue("Title", "Epic1")
            .WithValue("State", "Open");

        var result1 = await manager.CreateAsync("Foo", "Epic", epic.Properties);
        Assert.True(result1.Success);

        var feature = (await manager.CreateTemplateAsync("Foo", "Feature"))
            .WithValue("Title", "Feature1")
            .WithValue("Epic", RelationshipValueProvider.EncodeRelationship("realize", "Foo", result1.Id))
            .WithValue("State", "Open")
            .WithValue("AcceptanceCriteria", "Should be featuritis");
        var result = await manager.CreateAsync("Foo", "Feature", feature.Properties);

        Assert.True(result.Success);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task RelationshipValueProvider_ValueExists_WrongId()
    {
        // arrange
        var manager = BuildManager();
        var epic = (await manager.CreateTemplateAsync("Foo", "Epic"))
            .WithValue("Title", "Epic1")
            .WithValue("State", "Open");
        var result1 = await manager.CreateAsync("Foo", "Epic", epic.Properties);
        Assert.True(result1.Success);

        var feature = (await manager.CreateTemplateAsync("Foo", "Feature"))
            .WithValue("Title", "Feature1")
            .WithValue("Epic", RelationshipValueProvider.EncodeRelationship("realize", "Foo", "999"))
            .WithValue("State", "Open")
            .WithValue("AcceptanceCriteria", "Should be featuritis");
        var result = await manager.CreateAsync("Foo", "Feature", feature.Properties);

        Assert.False(result.Success);
        Assert.Collection(result.Errors,
            e => Assert.Equal(nameof(ValueProviderValidator), e.Source));
    }

    [Fact]
    public async Task RelationshipValueProvider_ValueExists_InvalidReference()
    {
        // arrange
        var manager = BuildManager();
        var epic = (await manager.CreateTemplateAsync("Foo", "Epic"))
            .WithValue("Title", "Epic1")
            .WithValue("State", "Open");
        var result1 = await manager.CreateAsync("Foo", "Epic", epic.Properties);
        Assert.True(result1.Success);

        var feature = (await manager.CreateTemplateAsync("Foo", "Feature"))
            .WithValue("Title", "Feature1")
            .WithValue("Epic", "adsfasdfasf")
            .WithValue("State", "Open")
            .WithValue("AcceptanceCriteria", "Should be featuritis");
        var result = await manager.CreateAsync("Foo", "Feature", feature.Properties);

        Assert.False(result.Success);
        Assert.Collection(result.Errors,
            e => Assert.Equal(nameof(ValueProviderValidator), e.Source));
    }

    private static WorkItemManager BuildManager()
    {
        return new WorkItemManager(new InMemoryDataProvider(), new CommonSdlcDescriptorProvider());
    }
}
