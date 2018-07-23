namespace Elementary.Compare
{
    public class ReflectedHierarchyRootNode : ReflectedHierarchyObjectNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyRootNode(object instance, IReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory)
        { }

        #region IReflectedHierarchyNode members

        public string Id => string.Empty;

        #endregion IReflectedHierarchyNode members
    }
}