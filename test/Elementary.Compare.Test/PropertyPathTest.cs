using Elementary.Hierarchy;
using Xunit;

namespace Elementary.Compare.Test
{
    public class PropertyPathTest
    {
        [Fact]
        public void Path_of_root_is_empty()
        {
            // ARRANGE

            var left = new { };

            // ACT

            var result = PropertyPath.Make(left, o => o);

            // ASSERT

            Assert.Equal(HierarchyPath.Create<string>(), result);
        }

        [Fact]
        public void Path_of_property_is_its_name()
        {
            // ARRANGE

            var left = new { a = 1 };

            // ACT

            var result = PropertyPath.Make(left, o => o.a);

            // ASSERT>

            Assert.Equal(HierarchyPath.Create("a"), result);
        }

        [Fact]
        public void Path_of_array_item_is_its_index()
        {
            // ARRANGE

            var left = new { a = new[] { 1 } };

            // ACT

            var result = PropertyPath.Make(left, o => o.a[0]);

            // ASSERT>

            Assert.Equal(HierarchyPath.Create("a", "0"), result);
        }
    }
}