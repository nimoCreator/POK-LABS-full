namespace POK_1;

internal sealed class LinearEquationResult
{
    public LinearEquationResult(bool hasSolution, double[] x, double[] db)
    {
        HasSolution = hasSolution;
        X = x;
        Db = db;
    }

    public bool HasSolution { get; }

    public double[] X { get; }

    public double[] Db { get; }
}

internal static class GaussSolver
{
    private const double Eps = 1.0e-10;

    public static LinearEquationResult Solve(double[,] a, double[] b)
    {
        int n = b.Length;
        double[][] c = new double[n][];
        double[] x = new double[n];
        double[] db = new double[n];

        for (int i = 0; i < n; i++)
        {
            c[i] = new double[n + 1];
            for (int j = 0; j < n; j++)
            {
                c[i][j] = a[i, j];
            }

            c[i][n] = b[i];
        }

        for (int k = 0; k < n - 1; k++)
        {
            double maxElement = Math.Abs(c[k][k]);
            int maxPosition = k;

            for (int i = k + 1; i < n; i++)
            {
                double element = Math.Abs(c[i][k]);
                if (element > maxElement)
                {
                    maxElement = element;
                    maxPosition = i;
                }
            }

            if (Math.Abs(maxElement) < Eps)
            {
                return new LinearEquationResult(false, x, db);
            }

            if (maxPosition != k)
            {
                (c[k], c[maxPosition]) = (c[maxPosition], c[k]);
            }

            double ckk = c[k][k];
            for (int i = k + 1; i < n; i++)
            {
                double multiplier = c[i][k] / ckk;
                c[i][k] = 0.0;

                for (int j = k + 1; j <= n; j++)
                {
                    c[i][j] -= multiplier * c[k][j];
                }
            }
        }

        if (Math.Abs(c[n - 1][n - 1]) < Eps)
        {
            return new LinearEquationResult(false, x, db);
        }

        for (int i = n - 1; i >= 0; i--)
        {
            x[i] = c[i][n];
            for (int j = i + 1; j < n; j++)
            {
                x[i] -= c[i][j] * x[j];
            }

            x[i] /= c[i][i];
        }

        for (int i = 0; i < n; i++)
        {
            double sum = 0.0;
            for (int j = 0; j < n; j++)
            {
                sum += a[i, j] * x[j];
            }

            db[i] = Math.Abs(sum) < Eps ? 0.0 : (b[i] - sum) / sum;
        }

        return new LinearEquationResult(true, x, db);
    }

    public static (double[,] A, double[] B) CreateRandomEquation(int n, Random random)
    {
        double[,] a = new double[n, n];
        double[] b = new double[n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                a[i, j] = 10.0 * random.NextDouble();
            }

            b[i] = 10.0 * random.NextDouble();
        }

        return (a, b);
    }
}
