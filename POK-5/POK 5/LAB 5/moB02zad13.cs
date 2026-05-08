using System.Linq;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad13 : IZadanie
{
    public string Nazwa => "moB02zad13";

    public void Run(TextWriter writer)
    {
        const double a = 0.0;
        const double b = Math.PI;
        const int nodeCount = 201;
        const int dataCount = 1000;
        const int maxIterationCount = 20;
        const double minimumCost = 1.0e-5;
        const double supportFactor = 2.0;
        const int seed = 301954;

        double step = (b - a) / (nodeCount - 1);
        double support = supportFactor * step;
        double[] nodes = new double[nodeCount];
        double[] nodeValues = new double[nodeCount];
        for (int i = 0; i < nodeCount; i++)
        {
            nodes[i] = a + i * step;
            nodeValues[i] = SourceFunction(nodes[i]);
        }

        double[] dataX = new double[dataCount];
        double[] dataY = new double[dataCount];
        for (int i = 0; i < dataCount; i++)
        {
            dataX[i] = a + (b - a) * i / (dataCount - 1);
            dataY[i] = SourceFunction(dataX[i]);
        }

        var random = new Random(seed);
        double[] parameters = new double[nodeCount];
        for (int k = 0; k < parameters.Length; k++)
        {
            parameters[k] = 0.35 + 0.30 * random.NextDouble();
        }

        var selectedCurves = new List<CurveSnapshot>();
        double[] sphParameters = Enumerable.Repeat(0.5, nodeCount).ToArray();
        double sphMaxError = MaxAbsoluteError(dataX, dataY, sphParameters, nodes, nodeValues, support);

        for (int iteration = 0; iteration <= maxIterationCount; iteration++)
        {
            double cost = Cost(dataX, dataY, parameters, nodes, nodeValues, support);
            if (iteration == 0 || iteration == 1 || iteration == 2 || iteration == 3 || iteration == 5 || iteration == 7 || iteration == 10 || iteration == 12 || iteration == 15 || iteration == 18 || iteration == 20)
            {
                selectedCurves.Add(BuildSnapshot(iteration, dataX, parameters, nodes, nodeValues, support));
            }

            writer.WriteLine("iteracja {0,2}: koszt = {1:e8}", iteration, cost);
            if (cost <= minimumCost || iteration == maxIterationCount)
            {
                break;
            }

            double[] gradient = Gradient(dataX, dataY, parameters, nodes, nodeValues, support);
            double[,] hessian = Hessian(dataX, parameters, nodes, nodeValues, support);
            double[] direction = SolveLinearSystem(hessian, Negate(gradient));

            double alpha = FindStepByBisection(dataX, dataY, parameters, direction, nodes, nodeValues, support, 0.0, 1.0, 10);
            if (double.IsNaN(alpha) || alpha <= 0.0)
            {
                alpha = 0.25;
            }

            for (int k = 0; k < parameters.Length; k++)
            {
                parameters[k] = Clamp(parameters[k] + alpha * direction[k], 0.0, 1.0);
            }
        }

        double optimalCost = Cost(dataX, dataY, parameters, nodes, nodeValues, support);
        double optimalMaxError = MaxAbsoluteError(dataX, dataY, parameters, nodes, nodeValues, support);
        double maxRelativeParameterError = parameters.Max(p => Math.Abs(p - 0.5) / 0.5);

        string outputDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "POK_5_results"));
        Directory.CreateDirectory(outputDirectory);
        string curvesPath = Path.Combine(outputDirectory, "moB02zad13_krzywe.xls");
        string parametersPath = Path.Combine(outputDirectory, "moB02zad13_parametry.xls");
        SaveCurves(curvesPath, dataX, dataY, sphParameters, parameters, selectedCurves, nodes, nodeValues, support);
        SaveParameters(parametersPath, parameters);

        writer.WriteLine();
        writer.WriteLine("aproksymacja gaussowska funkcji f(x) = sin(x) na przedziale <0, pi>");
        writer.WriteLine("N = {0}, liczba danych rozwojowych = {1}", nodeCount, dataCount);
        writer.WriteLine("krok dyskretyzacji = {0:e8}", step);
        writer.WriteLine("nosnik jadra h = {0:e8}", support);
        writer.WriteLine("koszt koncowy = {0:e8}", optimalCost);
        writer.WriteLine("maksymalny blad bezwzgledny aproksymanta optymalnego = {0:e8}", optimalMaxError);
        writer.WriteLine("maksymalny blad bezwzgledny aproksymanta SPH = {0:e8}", sphMaxError);
        writer.WriteLine("maksymalny blad wzgledny parametrow wzgledem 1/2 = {0:e8}", maxRelativeParameterError);
        writer.WriteLine("plik z krzywymi do Excela = {0}", curvesPath);
        writer.WriteLine("plik z parametrami do Excela = {0}", parametersPath);
        writer.WriteLine("wniosek: SPH jest bardzo blisko optimum, ale optymalizacja zwykle poprawia zachowanie brzegowe.");
    }

    private static double SourceFunction(double x) => Math.Sin(x);

    private static double Kernel(double distance, double support)
    {
        double q = distance / support;
        return Math.Exp(-q * q);
    }

    private static double Phi(double x, double[] p, double[] nodes, double[] nodeValues, double support)
    {
        double numerator = 0.0;
        double denominator = 0.0;
        for (int k = 0; k < nodes.Length; k++)
        {
            double u = Kernel(x - nodes[k], support);
            numerator += 2.0 * p[k] * nodeValues[k] * u;
            denominator += u;
        }
        return numerator / Math.Max(denominator, 1.0e-15);
    }

    private static double DPhi(int k, double x, double[] nodes, double[] nodeValues, double support)
    {
        double denominator = 0.0;
        for (int i = 0; i < nodes.Length; i++)
        {
            denominator += Kernel(x - nodes[i], support);
        }
        return 2.0 * nodeValues[k] * Kernel(x - nodes[k], support) / Math.Max(denominator, 1.0e-15);
    }

    private static double Cost(double[] dataX, double[] dataY, double[] p, double[] nodes, double[] nodeValues, double support)
    {
        double sum = 0.0;
        for (int i = 0; i < dataX.Length; i++)
        {
            double diff = Phi(dataX[i], p, nodes, nodeValues, support) - dataY[i];
            sum += diff * diff;
        }
        return sum / dataX.Length;
    }

    private static double[] Gradient(double[] dataX, double[] dataY, double[] p, double[] nodes, double[] nodeValues, double support)
    {
        double[] gradient = new double[p.Length];
        for (int s = 0; s < dataX.Length; s++)
        {
            double eta = Phi(dataX[s], p, nodes, nodeValues, support);
            double residual = eta - dataY[s];
            for (int k = 0; k < p.Length; k++)
            {
                gradient[k] += 2.0 * residual * DPhi(k, dataX[s], nodes, nodeValues, support);
            }
        }
        for (int k = 0; k < gradient.Length; k++)
        {
            gradient[k] /= dataX.Length;
        }
        return gradient;
    }

    private static double[,] Hessian(double[] dataX, double[] p, double[] nodes, double[] nodeValues, double support)
    {
        int n = p.Length;
        double[,] hessian = new double[n, n];
        for (int s = 0; s < dataX.Length; s++)
        {
            double[] d = new double[n];
            for (int k = 0; k < n; k++)
            {
                d[k] = DPhi(k, dataX[s], nodes, nodeValues, support);
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    hessian[i, j] += 2.0 * d[i] * d[j];
                }
            }
        }
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                hessian[i, j] /= dataX.Length;
            }
            hessian[i, i] += 1.0e-10;
        }
        return hessian;
    }

    private static double DirectionalDerivative(double[] dataX, double[] dataY, double[] p, double[] direction, double alpha, double[] nodes, double[] nodeValues, double support)
    {
        double[] moved = new double[p.Length];
        for (int k = 0; k < p.Length; k++)
        {
            moved[k] = Clamp(p[k] + alpha * direction[k], 0.0, 1.0);
        }
        double[] gradient = Gradient(dataX, dataY, moved, nodes, nodeValues, support);
        double dot = 0.0;
        for (int k = 0; k < p.Length; k++)
        {
            dot += gradient[k] * direction[k];
        }
        return dot;
    }

    private static double FindStepByBisection(double[] dataX, double[] dataY, double[] p, double[] direction, double[] nodes, double[] nodeValues, double support, double left, double right, int steps)
    {
        double fLeft = DirectionalDerivative(dataX, dataY, p, direction, left, nodes, nodeValues, support);
        double fRight = DirectionalDerivative(dataX, dataY, p, direction, right, nodes, nodeValues, support);
        if (Math.Sign(fLeft) == Math.Sign(fRight))
        {
            return 0.25;
        }
        for (int i = 0; i < steps; i++)
        {
            double mid = 0.5 * (left + right);
            double fMid = DirectionalDerivative(dataX, dataY, p, direction, mid, nodes, nodeValues, support);
            if (Math.Sign(fLeft) == Math.Sign(fMid))
            {
                left = mid;
                fLeft = fMid;
            }
            else
            {
                right = mid;
                fRight = fMid;
            }
        }
        return 0.5 * (left + right);
    }

    private static double[] SolveLinearSystem(double[,] matrix, double[] vector)
    {
        int n = vector.Length;
        double[,] a = new double[n, n + 1];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++) a[i, j] = matrix[i, j];
            a[i, n] = vector[i];
        }

        for (int k = 0; k < n; k++)
        {
            int pivot = k;
            for (int i = k + 1; i < n; i++)
            {
                if (Math.Abs(a[i, k]) > Math.Abs(a[pivot, k])) pivot = i;
            }
            if (pivot != k)
            {
                for (int j = k; j <= n; j++) (a[k, j], a[pivot, j]) = (a[pivot, j], a[k, j]);
            }
            double diagonal = Math.Abs(a[k, k]) < 1.0e-14 ? 1.0e-14 : a[k, k];
            for (int j = k; j <= n; j++) a[k, j] /= diagonal;
            for (int i = 0; i < n; i++)
            {
                if (i == k) continue;
                double factor = a[i, k];
                for (int j = k; j <= n; j++) a[i, j] -= factor * a[k, j];
            }
        }

        double[] solution = new double[n];
        for (int i = 0; i < n; i++) solution[i] = a[i, n];
        return solution;
    }

    private static double[] Negate(double[] vector)
    {
        double[] result = new double[vector.Length];
        for (int i = 0; i < vector.Length; i++) result[i] = -vector[i];
        return result;
    }

    private static double Clamp(double value, double min, double max) => Math.Min(max, Math.Max(min, value));

    private static double MaxAbsoluteError(double[] dataX, double[] dataY, double[] p, double[] nodes, double[] nodeValues, double support)
    {
        double max = 0.0;
        for (int i = 0; i < dataX.Length; i++)
        {
            max = Math.Max(max, Math.Abs(Phi(dataX[i], p, nodes, nodeValues, support) - dataY[i]));
        }
        return max;
    }

    private static CurveSnapshot BuildSnapshot(int iteration, double[] dataX, double[] p, double[] nodes, double[] nodeValues, double support)
    {
        double[] values = new double[dataX.Length];
        for (int i = 0; i < dataX.Length; i++) values[i] = Phi(dataX[i], p, nodes, nodeValues, support);
        return new CurveSnapshot(iteration, values);
    }

    private static void SaveCurves(string path, double[] x, double[] y, double[] sphParameters, double[] optimalParameters, List<CurveSnapshot> snapshots, double[] nodes, double[] nodeValues, double support)
    {
        using var writer = new StreamWriter(path, false);
        writer.Write("x;f(x);SPH;optymalny");
        foreach (CurveSnapshot snapshot in snapshots) writer.Write(";iter_" + snapshot.Iteration);
        writer.WriteLine();
        for (int i = 0; i < x.Length; i++)
        {
            writer.Write("{0};{1};{2};{3}", x[i], y[i], Phi(x[i], sphParameters, nodes, nodeValues, support), Phi(x[i], optimalParameters, nodes, nodeValues, support));
            foreach (CurveSnapshot snapshot in snapshots) writer.Write(";{0}", snapshot.Values[i]);
            writer.WriteLine();
        }
    }

    private static void SaveParameters(string path, double[] parameters)
    {
        using var writer = new StreamWriter(path, false);
        writer.WriteLine("k;parametr;SPH;blad_wzgledny");
        for (int k = 0; k < parameters.Length; k++)
        {
            writer.WriteLine("{0};{1};{2};{3}", k, parameters[k], 0.5, Math.Abs(parameters[k] - 0.5) / 0.5);
        }
    }

    private readonly record struct CurveSnapshot(int Iteration, double[] Values);
}
