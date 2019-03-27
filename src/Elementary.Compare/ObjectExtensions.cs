﻿using Elementary.Compare.ReflectedHierarchy;
using Elementary.Hierarchy;
using Elementary.Hierarchy.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Elementary.Compare
{
    public static class ObjectExtensions
    {
        //public static bool DeepEquals(this object left, object right)
        //{
        //    if (ReferenceEquals(left, right))
        //        return true;

        //    var leftLeafEnumerator = left.Flatten().GetEnumerator();
        //    var rightLeafEnumerator = right.Flatten().GetEnumerator();

        //    (bool left, bool right) moveNextInStreams = (leftLeafEnumerator.MoveNext(), rightLeafEnumerator.MoveNext());
        //    do
        //    {
        //        if (!StringComparer.InvariantCulture.Equals(leftLeafEnumerator.Current.Key, rightLeafEnumerator.Current.Key))
        //            return false;

        //        var leftType = leftLeafEnumerator.Current.Value.GetType();
        //        if (!EqualityComparer<Type>.Default.Equals(leftType, rightLeafEnumerator.Current.Value.GetType()))
        //            return false;

        //        // types are equal

        //        // if type is enumerable, check if both are empty
        //        if (leftType != typeof(string) && leftType.GetInterface(nameof(IEnumerable)) != null)
        //        {
        //            return EnumerableEqualityComparer.Default.Equals(leftLeafEnumerator.Current.Value as IEnumerable, rightLeafEnumerator.Current.Value as IEnumerable);
        //        }

        //        if (!EqualityComparer<object>.Default.Equals(leftLeafEnumerator.Current.Value, rightLeafEnumerator.Current.Value))
        //            return false;

        //        moveNextInStreams = (leftLeafEnumerator.MoveNext(), rightLeafEnumerator.MoveNext());
        //        if (moveNextInStreams.left != moveNextInStreams.right)
        //            return false;
        //    }
        //    while (moveNextInStreams.left && moveNextInStreams.right);

        //    return true;
        //}

        public static DiffResult Diff<TLeft, TRight>(this TLeft left, TRight right)
            where TLeft : class
            where TRight : class
        {
            // optimization removed: it doesnt fill the 'EqualValues' list.
            // for optimization it might be better to travers one instance and add the leaves to the equals list.
            // if (ReferenceEquals(left, right))
            //     return new DiffResult();

            return DiffFlattendLeaves(
                left is IFlattened fl ? fl : left.Flatten().Build(),
                right is IFlattened fr ? fr : right.Flatten().Build(),
                new DiffResult());
        }

        //private static DiffResult DiffFlattendInstances(this object left, IDictionary<string, KeyValuePair<string, object>> rightLeaves, DiffResult diffResult)
        //{
        //    foreach (var leftLeaf in left.Flatten())
        //    {
        //        if (rightLeaves.TryGetValue(leftLeaf.Key, out var rightLeaf))
        //        {
        //            DiffFlattenedLeavesValues(leftLeaf.Key, leftLeaf, rightLeaf, diffResult);
        //            rightLeaves.Remove(leftLeaf.Key);
        //        }
        //        else
        //        {
        //            diffResult.Missing.Right.Add(leftLeaf.Key);
        //        }
        //    }

        //    // add all uncompared left proerties to the diff result as 'missing'
        //    return rightLeaves.Aggregate(diffResult, (cr, kv) =>
        //    {
        //        cr.Missing.Left.Add(kv.Key);
        //        return cr;
        //    });
        //}

        private static DiffResult DiffFlattendLeaves(IFlattened flattenedLeft, IFlattened flattenedRight, DiffResult diffResult)
        {
            var flattendRightLeaves = flattenedRight.ToDictionary(kv => kv.Key);

            foreach (var leftLeaf in flattenedLeft)
            {
                if (flattendRightLeaves.TryGetValue(leftLeaf.Key, out var rightLeaf))
                {
                    DiffFlattenedLeavesValues(leftLeaf.Key, leftLeaf.Value, rightLeaf.Value, diffResult);
                    flattendRightLeaves.Remove(leftLeaf.Key);
                }
                else
                {
                    diffResult.Missing.Right.Add(leftLeaf.Key);
                }
            }

            // add all uncompared left properties to the result object
            return flattendRightLeaves.Aggregate(diffResult, (cr, kv) =>
            {
                cr.Missing.Left.Add(kv.Key);
                return cr;
            });
        }

        /// <summary>
        /// Compares values fro a supposedly equal leaves of a flattened objects. Leaves my differ in type and value.
        /// THe keys of the leaf is added to the diff result if  onethe comparison fails.
        /// </summary>
        /// <param name="leftLeafValue"></param>
        /// <param name="rightLeafValue"></param>
        /// <param name="compareResult"></param>
        private static void DiffFlattenedLeavesValues(string key, object leftLeafValue, object rightLeafValue, DiffResult compareResult)
        {
            // compare types of two values

            var leftType = GetTypeOfValueSafe(leftLeafValue);
            if (!EqualityComparer<Type>.Default.Equals(leftType, GetTypeOfValueSafe(rightLeafValue)))
                compareResult.Different.Types.Add(key);

            // compare the values.

            // if type is enumerable, check if both are empty
            if (leftType != typeof(string) && leftType.GetInterface(nameof(IEnumerable)) != null)
            {
                if (!EnumerableEqualityComparer.Default.Equals(leftLeafValue as IEnumerable, rightLeafValue as IEnumerable))
                    compareResult.Different.Values.Add(key);
            }
            else
            {
                // handle value as scalar value.
                if (!EqualityComparer<object>.Default.Equals(leftLeafValue, rightLeafValue))
                    compareResult.Different.Values.Add(key);
            }
            compareResult.EqualValues.Add(key);
        }

        private static Type GetTypeOfValueSafe(object value) => value?.GetType() ?? typeof(object);

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