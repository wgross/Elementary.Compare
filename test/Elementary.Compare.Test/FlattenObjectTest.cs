using System;
using System.Linq;
using Xunit;

namespace Elementary.Compare.Test
{
    public class FlattenObjectTest
    {
        private class CycleTestClass
        {
            public CycleTestClass child { get; set; }
        }

        private class CycleTestClass2
        {
            public int a { get; set; } = 1;
            public CycleTestClass2 child => new CycleTestClass2 { a = this.a + 1 };
        }

        private class CycleTestClass3
        {
            public int a { get; set; } = 1;
            public CycleTestClass3 parent { get; set; }
            public CycleTestClass3 child { get; set; }
        }

        private class CycleTestClass4
        {
            public int a { get; set; }

            public CycleTestClass4 child { get; set; }
        }

        [Fact]
        public void Create_flattened_collection_of_properties()
        {
            // ARRANGE

            var obj = new
            {
                a = 1,
                b = "b"
            };

            // ACT

            var result = obj.Flatten().Build();

            // ASSERT

            var resultCollection = result.ToArray();

            Assert.Equal(2, resultCollection.Count());
            Assert.Equal(new[] { "a", "b" }, resultCollection.Select(kv => kv.Key));
            Assert.Equal(new object[] { 1, "b" }, resultCollection.Select(kv => kv.Value));
        }

        [Fact]
        public void Create_flattened_collection_without_descending_into_string()
        {
            // ARRANGE

            var obj = new
            {
                b = "b"
            };

            // ACT

            var result = obj.Flatten().Build();

            // ASSERT

            Assert.Single(result);
            Assert.Equal("b", result.Single().Key);
            Assert.Equal(obj.b, result.Single().Value);
        }

        [Fact]
        public void Create_flattened_collection_without_descending_into_DateTime()
        {
            // ARRANGE

            var obj = new
            {
                a = DateTime.Now
            };

            // ACT

            var result = obj.Flatten().Build();

            // ASSERT

            Assert.Single(result);
            Assert.Equal("a", result.Single().Key);
            Assert.Equal(obj.a, result.Single().Value);
        }

        [Fact]
        public void Create_flattened_collection_without_inner_nodes()
        {
            // ARRANGE

            var obj = new
            {
                a = new
                {
                    b = DateTime.Now
                }
            };

            // ACT

            var result = obj.Flatten().Build();

            // ASSERT

            Assert.Single(result);
            Assert.Equal("a/b", result.Single().Key);
            Assert.Equal(obj.a.b, result.Single().Value);
        }

        [Fact]
        public void Create_flattened_collection_without_descending_into_excluded_property()
        {
            // ARRANGE

            var obj = new
            {
                b = new
                {
                    c = "skipped"
                },
                d = new
                {
                    e = 1 // <- this is the only leaf i'm expecting
                }
            };

            // ACT

            var result = obj.Flatten().Exclude(o => o.b).Build();

            // ASSERT

            Assert.Single(result);
            Assert.Equal("d/e", result.Single().Key);
            Assert.Equal(1, result.Single().Value);
        }

        [Fact]
        public void Create_flattened_collection_breaks_on_cycle_after_maxDepth()
        {
            // ARRANGE
            // create a cycle.

            var obj = new CycleTestClass
            {
                child = new CycleTestClass()
            };
            obj.child.child = obj;

            // ACT

            var result = obj.Flatten(maxDepth: 10).Build();

            // ASSERT

            Assert.Empty(result);
        }

        [Fact]
        public void Create_flattened_collection_from_cycle_contains_non_cyclic_leafs()
        {
            // ARRANGE
            // create a cycle.

            var obj = new CycleTestClass4
            {
                a = 1,
                child = new CycleTestClass4()
            };
            obj.child.child = obj;

            // ACT

            var result = obj.Flatten().Build();

            // ASSERT
            // cycle is discovered at second step. Two a's are included

            Assert.Equal(2, result.Count());
            Assert.Equal(new[] { "a", "child/a" }, result.Select(kv => kv.Key).ToArray());
        }

        [Fact]
        public void Create_flattened_collection_from_cycle_to_parent_contains_non_cyclic_leafs()
        {
            // ARRANGE
            // create a cycle.

            var obj = new CycleTestClass3
            {
                a = 1,
                child = new CycleTestClass3
                {
                    a = 2,
                }
            };
            obj.child.parent = obj;

            // ACT

            var result = obj.Flatten().Build();

            // ASSERT
            // cycle is discovered, the  parent of the root and the child of the child is added to the list
            // because their value is null and the are therefore leafs.

            Assert.Equal(4, result.Count());
            Assert.Equal(new[] { "a", "child/a", "child/child", "parent" }, result.Select(kv => kv.Key).ToArray());
        }

        [Fact]
        public void Create_flattened_collection_breaks_on_cycle_after_maxDepth_self_replicating()
        {
            // ARRANGE

            var obj = new CycleTestClass2();

            // ACT

            //var result = Assert.Throws<InvalidOperationException>(() => new Dictionary<string, object>(obj.Flatten(maxDepth: 10)));
            var result = obj.Flatten(maxDepth: 5).Build();

            // ASSERT
            // the cycle is no discovered, the traversal break after reachin max depth

            Assert.Equal(5, result.Count());
            Assert.Equal(new[] { "a", "child/a", "child/child/a", "child/child/child/a", "child/child/child/child/a" }, result.Select(kv => kv.Key).ToArray());
            Assert.Equal(new[] { 1, 2, 3, 4, 5, }, result.Select(kv => (int)kv.Value).ToArray());
        }
    }
}