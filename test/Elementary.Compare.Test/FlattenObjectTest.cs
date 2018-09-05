using System;
using System.Collections.Generic;
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
            public CycleTestClass2 child => new CycleTestClass2();
        }

        private class CycleTestClass3
        {
            public CycleTestClass3 Parent { get; set; }
            public CycleTestClass3 child { get; set; }
        }

        [Fact]
        public void Create_flat_map_of_properties()
        {
            // ARRANGE

            var obj = new
            {
                a = 1,
                b = "b"
            };

            // ACT

            var result = new Dictionary<string, object>(obj.Flatten());

            // ASSERT

            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey("a"));
            Assert.True(result.ContainsKey("b"));
        }

        [Fact]
        public void Create_flat_map_without_descending_into_string()
        {
            // ARRANGE

            var obj = new
            {
                b = "b"
            };

            // ACT

            var result = new Dictionary<string, object>(obj.Flatten());

            // ASSERT

            Assert.Single(result);
            Assert.True(result.ContainsKey("b"));
            Assert.IsType<string>(result.Single().Value);
        }

        [Fact]
        public void Create_flat_map_without_descending_into_DateTime()
        {
            // ARRANGE

            var obj = new
            {
                a = DateTime.Now
            };

            // ACT

            var result = new Dictionary<string, object>(obj.Flatten());

            // ASSERT

            Assert.Single(result);
            Assert.True(result.ContainsKey("a"));
            Assert.IsType<DateTime>(result.Single().Value);
        }

        [Fact]
        public void Create_flat_map_without_inner_nodes()
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

            var result = new Dictionary<string, object>(obj.Flatten());

            // ASSERT

            Assert.Single(result);
            Assert.True(result.ContainsKey("a/b"));
            Assert.IsType<DateTime>(result.Single().Value);
        }

        [Fact]
        public void Create_flat_map_breaks_on_cycle_after_maxDepth()
        {
            // ARRANGE

            var obj = new CycleTestClass
            {
                child = new CycleTestClass()
            };
            obj.child.child = obj;

            // ACT

            var result = Assert.Throws<InvalidOperationException>(() => new Dictionary<string, object>(obj.Flatten(maxDepth: 1)));

            // ASSERT

            Assert.Contains("maxDepth='1'", result.Message);
            Assert.Contains("path='child'", result.Message);
        }

        [Fact]
        public void Create_flat_map_breaks_on_cycle_after_maxDepth_self_replicating()
        {
            // ARRANGE

            var obj = new CycleTestClass2();

            // ACT

            var result = Assert.Throws<InvalidOperationException>(() => new Dictionary<string, object>(obj.Flatten(maxDepth: 10)));

            // ASSERT

            Assert.Contains("maxDepth='10'", result.Message);
            Assert.Contains("path='child/child/child/child/child/child/child/child/child/child'", result.Message);
        }

        [Fact]
        public void Create_flat_map_without_cycles()
        {
            // ARRANGE

            var obj1 = new CycleTestClass();
            var obj2 = new CycleTestClass { child = obj1 };

            // ACT

            var result = new Dictionary<string, object>(obj1.Flatten(maxDepth: 10));

            // ASSERT

            Assert.Single(result);
            Assert.Equal("child", result.Single().Key);
        }

        [Fact]
        public void Create_flat_map_breaks_on_cycle_after_maxDepth_self_replicating_2()
        {
            // ARRANGE

            var obj = new CycleTestClass3();
            obj.child = new CycleTestClass3 { Parent = obj };
            obj.child.child = new CycleTestClass3 { Parent = obj.child };

            // ACT

            var result = new Dictionary<string, object>(obj.Flatten(maxDepth: 10));

            // ASSERT

            Assert.Equal(3, result.Count);
        }
    }
}