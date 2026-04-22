using POK_1.assets;

namespace POK_1;

internal sealed class MoA07Zad2a : IZadanie
{
    public string Nazwa => "moA07zad2a";

    private readonly double _a;
    private readonly double _b;
    private readonly bool _fixedA;
    private readonly int _n;

    public MoA07Zad2a(double a, double b, bool fixedA, int n)
    {
        _a = a;
        _b = b;
        _fixedA = fixedA;
        _n = n;
    }

    public void Run(TextWriter writer)
    {
        double a = _a;
        double b = _b;
        double x = double.NaN;

        writer.WriteLine("exp(x - 1) + x^2 - 2 = 0");
        writer.WriteLine("a = {0:f6}  b = {1:f6}  punkt staly = {2}  n = {3}\n", _a, _b, _fixedA ? "a" : "b", _n);
        writer.WriteLine("| {0,3} | {1,14} | {2,14} | {3,14} | {4,13} |", "i", "a", "b", "x", "f(x)");
        writer.WriteLine("+-{0,3}-+-{1,14}-+-{2,14}-+-{3,14}-+-{4,13}-+", "---", "--------------", "--------------", "--------------", "-------------");

        for (int i = 1; i <= _n; i++)
        {
            x = Secant(a, b);
            double fx = MoA07Functions.Zad2(x);

            writer.WriteLine("| {0,3} | {1,14:f10} | {2,14:f10} | {3,14:f10} | {4,13:e6} |", i, a, b, x, fx);

            if (_fixedA)
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
        writer.WriteLine("f(x) = {0:e6}", MoA07Functions.Zad2(x));
        writer.WriteLine();
    }

    private static double Secant(double a, double b)
    {
        double fa = MoA07Functions.Zad2(a);
        double fb = MoA07Functions.Zad2(b);

        return (a * fb - b * fa) / (fb - fa);
    }
}
