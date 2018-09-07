using System;
using System.Collections.Generic;
using System.Text;

namespace Elementary.Compare.ReflectedHierarchy
{
    public class ReflectedHierarchyNodeEqualityComparer : IEqualityComparer<IReflectedHierarchyNode>
    {
        public bool Equals(IReflectedHierarchyNode x, IReflectedHierarchyNode y)
        {
            var left = ((ReflectedHierarchyNodeBase)x).State.NodeValue;
            var right = ((ReflectedHierarchyNodeBase)x).State.NodeValue;

            if (left == null && right == null)
                return false;

            return object.ReferenceEquals(left, right);
        }

        public int GetHashCode(IReflectedHierarchyNode obj)
        {
            return ((ReflectedHierarchyNodeBase)obj).State.NodeValue?.GetHashCode() ?? ((ReflectedHierarchyNodeBase)obj).State.GetHashCode();
        }
    }
}
