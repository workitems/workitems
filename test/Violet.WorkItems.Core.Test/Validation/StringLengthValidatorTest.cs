using System;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Violet.WorkItems.Types.CommonSdlc;
using Xunit;

namespace Violet.WorkItems.Validation;

public class StringLengthValidatorTest
{
    [Fact]
    public async Task StringLengthValidator_Validate_Success()
    {
        // arrange
        WorkItemManager manager = BuildManager();

        var properties = new Property[] {
                new Property("A", "String", "ABC"),
                new Property("B", "String", string.Empty), // as the template would create it
            };

        // act
        var result = await manager.CreateAsync("FOO", "BAR", properties);

        // assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.CreatedWorkItem);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task StringLengthValidator_Validate_NoSideEffect()
    {
        // arrange
        WorkItemManager manager = BuildManager();

        var properties = new Property[] {
                new Property("A", "String", "ABC"),
                new Property("B", "String", "ABCDEFGH"),
            };

        // act
        var result = await manager.CreateAsync("FOO", "BAR", properties);

        // assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.CreatedWorkItem);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("AB")]
    [InlineData("ABCDEFGH")]
    public async Task StringLengthValidator_Validate_ErrorTooLong(string data)
    {
        // arrange
        WorkItemManager manager = BuildManager();

        var properties = new Property[] {
                new Property("A", "String", data),
                new Property("B", "String", "bb"),
            };

        // act
        var result = await manager.CreateAsync("FOO", "BAR", properties);

        // assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.CreatedWorkItem);
        Assert.Collection(result.Errors,
            em =>
            {
                Assert.Equal(nameof(StringLengthValidator), em.Source);
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
            WorkItemDescriptor.Create("BAR", new PropertyDescriptor[] {
                    PropertyDescriptor.Create("A", "String", validators: new ValidatorDescriptor[] {
                        new StringLengthValidatorDescriptor(3, 5),
                    }),
                    PropertyDescriptor.Create("B", "String"),
            })
        ));
    }
}
