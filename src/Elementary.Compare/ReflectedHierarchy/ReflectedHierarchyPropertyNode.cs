﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    /// <summary>
    /// An inner node of the refleceted hierrachy refers always to an instance and a property info.
    /// The name of the property is the key of the child in the collectin of child nodes of ites owning instance.
    /// </summary>
    public sealed class ReflectedHierarchyPropertyNode : ReflectedPropertyNodeBase, IReflectedHierarchyNode
    {
        public ReflectedHierarchyPropertyNode(object instance, PropertyInfo propertyInfo, IReflectedHierarchyNodeFactory nodeFactory)
            : base(nodeFactory, new ReflectedHierarchyInstancePropertyNodeFlyweight(instance, propertyInfo))

        {
        }

        #region IHasChildNodes members

        public bool HasChildNodes => this.ChildNodes.Any();

        public IEnumerable<IReflectedHierarchyNode> ChildNodes => this
            .ChildPropertyInfos
            .OrderBy(pi => pi.Name, StringComparer.OrdinalIgnoreCase)
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