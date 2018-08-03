using Elementary.Compare.ReflectedHierarchy;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Elementary.Compare.Test.ReflectedHierarchy
{
    public class ReflectedHierarchyEnumerableNodeTest
    {
        [Fact]
        public void EnumerableNode_returns_array_as_value()
        {
            // ARRANGE

            var value = new { data = new[] { 1 } };

            // ACT

            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode(nameof(value.data));

            // ASSERT

            Assert.IsType<ReflectedHierarchyEnumerableNode>(node);
            Assert.Equal(nameof(value.data), node.Id);
            Assert.True(node.HasChildNodes);
            Assert.Equal(value.data, node.TryGetValue<int[]>().Item2);
            Assert.Single(node.ChildNodes);
            Assert.Equal(1, node.ChildNodes.Single().TryGetValue<int>().Item2);
            Assert.Equal(1, node.TryGetChildNode("0").Item2.TryGetValue<int>().Item2);
        }

        [Fact]
        public void EnumerableNode_returns_list_as_value()
        {
            // ARRANGE

            var value = new { data = new List<int> { 1 } };

            // ACT

            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode(nameof(value.data));

            // ASSERT

            Assert.IsType<ReflectedHierarchyEnumerableNode>(node);
            Assert.Equal(nameof(value.data), node.Id);
            Assert.True(node.HasChildNodes);
            Assert.Equal(value.data, node.TryGetValue<List<int>>().Item2);
            Assert.Single(node.ChildNodes);
            Assert.Equal(1, node.ChildNodes.Single().TryGetValue<int>().Item2);
            Assert.Equal(1, node.TryGetChildNode("0").Item2.TryGetValue<int>().Item2);
        }
    }
}