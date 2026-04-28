using POK_1.assets;

namespace POK_1;

internal sealed class MoA06Zad2 : IZadanie
{
    public string Nazwa => "moA06zad2";

    private readonly int _n;
    private readonly int _m;
    private readonly int _seed;

    public MoA06Zad2(int n, int m, int seed)
    {
        _n = n;
        _m = m;
        _seed = seed;
    }

    public void Run(TextWriter writer)
    {
        var random = new Random(_seed);
        int solutionCount = 0;
        double errorSum = 0.0;
        double maxError = 0.0;

        for (int step = 0; step < _m; step++)
        {
            var equation = GaussSolver.CreateRandomEquation(_n, random);
            var result = GaussSolver.Solve(equation.A, equation.B);

            if (!result.HasSolution)
            {
                continue;
            }

            solutionCount++;
            foreach (double db in result.Db)
            {
                double error = Math.Abs(db);
                errorSum += error;
                maxError = Math.Max(maxError, error);
            }
        }

        double averageError = solutionCount == 0 ? 0.0 : errorSum / (solutionCount * _n);

        writer.WriteLine("n = {0}  m = {1}  seed = {2}\n", _n, _m, _seed);
        writer.WriteLine("liczba przypadkow z rozwiazaniem = {0}", solutionCount);
        writer.WriteLine("liczba przypadkow bez rozwiazania = {0}", _m - solutionCount);
        writer.WriteLine("sredni blad wzgledny db = {0:e6}", averageError);
        writer.WriteLine("maksymalny blad wzgledny db = {0:e6}", maxError);
        writer.WriteLine();
    }
}
