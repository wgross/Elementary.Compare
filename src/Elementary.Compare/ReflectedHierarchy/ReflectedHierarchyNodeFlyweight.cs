using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public interface IReflectedHierarchyNodeFlyweight
    {
        object NodeValue { get; }

        string Id { get; }
    }

    public sealed class ReflectedHierarchyInstanceNodeFlyweight : IReflectedHierarchyNodeFlyweight
    {
        private readonly object instance;
        private readonly string id;

        public ReflectedHierarchyInstanceNodeFlyweight(object instance, string id)
        {
            this.instance = instance;
            this.id = id;
        }

        public object NodeValue => this.instance;

        public string Id => this.id;
    }

    public sealed class ReflectedHierarchyInstancePropertyNodeFlyweight : IReflectedHierarchyNodeFlyweight
    {
        private readonly object instance;

        private readonly PropertyInfo property;

        public ReflectedHierarchyInstancePropertyNodeFlyweight(object instance, PropertyInfo property)
        {
            this.instance = instance;
            this.property = property;
        }

        public object NodeValue => this.property.GetValue(this.instance);

        public string Id => this.property.Name;
    }
}