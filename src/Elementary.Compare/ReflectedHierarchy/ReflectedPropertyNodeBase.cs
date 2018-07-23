using System.Collections.Generic;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public abstract class ReflectedPropertyNodeBase : ReflectedNodeBase
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
        protected object NodeValue => this.propertyInfo.GetValue(this.instance);

        protected IEnumerable<PropertyInfo> ChildPropertyInfos => this.NodeValue.GetType().GetProperties();

        #region IReflectedHierarchyNode members

        /// <summary>
        /// The name of a property node is the name of the property it refers to.
        /// </summary>
        public string Id => this.propertyInfo.Name;

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