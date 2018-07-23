using System.Reflection;

namespace Elementary.Compare
{
    public interface IReflectedHierarchyNodeFactory
    {
        IReflectedHierarchyNode Create(object instance, string id);

        IReflectedHierarchyNode Create(object instance, PropertyInfo property);
    }
}