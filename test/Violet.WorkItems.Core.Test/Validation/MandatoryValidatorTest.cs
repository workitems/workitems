using System;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Violet.WorkItems.Types.CommonSdlc;
using Xunit;

namespace Violet.WorkItems.Validation;

public class MandatoryValidatorTest
{
    [Fact]
    public async Task MandatoryValidator_Validate_Success()
    {
        // arrange
        WorkItemManager manager = BuildManager();

        var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", string.Empty), // as the template would create it
            };

        // act
        var result = await manager.CreateAsync("FOO", "BAR", properties);

        // assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.ChangedWorkItem);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task MandatoryValidator_Validate_NoSideEffect()
    {
        // arrange
        WorkItemManager manager = BuildManager();

        var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", ""),
            };

        // act
        var result = await manager.CreateAsync("FOO", "BAR", properties);

        // assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.ChangedWorkItem);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task MandatoryValidator_Validate_Error()
    {
        // arrange
        WorkItemManager manager = BuildManager();

        var properties = new Property[] {
                new Property("A", "String", ""),
                new Property("B", "String", "bb"),
            };

        // act
        var result = await manager.CreateAsync("FOO", "BAR", properties);

        // assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.ChangedWorkItem);
        Assert.Collection(result.Errors,
            em =>
            {
                Assert.Equal(nameof(MandatoryValidator), em.Source);
                Assert.Equal(string.Empty, em.ErrorCode);
                Assert.Equal("FOO", em.ProjectCode);
                Assert.Equal("1", em.Id);
                Assert.Equal("A", em.Property);
            }
        );
    }

    private static WorkItemManager BuildManager()
    {
        return new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
            WorkItemDescriptor.Create("BAR", "BAR", new PropertyDescriptor[] {
                    PropertyDescriptor.Create("A", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    PropertyDescriptor.Create("B", "String"),
            })
        ));
    }
}
