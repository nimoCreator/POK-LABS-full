using POK_1.assets;

namespace POK_1;

internal sealed class MoA02Zad1 : IZadanie
{
    public string Nazwa => "moA02zad1";

    private readonly double _xStart;
    private readonly double _xEnd;
    private readonly double _xStep;

    public MoA02Zad1(double xStart, double xEnd, double xStep)
    {
        _xStart = xStart;
        _xEnd = xEnd;
        _xStep = xStep;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("| {0,3} | {1,7} |", "x", "y");
        writer.WriteLine("+-{0,3}-+-{1,7}-+", "---", "-------");

        for (double x = _xStart; x <= _xEnd + (_xStep / 100.0); x += _xStep)
        {
            writer.WriteLine("| {0,3:f1} | {1,7:f3} |", x, F1(x));
        }

        writer.WriteLine();
    }

    private static double F1(double x)
    {
        return ((((1.2 * x + 3.14) * x + 2.58) * x + 0.27) * x + 6.08) * x + 4.5;
    }
}
