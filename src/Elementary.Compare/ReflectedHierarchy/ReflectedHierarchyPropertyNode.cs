using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    /// <summary>
    /// An inner node of the refleceted hierrachy refers always to an instance and a property info.
    /// The name of the property is the key of the child in the collectin of child nodes of ites owning instance.
    /// </summary>
    public sealed class ReflectedHierarchyPropertyNode : ReflectedHierarchyObjectNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyPropertyNode(object instance, PropertyInfo propertyInfo, IReflectedHierarchyNodeFactory nodeFactory)
            : base(nodeFactory, ReflectedHierarchyNodeFlyweight.Create(instance, propertyInfo))

        { }
    }
}