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
internal class BlockGenerator
{
    internal readonly Dictionary<string, ParameterExpression> accessibleExpressions = new Dictionary<string, ParameterExpression>();

    internal Expression DefineFunction(string name, params Expression[] functions) => Expression.Lambda(Expression.Block(accessibleExpressions.Values.ToList(), functions), name: name, null);

}

internal static class TokenGeneratorExtensions
{
    internal static Expression DefineVariable(this BlockGenerator blockGenerator, Type type, string name)
    {
        var var = Expression.Variable(type, name);

        blockGenerator.accessibleExpressions.Add(name, var);

        return var;
    }
    internal static Expression Assign(this BlockGenerator blockGenerator, string paramName, object value)
    {
        var var = blockGenerator.accessibleExpressions[paramName];

        return Expression.Assign(var, Expression.Constant(value));
    }
    internal static Expression Print(this BlockGenerator blockGenerator, string paramName)
    {
        var var = blockGenerator.accessibleExpressions[paramName];

        return Expression.Invoke((string x) => Console.WriteLine(x), var);
    }

    internal static Expression Run(this BlockGenerator blockGenerator, Expression lambdaExpression)
    {
        return LambdaExpression.Invoke(lambdaExpression);
    }
}

