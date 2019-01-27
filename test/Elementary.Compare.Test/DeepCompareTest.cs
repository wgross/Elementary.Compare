using System.Linq;
using Xunit;

namespace Elementary.Compare.Test
{
    public class DiffTest
    {
        [Fact]
        public void Diff_equal_instances()
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

            var result = left.Diff(right);

            // ASSERT

            Assert.True(result.AreEqual);
            Assert.Single(result.EqualValues);
            Assert.Equal("a", result.EqualValues.Single());
        }

        [Fact]
        public void Diff_same_instance()
        {
            // ARRANGE

            var left = new
            {
                a = "a"
            };

            // ACT

            var result = left.Diff(left);

            // ASSERT

            Assert.True(result.AreEqual);
            Assert.Single(result.EqualValues);
        }

        [Fact]
        public void Diff_finds_additional_property()
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

            var result1 = obj1.Diff(obj2);
            var result2 = obj2.Diff(obj1);

            // ASSERT

            Assert.False(result1.AreEqual);
            Assert.False(result2.AreEqual);
            Assert.Single(result1.Missing);
            Assert.Equal("b", result1.Missing.Right.Single());
            Assert.Single(result2.Missing);
            Assert.Equal("b", result2.Missing.Left.Single());
        }

        [Fact]
        public void Diff_ignores_additional_property_if_excluded()
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

            var result1 = obj1.Flatten().Exclude(o => o.b).Diff(obj2);
            var result2 = obj2.Diff(obj1.Flatten().Exclude(o => o.b));

            // ASSERT

            Assert.False(result1.AreEqual);
            Assert.False(result2.AreEqual);
            Assert.Single(result1.Missing);
            Assert.Equal("b", result1.Missing.Right.Single());
            Assert.Single(result2.Missing);
            Assert.Equal("b", result2.Missing.Left.Single());
        }

        //[Fact]
        //public void Diff_excludes_additional_property()
        //{
        //    // ARRANGE

        //    var obj1 = new
        //    {
        //        a = "a",
        //        b = "b"
        //    };

        //    var obj2 = new
        //    {
        //        a = "a"
        //    };

        //    // ACT

        //    // var result1 = obj1.Diff(obj2);
        //    var result2 = ObjectExtensions.Diff(obj1, obj2.Diff(obj1.Flatten().Exclude(o => o.b)));

        //    // ASSERT

        //    //Assert.True(result1.AreEqual);
        //    Assert.True(result2.AreEqual);
        //}

        [Fact]
        public void Diff_finds_different_property_types()
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

            var result = left.Diff(right);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal("a", result.Different.Single());
            Assert.Equal("a", result.Different.Types.Single());
        }

        [Fact]
        public void Diff_finds_different_enumerable_items()
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

            var result = left.Diff(right);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal("a/0", result.Different.Single());
            Assert.Equal("a/0", result.Different.Values.Single());
        }

        [Fact]
        public void Diff_finds_additional_enumerable_items()
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

            var result1 = left.Diff(right);
            var result2 = right.Diff(left);

            // ASSERT

            Assert.False(result1.AreEqual);
            Assert.Single(result1.Missing);
            Assert.Equal("a/1", result1.Missing.Left.Single());

            Assert.False(result2.AreEqual);
            Assert.Single(result2.Missing);
            Assert.Equal("a/1", result2.Missing.Right.Single());
        }

        [Fact]
        public void Diff_finds_enumerable_items_having_different_types()
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

            var result = left.Diff(right);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Single(result.Different);
            Assert.Equal("a/0", result.Different.Types.Single());
        }
    }
}