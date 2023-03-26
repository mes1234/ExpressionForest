using ExpressionForest;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionForest;
internal class TokenVisitor : ExpressionVisitor
{
    private readonly ICollection<Token> _tokens;

    private Stack<HashSet<string>> _blockVariablesStack = new Stack<HashSet<string>>();

    public TokenVisitor()
    {
        _tokens = new List<Token>();
    }

    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node)
    {
        return node?.NodeType switch
        {
            ExpressionType.Add => PrintAndReturn(node),
            ExpressionType.AddChecked => PrintAndReturn(node),
            ExpressionType.And => PrintAndReturn(node),
            ExpressionType.AndAlso => PrintAndReturn(node),
            ExpressionType.ArrayLength => PrintAndReturn(node),
            ExpressionType.ArrayIndex => PrintAndReturn(node),
            ExpressionType.Call => PrintAndReturn(node),
            ExpressionType.Coalesce => PrintAndReturn(node),
            ExpressionType.Conditional => PrintAndReturn(node),
            ExpressionType.Constant => PrintAndReturn(node),
            ExpressionType.Convert => PrintAndReturn(node),
            ExpressionType.ConvertChecked => PrintAndReturn(node),
            ExpressionType.Divide => PrintAndReturn(node),
            ExpressionType.Equal => PrintAndReturn(node),
            ExpressionType.ExclusiveOr => PrintAndReturn(node),
            ExpressionType.GreaterThan => PrintAndReturn(node),
            ExpressionType.GreaterThanOrEqual => PrintAndReturn(node),
            ExpressionType.Invoke => DefineInvocation((InvocationExpression)node),
            ExpressionType.Lambda => DefineLambda((LambdaExpression)node),
            ExpressionType.LeftShift => PrintAndReturn(node),
            ExpressionType.LessThan => PrintAndReturn(node),
            ExpressionType.LessThanOrEqual => PrintAndReturn(node),
            ExpressionType.ListInit => PrintAndReturn(node),
            ExpressionType.MemberAccess => PrintAndReturn(node),
            ExpressionType.MemberInit => PrintAndReturn(node),
            ExpressionType.Modulo => PrintAndReturn(node),
            ExpressionType.Multiply => PrintAndReturn(node),
            ExpressionType.MultiplyChecked => PrintAndReturn(node),
            ExpressionType.Negate => PrintAndReturn(node),
            ExpressionType.UnaryPlus => PrintAndReturn(node),
            ExpressionType.NegateChecked => PrintAndReturn(node),
            ExpressionType.New => PrintAndReturn(node),
            ExpressionType.NewArrayInit => PrintAndReturn(node),
            ExpressionType.NewArrayBounds => PrintAndReturn(node),
            ExpressionType.Not => PrintAndReturn(node),
            ExpressionType.NotEqual => PrintAndReturn(node),
            ExpressionType.Or => PrintAndReturn(node),
            ExpressionType.OrElse => PrintAndReturn(node),
            ExpressionType.Parameter => DefineParameter((ParameterExpression)node),
            ExpressionType.Power => PrintAndReturn(node),
            ExpressionType.Quote => PrintAndReturn(node),
            ExpressionType.RightShift => PrintAndReturn(node),
            ExpressionType.Subtract => PrintAndReturn(node),
            ExpressionType.SubtractChecked => PrintAndReturn(node),
            ExpressionType.TypeAs => PrintAndReturn(node),
            ExpressionType.TypeIs => PrintAndReturn(node),
            ExpressionType.Assign => DefineAssign((BinaryExpression)node),
            ExpressionType.Block => DefineBlock((BlockExpression)node),
            ExpressionType.DebugInfo => PrintAndReturn(node),
            ExpressionType.Decrement => PrintAndReturn(node),
            ExpressionType.Dynamic => PrintAndReturn(node),
            ExpressionType.Default => PrintAndReturn(node),
            ExpressionType.Extension => PrintAndReturn(node),
            ExpressionType.Goto => PrintAndReturn(node),
            ExpressionType.Increment => PrintAndReturn(node),
            ExpressionType.Index => PrintAndReturn(node),
            ExpressionType.Label => PrintAndReturn(node),
            ExpressionType.RuntimeVariables => PrintAndReturn(node),
            ExpressionType.Loop => PrintAndReturn(node),
            ExpressionType.Switch => PrintAndReturn(node),
            ExpressionType.Throw => PrintAndReturn(node),
            ExpressionType.Try => PrintAndReturn(node),
            ExpressionType.Unbox => PrintAndReturn(node),
            ExpressionType.AddAssign => PrintAndReturn(node),
            ExpressionType.AndAssign => PrintAndReturn(node),
            ExpressionType.DivideAssign => PrintAndReturn(node),
            ExpressionType.ExclusiveOrAssign => PrintAndReturn(node),
            ExpressionType.LeftShiftAssign => PrintAndReturn(node),
            ExpressionType.ModuloAssign => PrintAndReturn(node),
            ExpressionType.MultiplyAssign => PrintAndReturn(node),
            ExpressionType.OrAssign => PrintAndReturn(node),
            ExpressionType.PowerAssign => PrintAndReturn(node),
            ExpressionType.RightShiftAssign => PrintAndReturn(node),
            ExpressionType.SubtractAssign => PrintAndReturn(node),
            ExpressionType.AddAssignChecked => PrintAndReturn(node),
            ExpressionType.MultiplyAssignChecked => PrintAndReturn(node),
            ExpressionType.SubtractAssignChecked => PrintAndReturn(node),
            ExpressionType.PreIncrementAssign => PrintAndReturn(node),
            ExpressionType.PreDecrementAssign => PrintAndReturn(node),
            ExpressionType.PostIncrementAssign => PrintAndReturn(node),
            ExpressionType.PostDecrementAssign => PrintAndReturn(node),
            ExpressionType.TypeEqual => PrintAndReturn(node),
            ExpressionType.OnesComplement => PrintAndReturn(node),
            ExpressionType.IsTrue => PrintAndReturn(node),
            ExpressionType.IsFalse => PrintAndReturn(node),
            _ => base.Visit(node),
        };
    }

    private Expression PrintAndReturn(Expression node)
    {
        Console.WriteLine($"Obtained Expression Node : {node.NodeType}");

        return node;
    }

    private Expression DefineBlock(BlockExpression block)
    {
        // Init new block
        _blockVariablesStack.Push(new HashSet<string>());

        _tokens.Add(new BlockStartToken());

        base.Visit(block);

        _tokens.Add(new BlockStopToken());

        _blockVariablesStack.Pop();

        return block;
    }

    private Expression DefineAssign(BinaryExpression assignment)
    {
        _tokens.Add(new AssignmentToken(assignment.Left.ToString(), assignment.Right.ToString()));

        return assignment;
    }

    private Expression DefineLambda(LambdaExpression lambda)
    {
        // Add definition only to user defined functions
        if (lambda.Name != null)
        {
            _tokens.Add(new DefineCallableToken(lambda.Name ?? lambda.Body.ToString()));

            return base.Visit(lambda);
        }

        return lambda;
    }

    private Expression DefineParameter(ParameterExpression parameter)
    {
        //  This is not user defined parameter
        if (parameter.Name == null) return parameter;

        // This is duplicate within current block
        if (_blockVariablesStack.Peek().Contains(parameter.Name)) return parameter;

        // Add new Parameter to current block
        _blockVariablesStack.Peek().Add(parameter.Name);

        _tokens.Add(new VariableToken(parameter.Name, parameter.Type));

        return parameter;
    }

    private Expression DefineInvocation(InvocationExpression invocation)
    {
        var lambdaExpr = ((LambdaExpression)invocation.Expression);

        string methodName = "";

        string? argName = null;

        if (lambdaExpr.Body is MethodCallExpression methodCall)
        {
            methodName = methodCall.Method.Name;

            var args = invocation.Arguments.FirstOrDefault();

            if (args != null)
            {
                argName = ((ParameterExpression)args).Name;
            }
        }
        else
        {
            methodName = lambdaExpr.Name ?? string.Empty;
        }



        _tokens.Add(new CallToken(methodName, argName));

        return invocation;
    }

}




