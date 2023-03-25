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

        var program = DefineProgram(
                        DefineFunction(
                            new Expression[] {
                                DefineVariable(typeof(string), textVarName),
                                Print(textVarName)
                            }
                        )
                      );

        Console.WriteLine("End");

    }
}