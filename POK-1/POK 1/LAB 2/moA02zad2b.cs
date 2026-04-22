using POK_1.assets;

namespace POK_1;

internal sealed class MoA02Zad2B : IZadanie
{
    public string Nazwa => "moA02zad2b";

    private readonly int _b;
    private readonly double _x0;
    private readonly int _n;

    public MoA02Zad2B(int b, double x0, int n)
    {
        _b = b;
        _x0 = x0;
        _n = n;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("b = {0,2:f1}  x0 = {1,2:f1}  n = {2}\n", _b, _x0, _n);
        writer.WriteLine("| {0,3} | {1,12} |", "i", "x");
        writer.WriteLine("+-{0,3}-+-{1,12}-+", "---", "------------");

        double x = _x0;
        writer.WriteLine("| {0,3} | {1,12:f10} |", 0, x);

        for (int i = 1; i < _n; i++)
        {
            x = _b / x;
            writer.WriteLine("| {0,3} | {1,12:f10} |", i, x);
        }

        writer.WriteLine();
    }
}
