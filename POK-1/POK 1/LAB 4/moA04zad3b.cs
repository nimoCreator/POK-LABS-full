using POK_1.assets;

namespace POK_1;

internal sealed class MoA04Zad3b : IZadanie
{
    public string Nazwa => "moA04zad3b";

    private readonly int _nMax;

    public MoA04Zad3b(int nMax)
    {
        _nMax = nMax;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("N = {0}\n", _nMax);
        writer.WriteLine("| {0,6} | {1,19} |", "n", "x");
        writer.WriteLine("+-{0,6}-+-{1,19}-+", "------", "-------------------");

        double sum = 0.0;
        for (int n = 0; n <= _nMax; n++)
        {
            sum += BaileyBorweinPlouffeTerm(n);
            writer.WriteLine("| {0,6} | {1,19:f15} |", n, sum);
        }

        writer.WriteLine();
        writer.WriteLine("x({0}) = {1:g15}", _nMax, sum);
        writer.WriteLine("   pi = {0:g15}", Math.PI);
        writer.WriteLine();
    }

    private static double BaileyBorweinPlouffeTerm(int n)
    {
        double factor = 1.0 / Math.Pow(16.0, n);
        return factor * (
            4.0 / ((8.0 * n) + 1.0) -
            2.0 / ((8.0 * n) + 4.0) -
            1.0 / ((8.0 * n) + 5.0) -
            1.0 / ((8.0 * n) + 6.0));
    }
}
