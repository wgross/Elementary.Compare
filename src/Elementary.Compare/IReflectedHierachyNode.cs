using Elementary.Hierarchy;

namespace Elementary.Compare
{
    public interface IReflectedHierarchyNode : IHasChildNodes<IReflectedHierarchyNode>, IHasIdentifiableChildNodes<string, IReflectedHierarchyNode>
    {
        string Id { get; }

        (bool, T) TryGetValue<T>();
    }
}