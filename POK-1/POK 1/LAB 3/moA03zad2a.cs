using POK_1.assets;

namespace POK_1;

internal sealed class MoA03Zad2a : IZadanie
{
    public string Nazwa => "moA03zad2a";

    private readonly double _pi;
    private readonly double _deltaPi;
    private readonly double _dPi;

    private readonly double _d;
    private readonly double _deltaD;
    private readonly double _dD;

    public MoA03Zad2a(double pi, double deltaPi, double dPi, double d, double deltaD, double dD)
    {
        _pi = pi;
        _deltaPi = deltaPi;
        _dPi = dPi;
        _d = d;
        _deltaD = deltaD;
        _dD = dD;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("V = pi * d^3 / 6");
        writer.WriteLine("pi = {0:f5}, DeltaPi = {1:f5}, dPi = {2:f5}", _pi, _deltaPi, _dPi);
        writer.WriteLine("D  = {0:f5}, DeltaD  = {1:f5}, dD  = {2:f5}", _d, _deltaD, _dD);
        writer.WriteLine();

        double minV = double.MaxValue;
        double maxV = double.MinValue;
        double minPi = _pi;
        double maxPi = _pi;
        double minD = _d;
        double maxD = _d;

        for (double pi = _pi - _deltaPi; pi <= _pi + _deltaPi + (_dPi / 2.0); pi += _dPi)
        {
            for (double d = _d - _deltaD; d <= _d + _deltaD + (_dD / 2.0); d += _dD)
            {
                double v = SphereVolume(pi, d);

                if (v < minV)
                {
                    minV = v;
                    minPi = pi;
                    minD = d;
                }

                if (v > maxV)
                {
                    maxV = v;
                    maxPi = pi;
                    maxD = d;
                }
            }
        }

        writer.WriteLine("Vmin = {0:f10} dla pi = {1:f5}, d = {2:f5}", minV, minPi, minD);
        writer.WriteLine("Vmax = {0:f10} dla pi = {1:f5}, d = {2:f5}", maxV, maxPi, maxD);
        writer.WriteLine("DeltaV = {0:f10}", maxV - minV);
        writer.WriteLine();
    }

    private static double SphereVolume(double pi, double d)
    {
        return pi * d * d * d / 6.0;
    }
}
