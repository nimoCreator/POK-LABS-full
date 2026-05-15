using System.Diagnostics;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad2 : IZadanie
{
    private readonly int _n;
    private readonly double _a;
    private readonly double _b;
    private readonly int _seed;

    public MoB02Zad2(int n, double a, double b, int seed = 301954)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Stopien wielomianu musi byc dodatni.");
        }

        if (b <= a)
        {
            throw new ArgumentOutOfRangeException(nameof(b), "Koniec przedzialu musi byc wiekszy od poczatku.");
        }

        _n = n;
        _a = a;
        _b = b;
        _seed = seed;
    }

    public string Nazwa => "moB02zad2";

    public void Run(TextWriter writer)
    {
        int nodeCount = _n + 1;
        var random = new Random(_seed);

        double[] xNodes = GenerateNodes(nodeCount, _a, _b, random);
        double[] yValues = GenerateValues(nodeCount, random);

        double x = _a + 0.37 * (_b - _a);
        double sx = EvaluateLagrangePolynomial(x, xNodes, yValues);

        writer.WriteLine($"stopien wielomianu n = {_n}");
        writer.WriteLine($"przedzial = <{_a:f6}, {_b:f6}>");
        writer.WriteLine($"liczba wezlow = {nodeCount}");
        writer.WriteLine($"seed = {_seed}");
        writer.WriteLine();

        writer.WriteLine("|   i |           x_i |           y_i |");
        writer.WriteLine("+-----+---------------+---------------+");
        for (int i = 0; i < nodeCount; i++)
        {
            writer.WriteLine("| {0,3} | {1,13:f8} | {2,13:f8} |", i, xNodes[i], yValues[i]);
        }

        writer.WriteLine();
        writer.WriteLine("wartosc wielomianu Lagrange'a dla dowolnego argumentu x:");
        writer.WriteLine("x = {0:f8}", x);
        writer.WriteLine("S(x) = {0:g15}", sx);

        writer.WriteLine();
        writer.WriteLine("wartosci bazowych wielomianow L_i(x) w wybranym punkcie:");
        writer.WriteLine("|   i |        L_i(x) |  y_i * L_i(x) |");
        writer.WriteLine("+-----+---------------+---------------+");

        for (int i = 0; i < nodeCount; i++)
        {
            double basis = EvaluateLagrangeBasis(i, x, xNodes);
            writer.WriteLine("| {0,3} | {1,13:g10} | {2,13:g10} |", i, basis, yValues[i] * basis);
        }

        writer.WriteLine();
        writer.WriteLine("sprawdzenie rownosci interpolacyjnych S(x_i) = y_i:");
        writer.WriteLine("|   i |        S(x_i) |           y_i |    |blad| |");
        writer.WriteLine("+-----+---------------+---------------+-----------+");

        double maxError = 0.0;
        const double tolerance = 1e-12;
        for (int i = 0; i < nodeCount; i++)
        {
            double interpolated = EvaluateLagrangePolynomial(xNodes[i], xNodes, yValues);
            double error = Math.Abs(interpolated - yValues[i]);
            maxError = Math.Max(maxError, error);

            Debug.Assert(error <= tolerance * Math.Max(1.0, Math.Abs(yValues[i])),
                $"Naruszona rownosc interpolacyjna dla i = {i}.");
            NumericAssert.AreAlmostEqual(
                interpolated,
                yValues[i],
                tolerance,
                $"Rownosc interpolacyjna nie zostala spelniona dla i = {i}.");

            writer.WriteLine("| {0,3} | {1,13:g10} | {2,13:g10} | {3,9:e2} |",
                i, interpolated, yValues[i], error);
        }

        writer.WriteLine();
        writer.WriteLine("maksymalny blad interpolacji = {0:e6}", maxError);
        writer.WriteLine("asercje zostaly spelnione z tolerancja {0:e0}", tolerance);
    }

    private static double[] GenerateNodes(int count, double a, double b, Random random)
    {
        double[] nodes = new double[count];
        double segmentLength = (b - a) / count;

        for (int i = 0; i < count; i++)
        {
            double left = a + i * segmentLength;
            double offset = 0.15 + 0.70 * random.NextDouble();
            nodes[i] = left + offset * segmentLength;
        }

        Array.Sort(nodes);
        return nodes;
    }

    private static double[] GenerateValues(int count, Random random)
    {
        double[] values = new double[count];
        for (int i = 0; i < count; i++)
        {
            values[i] = -1.0 + 2.0 * random.NextDouble();
        }

        return values;
    }

    private static double EvaluateLagrangePolynomial(double x, double[] xNodes, double[] yValues)
    {
        double sum = 0.0;
        for (int i = 0; i < xNodes.Length; i++)
        {
            sum += yValues[i] * EvaluateLagrangeBasis(i, x, xNodes);
        }

        return sum;
    }

    private static double EvaluateLagrangeBasis(int basisIndex, double x, double[] xNodes)
    {
        double product = 1.0;
        double xi = xNodes[basisIndex];

        for (int j = 0; j < xNodes.Length; j++)
        {
            if (j == basisIndex)
            {
                continue;
            }

            product *= (x - xNodes[j]) / (xi - xNodes[j]);
        }

        return product;
    }
}
