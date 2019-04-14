using Elementary.Hierarchy;
using System;

namespace Elementary.Compare.ReflectedHierarchy
{
    public interface IReflectedHierarchyNode : IHasChildNodes<IReflectedHierarchyNode>, IHasIdentifiableChildNodes<string, IReflectedHierarchyNode>
    {
        /// <summary>
        /// The id of a refelected node is the property name ir represents.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Retrieves the vaue of the reflected node as the given type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>false, if the vaue can't be converted to the given type, true otherwise</returns>
        (bool, T) TryGetValue<T>();

        /// <summary>
        /// The type of the value of the node.
        /// </summary>
        Type ValueType { get; }
    }
}