using POK_1.assets;

namespace POK_1;

internal sealed class MoA02Zad3C : IZadanie
{
    public string Nazwa => "moA02zad3c";

    private readonly double _x0;
    private readonly int _n;

    public MoA02Zad3C(double x0, int n)
    {
        _x0 = x0;
        _n = n;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("x0 = {0,3:f2}  n = {1}\n", _x0, _n);
        writer.WriteLine("| {0,3} | {1,14} |", "i", "x");
        writer.WriteLine("+-{0,3}-+-{1,14}-+", "---", "--------------");

        double x = _x0;
        writer.WriteLine("| {0,3} | {1,14:f10} |", 0, x);

        for (int i = 1; i < _n; i++)
        {
            double f = 2 * Math.Sin(x) - x;
            double fp = 2 * Math.Cos(x) - 1;
            x -= f / fp;
            writer.WriteLine("| {0,3} | {1,14:f10} |", i, x);
        }

        writer.WriteLine();
        writer.WriteLine("x = x({0}) = {1:f10}", _n, x);
        writer.WriteLine("|(2*sin(x) - x)/x| = {0:e2}", Math.Abs((2 * Math.Sin(x) - x) / x));
        writer.WriteLine();
    }
}
