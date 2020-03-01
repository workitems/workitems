using System;
using Xunit;

namespace Violet.WorkItems.ValueTypes
{
    public class CommonTypeExtensionsTest
    {
        [Fact]
        public void CommonTypeExtensions_Value_DataTypeMismatchSet()
        {
            // arrange
            long x = 2345;
            var property = new Property("FOO", nameof(Int32), string.Empty);

            // act & assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                property.Value(x);
            });
        }

        [Fact]
        public void CommonTypeExtensions_Value_DataTypeMismatchGet()
        {
            // arrange
            var property = new Property("FOO", nameof(Int32), string.Empty);

            // act & assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                property.Value(out long x);
            });
        }


        [Fact]
        public void CommonTypeExtensions_Value_Int32_DesierializeNullFails()
        {
            // arrange
            var property = new Property("FOO", nameof(Int32), string.Empty);

            // act & assert
            Assert.Throws<ArgumentException>(() =>
            {
                property.Value(out int result);
            });
        }
        [Fact]
        public void CommonTypeExtensions_Value_Int32_NullableAllowed()
        {
            // arrange
            var property = new Property("FOO", "Int32", string.Empty);

            // act
            property.NullableValue(out int? result);

            // assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1234)]
        public void CommonTypeExtensions_Value_Int32_NullableRoundtrip(int? x)
        {
            // arrange
            var property = new Property("FOO", "Int32", "1234");

            // act
            property.NullableValue(x);
            property.NullableValue(out int? result);

            // assert
            Assert.Equal(x, result);
        }

        [Fact]
        public void CommonTypeExtensions_Value_Int32_Roundtrip()
        {
            // arrange
            int x = 2345;
            var property = new Property("FOO", nameof(Int32), string.Empty);

            // act
            property.Value(x);
            property.Value(out int result);

            // assert
            Assert.Equal(x, result);
        }

        [Fact]
        public void CommonTypeExtensions_Value_Int64_Roundtrip()
        {
            // arrange
            long x = 2345;
            var property = new Property("FOO", nameof(Int64), string.Empty);

            // act
            property.Value(x);
            property.Value(out long result);

            // assert
            Assert.Equal(x, result);
        }

        [Fact]
        public void CommonTypeExtensions_Value_Single_Roundtrip()
        {
            // arrange
            float x = 3.14f;
            var property = new Property("FOO", nameof(Single), string.Empty);

            // act
            property.Value(x);
            property.Value(out float result);

            // assert
            Assert.Equal(x, result);
        }

        [Fact]
        public void CommonTypeExtensions_Value_Double_Roundtrip()
        {
            // arrange
            double x = Math.PI;
            var property = new Property("FOO", nameof(Double), string.Empty);

            // act
            property.Value(x);
            property.Value(out double result);

            // assert
            Assert.Equal(x, result);
        }

        [Fact]
        public void CommonTypeExtensions_Value_Decimal_Roundtrip()
        {
            // arrange
            decimal x = 3.14M;
            var property = new Property("FOO", nameof(Decimal), string.Empty);

            // act
            property.Value(x);
            property.Value(out decimal result);

            // assert
            Assert.Equal(x, result);
        }

        [Fact]
        public void CommonTypeExtensions_Value_DateTimeOffset_Roundtrip()
        {
            // arrange
            DateTimeOffset x = DateTimeOffset.Parse("2019-12-31T14:56:12.123+02:00");
            var property = new Property("FOO", nameof(DateTimeOffset), string.Empty);

            // act
            property.Value(x);
            property.Value(out DateTimeOffset result);

            // assert
            Assert.Equal(x, result);
            Assert.Equal("2019-12-31T14:56:12.123+02:00", property.Value);
        }
        [Fact]
        public void CommonTypeExtensions_Values_SuccessSetWithParams()
        {
            // arrange
            var property = new Property("A", "Int32", "");
            // act
            property.Values(1, 2, 3, 4);
            // assert
            Assert.Equal("1,2,3,4", property.Value);
        }
        [Fact]
        public void CommonTypeExtensions_Values_SuccessSetWithArray()
        {
            // arrange
            var property = new Property("A", "Int32", "");
            // act
            property.Values(new int[] { 1, 2, 3, 4 });
            // assert
            Assert.Equal("1,2,3,4", property.Value);
        }
        [Fact]
        public void CommonTypeExtensions_Values_SuccessGet()
        {
            // arrange
            var property = new Property("A", "Int32", "1,2,3,4");
            // act
            property.Values(out int[] result);
            // assert
            Assert.Collection(result,
                i => { Assert.Equal(1, i); },
                i => { Assert.Equal(2, i); },
                i => { Assert.Equal(3, i); },
                i => { Assert.Equal(4, i); }
            );
        }
        [Fact]
        public void CommonTypeExtensions_Values_InvalidData()
        {
            // arrange
            var property = new Property("A", "Int32", "1,2,A,4");
            // act & assert
            Assert.Throws<ArgumentException>(() =>
            {
                property.Values(out int[] result);
            });
        }
    }
}