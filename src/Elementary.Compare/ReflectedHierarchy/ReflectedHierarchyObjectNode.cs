namespace Elementary.Compare.ReflectedHierarchy
{
    public sealed class ReflectedHierarchyObjectNode : ReflectedHierarchyObjectNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyObjectNode(object instance, string id, IReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory)

        {
            this.Id = id;
        }

        #region IReflectedHierarchyNode members

        public string Id { get; }

        #endregion IReflectedHierarchyNode members
    }
}