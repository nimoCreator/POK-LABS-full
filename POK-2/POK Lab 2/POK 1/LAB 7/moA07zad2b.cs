using POK_1.assets;

namespace POK_1;

internal sealed class MoA07Zad2b : IZadanie
{
    public string Nazwa => "moA07zad2b";

    private readonly double _a;
    private readonly double _b;
    private readonly double _eps;

    public MoA07Zad2b(double a, double b, double eps)
    {
        _a = a;
        _b = b;
        _eps = eps;
    }

    public static double f(double x)
    {
        return Math.Exp(x - 1.0) + x * x - 2.0;
    }

    public void Run(TextWriter writer)
    {
        double a = _a;
        double b = _b;
        double x = double.NaN;
        double fx = double.NaN;
        int i = 0;

        writer.WriteLine("exp(x - 1) + x^2 - 2 = 0");
        writer.WriteLine("a = {0:f6}  b = {1:f6} eps = {2:e1}\n", _a, _b, _eps);
        writer.WriteLine("| {0,3} | {1,14} | {2,14} | {3,14} | {4,13} |", "i", "a", "b", "x", "f(x)");
        writer.WriteLine("+-{0,3}-+-{1,14}-+-{2,14}-+-{3,14}-+-{4,13}-+", "---", "--------------", "--------------", "--------------", "-------------");

        do
        {
            i++;
            x = Secant(a, b);
            fx = f(x);

            writer.WriteLine("| {0,3} | {1,14:f10} | {2,14:f10} | {3,14:f10} | {4,13:e6} |", i, a, b, x, fx);

            a = x;
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
        double fa = f(a);
        double fb = f(b);

        return (a * fb - b * fa) / (fb - fa);
    }
}
