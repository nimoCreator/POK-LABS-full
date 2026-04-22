using POK_1.assets;

namespace POK_1;

internal sealed class MoA03Zad4 : IZadanie
{
    public string Nazwa => "moA03zad4";

    private readonly int _nMax;
    private readonly double _x0;
    private readonly double _x1;

    public MoA03Zad4(int nMax, double x0, double x1)
    {
        _nMax = nMax;
        _x0 = x0;
        _x1 = x1;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("N = {0}\n", _nMax);
        writer.WriteLine("| {0,3} | {1,14} | {2,14} |", "n", "x", "pow");
        writer.WriteLine("+-{0,3}-+-{1,14}-+-{2,14}-+", "---", "--------------", "--------------");

        double previous = _x0;
        double current = _x1;

        for (int n = 2; n <= _nMax; n++)
        {
            double next = (13.0 / 3.0) * current - (4.0 / 3.0) * previous;
            double exact = Math.Pow(1.0 / 3.0, n);

            writer.WriteLine("| {0,3} | {1,14:e6} | {2,14:e6} |", n, next, exact);

            previous = current;
            current = next;
        }

        writer.WriteLine();
    }
}
