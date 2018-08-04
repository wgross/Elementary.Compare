namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyObjectRootNode : ReflectedHierarchyObjectNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyObjectRootNode(object instance, IReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory)
        { }

        #region IReflectedHierarchyNode members

        public string Id => string.Empty;

        #endregion IReflectedHierarchyNode members
    }
}