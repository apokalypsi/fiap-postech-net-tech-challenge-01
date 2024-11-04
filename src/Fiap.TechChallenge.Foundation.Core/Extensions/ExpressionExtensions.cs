using System.Linq.Expressions;
using System.Reflection;

namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class ExpressionExtensions
{
    public static PropertyInfo GetPropertyInfo<T, TValue>(this Expression<Func<T, TValue>> expression)
    {
        if (expression is null)
            throw new ArgumentNullException(nameof(expression),
                "Expected a property expression, but found <null>.");

        var memberInfo = AttemptToGetMemberInfoFromCastExpression(expression) ??
                         AttemptToGetMemberInfoFromMemberExpression(expression);

        if (!(memberInfo is PropertyInfo propertyInfo))
            throw new ArgumentException(
                "Cannot use <" + expression.Body + "> when a property expression is expected.",
                nameof(expression));

        return propertyInfo;
    }

    private static MemberInfo AttemptToGetMemberInfoFromMemberExpression<T, TValue>(
        Expression<Func<T, TValue>> expression)
    {
        if (expression.Body is MemberExpression memberExpression) return memberExpression.Member;

        return null;
    }

    private static MemberInfo AttemptToGetMemberInfoFromCastExpression<T, TValue>(
        Expression<Func<T, TValue>> expression)
    {
        if (expression.Body is UnaryExpression castExpression)
            return ((MemberExpression)castExpression.Operand).Member;

        return null;
    }

    /// <summary>
    ///     Gets a dotted path of property names representing the property expression. E.g. Parent.Child.Sibling.Name.
    /// </summary>
    public static string GetMemberPath<TDeclaringType, TPropertyType>(
        this Expression<Func<TDeclaringType, TPropertyType>> expression)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression), "Expected an expression, but found <null>.");

        var segments = new List<string>();
        Expression node = expression;

        var unsupportedExpressionMessage = $"Expression <{expression.Body}> cannot be used to select a member.";

        while (node != null)
            switch (node.NodeType)
            {
                case ExpressionType.Lambda:
                    node = ((LambdaExpression)node).Body;
                    break;

                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var unaryExpression = (UnaryExpression)node;
                    node = unaryExpression.Operand;
                    break;

                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)node;
                    node = memberExpression.Expression;

                    segments.Add(memberExpression.Member.Name);
                    break;

                case ExpressionType.ArrayIndex:
                    var binaryExpression = (BinaryExpression)node;
                    var constantExpression = (ConstantExpression)binaryExpression.Right;
                    node = binaryExpression.Left;

                    segments.Add("[" + constantExpression.Value + "]");
                    break;

                case ExpressionType.Parameter:
                    node = null;
                    break;

                case ExpressionType.Call:
                    var methodCallExpression = (MethodCallExpression)node;
                    if (methodCallExpression.Method.Name != "get_Item" ||
                        methodCallExpression.Arguments.Count != 1 ||
                        !(methodCallExpression.Arguments[0] is ConstantExpression))
                        throw new ArgumentException(unsupportedExpressionMessage, nameof(expression));

                    constantExpression = (ConstantExpression)methodCallExpression.Arguments[0];
                    node = methodCallExpression.Object;
                    segments.Add("[" + constantExpression.Value + "]");
                    break;

                default:
                    throw new ArgumentException(unsupportedExpressionMessage, nameof(expression));
            }

        var reversedSegments = segments.AsEnumerable().Reverse().ToArray();
        var segmentPath = string.Join(".", reversedSegments);
        return segmentPath.Replace(".[", "[");
    }

    internal static string GetMethodName(Expression<Action> action)
    {
        return ((MethodCallExpression)action.Body).Method.Name;
    }
}