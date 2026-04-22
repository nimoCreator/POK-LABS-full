using POK_1.assets;

namespace POK_1;

internal sealed class MoA02Zad3B : IZadanie
{
    public string Nazwa => "moA02zad3b";

    private readonly double _x0;
    private readonly int _n;

    public MoA02Zad3B(double x0, int n)
    {
        _x0 = x0;
        _n = n;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("x0 = {0,3:f1}  n = {1}\n", _x0, _n);
        writer.WriteLine("| {0,3} | {1,12} |", "i", "x");
        writer.WriteLine("+-{0,3}-+-{1,12}-+", "---", "------------");

        double x = _x0;
        writer.WriteLine("| {0,3} | {1,12:f10} |", 0, x);

        for (int i = 1; i < _n; i++)
        {
            x = 2 * Math.Sin(x);
            writer.WriteLine("| {0,3} | {1,12:f10} |", i, x);
        }

        writer.WriteLine();
        writer.WriteLine("x = x({0}) = {1:f10}", _n, x);
        writer.WriteLine("|(2*sin(x) - x)/x| = {0:e2}", Math.Abs((2 * Math.Sin(x) - x) / x));
        writer.WriteLine();
    }
}
