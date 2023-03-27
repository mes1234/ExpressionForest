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
    internal readonly Dictionary<string, ParameterExpression> accessibleExpressions = new();
    internal readonly Dictionary<string, ParameterExpression> arguments = new();

    internal Expression DefineFunction(string name, IEnumerable<ParameterExpression>? _, params Expression[] functions)
    {
        var lambda = Expression.Lambda(
             Expression.Block(accessibleExpressions.Select(x => x.Value), functions),
             name: name,
             parameters: arguments.Select(x => x.Value).ToArray());

        return lambda;
    }

}

internal static class TokenGeneratorExtensions
{
    internal static IEnumerable<ParameterExpression>? DefineArguments(this BlockGenerator blockGenerator, params (Type type, string name)[] definitions)
    {
        var args = definitions.Select(x => Expression.Variable(x.type, x.name));

        foreach (var arg in args ?? Enumerable.Empty<ParameterExpression>())
        {
            blockGenerator.arguments.Add(arg.Name ?? "undefined", arg);
        }

        return args;
    }



    internal static Expression DefineVariable(this BlockGenerator blockGenerator, Type type, string name)
    {
        var var = Expression.Variable(type, name);

        blockGenerator.accessibleExpressions.Add(name, var);

        return var;
    }

    internal static Expression AssignLiteral(this BlockGenerator blockGenerator, string paramName, object value)
    {
        var var = blockGenerator.accessibleExpressions[paramName];

        return Expression.Assign(var, Expression.Constant(value));
    }

    internal static Expression Assign(this BlockGenerator blockGenerator, string paramNameTo, string paramNameFrom)
    {
        var left = blockGenerator.accessibleExpressions[paramNameTo];
        var right = blockGenerator.arguments[paramNameFrom];

        return Expression.Assign(left, right);
    }

    internal static Expression Print(this BlockGenerator blockGenerator, string paramName)
    {
        var var = blockGenerator.accessibleExpressions[paramName];

        return Expression.Invoke((string x) => Console.WriteLine(x), var);
    }

    internal static Expression Run(this BlockGenerator blockGenerator, Expression lambdaExpression, params string[] args)
    {
        var argsExpressions = blockGenerator.accessibleExpressions.Where(x => args.ToList().Contains(x.Key)).Select(x => x.Value).ToArray();

        return LambdaExpression.Invoke(lambdaExpression, argsExpressions);
    }
}

