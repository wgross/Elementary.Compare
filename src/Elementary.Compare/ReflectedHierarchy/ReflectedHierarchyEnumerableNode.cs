using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyEnumerableNode : ReflectedHierarchyEnumerableNodeBase, IReflectedHierarchyNode
    {
        private readonly PropertyInfo propertyInfo;

        public ReflectedHierarchyEnumerableNode(object instance, PropertyInfo propertyInfo, ReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory, new ReflectedHierarchyInstancePropertyNodeFlyweight(instance, propertyInfo))
        {
            this.propertyInfo = propertyInfo;
        }
    }
}