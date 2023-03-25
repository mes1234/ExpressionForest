using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionForest;
internal class GolangBuilder
{

}

internal abstract class Token
{
    protected readonly TokenType _tokenType;
    protected Token(TokenType tokenType)
    {
        _tokenType = tokenType;
    }
}

internal class BlockToken : Token
{
    private readonly string _name;

    private readonly ICollection<Token> _innerTokens;

    public BlockToken(string name) : base(TokenType.FunctionDefinition)
    {
        _name = name;
        _innerTokens = new List<Token>();
    }

    public void Attach(Token token) => _innerTokens.Add(token);

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
    private readonly VariableToken _left;
    private readonly object _right;

    public AssignmentToken(VariableToken variableToken, object value) : base(TokenType.Assignment)
    {
        _left = variableToken;
        _right = value;

        //TODO validate type match!
    }
}

/// <summary>
/// Only no argument calls are supported for now
/// </summary>
internal class CallToken : Token
{
    private readonly BlockToken _blockToken;

    public CallToken(BlockToken blockToken) : base(TokenType.Call)
    {
        _blockToken = blockToken;
    }
}

internal enum TokenType
{
    VariableDefinition,
    FunctionDefinition,
    Call,
    Assignment
}

