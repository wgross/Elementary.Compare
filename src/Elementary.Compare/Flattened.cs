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
        public static Flattened<T> Flatten<T>(this T instance, int? maxDepth = null, Action<Flattened<T>> setup = null) => new Flattened<T>(instance, maxDepth);
    }

    /// <summary>
    /// Marker interface to make a collection of key value pairs from a flattened object from an other object.
    /// This prohibits a flatted key value collections is flatted again.
    /// </summary>
    public interface IFlattened : IEnumerable<KeyValuePair<string, object>>
    {
    }

    public sealed class Flattened<T> : IFlattened
    {
        private List<HierarchyPath<string>> Excluded { get; } = new List<HierarchyPath<string>>();

        public T Instance { get; }

        public int? MaxDepth { get; }

        public Flattened(T instance, int? maxDepth)
        {
            this.Instance = instance;
            this.MaxDepth = maxDepth.GetValueOrDefault(100);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var h = ReflectedHierarchy.ReflectedHierarchyFactory.Create(this.Instance, new ReflectedHierarchyNodeFactory());
            foreach (var (node, path) in h.DescendantsAndSelfWithPathAvoidCycles(n => n.ChildNodes, this.MaxDepth, new ReflectedHierarchyNodeEqualityComparer()))
            {
                var parentPath = HierarchyPath.Create(path.Skip(1).Select(p => p.Id).ToArray());

                var pathAsString = $"{string.Join("/", parentPath)}/{node.Id}".TrimStart('/');

                if (parentPath.Items.Count() >= this.MaxDepth)
                    throw new InvalidOperationException($"Traversal stopped: maxDepth='{this.MaxDepth}' was reached at path='{pathAsString}'.");

                if (node.HasChildNodes)
                    continue; // skip this node, it this isn't a leave.

                // if the nodes path is found in the excluded collection it is skipped.
                // this is the most epensive test, should therefor be the last one.
                if (this.Excluded.Any(ex => ex.Equals(parentPath) || ex.IsAncestorOf(parentPath)))
                    continue;

                yield return new KeyValuePair<string, object>(pathAsString, node.TryGetValue<object>().Item2);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public Flattened<T> Exclude(HierarchyPath<string> propertyPath)
        {
            this.Excluded.Add(propertyPath);
            return this;
        }

        public Flattened<T> Exclude(Expression<Func<T, object>> selectPropertyPath)
        {
            this.Excluded.Add(HierarchyPath.Create(PropertyPath.PathSegments<T>(selectPropertyPath)));
            return this;
        }
    }
}