namespace Elementary.Compare
{
    public abstract class ReflectedNodeBase
    {
        protected readonly object instance;
        protected readonly IReflectedHierarchyNodeFactory nodeFactory;

        public ReflectedNodeBase(object instance, IReflectedHierarchyNodeFactory nodeFactory)
        {
            this.instance = instance;
            this.nodeFactory = nodeFactory;
        }
    }
}