using POK_1.assets;

namespace POK_1;

internal sealed class MoA04Zad2b : IZadanie
{
    public string Nazwa => "moA04zad2b";

    private readonly (double A, int N)[] _cases;

    public MoA04Zad2b(params (double A, int N)[] cases)
    {
        _cases = cases;
    }

    public void Run(TextWriter writer)
    {
        foreach (var expCase in _cases)
        {
            writer.WriteLine("a = {0:f1}   N = {1}\n", expCase.A, expCase.N);
            writer.WriteLine("| {0,6} | {1,15} |", "n", "x");
            writer.WriteLine("+-{0,6}-+-{1,15}-+", "------", "---------------");

            double sum = 1.0;
            double term = 1.0;
            writer.WriteLine("| {0,6} | {1,15:e7} |", 0, sum);

            for (int n = 1; n <= expCase.N; n++)
            {
                term *= expCase.A / n;
                sum += term;
                writer.WriteLine("| {0,6} | {1,15:e7} |", n, sum);
            }

            writer.WriteLine();
            writer.WriteLine("x({0}) = {1:g15}", expCase.N, sum);
            writer.WriteLine("  exp = {0:g15}", Math.Exp(expCase.A));
            writer.WriteLine();
        }
    }
}
