using System.Linq;
using Xunit;

namespace Elementary.Compare.Test
{
    public class DeepCompareTest
    {
        [Fact]
        public void DeepCompare_equal_instances()
        {
            // ARRANGE

            var left = new
            {
                a = "a"
            };

            var right = new
            {
                a = "a"
            };

            // ACT

            var result = left.DeepCompare(right);

            // ASSERT

            Assert.True(result.AreEqual);
            Assert.Single(result.EqualValues);
            Assert.Equal("a", result.EqualValues.Single());
        }

        [Fact]
        public void DeepCompare_same_instance()
        {
            // ARRANGE

            var left = new
            {
                a = "a"
            };

            // ACT

            var result = left.DeepCompare(left);

            // ASSERT

            Assert.True(result.AreEqual);
            Assert.Single(result.EqualValues);
        }

        [Fact]
        public void DeepCompare_finds_additional_property()
        {
            // ARRANGE

            var obj1 = new
            {
                a = "a",
                b = "b"
            };

            var obj2 = new
            {
                a = "a"
            };

            // ACT

            var result1 = obj1.DeepCompare(obj2);
            var result2 = obj2.DeepCompare(obj1);

            // ASSERT

            Assert.False(result1.AreEqual);
            Assert.False(result2.AreEqual);
            Assert.Equal("b", result1.Missing.Right.Single());
            Assert.Equal("b", result2.Missing.Left.Single());
        }

        [Fact]
        public void DeepCompare_finds_different_property_types()
        {
            // ARRANGE

            var left = new
            {
                a = 1,
            };

            var right = new
            {
                a = "a"
            };

            // ACT

            var result = left.DeepCompare(right);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal("a", result.Different.Types.Single());
        }

        [Fact]
        public void DeepCompare_finds_different_enumerable_items()
        {
            // ARRANGE

            var left = new
            {
                a = new[] { 1 },
            };

            var right = new
            {
                a = new[] { 2 },
            };

            // ACT

            var result = left.DeepCompare(right);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal("a/0", result.Different.Values.Single());
        }

        [Fact]
        public void DeepCompare_finds_additional_enumerable_items()
        {
            // ARRANGE

            var left = new
            {
                a = new[] { 1 },
            };

            var right = new
            {
                a = new[] { 1, 2 },
            };

            // ACT

            var result1 = left.DeepCompare(right);
            var result2 = right.DeepCompare(left);

            // ASSERT

            Assert.False(result1.AreEqual);
            Assert.False(result2.AreEqual);
            Assert.Equal("a/1", result1.Missing.Left.Single());
            Assert.Equal("a/1", result2.Missing.Right.Single());
        }

        [Fact]
        public void DeepCompare_finds_enumerable_items_having_different_types()
        {
            // ARRANGE

            var left = new
            {
                a = new int[] { 1 },
            };

            var right = new
            {
                a = new long[] { 1 },
            };

            // ACT

            var result1 = left.DeepCompare(right);

            // ASSERT

            Assert.False(result1.AreEqual);
            Assert.Equal("a/0", result1.Different.Types.Single());
        }
    }
}