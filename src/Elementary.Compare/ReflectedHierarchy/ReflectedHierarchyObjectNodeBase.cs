using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedHierarchyObjectNodeBase : ReflectedHierarchyNodeBase
    {
        public ReflectedHierarchyObjectNodeBase(IReflectedHierarchyNodeFactory nodeFactory, IReflectedHierarchyNodeFlyweight state)
            : base(nodeFactory, state)
        { }

        protected IEnumerable<PropertyInfo> ChildPropertyInfos => this.State.NodeValueType
            .GetProperties()
            .OrderBy(pi => pi.Name, StringComparer.OrdinalIgnoreCase);

        #region IHasChildNodes members

        public bool HasChildNodes => this.ChildNodes.Any();

        public IEnumerable<IReflectedHierarchyNode> ChildNodes => this
            .ChildPropertyInfos
            .Select(pi => this.nodeFactory.Create(this.State.NodeValue, pi))
            .Where(n => n != null);

        #endregion IHasChildNodes members

        #region IHasIdentifiableChildNode members

        public (bool, IReflectedHierarchyNode) TryGetChildNode(string id)
        {
            var childNode = this.ChildPropertyInfos
               .Where(pi => pi.Name.Equals(id))
               .Select(pi => this.nodeFactory.Create(this.State.NodeValue, pi))
               .FirstOrDefault();

            return (childNode != null, childNode);
        }

        #endregion IHasIdentifiableChildNode members
    }
}