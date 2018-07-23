using Elementary.Compare.ReflectedHierarchy;
using System;
using System.Collections.Generic;
using Xunit;

namespace Elementary.Compare.Test.ReflectedHierarchy
{
    public class FlattedObjectHierarchyNodeFactoryTest
    {
        [Fact]
        public void FlattedObjectNodeFactory_creates_enumerable_node_from_array()
        {
            // ARRANGE

            var factory = new ReflectedHierarchyNodeFactory();
            var obj = new
            {
                data = new[] { 1 }
            };

            // ACT

            var result = factory.Create(obj, obj.GetType().GetProperty(nameof(obj.data)));

            // ASSERT

            Assert.IsType<ReflectedHierarchyEnumerableNode>(result);
        }

        [Fact]
        public void FlattedObjectNodeFactory_creates_enumerable_node_from_list()
        {
            // ARRANGE

            var factory = new ReflectedHierarchyNodeFactory();
            var obj = new
            {
                data = new List<int> { 1 }
            };

            // ACT

            var result = factory.Create(obj, obj.GetType().GetProperty(nameof(obj.data)));

            // ASSERT

            Assert.IsType<ReflectedHierarchyEnumerableNode>(result);
        }

        [Fact]
        public void FlattedObjectNodeFactory_creates_property_node_from_string()
        {
            // ARRANGE

            var factory = new ReflectedHierarchyNodeFactory();
            var obj = new
            {
                data = "str"
            };

            // ACT

            var result = factory.Create(obj, obj.GetType().GetProperty(nameof(obj.data)));

            // ASSERT

            Assert.IsType<ReflectedHierarchyPropertyNode>(result);
        }

        [Fact]
        public void FlattedObjectNodeFactory_doesnt_create_child_nodes_for_string()
        {
            // ARRANGE

            var factory = new ReflectedHierarchyNodeFactory();
            var str = "string";

            // ACT
            // string properties are ignored

            var result = factory.Create(str, typeof(string).GetProperty(nameof(string.Length)));

            // ASSERT

            Assert.Null(result);
        }

        [Fact]
        public void FlattedObjectNodeFactory_doesnt_create_child_nodes_for_value_types()
        {
            // ARRANGE

            var factory = new ReflectedHierarchyNodeFactory();
            var date = DateTime.Now;

            // ACT

            var result = factory.Create(date, typeof(DateTime).GetProperty(nameof(DateTime.Second)));

            // ASSERT

            Assert.Null(result);
        }
    }
}