using System;
using Xunit;

namespace Elementary.Compare.Test
{
    public class NoPropertyHasDefaultValueTest
    {
        [Fact]
        public void NoPropertyHasDefaultValue_accepts_filled_object()
        {
            // ARRANGE

            var obj = new
            {
                str = "str",
                number = 1,
                number2 = new Nullable<int>(1),
                array = new[] { DateTime.Now }
            };

            // ACT

            var result = obj.NoPropertyHasDefaultValue();

            // ASSERT

            Assert.True(result);
        }

        [Fact]
        public void NoPropertyHasDefaultValue_rejects_null_reference()
        {
            // ARRANGE

            var obj = new
            {
                str = (string)null
            };

            // ACT

            var result = obj.NoPropertyHasDefaultValue();

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void NoPropertyHasDefaultValue_rejects_null_value()
        {
            // ARRANGE

            var obj = new
            {
                i = default(int)
            };

            // ACT

            var result = obj.NoPropertyHasDefaultValue();

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void NoPropertyHasDefaultValue_rejects_null_nullable_value()
        {
            // ARRANGE

            var obj = new
            {
                i = default(int?)
            };

            // ACT

            var result = obj.NoPropertyHasDefaultValue();

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void NoPropertyHasDefaultValue_rejects_nullable_value_with_default_value()
        {
            // ARRANGE

            var obj = new
            {
                i = new Nullable<int>(default(int))
            };

            // ACT

            var result = obj.NoPropertyHasDefaultValue();

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void NoPropertyHasDefaultValue_rejects_empty_enumerable()
        {
            // ARRANGE

            var obj = new
            {
                i = new int[0]
            };

            // ACT

            var result = obj.NoPropertyHasDefaultValue();

            // ASSERT

            Assert.False(result);
        }
    }
}