namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyObjectRootNode : ReflectedHierarchyObjectNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyObjectRootNode(object instance, IReflectedHierarchyNodeFactory nodeFactory)
            : base(nodeFactory, ReflectedHierarchyNodeFlyweight.Create(instance, string.Empty))
        { }
    }
}