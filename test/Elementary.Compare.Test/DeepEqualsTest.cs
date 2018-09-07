using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Elementary.Compare.Test
{
    public class DeepEqualsTest
    {
        public class Changeable<T>
        {
            public T Data { get; set; }
        }

        [Fact]
        public void Instances_are_equal()
        {
            // ARRANGE

            var left = new
            {
                a = "a",
                b = 1,
                c = new[] { 1, 2 },
                d = new List<int> { 1, 2 },
                e = new List<int> { 1, 2 },
                f = new List<int>()
            };

            var right = new
            {
                a = "a",
                b = 1,
                c = new[] { 1, 2 },
                d = new List<int> { 1, 2 },
                e = new[] { 1, 2 },
                f = new List<int>()
            };

            // ACT

            var result = left.DeepEquals(right);

            // ASSERT

            Assert.True(result);
        }

        [Fact]
        public void Instances_are_equal_for_same_instances()
        {
            // ARRANGE

            var left = new
            {
                a = "a"
            };

            // ACT

            var result = left.DeepEquals(left);

            // ASSERT

            Assert.True(result);
        }

        [Fact]
        public void Instances_not_equal_on_additional_property()
        {
            // ARRANGE

            var left = new
            {
                a = "a",
                b = "b"
            };

            var right = new
            {
                a = "a"
            };

            // ACT

            var result1 = left.DeepEquals(right);
            var result2 = right.DeepEquals(left);

            // ASSERT

            Assert.False(result1);
            Assert.False(result2);
        }

        [Fact]
        public void Instances_not_equal_on_different_pathes()
        {
            // ARRANGE

            var left = new
            {
                b = "a",
            };

            var right = new
            {
                a = "a"
            };

            // ACT

            var result = left.DeepEquals(right);

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void Instances_not_equal_on_different_types()
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

            var result = left.DeepEquals(right);

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void Instances_not_equal_on_different_values()
        {
            // ARRANGE

            var left = new
            {
                a = "a",
            };

            var right = new
            {
                a = "b"
            };

            // ACT

            var result = left.DeepEquals(right);

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void Checkpoint_not_equal_after_array_item_has_changed()
        {
            // ARRANGE

            var left = new Changeable<string[]>
            {
                Data = new[] { "a" }
            };

            var checkpoint = left.Flatten().ToArray();

            left.Data[0] = "b";

            // ACT

            var result = left.DeepCompare(checkpoint);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal(left.PropertyPath(l => l.Data[0]).ToString(), result.Different.Values.Single());
        }

        [Fact]
        public void Checkpoint_not_equal_after_array_item_was_removed()
        {
            // ARRANGE

            var left = new Changeable<string[]>
            {
                Data = new[] { "a" }
            };

            var checkpoint = left.Flatten().ToArray();

            left.Data = new string[0];

            // ACT

            var result = left.DeepCompare(checkpoint);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal(left.PropertyPath(l => l.Data[0]).ToString(), result.Missing.Left.Single());
        }

        [Fact]
        public void Checkpoint_not_equal_after_array_item_was_added()
        {
            // ARRANGE

            var left = new Changeable<string[]>
            {
                Data = new[] { "a" }
            };

            var checkpoint = left.Flatten().ToArray();

            left.Data = new[] { "a", "b" };

            // ACT

            var result = left.DeepCompare(checkpoint);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal(left.PropertyPath(l => l.Data[1]).ToString(), result.Missing.Right.Single());
        }

        [Fact]
        public void Checkpoint_not_equal_after_value_has_changed()
        {
            // ARRANGE

            var left = new Changeable<string>
            {
                Data = "a"
            };

            var checkpoint = left.Flatten().ToArray();

            left.Data = "b";

            // ACT

            var result = left.DeepCompare(checkpoint);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal(left.PropertyPath(l => l.Data).ToString(), result.Different.Values.Single());
        }
    }
}