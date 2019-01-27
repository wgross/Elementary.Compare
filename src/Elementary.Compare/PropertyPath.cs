using Elementary.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Elementary.Compare
{
    public static class PropertyPath
    {
        public static HierarchyPath<string> Make<TRoot>(TRoot root, Expression<Func<TRoot, object>> path)
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

        public static IEnumerable<string> PathSegments<T>(Expression<Func<T, object>> access)
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
    }
}