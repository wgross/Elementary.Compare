namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyEnumerableRootNode : ReflectedHierarchyEnumerableNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyEnumerableRootNode(object instance, IReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory, new ReflectedHierarchyInstanceNodeFlyweight(instance, string.Empty))
        { }
    }
}