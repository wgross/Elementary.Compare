namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedHierarchyNodeBase
    {
        protected readonly IReflectedHierarchyNodeFactory nodeFactory;
        private readonly IReflectedHierarchyNodeFlyweight state;

        public ReflectedHierarchyNodeBase(IReflectedHierarchyNodeFactory nodeFactory, IReflectedHierarchyNodeFlyweight state)
        {
            this.nodeFactory = nodeFactory;
            this.state = state;
        }

        protected IReflectedHierarchyNodeFlyweight State => this.state;

        public string Id => this.state.Id;
    }
}