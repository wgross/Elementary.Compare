using Elementary.Compare.ReflectedHierarchy;
using Xunit;

namespace Elementary.Compare.Test.ReflectedHierarchy
{
    public class ReflectedHierarchyNodeEqualityComparerTestTest
    {
        private readonly ReflectedHierarchyNodeEqualityComparer comparer;
        private readonly ReflectedHierarchyNodeFactory factory;

        public ReflectedHierarchyNodeEqualityComparerTestTest()
        {
            this.comparer = new ReflectedHierarchyNodeEqualityComparer();
            this.factory = new ReflectedHierarchyNodeFactory();
        }

        [Fact]
        public void ReflectedHierachyNodeEqualityComparer_accepts_same_instance_same_property()
        {
            // ARRANGE

            var obj = new
            {
                data = "data"
            };

            var left = this.factory.Create(obj, obj.GetType().GetProperty("data"));
            var right = this.factory.Create(obj, obj.GetType().GetProperty("data"));

            // ACT

            var result = this.comparer.Equals(left, right);

            // ASSERT

            Assert.True(result);
            Assert.Equal(this.comparer.GetHashCode(left), this.comparer.GetHashCode(right));
        }

        [Theory]
        [InlineData((object)1)]
        [InlineData((object)"text")]
        public void ReflectedHierachyNodeEqualityComparer_accepts_different_instance_different_property_same_value(object value)
        {
            // ARRANGE

            var obj1 = new
            {
                data1 = value
            };

            var obj2 = new
            {
                data2 = value
            };

            var left = this.factory.Create(obj1, obj1.GetType().GetProperty("data1"));
            var right = this.factory.Create(obj2, obj2.GetType().GetProperty("data2"));

            // ACT

            var result = this.comparer.Equals(left, right);

            // ASSERT

            Assert.True(result);
            Assert.Equal(this.comparer.GetHashCode(left), this.comparer.GetHashCode(right));
        }

        [Fact]
        public void ReflectedHierachyNodeEqualityComparer_rejects_nulls()
        {
            // ARRANGE

            var obj = new
            {
                data = (object)null
            };

            var left = this.factory.Create(obj, obj.GetType().GetProperty("data"));
            var right = this.factory.Create(obj, obj.GetType().GetProperty("data"));

            // ACT

            var result = this.comparer.Equals(left, right);

            // ASSERT

            Assert.False(result);
        }
    }
}