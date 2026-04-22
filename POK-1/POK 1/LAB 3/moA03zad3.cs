using POK_1.assets;

namespace POK_1;

internal sealed class MoA03Zad3 : IZadanie
{
    public string Nazwa => "moA03zad3";

    private readonly double _a;
    private readonly int _n;

    public MoA03Zad3(double a, int n)
    {
        _a = a;
        _n = n;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("a = {0:f1}  n = {1}\n", _a, _n);
        writer.WriteLine("| {0,8} | {1,13} | {2,13} | {3,6} | {4,6} | {5,6} |", "x", "f(x)", "g(x)", "df", "dg", "dfg");
        writer.WriteLine("+-{0,8}-+-{1,13}-+-{2,13}-+-{3,6}-+-{4,6}-+-{5,6}-+", "--------", "-------------", "-------------", "------", "------", "------");

        for (int i = 0; i < _n; i++)
        {
            double x = GenerateX(_a, i);
            double s1 = Math.Sqrt(1 + x * x);
            double s2 = Math.Sqrt(1 - x * x);

            double s1Rounded = Math.Round(s1, 11);
            double s2Rounded = Math.Round(s2, 7);

            double f = CalculateF(s1Rounded, s2Rounded);
            double g = CalculateG(x, s1Rounded, s2Rounded);

            double deltaS1 = CalculateRelativeRoundingError(s1, 11);
            double deltaS2 = CalculateRelativeRoundingError(s2, 7);

            double deltaF = CalculateDeltaF(s1, s2, f, deltaS1, deltaS2);
            double deltaG = CalculateDeltaG(s1, s2, deltaS1, deltaS2);
            double deltaFG = Math.Abs((g - f) / g);

            writer.WriteLine(
                "| {0,8:f6} | {1,13:e6} | {2,13:e6} | {3,6:e0} | {4,6:e0} | {5,6:e0} |",
                x,
                f,
                g,
                deltaF,
                deltaG,
                deltaFG);
        }

        writer.WriteLine();
    }

    private static double GenerateX(double a, int step)
    {
        return a / Math.Pow(2, step + 2);
    }

    private static double CalculateF(double s1, double s2)
    {
        return s1 - s2;
    }

    private static double CalculateG(double x, double s1, double s2)
    {
        return (2 * x * x) / (s1 + s2);
    }

    private static double CalculateRelativeRoundingError(double value, int digitsAfterDecimal)
    {
        double absoluteErrorBound = 0.5 * Math.Pow(10, -digitsAfterDecimal);
        return absoluteErrorBound / Math.Abs(value);
    }

    private static double CalculateDeltaF(double s1, double s2, double f, double deltaS1, double deltaS2)
    {
        return Math.Abs((s1 / Math.Abs(f)) * deltaS1 + (s2 / Math.Abs(f)) * deltaS2);
    }

    private static double CalculateDeltaG(double s1, double s2, double deltaS1, double deltaS2)
    {
        double denominator = s1 + s2;
        return Math.Abs((s1 / denominator) * deltaS1 + (s2 / denominator) * deltaS2);
    }
}
