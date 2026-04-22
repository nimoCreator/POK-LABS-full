using POK_1.assets;

namespace POK_1;

internal sealed class MoA04Zad1a : IZadanie
{
    public string Nazwa => "moA04zad1a";

    private readonly int _nMax;
    private readonly int _summaryStep;

    public MoA04Zad1a(int nMax, int summaryStep)
    {
        _nMax = nMax;
        _summaryStep = summaryStep;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("N = {0}\n", _nMax);
        writer.WriteLine("| {0,6} | {1,14} |", "n", "x");
        writer.WriteLine("+-{0,6}-+-{1,14}-+", "------", "--------------");

        double x = 0.0;
        for (int n = 1; n <= _nMax; n++)
        {
            x = Math.Pow(1.0 + (1.0 / n), n);
            if (ShouldPrint(n))
            {
                writer.WriteLine("| {0,6} | {1,14:f10} |", n, x);
            }
        }

        writer.WriteLine();
        writer.WriteLine("x({0}) = {1:g15}", _nMax, x);
        writer.WriteLine("      e = {0:g15}", Math.E);
        writer.WriteLine();
    }

    private bool ShouldPrint(int n)
    {
        return n <= 10 || n % _summaryStep == 0 || n == _nMax;
    }
}
