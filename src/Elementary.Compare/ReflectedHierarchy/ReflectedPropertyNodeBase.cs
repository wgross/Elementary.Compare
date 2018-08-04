using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedPropertyNodeBase : ReflectedHierarchyNodeBase
    {
        protected readonly PropertyInfo propertyInfo;

        public ReflectedPropertyNodeBase(object instance, PropertyInfo propertyInfo, IReflectedHierarchyNodeFactory nodeFactory)
            : base(instance, nodeFactory)

        {
            this.propertyInfo = propertyInfo;
        }

        /// <summary>
        /// A property node resolves its inner value by getting the property value of the instance the property
        /// actually belongs to,
        /// </summary>
        override protected object NodeValue => this.propertyInfo.GetValue(this.instance);

        protected IEnumerable<PropertyInfo> ChildPropertyInfos => this.NodeValue?.GetType().GetProperties() ?? Enumerable.Empty<PropertyInfo>();

        #region IReflectedHierarchyNode members

        /// <summary>
        /// The name of a property node is the name of the property it refers to.
        /// </summary>
        public string Id => this.propertyInfo.Name;

        public (bool, T) TryGetValue<T>()
        {
            var nodeValue = this.NodeValue;
            if (nodeValue != null)
            {
                if (!typeof(T).IsAssignableFrom(nodeValue.GetType()))
                    return (false, default(T));
            }
            else if (!typeof(T).IsAssignableFrom(this.propertyInfo.PropertyType))
               return (false, default(T));

            return (true, (T)nodeValue);
        }

        #endregion IReflectedHierarchyNode members
    }
}