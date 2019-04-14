using System;
using System.Reflection;

namespace Elementary.Compare.ReflectedHierarchy
{
    public interface IReflectedHierarchyNodeFlyweight
    {
        object NodeValue { get; }

        Type NodeValueType { get; }

        string Id { get; }
    }

    public sealed class ReflectedHierarchyNodeFlyweight : IReflectedHierarchyNodeFlyweight
    {
        public static ReflectedHierarchyNodeFlyweight Create(object instance, PropertyInfo property) => new ReflectedHierarchyNodeFlyweight(
            instance,
            property.Name,
            nodeValue: property.GetValue(instance),
            nodeValueType: property.GetValue(instance)?.GetType() ?? property.PropertyType);

        public static ReflectedHierarchyNodeFlyweight Create(object instance, string id) => new ReflectedHierarchyNodeFlyweight(
            instance,
            id,
            nodeValue: instance,
            nodeValueType: instance.GetType());

        public object Instance { get; }

        public string Id { get; }

        public object NodeValue { get; }

        public Type NodeValueType { get; }

        private ReflectedHierarchyNodeFlyweight(object instance, string id, object nodeValue, Type nodeValueType)
        {
            this.Instance = instance;
            this.Id = id;
            this.NodeValue = nodeValue;
            this.NodeValueType = nodeValueType;
        }
    }
}