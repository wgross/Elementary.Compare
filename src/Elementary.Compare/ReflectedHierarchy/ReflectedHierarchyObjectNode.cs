namespace Elementary.Compare.ReflectedHierarchy
{
    public sealed class ReflectedHierarchyObjectNode : ReflectedHierarchyObjectNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyObjectNode(object instance, string id, IReflectedHierarchyNodeFactory nodeFactory)
            : base(nodeFactory, new ReflectedHierarchyInstanceNodeFlyweight(instance, id))

        {
        }
    }
}