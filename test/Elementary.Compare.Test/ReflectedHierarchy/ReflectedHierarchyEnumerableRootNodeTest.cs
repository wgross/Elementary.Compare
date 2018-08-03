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
    }
}