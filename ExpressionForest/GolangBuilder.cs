using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpressionForest;

internal abstract class Token
{
    protected readonly TokenType _tokenType;
    protected Token(TokenType tokenType)
    {
        _tokenType = tokenType;
    }
}

internal class BlockStartToken : Token
{
    public BlockStartToken() : base(TokenType.FunctionDefinitionStart)
    {

    }
}
internal class BlockStopToken : Token
{
    public BlockStopToken() : base(TokenType.FunctionDefinitionEnd)
    {

    }
}

internal class VariableToken : Token
{
    private readonly string _name;
    private readonly Type _type;

    public VariableToken(string name, Type type) : base(TokenType.VariableDefinition)
    {
        _name = name;
        _type = type;
    }
}

internal class AssignmentToken : Token
{
    private readonly string _left;
    private readonly string _right;

    public AssignmentToken(string left, string right) : base(TokenType.Assignment)
    {
        _left = left;
        _right = right;

        //TODO validate type match!
    }
}

/// <summary>
/// Only no argument calls are supported for now
/// </summary>
internal class DefineCallableToken : Token
{
    private readonly string _name;
    public DefineCallableToken(string name) : base(TokenType.CallableDefinition)
    {
        _name = name;
    }
}
/// <summary>
/// Only no argument calls are supported for now
/// </summary>
internal class CallToken : Token
{
    private readonly string _name;
    public CallToken(string name) : base(TokenType.Call)
    {
        _name = name;
    }
}

internal enum TokenType
{
    VariableDefinition,
    FunctionDefinitionStart,
    FunctionDefinitionEnd,
    Call,
    CallableDefinition,
    Assignment
}

