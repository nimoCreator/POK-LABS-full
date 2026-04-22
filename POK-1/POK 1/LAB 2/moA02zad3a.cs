using POK_1.assets;

namespace POK_1;

internal sealed class MoA02Zad3A : IZadanie
{
    public string Nazwa => "moA02zad3a";

    private readonly double _dx;
    private readonly double _x0;

    public MoA02Zad3A(double dx, double x0)
    {
        _dx = dx;
        _x0 = x0;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("dx = {0,8:f4}\n", _dx);

        int steps = 0;
        double xFound = _x0;
        double fx = _x0;
        double gx = 2 * Math.Sin(_x0);

        for (double x = _x0; x <= Math.PI; x += _dx)
        {
            steps++;
            fx = x;
            gx = 2 * Math.Sin(x);

            if (Math.Abs(fx - gx) <= _dx)
            {
                xFound = x;
                break;
            }
        }

        writer.WriteLine("{0}) x = {1:f10}", steps, xFound);
        writer.WriteLine("       f(x) = {0:f10}", fx);
        writer.WriteLine("       g(x) = {0:f10}", gx);
        writer.WriteLine();
    }
}
