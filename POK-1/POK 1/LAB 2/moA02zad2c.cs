using POK_1.assets;

namespace POK_1;

internal sealed class MoA02Zad2C : IZadanie
{
    public string Nazwa => "moA02zad2c";

    private readonly double _bStart;
    private readonly double _bEnd;
    private readonly double _bStep;
    private readonly double _x0;
    private readonly int _n;

    public MoA02Zad2C(double bStart, double bEnd, double bStep, double x0, int n)
    {
        _bStart = bStart;
        _bEnd = bEnd;
        _bStep = bStep;
        _x0 = x0;
        _n = n;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("x0 = {0,2:f1}  n = {1}\n", _x0, _n);
        writer.WriteLine("| {0,4} | {1,12} | {2,12} |", "b", "x", "sqrt(b)");
        writer.WriteLine("+-{0,4}-+-{1,12}-+-{2,12}-+", "----", "------------", "------------");

        for (double b = _bStart; b <= _bEnd + (_bStep / 100.0); b += _bStep)
        {
            double x = _x0;

            for (int i = 0; i < _n; i++)
            {
                x = 0.5 * (x + b / x);
            }

            writer.WriteLine("| {0,4:f1} | {1,12:f10} | {2,12:f10} |", b, x, Math.Sqrt(b));
        }

        writer.WriteLine();
    }
}
