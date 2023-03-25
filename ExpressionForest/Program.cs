using Playground;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using static ExpressionForest.TokenGenerator;

namespace ExpressionForest;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Beginning");

        var textVarName = "text";

        var program = DefineFunction("main",
                                DefineVariable(typeof(string), textVarName),
                                Assign(textVarName, "Hello World!"),
                                Print(textVarName)
                        );

        var visitor = new GolangVisitor();

        visitor.Visit(program);

        Console.WriteLine("End");

    }
}