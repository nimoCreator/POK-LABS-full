using POK_1.assets;

namespace POK_1;

internal sealed class MoA07Zad3b : IZadanie
{
    public string Nazwa => "moA07zad3b";

    private readonly double _a;
    private readonly double _b;
    private readonly double _eps;

    public MoA07Zad3b(double a, double b, double eps)
    {
        _a = a;
        _b = b;
        _eps = eps;
    }

    public void Run(TextWriter writer)
    {
        double x = 0.5 * (_a + _b);
        double fx = MoA07Functions.Zad3(x);
        int i = 0;

        writer.WriteLine("2^(x^2 - 5*x + 10)*log(2)*(2*x - 5) = 0");
        writer.WriteLine("a = {0:f6}  b = {1:f6}  eps = {2:e1}\n", _a, _b, _eps);
        writer.WriteLine("| {0,3} | {1,14} | {2,13} |", "i", "x", "f(x)");
        writer.WriteLine("+-{0,3}-+-{1,14}-+-{2,13}-+", "---", "--------------", "-------------");
        writer.WriteLine("| {0,3} | {1,14:f10} | {2,13:e6} |", i, x, fx);

        while (Math.Abs(fx) > _eps && i < 1000)
        {
            i++;
            x -= fx / MoA07Functions.Zad3Derivative(x);
            fx = MoA07Functions.Zad3(x);
            writer.WriteLine("| {0,3} | {1,14:f10} | {2,13:e6} |", i, x, fx);
        }

        writer.WriteLine();
        writer.WriteLine("x = {0:g15}", x);
        writer.WriteLine("f(x) = {0:e6}", fx);
        writer.WriteLine("liczba krokow = {0}", i);
        writer.WriteLine();
    }
}
