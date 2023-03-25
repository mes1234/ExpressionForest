using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionForest;
internal static class TokenGenerator
{
    internal static Expression DefineProgram(Expression mainMethod) => Expression.Call(mainMethod);

    internal static Expression DefineFunction(Expression[] functions) => Expression.Block(functions);

    internal static Expression DefineVariable(Type type, string name) => Expression.Variable(type, name);

    internal static Expression Print(string name) => Expression.Invoke((string x) => Console.WriteLine(x), Expression.Constant(name));
}
