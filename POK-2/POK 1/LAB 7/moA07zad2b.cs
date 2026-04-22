using POK_1.assets;

namespace POK_1;

internal sealed class MoA07Zad2b : IZadanie
{
    public string Nazwa => "moA07zad2b";

    private readonly double _a;
    private readonly double _b;
    private readonly bool _fixedA;
    private readonly double _eps;

    public MoA07Zad2b(double a, double b, bool fixedA, double eps)
    {
        _a = a;
        _b = b;
        _fixedA = fixedA;
        _eps = eps;
    }

    public void Run(TextWriter writer)
    {
        double a = _a;
        double b = _b;
        double x = double.NaN;
        double fx = double.NaN;
        int i = 0;

        writer.WriteLine("exp(x - 1) + x^2 - 2 = 0");
        writer.WriteLine("a = {0:f6}  b = {1:f6}  punkt staly = {2}  eps = {3:e1}\n", _a, _b, _fixedA ? "a" : "b", _eps);
        writer.WriteLine("| {0,3} | {1,14} | {2,14} | {3,14} | {4,13} |", "i", "a", "b", "x", "f(x)");
        writer.WriteLine("+-{0,3}-+-{1,14}-+-{2,14}-+-{3,14}-+-{4,13}-+", "---", "--------------", "--------------", "--------------", "-------------");

        do
        {
            i++;
            x = Secant(a, b);
            fx = MoA07Functions.Zad2(x);

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
        while (Math.Abs(fx) > _eps && i < 1000);

        writer.WriteLine();
        writer.WriteLine("x = {0:g15}", x);
        writer.WriteLine("f(x) = {0:e6}", fx);
        writer.WriteLine("liczba krokow = {0}", i);
        writer.WriteLine();
    }

    private static double Secant(double a, double b)
    {
        double fa = MoA07Functions.Zad2(a);
        double fb = MoA07Functions.Zad2(b);

        return (a * fb - b * fa) / (fb - fa);
    }
}
