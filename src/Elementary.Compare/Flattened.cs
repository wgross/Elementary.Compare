using Elementary.Compare.ReflectedHierarchy;
using Elementary.Hierarchy;
using Elementary.Hierarchy.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Elementary.Compare
{
    public static class Flattened
    {
        /// <summary>
        /// Returns a key value stream of pathes to leaves and the leaves values.
        /// The travesal breaks after a depth of 100 nodes.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="maxDepth"></param>
        /// <returns></returns>
        public static FlattenedObjectBuilder<T> Flatten<T>(this T instance, int? maxDepth = null) => new FlattenedObjectBuilder<T>(instance, maxDepth);
    }

    /// <summary>
    /// Marker interface to make a collection of key value pairs from a flattened object from an other object.
    /// This prohibits a flatted key value collections is flatted again.
    /// </summary>
    public interface IFlattened : IEnumerable<KeyValuePair<string, object>>
    {
    }

    /// <summary>
    /// Satrts the deinition how to create a flattened and comparable representation of a
    /// given object instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FlattenedObjectBuilder<T>
    {
        private List<HierarchyPath<string>> Excluded { get; } = new List<HierarchyPath<string>>();

        public T Instance { get; }

        public int? MaxDepth { get; }

        public FlattenedObjectBuilder(T instance, int? maxDepth)
        {
            this.Instance = instance;
            this.MaxDepth = maxDepth.GetValueOrDefault(100);
        }

        private static string JoinParentPathAndNodeId(IReflectedHierarchyNode node, HierarchyPath<string> parentPath) => $"{string.Join("/", parentPath)}/{node.Id}".TrimStart('/');

        private IEnumerable<KeyValuePair<string, object>> Traverse()
        {
            var h = ReflectedHierarchy.ReflectedHierarchyFactory.Create(this.Instance, new ReflectedHierarchyNodeFactory());
            foreach (var (node, path) in h.DescendantsAndSelfWithPathAvoidCycles(n => n.ChildNodes, this.MaxDepth, new ReflectedHierarchyNodeEqualityComparer()))
            {
                var parentPath = HierarchyPath.Create(path.Skip(1).Select(p => p.Id).ToArray());
                var nodePath = parentPath.Join(node.Id);
                string nodePathAsString = nodePath.ToString();

                if (parentPath.Items.Count() >= this.MaxDepth)
                    throw new InvalidOperationException($"Traversal stopped: maxDepth='{this.MaxDepth}' was reached at path='{nodePathAsString}'.");

                if (node.HasChildNodes)
                    continue; // skip this node, its not a leave.

                // if the nodes path is found in the excluded collection it is skipped.
                // this is the most epensive test, should therefor be the last one.
                if (this.Excluded.Any(ex => ex.Equals(nodePath) || ex.IsAncestorOf(nodePath)))
                    continue;

                yield return new KeyValuePair<string, object>(nodePathAsString, node.TryGetValue<object>().Item2);
            }
        }

        public FlattenedObjectBuilder<T> Exclude(Expression<Func<T, object>> selectPropertyPath)
        {
            this.Excluded.Add(HierarchyPath.Create(PropertyPath.PathSegments<T>(selectPropertyPath)));
            return this;
        }

        /// <summary>
        /// Finshes the defineitionof the comparison process.
        /// </summary>
        /// <returns></returns>
        public FlattenedObject Build() => new FlattenedObject(this.Traverse());
    }

    public sealed class FlattenedObject : IFlattened
    {
        private KeyValuePair<string, object>[] Leaves { get; }

        public FlattenedObject(IEnumerable<KeyValuePair<string, object>> leaves)
        {
            this.Leaves = leaves.ToArray();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (int i = 0; i < this.Leaves.Length; i++)
                yield return this.Leaves[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}