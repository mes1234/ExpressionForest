using System.Linq.Expressions;

namespace ExpressionForest;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Beginning");

        var textVarName = "text";

        var subBlock = new BlockGenerator();
        var subProgram = subBlock.DefineFunction(textVarName,
            subBlock.DefineVariable(typeof(string), textVarName),
            subBlock.Assign(textVarName, "Hello World Inner!"),
            subBlock.Print(textVarName));


        var block = new BlockGenerator();
        var program = block.DefineFunction(textVarName,
             block.DefineVariable(typeof(string), textVarName),
             block.Assign(textVarName, "Hello World!"),
             block.Print(textVarName),
             block.Run(subProgram));


        if (program is LambdaExpression lambdaProgram)
        {
            var runner = lambdaProgram.Compile();

            runner.DynamicInvoke();
        }

        //var visitor = new GolangVisitor();

        //visitor.Visit(program);

        Console.WriteLine("End");

    }
}