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

        [Fact]
        public void PropertyNode_returns_childNode()
        {
            // ARRANGE

            var value = new { data = new { child = DateTime.Now } };
            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode(nameof(value.data));

            // ACT

            var (result, child) = node.TryGetChildNode("child");

            // ASSERT

            Assert.True(result);
            Assert.Equal("child", child.Id);
        }

        [Fact]
        public void PropertyNode_returns_false_on_unknown_child()
        {
            // ARRANGE

            var value = new { data = DateTime.Now };
            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode(nameof(value.data));

            // ACT

            var (result, _) = node.TryGetChildNode("child");

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void PropertyNode_returns_false_on_invalid_value_type()
        {
            // ARRANGE

            var value = new { data = DateTime.Now };
            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode(nameof(value.data));

            // ACT

            var (result, _) = node.TryGetValue<int>();

            // ASSERT

            Assert.False(result);
        }

        [Fact]
        public void PropertyNode_returns_false_on_invalid_property_type()
        {
            // ARRANGE
            // witout a value the static property type has to decide

            var value = new { data = (DateTime?)null };
            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode(nameof(value.data));

            // ACT

            var (result, _) = node.TryGetValue<int>();

            // ASSERT

            Assert.False(result);
        }
    }
}