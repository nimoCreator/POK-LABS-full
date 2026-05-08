using System.Globalization;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad10 : IZadanie
{
    public string Nazwa => "moB02zad10";

    public void Run(TextWriter writer)
    {
        int n = 21;
        double xmin = -4.0;
        double xmax = 6.0;
        double alpha = 2.75;
        double beta = -1.20;
        double epsilon = 1.0e-2;
        int seed = 301954;

        var random = new Random(seed);
        double step = (xmax - xmin) / (n - 1);
        double[] x = new double[n];
        double[] y = new double[n];

        for (int i = 0; i < n; i++)
        {
            x[i] = xmin + i * step;
            double disturbance = epsilon * (2.0 * random.NextDouble() - 1.0);
            y[i] = alpha * x[i] + beta + disturbance;
        }

        LinearFit fit = FitLine(x, y);
        double alphaRelativeError = RelativeError(fit.A, alpha);
        double betaRelativeError = RelativeError(fit.B, beta);

        writer.WriteLine("regresja liniowa dla danych sztucznie zaburzonych");
        writer.WriteLine("N = {0}", n);
        writer.WriteLine("xmin = {0:f6}, xmax = {1:f6}", xmin, xmax);
        writer.WriteLine("krok dyskretyzacji dx = {0:f8}", step);
        writer.WriteLine("parametry generowania: alpha = {0:f8}, beta = {1:f8}", alpha, beta);
        writer.WriteLine("zaburzenie losowe epsilon = {0:e3}", epsilon);
        writer.WriteLine("seed = {0}", seed);
        writer.WriteLine();

        writer.WriteLine("|   i |             x_i |             y_i |");
        writer.WriteLine("+-----+-----------------+-----------------+");
        for (int i = 0; i < n; i++)
        {
            writer.WriteLine("| {0,3} | {1,15:f8} | {2,15:f8} |", i, x[i], y[i]);
        }

        writer.WriteLine();
        writer.WriteLine("sumy kontrolne:");
        writer.WriteLine("suma x      = {0:f10}", fit.SumX);
        writer.WriteLine("suma y      = {0:f10}", fit.SumY);
        writer.WriteLine("suma x*x    = {0:f10}", fit.SumXX);
        writer.WriteLine("suma x*y    = {0:f10}", fit.SumXY);
        writer.WriteLine();

        writer.WriteLine("wyniki regresji liniowej y = a*x + b:");
        writer.WriteLine("a = {0:f10}", fit.A);
        writer.WriteLine("b = {0:f10}", fit.B);
        writer.WriteLine();
        writer.WriteLine("bledy wzgledne wzgledem parametrow uzytych w generatorze danych:");
        writer.WriteLine("delta alpha = |a - alpha| / |alpha| = {0:e6}", alphaRelativeError);
        writer.WriteLine("delta beta  = |b - beta|  / |beta|  = {0:e6}", betaRelativeError);
        writer.WriteLine();
        writer.WriteLine("wniosek: dla malego epsilon dopasowane parametry sa bliskie parametrom generatora danych.");
    }

    private static LinearFit FitLine(double[] x, double[] y)
    {
        if (x.Length != y.Length || x.Length < 2)
        {
            throw new ArgumentException("Zbiory x oraz y musza miec ten sam rozmiar wiekszy od 1.");
        }

        int n = x.Length;
        double sumX = 0.0;
        double sumY = 0.0;
        double sumXX = 0.0;
        double sumXY = 0.0;

        for (int i = 0; i < n; i++)
        {
            sumX += x[i];
            sumY += y[i];
            sumXX += x[i] * x[i];
            sumXY += x[i] * y[i];
        }

        double denominator = n * sumXX - sumX * sumX;
        if (Math.Abs(denominator) < 1.0e-15)
        {
            throw new InvalidOperationException("Nie mozna wykonac regresji, bo mianownik jest zbyt maly.");
        }

        double a = (n * sumXY - sumX * sumY) / denominator;
        double b = (sumY - a * sumX) / n;
        return new LinearFit(a, b, sumX, sumY, sumXX, sumXY);
    }

    private static double RelativeError(double value, double expected)
    {
        double denominator = Math.Max(Math.Abs(expected), 1.0e-15);
        return Math.Abs(value - expected) / denominator;
    }

    private readonly record struct LinearFit(double A, double B, double SumX, double SumY, double SumXX, double SumXY);
}
