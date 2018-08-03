using Elementary.Compare.ReflectedHierarchy;
using System;
using Xunit;

namespace Elementary.Compare.Test.ReflectedHierarchy
{
    public class ReflectedHierarchyPropertyNodeTest
    {
        [Fact]
        public void PropertyNode_returns_ref_type_as_value()
        {
            // ARRANGE

            var value = new { data = new { } };

            // ACT

            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode(nameof(value.data));

            // ASSERT

            Assert.IsType<ReflectedHierarchyPropertyNode>(node);
            Assert.Equal(nameof(value.data), node.Id);
            Assert.False(node.HasChildNodes);
            Assert.Equal(value.data, node.TryGetValue<object>().Item2);
            Assert.Empty(node.ChildNodes);
        }

        [Fact]
        public void PropertyNode_returns_scalar_type_as_value()
        {
            // ARRANGE

            var value = new { data = DateTime.Now };

            // ACT

            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode(nameof(value.data));

            // ASSERT

            Assert.IsType<ReflectedHierarchyPropertyNode>(node);
            Assert.Equal(nameof(value.data), node.Id);
            Assert.False(node.HasChildNodes);
            Assert.Equal(value.data, node.TryGetValue<DateTime>().Item2);
            Assert.Empty(node.ChildNodes);
        }
    }
}