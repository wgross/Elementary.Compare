namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedHierarchyNodeBase
    {
        protected readonly object instance;
        protected readonly IReflectedHierarchyNodeFactory nodeFactory;

        public ReflectedHierarchyNodeBase(object instance, IReflectedHierarchyNodeFactory nodeFactory)
        {
            this.instance = instance;
            this.nodeFactory = nodeFactory;
        }
    }
}