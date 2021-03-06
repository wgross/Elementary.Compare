﻿using Elementary.Compare.ReflectedHierarchy;
using Elementary.Hierarchy;
using Elementary.Hierarchy.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Elementary.Compare
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns a key value stream of pathes to leaves and the leaves values.
        /// The travesal breaks after a depth of 100 nodes.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="maxDepth"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> Flatten(this object root, int? maxDepth = null)
        {
            var h = ReflectedHierarchy.ReflectedHierarchyFactory.Create(root, new ReflectedHierarchyNodeFactory());
            var flatted_h = new Dictionary<string, object>();
            foreach (var (node, path) in h.DescendantsWithPath(getChildren: n => n.ChildNodes, depthFirst: true, maxDepth: maxDepth.GetValueOrDefault(100)))
            {
                var parentPathAry = path.Select(p => p.Id).ToArray();
                var pathAsString = $"{string.Join("/", parentPathAry)}/{node.Id}".TrimStart('/');

                // first check if we are already at max level
                if (parentPathAry.Length == maxDepth)
                    throw new InvalidOperationException($"Traversal stopped: maxDepth='{maxDepth.GetValueOrDefault(100)}' was reached at path='{pathAsString}'.");

                if (maxDepth.GetValueOrDefault(100) > parentPathAry.Length)
                    if (node.HasChildNodes)
                        continue;

                var (success, value) = node.TryGetValue<object>();

                yield return new KeyValuePair<string, object>(pathAsString, value);
            }
        }

        public static bool DeepEquals(this object left, object right)
        {
            if (ReferenceEquals(left, right))
                return true;

            var leftLeafEnumerator = left.Flatten().GetEnumerator();
            var rightLeafEnumerator = right.Flatten().GetEnumerator();

            (bool left, bool right) moveNextInStreams = (leftLeafEnumerator.MoveNext(), rightLeafEnumerator.MoveNext());
            do
            {
                if (!StringComparer.InvariantCulture.Equals(leftLeafEnumerator.Current.Key, rightLeafEnumerator.Current.Key))
                    return false;

                if (!EqualityComparer<Type>.Default.Equals(leftLeafEnumerator.Current.Value.GetType(), rightLeafEnumerator.Current.Value.GetType()))
                    return false;

                if (!EqualityComparer<object>.Default.Equals(leftLeafEnumerator.Current.Value, rightLeafEnumerator.Current.Value))
                    return false;

                moveNextInStreams = (leftLeafEnumerator.MoveNext(), rightLeafEnumerator.MoveNext());
                if (moveNextInStreams.left != moveNextInStreams.right)
                    return false;
            }
            while (moveNextInStreams.left && moveNextInStreams.right);

            return true;
        }

        public static DeepCompareResult DeepCompare(this object left, object right)
        {
            return left.DeepCompare(right.Flatten().ToDictionary(kv => kv.Key));
        }

        public static DeepCompareResult DeepCompare(this object left, IEnumerable<KeyValuePair<string, object>> right)
        {
            return left.DeepCompare(right.ToDictionary(kv => kv.Key));
        }

        private static DeepCompareResult DeepCompare(this object left, IDictionary<string, KeyValuePair<string, object>> rightLeaves)
        {
            // optimization removed: it doenst fill the 'EqualValues' list
            // if (ReferenceEquals(left, right))
            //     return new DeepCompareResult();

            var leftLeafEnumerator = left.Flatten().GetEnumerator();
            var compareResult = new DeepCompareResult();

            foreach (var leftLeaf in left.Flatten())
            {
                if (rightLeaves.TryGetValue(leftLeaf.Key, out var rightLeaf))
                {
                    DeepCompareLeaves(leftLeaf, rightLeaf, compareResult);
                    rightLeaves.Remove(leftLeaf.Key);
                }
                else
                {
                    compareResult.Missing.Right.Add(leftLeaf.Key);
                }
            }

            // add all uncompared left proerties to the result object
            return rightLeaves.Aggregate(compareResult, (cr, kv) =>
            {
                cr.Missing.Left.Add(kv.Key);
                return cr;
            });
        }

        private static void DeepCompareLeaves(KeyValuePair<string, object> leftLeaf, KeyValuePair<string, object> rightLeaf, DeepCompareResult compareResult)
        {
            if (!EqualityComparer<Type>.Default.Equals(GetTypeOfValueSafe(leftLeaf.Value), GetTypeOfValueSafe(rightLeaf.Value)))
                compareResult.Different.Types.Add(leftLeaf.Key);

            if (!EqualityComparer<object>.Default.Equals(leftLeaf.Value, rightLeaf.Value))
                compareResult.Different.Values.Add(leftLeaf.Key);

            compareResult.EqualValues.Add(leftLeaf.Key);
        }

        private static Type GetTypeOfValueSafe(object value) => value?.GetType() ?? typeof(object);

        public static HierarchyPath<string> PropertyPath<TRoot>(this TRoot root, Expression<Func<TRoot, object>> path)
        {
            return HierarchyPath.Create(PathSegments<TRoot>(root, path).Reverse());
        }

        public static IEnumerable<string> PathSegments<T>(T instance, Expression<Func<T, object>> access)
        {
            Expression current = access.Body;

            while (current != null)
            {
                if (current is UnaryExpression && current.NodeType == ExpressionType.Convert)
                {
                    current = ((UnaryExpression)current).Operand;
                }
                else if (current is MemberExpression)
                {
                    var currentMemberExpression = current as MemberExpression;
                    yield return currentMemberExpression.Member.Name;
                    current = currentMemberExpression.Expression;
                }
                else if (current is BinaryExpression)
                {
                    var currentBinaryExpression = current as BinaryExpression;
                    yield return ((ConstantExpression)currentBinaryExpression.Right).Value.ToString();
                    current = currentBinaryExpression.Left;
                }
                else if (current is ParameterExpression)
                {
                    var parameterExpression = current as ParameterExpression;
                    current = null;
                }
            }

            yield break;
        }

        /// <summary>
        /// Verifies if all properties of the given object have value != default(T). This is useful to
        /// check it test object is not completely filled with data.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static bool NoPropertyHasDefaultValue(this object root)
        {
            var h = ReflectedHierarchy.ReflectedHierarchyFactory.Create(root, new ReflectedHierarchyNodeFactory());
            foreach (var node in h.DescendantsAndSelf())
            {
                // a cast to object must never fail
                var (_, value) = node.TryGetValue<object>();

                // having a value of 'null' (ref type) is rejected
                if (value is null)
                    return false;

                var valueType = node.ValueType;

                // a value type having its default value is rejected
                if (valueType.IsValueType)
                    if (Activator.CreateInstance(node.ValueType).Equals(value))
                        return false;

                // an enumerable type without elements is rejected
                if (valueType.GetInterface(typeof(IEnumerable).Name) != null)
                    if (!((IEnumerable)value).GetEnumerator().MoveNext())
                        return false;
            }
            return true;
        }
    }
}