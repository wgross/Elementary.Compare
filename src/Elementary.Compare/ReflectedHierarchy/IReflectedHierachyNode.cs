using Elementary.Hierarchy;
using System;

namespace Elementary.Compare.ReflectedHierarchy
{
    public interface IReflectedHierarchyNode : IHasChildNodes<IReflectedHierarchyNode>, IHasIdentifiableChildNodes<string, IReflectedHierarchyNode>
    {
        string Id { get; }

        (bool, T) TryGetValue<T>();

        Type ValueType { get; }
    }
}