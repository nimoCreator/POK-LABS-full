using POK_1.assets;

namespace POK_1;

internal sealed class MoA04Zad1b : IZadanie
{
    public string Nazwa => "moA04zad1b";

    private readonly int _nMax;

    public MoA04Zad1b(int nMax)
    {
        _nMax = nMax;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("N = {0}\n", _nMax);
        writer.WriteLine("| {0,3} | {1,14} |", "n", "x");
        writer.WriteLine("+-{0,3}-+-{1,14}-+", "---", "--------------");

        double sum = 1.0;
        double term = 1.0;
        writer.WriteLine("| {0,3} | {1,14:f10} |", 0, sum);

        for (int n = 1; n <= _nMax; n++)
        {
            term /= n;
            sum += term;
            writer.WriteLine("| {0,3} | {1,14:f10} |", n, sum);
        }

        writer.WriteLine();
        writer.WriteLine("x({0}) = {1:g15}", _nMax, sum);
        writer.WriteLine("    e = {0:g15}", Math.E);
        writer.WriteLine();
    }
}
