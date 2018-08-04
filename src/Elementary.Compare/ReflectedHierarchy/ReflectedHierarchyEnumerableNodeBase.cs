using Elementary.Hierarchy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedHierarchyEnumerableNodeBase : ReflectedHierarchyNodeBase, IHasChildNodes<IReflectedHierarchyNode>
    {
        public ReflectedHierarchyEnumerableNodeBase(object instance, IReflectedHierarchyNodeFactory nodeFactory, IReflectedHierarchyNodeFlyweight state)
            : base(nodeFactory, state)
        {
        }

        #region IHasChildNodes members

        public bool HasChildNodes => ((IEnumerable)this.State.NodeValue).Cast<object>().Any();

        public IEnumerable<IReflectedHierarchyNode> ChildNodes
        {
            get
            {
                int i = 0;
                return ((IEnumerable)this.State.NodeValue).Cast<object>().Select(n => this.nodeFactory.Create(n, i++.ToString(CultureInfo.InvariantCulture)));
            }
        }

        #endregion IHasChildNodes members

        #region IHasIdentifiableChildNodes

        public (bool, IReflectedHierarchyNode) TryGetChildNode(string id)
        {
            if (!int.TryParse(id, out var index))
                return (false, null);
            try
            {
                return (true, this.nodeFactory.Create(((IEnumerable)this.State.NodeValue).Cast<object>().ElementAt(index), id));
            }
            catch (ArgumentOutOfRangeException)
            {
                return (false, null);
            }
        }

        #endregion IHasIdentifiableChildNodes

        #region IReflectedHierarchyNode members

        public (bool, T) TryGetValue<T>()
        {
            var nodeValue = this.State.NodeValue;
            if (!typeof(T).IsAssignableFrom(nodeValue.GetType()))
                return (false, default(T));

            return (true, (T)nodeValue);
        }

        #endregion IReflectedHierarchyNode members
    }
}