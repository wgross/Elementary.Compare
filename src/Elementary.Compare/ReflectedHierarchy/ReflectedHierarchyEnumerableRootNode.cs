using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyEnumerableRootNode : IReflectedHierarchyNode
    {
        private readonly object instance;
        private readonly IReflectedHierarchyNodeFactory nodeFactory;

        public ReflectedHierarchyEnumerableRootNode(object instance, IReflectedHierarchyNodeFactory nodeFactory)
        {
            this.instance = instance;
            this.nodeFactory = nodeFactory;
        }

        /// <summary>
        /// A property node resolves its inner value by getting the property value of the instance the property
        /// actually belongs to,
        /// </summary>
        private object NodeValue => this.instance;

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

        #region IReflectedHierarchyNode members

        public string Id => string.Empty;

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