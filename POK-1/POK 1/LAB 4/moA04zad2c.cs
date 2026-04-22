using POK_1.assets;

namespace POK_1;

internal sealed class MoA04Zad2c : IZadanie
{
    public string Nazwa => "moA04zad2c";

    private readonly (double A, int N)[] _cases;

    public MoA04Zad2c(params (double A, int N)[] cases)
    {
        _cases = cases;
    }

    public void Run(TextWriter writer)
    {
        foreach (var expCase in _cases)
        {
            writer.WriteLine("a = {0:f1}   N = {1}\n", expCase.A, expCase.N);
            writer.WriteLine("| {0,6} | {1,15} |", "n", "y");
            writer.WriteLine("+-{0,6}-+-{1,15}-+", "------", "---------------");

            double positiveArgument = -expCase.A;
            double sum = 1.0;
            double term = 1.0;
            double y = 1.0 / sum;
            writer.WriteLine("| {0,6} | {1,15:e7} |", 0, y);

            for (int n = 1; n <= expCase.N; n++)
            {
                term *= positiveArgument / n;
                sum += term;
                y = 1.0 / sum;
                writer.WriteLine("| {0,6} | {1,15:e7} |", n, y);
            }

            writer.WriteLine();
            writer.WriteLine("y({0}) = {1:g15}", expCase.N, y);
            writer.WriteLine("  exp = {0:g15}", Math.Exp(expCase.A));
            writer.WriteLine();
        }
    }
}
