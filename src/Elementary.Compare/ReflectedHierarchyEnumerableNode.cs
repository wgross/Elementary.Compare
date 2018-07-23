using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Elementary.Compare
{
    internal class ReflectedHierarchyEnumerableNode : ReflectedPropertyNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyEnumerableNode(object instance, PropertyInfo propertyInfo, ReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, propertyInfo, nodeFactory)
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

        #region IHasIdentifiableChildNodes

        public (bool, IReflectedHierarchyNode) TryGetChildNode(string id)
        {
            if (!int.TryParse(id, out var index))
                return (false, null);
            try
            {
                return (true, this.nodeFactory.Create(((IEnumerable)this.NodeValue).Cast<object>().ElementAt(index), id));
            }
            catch (ArgumentOutOfRangeException)
            {
                return (false, null);
            }
        }

        #endregion IHasIdentifiableChildNodes
    }
}