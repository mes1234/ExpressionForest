
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Playground;
public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Playground started");



        var visitor = new InspectingVisitor();

        visitor.Visit(expression.Expression);

        var dump = expression.Select(x => x.ToString());

        foreach (var x in dump)
        {
            Console.WriteLine(x);
        }

    }
}
