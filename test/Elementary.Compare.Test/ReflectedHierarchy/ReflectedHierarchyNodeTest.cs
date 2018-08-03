using Elementary.Compare.ReflectedHierarchy;
using Elementary.Hierarchy;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Elementary.Compare.Test.ReflectedHierarchy
{
    public class ReflectedHierarchyNodeTest
    {
        private class ReadWritePropertyParent<T>
        {
            public T Property { get; set; }
        }

        #region Node calls factory for child node creation

        [Fact]
        public void Create_property_child_node_with_factory()
        {
            // ARRANGE

            var nodeValue = new { a = 1 };
            var factory = new Mock<IReflectedHierarchyNodeFactory>(MockBehavior.Strict);
            factory
                .Setup(f => f.Create(nodeValue, It.Is<PropertyInfo>(pi => pi.Name.Equals("a"))))
                .Returns((IReflectedHierarchyNode)null);

            var hierarchyNode = ReflectedHierarchyFactory.Create(nodeValue, factory.Object);

            // ACT

            var result = hierarchyNode.ChildNodes.ToArray();

            // ASSERT

            factory.VerifyAll();
        }

        #endregion Node calls factory for child node creation

        #region Map object properties to inner nodes

        [Fact]
        public void Create_node_from_scalar_value_type_property()
        {
            // ARRANGE

            var obj = new { property = (int)1 };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var result = hierarchyNode.Children().ToArray();

            // ASSERT

            Assert.Single(result);
            Assert.False(result.Single().HasChildNodes);
            Assert.Equal("property", result.Single().Id);
        }

        [Fact]
        public void Create_node_from_scalar_nullable_value_type_property()
        {
            // ARRANGE

            var obj = new { property = (int?)1 };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var result = hierarchyNode.Children().ToArray();

            // ASSERT

            Assert.Single(result);
            Assert.False(result.Single().HasChildNodes);
            Assert.Equal("property", result.Single().Id);
        }

        [Fact]
        public void Create_node_from_null_in_nullable_value_type_property()
        {
            // ARRANGE

            var obj = new { property = (int?)null };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var result = hierarchyNode.Children().ToArray();

            // ASSERT

            Assert.Single(result);
            Assert.False(result.Single().HasChildNodes);
            Assert.Equal("property", result.Single().Id);
        }

        [Fact]
        public void Create_node_from_scalar_ref_type_property()
        {
            // ARRANGE

            var obj = new { property = "1" };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var result = hierarchyNode.Children().ToArray();

            // ASSERT
            // string is a leaf by definition. It's properzies arr ignored

            Assert.Single(result);
            Assert.False(result.Single().HasChildNodes);
            Assert.Equal("property", result.Single().Id);
        }

        [Fact]
        public void Create_node_from_scalar_array_type_property()
        {
            // ARRANGE

            var obj = new { property = new[] { 1, 2 } };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var result = hierarchyNode.Children().ToArray();

            // ASSERT
            // An array has its items as child nodes

            Assert.Single(result);
            Assert.True(result.Single().HasChildNodes);
            Assert.Equal("property", result.Single().Id);
        }

        [Fact]
        public void Sort_property_names_ascending_at_object_node()
        {
            // ARRANGE

            var obj1 = new
            {
                b = "b",
                a = 1
            };

            var obj2 = new
            {
                a = 1,
                b = "b"
            };

            var hierarchyNode1 = ReflectedHierarchyFactory.Create(obj1);
            var hierarchyNode2 = ReflectedHierarchyFactory.Create(obj2);

            // ACT

            var result1 = hierarchyNode1.Children().ToArray();
            var result2 = hierarchyNode2.Children().ToArray();

            // ASSERT

            Assert.Equal(new[] { "a", "b" }, result1.Select(n => n.Id));
            Assert.Equal(new[] { "a", "b" }, result2.Select(n => n.Id));
        }

        [Fact]
        public void Sort_property_names_ascending_at_property_node()
        {
            // ARRANGE

            var obj1 = new
            {
                property = new
                {
                    b = "b",
                    a = 1
                }
            };

            var obj2 = new
            {
                property = new
                {
                    a = 1,
                    b = "b"
                }
            };

            var hierarchyNode1 = ReflectedHierarchyFactory.Create(obj1);
            var hierarchyNode2 = ReflectedHierarchyFactory.Create(obj2);

            // ACT

            var result1 = hierarchyNode1.DescendantAt(HierarchyPath.Create("property")).Children().ToArray();
            var result2 = hierarchyNode2.DescendantAt(HierarchyPath.Create("property")).Children().ToArray();

            // ASSERT

            Assert.Equal(new[] { "a", "b" }, result1.Select(n => n.Id));
            Assert.Equal(new[] { "a", "b" }, result2.Select(n => n.Id));
        }

        #endregion Map object properties to inner nodes

        #region TryGet node by name

        [Fact]
        public void Retrieve_property_as_child_from_root_node()
        {
            // ARRANGE

            var obj = new { property = (string)"1" };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("property");

            // ASSERT

            Assert.True(success);
            Assert.Equal("property", result.Id);
        }

        [Fact]
        public void Retrieve_property_as_child_from_property_node()
        {
            // ARRANGE

            var obj = new { child = new { property = (string)"1" } };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("child").Item2.TryGetChildNode("property");

            // ASSERT

            Assert.True(success);
            Assert.Equal("property", result.Id);
        }

        [Fact]
        public void Retrieve_property_as_child_fails_on_unkown_name()
        {
            // ARRANGE

            var obj = new { property = (string)"1" };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("wrong");

            // ASSERT

            Assert.False(success);
        }

        #endregion TryGet node by name

        #region TryGetValue by enumerable index

        [Fact]
        public void Retrieve_array_item_by_index_from_enumerable_node()
        {
            // ARRANGE

            var obj = new { property = new[] { 1, 2 } };
            var (_, hierarchyNode) = ReflectedHierarchyFactory.Create(obj).TryGetChildNode("property");

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("0");

            // ASSERT

            Assert.True(success);
            Assert.Equal("0", result.Id);
        }

        [Fact]
        public void Retrieve_list_item_by_index_from_enumerable_node()
        {
            // ARRANGE

            var obj = new { property = new List<int> { 1, 2 } };
            var (_, hierarchyNode) = ReflectedHierarchyFactory.Create(obj).TryGetChildNode("property");

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("0");

            // ASSERT

            Assert.True(success);
            Assert.Equal("0", result.Id);
        }

        [Fact]
        public void Retrieve_array_item_by_index_from_enumerable_node_fails_on_wrong_index()
        {
            // ARRANGE

            var obj = new { property = new[] { 1, 2 } };
            var (_, hierarchyNode) = ReflectedHierarchyFactory.Create(obj).TryGetChildNode("property");

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("2");

            // ASSERT

            Assert.False(success);
        }

        [Fact]
        public void Retrieve_array_item_by_index_from_enumerable_node_fails_on_index_not_a_number()
        {
            // ARRANGE

            var obj = new { property = new[] { 1, 2 } };
            var (_, hierarchyNode) = ReflectedHierarchyFactory.Create(obj).TryGetChildNode("property");

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("number");

            // ASSERT

            Assert.False(success);
        }

        [Fact]
        public void Retrieve_list_item_by_index_from_enumerable_node_fails_on_wrong_index()
        {
            // ARRANGE

            var obj = new { property = new List<int> { 1, 2 } };
            var (_, hierarchyNode) = ReflectedHierarchyFactory.Create(obj).TryGetChildNode("property");

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("2");

            // ASSERT

            Assert.False(success);
        }

        [Fact]
        public void Retrieve_list_item_by_index_from_enumerable_node_fails_on_index_not_a_number()
        {
            // ARRANGE

            var obj = new { property = new List<int> { 1, 2 } };
            var (_, hierarchyNode) = ReflectedHierarchyFactory.Create(obj).TryGetChildNode("property");

            // ACT

            var (success, result) = hierarchyNode.TryGetChildNode("number");

            // ASSERT

            Assert.False(success);
        }

        #endregion TryGetValue by enumerable index

        #region TryGet value from node

        [Fact]
        public void Get_struct_value_from_root()

        {
            // ARRANGE

            var hierarchyNode = ReflectedHierarchyFactory.Create(1);

            // ACT

            var (success, value) = hierarchyNode.TryGetValue<int>();

            // ASSERT

            Assert.True(success);
            Assert.Equal(1, value);
        }

        [Fact]
        public void Get_ref_value_from_root()

        {
            // ARRANGE

            var hierarchyNode = ReflectedHierarchyFactory.Create("1");

            // ACT

            var (success, value) = hierarchyNode.TryGetValue<string>();

            // ASSERT

            Assert.True(success);
            Assert.Equal("1", value);
        }

        [Fact]
        public void Get_ref_value_from_property_node()
        {
            // ARRANGE

            var obj = new { property = (string)"1" };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, value) = hierarchyNode.TryGetChildNode("property").Item2.TryGetValue<string>();

            // ASSERT

            Assert.True(success);
            Assert.Equal("1", value);
        }

        [Fact]
        public void Get_struct_value_from_property_node()
        {
            // ARRANGE

            var obj = new { property = 1 };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, value) = hierarchyNode.TryGetChildNode("property").Item2.TryGetValue<int>();

            // ASSERT

            Assert.True(success);
            Assert.Equal(1, value);
        }

        [Fact]
        public void Get_struct_value_null_from_property_node()
        {
            // ARRANGE

            var obj = new { property = (int?)null };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, value) = hierarchyNode.TryGetChildNode("property").Item2.TryGetValue<int?>();

            // ASSERT

            Assert.True(success);
            Assert.Null(value);
        }

        [Fact]
        public void Get_struct_value_as_object_from_property_node()
        {
            // ARRANGE

            var obj = new { property = 1 };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, value) = hierarchyNode.TryGetChildNode("property").Item2.TryGetValue<object>();

            // ASSERT

            Assert.True(success);
            Assert.Equal(1, value);
        }

        [Fact]
        public void Get_ref_value_as_object_from_property_node()
        {
            // ARRANGE

            var obj = new { property = "1" };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, value) = hierarchyNode.TryGetChildNode("property").Item2.TryGetValue<object>();

            // ASSERT

            Assert.True(success);
            Assert.Equal("1", value);
        }

        [Fact]
        public void Get_value_from_property_node_fails_on_wrong_type()
        {
            // ARRANGE

            var obj = new { property = 1 };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT

            var (success, value) = hierarchyNode.TryGetChildNode("property").Item2.TryGetValue<string>();

            // ASSERT

            Assert.False(success);
        }

        [Fact]
        public void Get_null_value_from_property_node_fails_on_wrong_type()
        {
            // ARRANGE

            var obj = new { property = (int?)null };
            var hierarchyNode = ReflectedHierarchyFactory.Create(obj);

            // ACT
            // ask for the wrong type fails because propery tyoe in incompatible

            var (success, value) = hierarchyNode.TryGetChildNode("property").Item2.TryGetValue<string>();

            // ASSERT

            Assert.False(success);
        }

        [Fact]
        public void Get_value_from_root_fails_on_wrong_type()
        {
            // ARRANGE

            var hierarchyNode = ReflectedHierarchyFactory.Create(1);

            // ACT

            var (success, value) = hierarchyNode.TryGetValue<string>();

            // ASSERT

            Assert.False(success);
        }

        #endregion TryGet value from node

        #region Taverse the hierachy

        [Fact]
        public void Object_without_properties_hasnt_descendants()
        {
            // ARRANGE

            var obj = new { };

            // ACT

            var result = ReflectedHierarchyFactory.Create(obj).Descendants().ToArray();

            // ASSERT

            Assert.Empty(result);
        }

        [Fact]
        public void Object_with_int_property_has_single_descendant()
        {
            // ARRANGE

            var obj = new
            {
                a = 1,
            };

            // ACT

            var result = ReflectedHierarchyFactory.Create(obj).Descendants().ToArray();

            // ASSERT

            Assert.Single(result);
        }

        [Fact]
        public void Object_with_string_property_has_single_descendant()
        {
            // ARRANGE

            var obj = new
            {
                a = "string",
            };

            // ACT

            var result = ReflectedHierarchyFactory.Create(obj).Descendants().ToArray();

            // ASSERT
            // retuirns property "a" and a's value property Length. Chars is ignored.

            Assert.Single(result);
        }

        [Fact]
        private void Object_with_two_properties_create_three_descendants()
        {
            // ARRANGE

            var obj = new
            {
                a = 1,
                b = "b"
            };

            // ACT

            var result = ReflectedHierarchyFactory.Create(obj).Descendants().ToArray();

            // ASSERT

            Assert.Equal(2, result.Count());
        }

        #endregion Taverse the hierachy
    }
}