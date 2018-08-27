using System.Collections;

namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyFactory
    {
        public static IReflectedHierarchyNode Create<T>(T root)
        {
            return Create(root, new ReflectedHierarchyNodeFactory());
        }

        public static IReflectedHierarchyNode Create<T>(T root, IReflectedHierarchyNodeFactory nodeFactory)
        {
            if (typeof(T) != typeof(string)) // string is also enumerable but is treated like a 'value type'
                if (typeof(T).GetInterface(typeof(IEnumerable).Name) != null)
                    return new ReflectedHierarchyEnumerableRootNode(root, nodeFactory);

            return new ReflectedHierarchyObjectRootNode(root, nodeFactory);
        }
    }
}