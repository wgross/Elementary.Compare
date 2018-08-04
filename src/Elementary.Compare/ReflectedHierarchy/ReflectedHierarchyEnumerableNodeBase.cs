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
        protected readonly IReflectedHierarchyNodeFlyweight state;

        public ReflectedHierarchyEnumerableNodeBase(object instance, IReflectedHierarchyNodeFactory nodeFactory, IReflectedHierarchyNodeFlyweight state)
            : base(instance, nodeFactory)
        {
            this.state = state;
        }

        #region IHasChildNodes members

        public bool HasChildNodes => ((IEnumerable)this.state.NodeValue).Cast<object>().Any();

        public IEnumerable<IReflectedHierarchyNode> ChildNodes
        {
            get
            {
                int i = 0;
                return ((IEnumerable)this.state.NodeValue).Cast<object>().Select(n => this.nodeFactory.Create(n, i++.ToString(CultureInfo.InvariantCulture)));
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
                return (true, this.nodeFactory.Create(((IEnumerable)this.state.NodeValue).Cast<object>().ElementAt(index), id));
            }
            catch (ArgumentOutOfRangeException)
            {
                return (false, null);
            }
        }

        #endregion IHasIdentifiableChildNodes

        #region IReflectedHierarchyNode members

        public string Id => this.state.Id;

        public (bool, T) TryGetValue<T>()
        {
            var nodeValue = this.state.NodeValue;
            if (!typeof(T).IsAssignableFrom(nodeValue.GetType()))
                return (false, default(T));

            return (true, (T)nodeValue);
        }

        #endregion IReflectedHierarchyNode members
    }
}