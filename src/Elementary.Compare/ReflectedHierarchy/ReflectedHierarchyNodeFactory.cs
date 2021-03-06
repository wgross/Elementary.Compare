﻿using System.Collections;
using System.Linq;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyNodeFactory : IReflectedHierarchyNodeFactory
    {
        public virtual IReflectedHierarchyNode Create(object instance, PropertyInfo property)
        {
            if (instance is null)
                return null;

            var instanceType = instance.GetType();

            if (typeof(string).Equals(instanceType))
                return null; // ignore strings properties

            if (instanceType.IsValueType)
                return null;

            if (property.GetIndexParameters().Any())
                return null; // exclude indexers

            if (property.PropertyType != typeof(string)) // string is also enumerable but is treated like a 'value type'
                if (property.PropertyType.GetInterface(typeof(IEnumerable).Name) != null)
                    return new ReflectedHierarchyEnumerableNode(instance, property, this);

            return new ReflectedHierarchyPropertyNode(instance, property, nodeFactory: this);
        }

        public IReflectedHierarchyNode Create(object instance, string id)
        {
            return new ReflectedHierarchyObjectNode(instance, id, this);
        }
    }
}