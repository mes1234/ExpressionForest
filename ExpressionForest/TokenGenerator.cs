using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpressionForest;
internal static class TokenGenerator
{
    private static readonly Dictionary<string, ParameterExpression> _accessibleExpressions = new Dictionary<string, ParameterExpression>();

    internal static Expression DefineFunction(string name, params Expression[] functions) => Expression.Lambda(Expression.Block(_accessibleExpressions.Values.ToList(), functions), name: name, null);
    internal static Expression DefineVariable(Type type, string name)
    {
        var var = Expression.Variable(type, name);

        _accessibleExpressions.Add(name, var);

        return var;
    }
    internal static Expression Assign(string paramName, object value)
    {
        var var = _accessibleExpressions[paramName];

        return Expression.Assign(var, Expression.Constant(value));
    }
    internal static Expression Print(string paramName)
    {
        var var = _accessibleExpressions[paramName];

        return Expression.Invoke((string x) => Console.WriteLine(x), var);
    }
}

