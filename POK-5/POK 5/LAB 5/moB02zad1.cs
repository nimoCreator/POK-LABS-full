using System.Diagnostics;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad1 : IZadanie
{
    public string Nazwa => "moB02zad1";

    private readonly int _n;
    private readonly double _a;
    private readonly double _b;
    private readonly int _seed;

    public MoB02Zad1(int n, double a, double b, int seed = 301954)
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

    public void Run(TextWriter writer)
    {
        int nodeCount = _n + 1;
        var random = new Random(_seed);

        double[] xNodes = GenerateNodes(nodeCount, _a, _b, random);
        double[] yValues = GenerateValues(nodeCount, random);

        var cache = new Dictionary<(int From, int To), double>();
        double[] coefficients = BuildNewtonCoefficients(xNodes, yValues, cache);

        double x = _a + 0.37 * (_b - _a);
        double sx = EvaluateNewtonPolynomial(x, xNodes, coefficients);

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
        writer.WriteLine("wspolczynniki wielomianu Newtona:");
        writer.WriteLine("|   k |       f[x0..xk] |");
        writer.WriteLine("+-----+-----------------+");
        for (int k = 0; k < coefficients.Length; k++)
        {
            writer.WriteLine("| {0,3} | {1,15:g10} |", k, coefficients[k]);
        }

        writer.WriteLine();
        writer.WriteLine("wartosc wielomianu dla dowolnego argumentu x:");
        writer.WriteLine("x = {0:f8}", x);
        writer.WriteLine("S(x) = {0:g15}", sx);

        writer.WriteLine();
        writer.WriteLine("sprawdzenie rownosci interpolacyjnych S(x_i) = y_i:");
        writer.WriteLine("|   i |        S(x_i) |           y_i |    |blad| |");
        writer.WriteLine("+-----+---------------+---------------+-----------+");

        double maxError = 0.0;
        const double tolerance = 1e-9;
        for (int i = 0; i < nodeCount; i++)
        {
            double interpolated = EvaluateNewtonPolynomial(xNodes[i], xNodes, coefficients);
            double error = Math.Abs(interpolated - yValues[i]);
            maxError = Math.Max(maxError, error);

            Debug.Assert(error <= tolerance, $"Naruszona rownosc interpolacyjna dla i = {i}.");
            if (error > tolerance)
            {
                throw new InvalidOperationException(
                    $"Rownosc interpolacyjna nie zostala spelniona dla i = {i}. Blad = {error:e6}.");
            }

            writer.WriteLine("| {0,3} | {1,13:g10} | {2,13:g10} | {3,9:e2} |",
                i, interpolated, yValues[i], error);
        }

        writer.WriteLine();
        writer.WriteLine("maksymalny blad interpolacji = {0:e6}", maxError);
        writer.WriteLine("asercje zostaly spelnione");
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

    private static double[] BuildNewtonCoefficients(
        double[] xNodes,
        double[] yValues,
        Dictionary<(int From, int To), double> cache)
    {
        double[] coefficients = new double[xNodes.Length];
        for (int k = 0; k < coefficients.Length; k++)
        {
            coefficients[k] = DividedDifference(0, k, xNodes, yValues, cache);
        }

        return coefficients;
    }

    private static double DividedDifference(
        int from,
        int to,
        double[] xNodes,
        double[] yValues,
        Dictionary<(int From, int To), double> cache)
    {
        if (cache.TryGetValue((from, to), out double cachedValue))
        {
            return cachedValue;
        }

        double result;
        if (from == to)
        {
            result = yValues[from];
        }
        else
        {
            double right = DividedDifference(from + 1, to, xNodes, yValues, cache);
            double left = DividedDifference(from, to - 1, xNodes, yValues, cache);
            result = (right - left) / (xNodes[to] - xNodes[from]);
        }

        cache[(from, to)] = result;
        return result;
    }

    private static double EvaluateNewtonPolynomial(double x, double[] xNodes, double[] coefficients)
    {
        double result = coefficients[^1];
        for (int i = coefficients.Length - 2; i >= 0; i--)
        {
            result = result * (x - xNodes[i]) + coefficients[i];
        }

        return result;
    }
}
