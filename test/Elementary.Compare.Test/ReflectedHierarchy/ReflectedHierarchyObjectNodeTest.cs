using Elementary.Compare.ReflectedHierarchy;
using System;
using Xunit;

namespace Elementary.Compare.Test.ReflectedHierarchy
{
    public class ReflectedHierarchyObjectNodeTest
    {
        [Fact]
        public void ObjectNode_returns_ref_type_as_value()
        {
            // ARRANGE

            var value = new[] { "str" };

            // ACT

            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode("0");

            // ASSERT

            Assert.IsType<ReflectedHierarchyObjectNode>(node);
            Assert.Equal("0", node.Id);
            Assert.False(node.HasChildNodes);
            Assert.Equal("str", node.TryGetValue<string>().Item2);
            Assert.Empty(node.ChildNodes);
        }

        [Fact]
        public void ObjectNode_returns_value_type_as_value()
        {
            // ARRANGE

            var value = new[] { DateTime.Now };

            // ACT

            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode("0");

            // ASSERT

            Assert.IsType<ReflectedHierarchyObjectNode>(node);
            Assert.Equal("0", node.Id);
            Assert.False(node.HasChildNodes);
            Assert.Equal(value[0], node.TryGetValue<DateTime>().Item2);
            Assert.Empty(node.ChildNodes);
        }

        [Fact]
        public void ObjectNode_returns_false_on_invalid_value_type()
        {
            // ARRANGE

            var value = new[] { DateTime.Now };
            var (_, node) = ReflectedHierarchyFactory.Create(value).TryGetChildNode("0");

            // ACT

            var (result, _) = node.TryGetValue<int>();

            // ASSERT

            Assert.IsType<ReflectedHierarchyObjectNode>(node);
            Assert.False(result);
        }

        [Fact]
        public void ObjectNode_ValueType_provides_properties_raw_type()
        {
            // ARRANGE

            var node = ReflectedHierarchyFactory.Create(1);

            // ACT

            var result = node.ValueType;

            // ASSERT

            Assert.IsType<ReflectedHierarchyObjectRootNode>(node);
            Assert.IsAssignableFrom<ReflectedHierarchyObjectNodeBase>(node);
            Assert.Equal(typeof(int), result);
        }
    }
}