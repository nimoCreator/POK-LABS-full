namespace POK_5;

internal static class MoB02PerceptronTools
{
    public const int InputSize = 3;
    public const int OutputSize = 2;
    public const int ParameterCount = 8;

    public static DataPair[] GenerateData(int count, int seed)
    {
        var random = new Random(seed);
        var data = new DataPair[count];
        for (int i = 0; i < count; i++)
        {
            double[] x = new double[]
            {
                2.0 * random.NextDouble() - 1.0,
                2.0 * random.NextDouble() - 1.0,
                2.0 * random.NextDouble() - 1.0
            };
            double[] y = Target(x);
            data[i] = new DataPair(x, y);
        }
        return data;
    }

    public static double[] Target(double[] x)
    {
        return new double[]
        {
            Math.Tanh(0.20 + 0.80 * x[0] - 0.40 * x[1] + 0.30 * x[2]),
            Math.Tanh(-0.10 - 0.50 * x[0] + 0.70 * x[1] + 0.60 * x[2])
        };
    }

    public static double[] Predict(double[] x, double[] p)
    {
        double s0 = p[0] + p[1] * x[0] + p[2] * x[1] + p[3] * x[2];
        double s1 = p[4] + p[5] * x[0] + p[6] * x[1] + p[7] * x[2];
        return new double[] { Math.Tanh(s0), Math.Tanh(s1) };
    }

    public static double Loss(DataPair pair, double[] p)
    {
        double[] eta = Predict(pair.X, p);
        double sum = 0.0;
        for (int j = 0; j < OutputSize; j++)
        {
            double difference = eta[j] - pair.Y[j];
            sum += difference * difference;
        }
        return sum / OutputSize;
    }

    public static double Cost(DataPair[] data, double[] p)
    {
        double sum = 0.0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += Loss(data[i], p);
        }
        return sum / data.Length;
    }

    public static double[] NumericalGradient(Func<double[], double> function, double[] p, double h)
    {
        double[] gradient = new double[p.Length];
        double[] left = (double[])p.Clone();
        double[] right = (double[])p.Clone();

        for (int k = 0; k < p.Length; k++)
        {
            left[k] = p[k] - h;
            right[k] = p[k] + h;
            gradient[k] = (function(right) - function(left)) / (2.0 * h);
            left[k] = p[k];
            right[k] = p[k];
        }

        return gradient;
    }

    public static double[] RandomParameters(int seed, double scale)
    {
        var random = new Random(seed);
        double[] p = new double[ParameterCount];
        for (int i = 0; i < p.Length; i++)
        {
            p[i] = scale * (2.0 * random.NextDouble() - 1.0);
        }
        return p;
    }

    public static double RelativeError(double value, double expected)
    {
        return Math.Abs(value - expected) / Math.Max(Math.Abs(expected), 1.0e-12);
    }

    public readonly record struct DataPair(double[] X, double[] Y);
}
