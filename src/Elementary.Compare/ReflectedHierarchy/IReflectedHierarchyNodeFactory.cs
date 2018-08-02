using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public interface IReflectedHierarchyNodeFactory
    {
        IReflectedHierarchyNode Create(object instance, string id);

        IReflectedHierarchyNode Create(object instance, PropertyInfo property);
    }
}