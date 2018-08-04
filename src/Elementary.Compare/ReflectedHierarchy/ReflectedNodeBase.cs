namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedHierarchyNodeBase
    {
        protected readonly object instance;
        protected readonly IReflectedHierarchyNodeFactory nodeFactory;
        protected readonly IReflectedHierarchyNodeFlyweight state;

        public ReflectedHierarchyNodeBase(object instance, IReflectedHierarchyNodeFactory nodeFactory, IReflectedHierarchyNodeFlyweight state)
        {
            this.instance = instance;
            this.nodeFactory = nodeFactory;
            this.state = state;
        }

        protected object NodeValue => this.state.NodeValue;

        public string Id => this.state.Id;
    }
}