using System;
using System.Collections;
using System.Linq;

namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyEnumerableRootNode : ReflectedHierarchyEnumerableNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyEnumerableRootNode(object instance, IReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory)
        { }

        /// <summary>
        /// A property node resolves its inner value by getting the property value of the instance the property
        /// actually belongs to,
        /// </summary>
        override protected object NodeValue => this.instance;

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

        #endregion IReflectedHierarchyNode members
    }
}