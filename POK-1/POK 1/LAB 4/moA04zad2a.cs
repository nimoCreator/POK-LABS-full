using POK_1.assets;

namespace POK_1;

internal sealed class MoA04Zad2a : IZadanie
{
    public string Nazwa => "moA04zad2a";

    private readonly (double A, int N, int SummaryStep)[] _cases;

    public MoA04Zad2a(params (double A, int N, int SummaryStep)[] cases)
    {
        _cases = cases;
    }

    public void Run(TextWriter writer)
    {
        foreach (var expCase in _cases)
        {
            writer.WriteLine("a = {0:f1}   N = {1}\n", expCase.A, expCase.N);
            writer.WriteLine("| {0,6} | {1,14} |", "n", "x");
            writer.WriteLine("+-{0,6}-+-{1,14}-+", "------", "--------------");

            double x = 0.0;
            for (int n = 1; n <= expCase.N; n++)
            {
                x = Math.Pow(1.0 + (expCase.A / n), n);
                if (ShouldPrint(n, expCase.N, expCase.SummaryStep))
                {
                    writer.WriteLine("| {0,6} | {1,14:e7} |", n, x);
                }
            }

            writer.WriteLine();
            writer.WriteLine("x({0}) = {1:g15}", expCase.N, x);
            writer.WriteLine("     exp = {0:g15}", Math.Exp(expCase.A));
            writer.WriteLine();
        }
    }

    private static bool ShouldPrint(int n, int nMax, int summaryStep)
    {
        return n <= 10 || n % summaryStep == 0 || n == nMax;
    }
}
