using System.Linq.Expressions;
using static ExpressionForest.TokenGeneratorExtensions;

namespace ExpressionForest;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Beginning");

        var innerTextVarArg = "arg1";
        var innerTextVarName = "innerText";

        var subBlock = new BlockGenerator();
        var subProgram = subBlock.DefineFunction(
            "hello_inner",
            subBlock.DefineArguments((typeof(string), innerTextVarArg)),
            subBlock.DefineVariable(typeof(string), innerTextVarName),
            subBlock.Assign(innerTextVarName, innerTextVarArg),
            subBlock.Print(innerTextVarName));

        var outerTextVarName = "outerText";

        var block = new BlockGenerator();
        var program = block.DefineFunction("main",
             null,
             subProgram,
             block.DefineVariable(typeof(string), outerTextVarName),
             block.AssignLiteral(outerTextVarName, "Hello World Inner"),
             block.Run(subProgram, outerTextVarName),
              block.AssignLiteral(outerTextVarName, "Hello World"),
             block.Print(outerTextVarName),
             block.Run(subProgram, outerTextVarName));


        if (program is LambdaExpression lambdaProgram)
        {
            var runner = lambdaProgram.Compile();

            runner.DynamicInvoke();
        }

        var visitor = new TokenVisitor();

        visitor.Visit(program);

        Console.WriteLine("End");

    }
}