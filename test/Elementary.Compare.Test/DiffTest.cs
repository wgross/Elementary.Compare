using System.Linq;
using Xunit;

namespace Elementary.Compare.Test
{
    public class DiffTest
    {
        public class Changeable<T>
        {
            public T Data { get; set; }
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
            // same instance sare identical. Ist leaves are traversed.

            Assert.True(result.AreEqual);
            Assert.Single(result.EqualValues);
        }

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
            // instances are structural indentical. They contain only one leaf

            Assert.True(result.AreEqual);
            Assert.Single(result.EqualValues);
            Assert.Equal("a", result.EqualValues.Single());
        }

        [Fact]
        public void Diff_instances_with_equal_checkpoint()
        {
            // ARRANGE

            var left = new
            {
                a = "a"
            };

            var checkpoint = left.Flatten().Build();

            // ACT

            var result = left.Diff(checkpoint);

            // ASSERT
            // instances is structurally equal to checkpoint

            Assert.True(result.AreEqual);
            Assert.Single(result.EqualValues);
            Assert.Equal("a", result.EqualValues.Single());
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
            // instances are structurally different. One leaf is equal

            Assert.False(result1.AreEqual);
            Assert.False(result2.AreEqual);
            Assert.Single(result1.Missing);
            Assert.Equal("b", result1.Missing.Right.Single());
            Assert.Single(result2.Missing);
            Assert.Equal("b", result2.Missing.Left.Single());
            Assert.Single(result1.EqualValues);
            Assert.Single(result2.EqualValues);
        }

        [Fact]
        public void Diff_finds_additional_property_casing()
        {
            // ARRANGE

            var obj1 = new
            {
                A = "a",
            };

            var obj2 = new
            {
                a = "a"
            };

            // ACT

            var result1 = obj1.Diff(obj2);
            var result2 = obj2.Diff(obj1);

            // ASSERT
            // instances are structurally different. One leaf is equal

            Assert.False(result1.AreEqual);
            Assert.False(result2.AreEqual);
            Assert.Equal(2, result1.Missing.Count());
            Assert.Equal("A", result1.Missing.Right.Single());
            Assert.Equal("a", result1.Missing.Left.Single());
            Assert.Equal(2, result2.Missing.Count());
            Assert.Equal("A", result2.Missing.Left.Single());
            Assert.Equal("a", result2.Missing.Right.Single());
            Assert.Empty(result1.EqualValues);
            Assert.Empty(result2.EqualValues);
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
            // exlcude property 'b' from comparision

            var result1 = obj1.Flatten().Exclude(o => o.b).Build().Diff(obj2);
            var result2 = obj2.Diff(obj1.Flatten().Exclude(o => o.b).Build());

            // ASSERT
            // comparison result is 'equal' if the different property is ignored.
            // only the singel equal peoperty is in the report

            Assert.True(result1.AreEqual);
            Assert.True(result2.AreEqual);
            Assert.Empty(result1.Missing);
            Assert.Empty(result2.Missing);
            Assert.Single(result1.EqualValues);
            Assert.Single(result2.EqualValues);
        }

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
        public void Diff_finds_different_property_values()
        {
            // ARRANGE

            var left = new
            {
                a = 1,
            };

            var right = new
            {
                a = 2
            };

            // ACT

            var result = left.Diff(right);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal("a", result.Different.Single());
            Assert.Equal("a", result.Different.Values.Single());
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
        public void Diff_finds_different_enumerable_items_from_checkpoint()
        {
            // ARRANGE

            var left = new
            {
                a = new[] { 1 },
            };

            var checkpoint = left.Flatten().Build();

            left.a[0] = 2;

            // ACT

            var result = left.Diff(checkpoint);

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
        public void Diff_finds_missing_enumerable_items_from_checkpoint()
        {
            // ARRANGE

            var left = new Changeable<string[]>
            {
                Data = new[] { "a" }
            };

            var checkpoint = left.Flatten().Build();

            left.Data = new string[0];

            // ACT

            var result = left.Diff(checkpoint);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal(PropertyPath.Make(left, l => l.Data[0]).ToString(), result.Missing.Left.Single());
        }

        [Fact]
        public void Diff_finds_added_enumerable_items_from_checkpoint()
        {
            // ARRANGE

            var left = new Changeable<string[]>
            {
                Data = new[] { "a" }
            };

            var checkpoint = left.Flatten().Build();

            left.Data = new[] { "a", "b" };

            // ACT

            var result = left.Diff(checkpoint);

            // ASSERT

            Assert.False(result.AreEqual);
            Assert.Equal(PropertyPath.Make(left, l => l.Data[1]).ToString(), result.Missing.Right.Single());
            result.Different.Values.Single()
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