using System;

namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedHierarchyNodeBase
    {
        protected readonly IReflectedHierarchyNodeFactory nodeFactory;

        public ReflectedHierarchyNodeBase(IReflectedHierarchyNodeFactory nodeFactory, IReflectedHierarchyNodeFlyweight state)
        {
            this.nodeFactory = nodeFactory;
            this.State = state;
        }

        public IReflectedHierarchyNodeFlyweight State { get; }

        #region IReflectedHierarchyNode members

        public string Id => this.State.Id;

        /// <summary>
        /// The valu eof the root node is the object is wraps
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public (bool, T) TryGetValue<T>()
        {
            var nodeValue = this.State.NodeValue;
            if (!typeof(T).IsAssignableFrom(this.State.NodeValueType))
                return (false, default(T));

            return (true, (T)nodeValue);
        }

        public Type ValueType => this.State.NodeValueType;

        #endregion IReflectedHierarchyNode members
    }
}