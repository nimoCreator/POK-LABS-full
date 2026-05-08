using System.Globalization;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad5 : IZadanie
{
    public string Nazwa => "moB02zad5";

    public void Run(TextWriter writer)
    {
        double a = ReadDouble("Podaj poczatek przedzialu a", 0.0);
        double b = ReadDouble("Podaj koniec przedzialu b", 10);
        if (b <= a)
        {
            throw new ArgumentOutOfRangeException(nameof(b), "Koniec przedzialu musi byc wiekszy od poczatku.");
        }

        int nodeCount = ReadInt("Podaj rozmiar N", 101);
        if (nodeCount < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(nodeCount), "Rozmiar N musi byc co najmniej rowny 2.");
        }

        double support = ReadDouble("Podaj nosnik h", 0.05);
        if (support <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(support), "Nosnik musi byc dodatni.");
        }

        const int sampleCount = 501;
        var approximator = new Gauss1D(nodeCount);
        approximator.Apply(Function, a, b, support);

        string outputDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "POK_5_results"));
        Directory.CreateDirectory(outputDirectory);
        string plotFilePath = Path.Combine(outputDirectory, "moB02zad5_plot.csv");

        double trimMargin = Math.Min(3.0 * support, 0.24 * (b - a));

        ErrorMetrics fullMetrics = MeasureErrors(approximator, a, b, sampleCount, 0.0, plotFilePath);
        ErrorMetrics trimmedMetrics = MeasureErrors(approximator, a, b, sampleCount, trimMargin, null);

        writer.WriteLine("aproksymacja gaussowska SPH funkcji f(x) = sin(x)");
        writer.WriteLine("przedzial = <{0:f6}, {1:f6}>", a, b);
        writer.WriteLine("rozmiar N = {0}", nodeCount);
        writer.WriteLine("krok dyskretyzacji = {0:f8}", approximator.Step);
        writer.WriteLine("nosnik h = {0:f8}", support);
        writer.WriteLine("liczba punktow ewaluacji = {0}", sampleCount);
        writer.WriteLine("obciecie brzegow do badan dodatkowych = {0:f8}", trimMargin);
        writer.WriteLine();

        writer.WriteLine("|   i |           x_i |         f(x_i) |");
        writer.WriteLine("+-----+---------------+----------------+");
        int previewCount = Math.Min(nodeCount, 12);
        for (int i = 0; i < previewCount; i++)
        {
            double x = a + i * approximator.Step;
            writer.WriteLine("| {0,3} | {1,13:f8} | {2,14:f10} |", i, x, Function(x));
        }

        writer.WriteLine();
        writer.WriteLine("bledy aproksymacji w calym przedziale:");
        writer.WriteLine("maksymalny blad bezwzgledny = {0:e6}", fullMetrics.MaxAbsoluteError);
        writer.WriteLine("RMSE = {0:e6}", fullMetrics.RootMeanSquareError);
        writer.WriteLine("punkt maksymalnego bledu x = {0:f8}", fullMetrics.XAtMaxError);
        writer.WriteLine();

        writer.WriteLine("bledy aproksymacji po obcieciu brzegow:");
        writer.WriteLine("maksymalny blad bezwzgledny = {0:e6}", trimmedMetrics.MaxAbsoluteError);
        writer.WriteLine("RMSE = {0:e6}", trimmedMetrics.RootMeanSquareError);
        writer.WriteLine("punkt maksymalnego bledu x = {0:f8}", trimmedMetrics.XAtMaxError);
        writer.WriteLine();

        writer.WriteLine("porownanie miar bledu:");
        writer.WriteLine(
            fullMetrics.MaxAbsoluteError > fullMetrics.RootMeanSquareError
                ? "dla calego przedzialu blad maksymalny jest wiekszy od RMSE"
                : "dla calego przedzialu RMSE nie jest mniejszy od bledu maksymalnego");
        writer.WriteLine(
            trimmedMetrics.MaxAbsoluteError > trimmedMetrics.RootMeanSquareError
                ? "po obcieciu brzegow blad maksymalny nadal przewaza nad RMSE"
                : "po obcieciu brzegow RMSE nie jest mniejszy od bledu maksymalnego");
        writer.WriteLine();

        writer.WriteLine("wplyw rozmiaru N oraz nosnika h na blad:");
        writer.WriteLine("|   N |          h |      max full |       rmse full |      max trim |       rmse trim |");
        writer.WriteLine("+-----+------------+---------------+-----------------+---------------+-----------------+");

        foreach (int nVariant in BuildNodeVariants(nodeCount))
        {
            foreach (double hVariant in BuildSupportVariants(support))
            {
                var variantApproximator = new Gauss1D(nVariant);
                variantApproximator.Apply(Function, a, b, hVariant);
                ErrorMetrics variantFull = MeasureErrors(variantApproximator, a, b, sampleCount, 0.0, null);
                double variantTrimMargin = Math.Min(3.0 * hVariant, 0.24 * (b - a));
                ErrorMetrics variantTrimmed = MeasureErrors(variantApproximator, a, b, sampleCount, variantTrimMargin, null);

                writer.WriteLine(
                    "| {0,3} | {1,10:f6} | {2,13:e3} | {3,15:e3} | {4,13:e3} | {5,15:e3} |",
                    nVariant,
                    hVariant,
                    variantFull.MaxAbsoluteError,
                    variantFull.RootMeanSquareError,
                    variantTrimmed.MaxAbsoluteError,
                    variantTrimmed.RootMeanSquareError);
            }
        }

        writer.WriteLine();
        writer.WriteLine("plik danych do wykresu = {0}", plotFilePath);
    }

    private static double ReadDouble(string prompt, double defaultValue)
    {
        Console.Write($"{prompt} [{defaultValue.ToString(CultureInfo.InvariantCulture)}]: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return defaultValue;
        }

        return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
            ? value
            : defaultValue;
    }

    private static int ReadInt(string prompt, int defaultValue)
    {
        Console.Write($"{prompt} [{defaultValue}]: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return defaultValue;
        }

        return int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value) && value > 0
            ? value
            : defaultValue;
    }

    private static double Function(double x)
    {
        return Math.Sin(x);
    }

    private static IReadOnlyList<int> BuildNodeVariants(int nodeCount)
    {
        return new[]
        {
            Math.Max(11, nodeCount / 2),
            nodeCount,
            Math.Max(nodeCount + 1, nodeCount * 2)
        }
        .Distinct()
        .OrderBy(x => x)
        .ToArray();
    }

    private static IReadOnlyList<double> BuildSupportVariants(double support)
    {
        return new[]
        {
            0.5 * support,
            support,
            1.5 * support
        }
        .Distinct()
        .OrderBy(x => x)
        .ToArray();
    }

    private static ErrorMetrics MeasureErrors(
        Gauss1D approximator,
        double a,
        double b,
        int sampleCount,
        double trimMargin,
        string? plotFilePath)
    {
        double from = a + trimMargin;
        double to = b - trimMargin;

        bool writePlot = !string.IsNullOrWhiteSpace(plotFilePath);
        using StreamWriter? plotWriter = writePlot ? new StreamWriter(plotFilePath!, false) : null;
        if (writePlot)
        {
            plotWriter!.WriteLine("x;f(x);g(x);blad_bezwzgledny");
        }

        double maxError = 0.0;
        double xAtMaxError = a;
        double squaredErrorSum = 0.0;
        int usedCount = 0;

        for (int i = 0; i < sampleCount; i++)
        {
            double x = a + (b - a) * i / (sampleCount - 1);
            double fx = Function(x);
            double gx = approximator.Value(x);
            double error = Math.Abs(fx - gx);

            if (writePlot)
            {
                plotWriter!.WriteLine(string.Create(
                    CultureInfo.InvariantCulture,
                    $"{x:f8};{fx:f10};{gx:f10};{error:e6}"));
            }

            if (x < from || x > to)
            {
                continue;
            }

            usedCount++;
            squaredErrorSum += error * error;
            if (error > maxError)
            {
                maxError = error;
                xAtMaxError = x;
            }
        }

        double rmse = usedCount == 0 ? 0.0 : Math.Sqrt(squaredErrorSum / usedCount);
        return new ErrorMetrics(maxError, rmse, xAtMaxError);
    }

    private readonly record struct ErrorMetrics(double MaxAbsoluteError, double RootMeanSquareError, double XAtMaxError);
}
