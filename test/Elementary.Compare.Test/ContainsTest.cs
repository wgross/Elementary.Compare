using System.Linq;
using Xunit;

namespace Elementary.Compare.Test
{
    public class ContainsTest
    {
        [Fact]
        public void Contains_accepts_same_instance()
        {
            // ARRANGE

            var instance = new
            {
                a = 1
            };

            // ACT

            var result = instance.Contains(instance, out var _);

            // ASSERT

            Assert.True(result);
        }

        [Fact]
        public void Contains_accepts_equal_instance()
        {
            // ARRANGE

            var left = new
            {
                a = 1
            };

            var right = new
            {
                a = 1
            };

            // ACT

            var result = left.Contains(right, out var _);

            // ASSERT

            Assert.True(result);
        }

        [Fact]
        public void Contains_rejects_missing_property()
        {
            // ARRANGE

            var left = new
            {
                a = 1,
                b = "b"
            };

            var right = new
            {
                a = 1
            };

            // ACT

            var result = left.Contains(right, out var missingRight);

            // ASSERT

            Assert.False(result);
            Assert.Equal("b", missingRight.Single());
        }
    }
}