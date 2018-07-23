namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyFactory
    {
        public static IReflectedHierarchyNode Create<T>(T root)
        {
            return new ReflectedHierarchyRootNode(root, new ReflectedHierarchyNodeFactory());
        }

        public static IReflectedHierarchyNode Create<T>(T root, IReflectedHierarchyNodeFactory nodeFactory)
        {
            return new ReflectedHierarchyRootNode(root, nodeFactory);
        }
    }
}