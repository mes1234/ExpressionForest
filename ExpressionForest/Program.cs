using System.Linq.Expressions;

namespace ExpressionForest;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Beginning");

        var innerTextVarName = "innerText";

        var subBlock = new BlockGenerator();
        var subProgram = subBlock.DefineFunction("hello_inner",
            subBlock.DefineVariable(typeof(string), innerTextVarName),
            subBlock.Assign(innerTextVarName, "Hello World Inner!"),
            subBlock.Print(innerTextVarName));

        var outerTextVarName = "outerText";

        var block = new BlockGenerator();
        var program = block.DefineFunction("main",
             subProgram,
             block.DefineVariable(typeof(string), outerTextVarName),
             block.Assign(outerTextVarName, "Hello World!"),
             block.Print(outerTextVarName),
             block.Run(subProgram));


        if (program is LambdaExpression lambdaProgram)
        {
            var runner = lambdaProgram.Compile();

            runner.DynamicInvoke();
        }

        var visitor = new GolangVisitor();

        visitor.Visit(program);

        Console.WriteLine("End");

    }
}