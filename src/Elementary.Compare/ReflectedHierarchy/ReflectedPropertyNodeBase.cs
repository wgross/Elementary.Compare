using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedPropertyNodeBase : ReflectedHierarchyNodeBase
    {
        public ReflectedPropertyNodeBase(object instance, PropertyInfo propertyInfo, IReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory, new ReflectedHierarchyInstancePropertyNodeFlyweight(instance, propertyInfo))

        {
        }

        protected IEnumerable<PropertyInfo> ChildPropertyInfos => this.NodeValue?.GetType().GetProperties() ?? Enumerable.Empty<PropertyInfo>();

        #region IReflectedHierarchyNode members

        public (bool, T) TryGetValue<T>()
        {
            if (!typeof(T).IsAssignableFrom(this.state.NodeValueType))
                return (false, default(T));

            return (true, (T)this.NodeValue);
        }

        #endregion IReflectedHierarchyNode members
    }
}