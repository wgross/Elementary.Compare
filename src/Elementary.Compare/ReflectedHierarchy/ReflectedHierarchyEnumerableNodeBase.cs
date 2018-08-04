using Elementary.Hierarchy;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedHierarchyEnumerableNodeBase : ReflectedHierarchyNodeBase, IHasChildNodes<IReflectedHierarchyNode>
    {
        public ReflectedHierarchyEnumerableNodeBase(object instance, IReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory)
        {
        }

        #region IHasChildNodes members

        public bool HasChildNodes => ((IEnumerable)this.NodeValue).Cast<object>().Any();

        public IEnumerable<IReflectedHierarchyNode> ChildNodes
        {
            get
            {
                int i = 0;
                return ((IEnumerable)this.NodeValue).Cast<object>().Select(n => this.nodeFactory.Create(n, i++.ToString(CultureInfo.InvariantCulture)));
            }
        }

        #endregion IHasChildNodes members

        #region IReflectedHierarchyNode members

        public (bool, T) TryGetValue<T>()
        {
            var nodeValue = this.NodeValue;
            if (!typeof(T).IsAssignableFrom(nodeValue.GetType()))
                return (false, default(T));

            return (true, (T)nodeValue);
        }

        #endregion IReflectedHierarchyNode members
    }
}