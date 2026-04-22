using System.Diagnostics;
using POK_1.assets;

namespace POK_1;

internal sealed class MoA06Zad3 : IZadanie
{
    public string Nazwa => "moA06zad3";

    private readonly int _nMax;
    private readonly int _m;
    private readonly int _seed;

    public MoA06Zad3(int nMax, int m, int seed)
    {
        _nMax = nMax;
        _m = m;
        _seed = seed;
    }

    public void Run(TextWriter writer)
    {
        var random = new Random(_seed);

        writer.WriteLine("N = {0}  m = {1}  seed = {2}\n", _nMax, _m, _seed);
        writer.WriteLine("n;czas_ms;liczba_rozwiazan");

        for (int n = 2; n <= _nMax; n++)
        {
            int solutionCount = 0;
            var stopwatch = Stopwatch.StartNew();

            for (int step = 0; step < _m; step++)
            {
                var equation = GaussSolver.CreateRandomEquation(n, random);
                var result = GaussSolver.Solve(equation.A, equation.B);
                if (result.HasSolution)
                {
                    solutionCount++;
                }
            }

            stopwatch.Stop();

            double averageTime = solutionCount == 0
                ? 0.0
                : stopwatch.Elapsed.TotalMilliseconds / solutionCount;

            writer.WriteLine("{0};{1:f6};{2}", n, averageTime, solutionCount);
        }

        writer.WriteLine();
    }
}
