using POK_1.assets;

namespace POK_1;

internal sealed class MoA07Zad3a : IZadanie
{
    public string Nazwa => "moA07zad3a";

    private readonly double _a;
    private readonly double _b;
    private readonly int _n;

    public MoA07Zad3a(double a, double b, int n)
    {
        _a = a;
        _b = b;
        _n = n;
    }

    public void Run(TextWriter writer)
    {
        double x = 0.5 * (_a + _b);

        writer.WriteLine("2^(x^2 - 5*x + 10)*log(2)*(2*x - 5) = 0");
        writer.WriteLine("a = {0:f6}  b = {1:f6}  n = {2}\n", _a, _b, _n);
        writer.WriteLine("| {0,3} | {1,14} | {2,13} |", "i", "x", "f(x)");
        writer.WriteLine("+-{0,3}-+-{1,14}-+-{2,13}-+", "---", "--------------", "-------------");
        writer.WriteLine("| {0,3} | {1,14:f10} | {2,13:e6} |", 0, x, f(x));

        for (int i = 1; i <= _n; i++)
        {
            x -= f(x) / df(x);
            writer.WriteLine("| {0,3} | {1,14:f10} | {2,13:e6} |", i, x, f(x));
        }

        writer.WriteLine();
        writer.WriteLine("x({0}) = {1:g15}", _n, x);
        writer.WriteLine("f(x) = {0:e6}", f(x));
        writer.WriteLine();
    }

    public static double f(double x)
    {
        double log2 = Math.Log(2.0);
        double p = x * x - 5.0 * x + 10.0;
        return Math.Pow(2.0, p) * log2 * (2.0 * x - 5.0);
    }

    public static double df(double x)
    {
        double log2 = Math.Log(2.0);
        double p = x * x - 5.0 * x + 10.0;
        double n = 2.0 * x - 5.0;

        return log2 * Math.Pow(2.0, p) * (log2 * n * n + 2);
    }
}
