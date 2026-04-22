using POK_1.assets;

namespace POK_1;

internal sealed class MoA06Zad1 : IZadanie
{
    public string Nazwa => "moA06zad1";

    private readonly double[,] _a;
    private readonly double[] _b;

    public MoA06Zad1(double[,] a, double[] b)
    {
        _a = a;
        _b = b;
    }

    public void Run(TextWriter writer)
    {
        int n = _b.Length;

        writer.WriteLine("rzad ukladu n = {0}\n", n);
        writer.WriteLine("macierz A oraz wektor b:");
        for (int i = 0; i < n; i++)
        {
            writer.Write("|");
            for (int j = 0; j < n; j++)
            {
                writer.Write(" {0,10:f6}", _a[i, j]);
            }

            writer.WriteLine(" | {0,10:f6}", _b[i]);
        }

        writer.WriteLine();

        var result = GaussSolver.Solve(_a, _b);
        if (!result.HasSolution)
        {
            writer.WriteLine("brak jednoznacznego rozwiazania");
            return;
        }

        writer.WriteLine("otrzymane rozwiazanie rownania -->");
        writer.WriteLine("| {0,3} | {1,16} | {2,13} |", "i", "x", "db");
        writer.WriteLine("+-{0,3}-+-{1,16}-+-{2,13}-+", "---", "----------------", "-------------");

        for (int i = 0; i < n; i++)
        {
            writer.WriteLine("| {0,3} | {1,16:g12} | {2,13:e6} |", i + 1, result.X[i], result.Db[i]);
        }

        writer.WriteLine();
    }
}
