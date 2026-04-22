using POK_1.assets;

namespace POK_1;

internal sealed class MoA07Zad1a : IZadanie
{
    public string Nazwa => "moA07zad1a";

    private readonly double _a;
    private readonly double _b;
    private readonly int _n;

    public MoA07Zad1a(double a, double b, int n)
    {
        _a = a;
        _b = b;
        _n = n;
    }

    public void Run(TextWriter writer)
    {
        double a = _a;
        double b = _b;
        double x = double.NaN;

        writer.WriteLine("tan^3(x) - 4*tan(x) = 0");
        writer.WriteLine("a = {0:f6}  b = {1:f6}  n = {2}\n", _a, _b, _n);
        writer.WriteLine("| {0,3} | {1,14} | {2,14} | {3,14} | {4,13} |", "i", "a", "b", "x", "f(x)");
        writer.WriteLine("+-{0,3}-+-{1,14}-+-{2,14}-+-{3,14}-+-{4,13}-+", "---", "--------------", "--------------", "--------------", "-------------");

        for (int i = 1; i <= _n; i++)
        {
            x = 0.5 * (a + b);
            double fx = MoA07Functions.Zad1(x);

            writer.WriteLine("| {0,3} | {1,14:f10} | {2,14:f10} | {3,14:f10} | {4,13:e6} |", i, a, b, x, fx);

            if (MoA07Functions.Zad1(a) * fx <= 0.0)
            {
                b = x;
            }
            else
            {
                a = x;
            }
        }

        writer.WriteLine();
        writer.WriteLine("x({0}) = {1:g15}", _n, x);
        writer.WriteLine("f(x) = {0:e6}", MoA07Functions.Zad1(x));
        writer.WriteLine();
    }
}
