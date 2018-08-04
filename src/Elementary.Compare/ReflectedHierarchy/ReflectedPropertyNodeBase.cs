using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedPropertyNodeBase : ReflectedHierarchyNodeBase
    {
        public ReflectedPropertyNodeBase(IReflectedHierarchyNodeFactory nodeFactory, IReflectedHierarchyNodeFlyweight state)
            : base(nodeFactory, state)

        {
        }

        protected IEnumerable<PropertyInfo> ChildPropertyInfos => this.State.NodeValue?.GetType().GetProperties() ?? Enumerable.Empty<PropertyInfo>();

        #region IReflectedHierarchyNode members

        public (bool, T) TryGetValue<T>()
        {
            if (!typeof(T).IsAssignableFrom(this.State.NodeValueType))
                return (false, default(T));

            return (true, (T)this.State.NodeValue);
        }

        #endregion IReflectedHierarchyNode members
    }
}