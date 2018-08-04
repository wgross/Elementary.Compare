namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyObjectRootNode : ReflectedHierarchyObjectNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyObjectRootNode(object instance, IReflectedHierarchyNodeFactory nodeFactory)
            : base(nodeFactory, new ReflectedHierarchyInstanceNodeFlyweight(instance, string.Empty))
        { }

    }
}