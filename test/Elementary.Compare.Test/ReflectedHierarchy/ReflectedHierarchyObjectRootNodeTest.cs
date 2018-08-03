using Elementary.Compare.ReflectedHierarchy;
using System;
using Xunit;

namespace Elementary.Compare.Test.ReflectedHierarchy
{
    public class ReflectedHierarchyObjectRootNodeTest
    {
        [Fact]
        public void RootObjectNode_returns_ref_type_as_value()
        {
            // ARRANGE

            var value = new { };

            // ACT

            var node = ReflectedHierarchyFactory.Create(value);

            // ASSERT

            Assert.IsType<ReflectedHierarchyObjectRootNode>(node);
            Assert.Empty(node.Id);
            Assert.False(node.HasChildNodes);
            Assert.Equal(value, node.TryGetValue<object>().Item2);
            Assert.Empty(node.ChildNodes);
        }

        [Fact]
        public void RootObjectNode_returns_scalar_type_as_value()
        {
            // ARRANGE

            var value = DateTime.Now;

            // ACT

            var node = ReflectedHierarchyFactory.Create(value);

            // ASSERT

            Assert.IsType<ReflectedHierarchyObjectRootNode>(node);
            Assert.Empty(node.Id);
            Assert.False(node.HasChildNodes);
            Assert.Equal(value, node.TryGetValue<DateTime>().Item2);
            Assert.Empty(node.ChildNodes);
        }
    }
}