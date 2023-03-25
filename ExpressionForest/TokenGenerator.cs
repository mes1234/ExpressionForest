using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionForest;
internal static class TokenGenerator
{
    internal static Expression DefineFunction(string name, params Expression[] functions) => Expression.Lambda(Expression.Block(functions), name: name, null);
    internal static Expression DefineVariable(Type type, string name) => Expression.Variable(type, name);
    internal static Expression Assign(string name, object value) => Expression.Assign((Expression x) => name, (Expression y) => value);
    internal static Expression Print(string name) => Expression.Invoke((string x) => Console.WriteLine(x), Expression.Constant(name));
}
