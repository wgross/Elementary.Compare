using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyEnumerableNode : ReflectedHierarchyEnumerableNodeBase, IReflectedHierarchyNode
    {
        private readonly PropertyInfo propertyInfo;

        public ReflectedHierarchyEnumerableNode(object instance, PropertyInfo propertyInfo, ReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory)
        {
            this.propertyInfo = propertyInfo;
        }

        protected override object NodeValue => this.propertyInfo.GetValue(base.instance);

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

        #region IReflectedHierarchyNode members

        public string Id => this.propertyInfo.Name;

        #endregion IReflectedHierarchyNode members


    }
}