using System;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Violet.WorkItems.Validation;
using Xunit;

namespace Violet.WorkItems.ValueProvider
{
    public class EnumValueProviderTest
    {
        [Fact]
        public async Task EnumValueProvider_Validate_PropertyInValues()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "a"),
                new Property("B", "String", "d"),
                new Property("C", "String", "c"),
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
        public async Task EnumValueProvider_Validate_PropertyNotInValues()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "a"),
                new Property("B", "String", "b"),
                new Property("C", "String", "c"),
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
                    Assert.Equal(nameof(ValueProviderValidator), em.Source);
                    Assert.Equal(string.Empty, em.ErrorCode);
                    Assert.Equal("FOO", em.ProjectCode);
                    Assert.Equal("1", em.Id);
                    Assert.Equal("B", em.Property);
                }
            );
        }

        [Fact]
        public async Task EnumValueProvider_Validate_MultipleValuesAllValid()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "a"),
                new Property("B", "String", "d"),
                new Property("C", "String", "d,c"),
            };

            // act
            var result = await manager.CreateAsync("FOO", "BAR", properties);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.CreatedWorkItem);
            Assert.Collection(result.Errors);
        }

        [Fact]
        public async Task EnumValueProvider_Validate_IgnoreNullValue()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "a"),
                new Property("B", "String", "d"),
                new Property("C", "String", ""),
            };

            // act
            var result = await manager.CreateAsync("FOO", "BAR", properties);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.CreatedWorkItem);
            Assert.Collection(result.Errors);
        }

        [Fact]
        public async Task EnumValueProvider_Validate_MultipleValuesOneInvalid()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "a"),
                new Property("B", "String", "d"),
                new Property("C", "String", "b,d"),
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
                    Assert.Equal(nameof(ValueProviderValidator), em.Source);
                    Assert.Equal(string.Empty, em.ErrorCode);
                    Assert.Equal("FOO", em.ProjectCode);
                    Assert.Equal("1", em.Id);
                    Assert.Equal("C", em.Property);
                }
            );
        }


        private static WorkItemManager BuildManager()
        {
            return new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
                new WorkItemDescriptor("BAR", new PropertyDescriptor[] {
                    new PropertyDescriptor("A", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("B", "String", valueProvider: new EnumValueProviderDescriptor(new EnumValue("d", "D"), new EnumValue("c", "C"))),

                    new PropertyDescriptor("C", "String", propertyType: PropertyType.MultipleValueFromProvider, valueProvider: new EnumValueProviderDescriptor(new EnumValue("d", "D"), new EnumValue("c", "C"))),
                })
            ));
        }
    }
}