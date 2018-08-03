using Elementary.Compare.ReflectedHierarchy;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Elementary.Compare.Test.ReflectedHierarchy
{
    public class ReflectedHierarchyEnumerableRootNodeTest
    {
        [Fact]
        public void EnumerableRootNode_returns_array_as_value()
        {
            // ARRANGE

            var value = new[] { 1 };

            // ACT

            var node = ReflectedHierarchyFactory.Create(value);

            // ASSERT

            Assert.IsType<ReflectedHierarchyEnumerableRootNode>(node);
            Assert.Empty(node.Id);
            Assert.True(node.HasChildNodes);
            Assert.Equal(value, node.TryGetValue<int[]>().Item2);
            Assert.Single(node.ChildNodes);
            Assert.Equal(1, node.ChildNodes.Single().TryGetValue<int>().Item2);
            Assert.Equal(1, node.TryGetChildNode("0").Item2.TryGetValue<int>().Item2);
        }

        [Fact]
        public void EnumerableRootNode_returns_list_as_value()
        {
            // ARRANGE

            var value = new List<int> { 1 };

            // ACT

            var node = ReflectedHierarchyFactory.Create(value);

            // ASSERT

            Assert.IsType<ReflectedHierarchyEnumerableRootNode>(node);
            Assert.Empty(node.Id);
            Assert.True(node.HasChildNodes);
            Assert.Equal(value, node.TryGetValue<List<int>>().Item2);
            Assert.Single(node.ChildNodes);
            Assert.Equal(1, node.ChildNodes.Single().TryGetValue<int>().Item2);
            Assert.Equal(1, node.TryGetChildNode("0").Item2.TryGetValue<int>().Item2);
        }

        [Fact]
        public void EnumerableRootNode_returns_false_on_index_out_of_range()
        {
            // ARRANGE

            var value = new List<int> { 1 };
            var node = ReflectedHierarchyFactory.Create(value);

            // ACT

            var (result, _) = node.TryGetChildNode("1");

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void EnumerableRootNode_returns_false_on_invalid_index()
        {
            // ARRANGE

            var value = new List<int> { 1 };
            var node = ReflectedHierarchyFactory.Create(value);


            // ACT

            var (result, _) = node.TryGetChildNode("not int");

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void EnumerableRootNode_returns_false_on_invalid_value_type()
        {
            // ARRANGE

            var value = new List<int> { 1 };
            var node = ReflectedHierarchyFactory.Create(value);


            // ACT

            var (result, _) = node.TryGetValue<int[]>();

            // ASSERT

            Assert.False(result);
        }
    }
}