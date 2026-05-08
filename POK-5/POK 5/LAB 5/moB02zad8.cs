using System.Globalization;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad8 : IZadanie
{
    public string Nazwa => "moB02zad8";

    public void Run(TextWriter writer)
    {
        double a = ReadDouble("Podaj poczatek przedzialu a", 0.0);
        double b = ReadDouble("Podaj koniec przedzialu b", 10);
        if (b <= a)
        {
            throw new ArgumentOutOfRangeException(nameof(b), "Koniec przedzialu b musi byc wiekszy od poczatku a.");
        }

        double c = ReadDouble("Podaj poczatek drugiego przedzialu c", 0.0);
        double d = ReadDouble("Podaj koniec drugiego przedzialu d", 10);
        if (d <= c)
        {
            throw new ArgumentOutOfRangeException(nameof(d), "Przyjalem standardowy warunek d > c dla drugiego przedzialu.");
        }

        int nodeCountX = ReadInt("Podaj rozmiar N", 31);
        int nodeCountY = ReadInt("Podaj rozmiar M", 31);
        if (nodeCountX < 2 || nodeCountY < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(nodeCountX), "Rozmiary N i M musza byc co najmniej rowne 2.");
        }

        double support = ReadDouble("Podaj nosnik h", 0.12);
        if (support <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(support), "Nosnik musi byc dodatni.");
        }

        int sampleCountX = 41;
        int sampleCountY = 41;

        var approximator = new Gauss2D(nodeCountX, nodeCountY);
        approximator.Apply(Function, a, b, c, d, support);

        string outputDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "POK_5_results"));
        Directory.CreateDirectory(outputDirectory);
        string plotFilePath = Path.Combine(outputDirectory, "moB02zad8_plot.csv");

        double trimMargin = Math.Min(3.0 * support, 0.24 * Math.Min(b - a, d - c));

        ErrorMetrics fullMetrics = MeasureErrors(approximator, a, b, c, d, sampleCountX, sampleCountY, 0.0, plotFilePath);
        ErrorMetrics trimmedMetrics = MeasureErrors(approximator, a, b, c, d, sampleCountX, sampleCountY, trimMargin, null);

        writer.WriteLine("dwuwymiarowa aproksymacja gaussowska SPH funkcji f(x,y) = sin(x) * sin(y)");
        writer.WriteLine("przedzial X = <{0:f6}, {1:f6}>", a, b);
        writer.WriteLine("przedzial Y = <{0:f6}, {1:f6}>", c, d);
        writer.WriteLine("rozmiar siatki N x M = {0} x {1}", nodeCountX, nodeCountY);
        writer.WriteLine("krok hx = {0:f8}", approximator.StepX);
        writer.WriteLine("krok hy = {0:f8}", approximator.StepY);
        writer.WriteLine("pole komorki = {0:f8}", approximator.CellArea);
        writer.WriteLine("nosnik h = {0:f8}", support);
        writer.WriteLine("siatka ewaluacji = {0} x {1}", sampleCountX, sampleCountY);
        writer.WriteLine("obciecie brzegow do badan dodatkowych = {0:f8}", trimMargin);
        writer.WriteLine();

        writer.WriteLine("przykladowe wezly siatki i wartosci funkcji:");
        writer.WriteLine("|   i |   j |         x_i |         y_j |       f(x_i,y_j) |");
        writer.WriteLine("+-----+-----+-------------+-------------+------------------+");

        int previewX = Math.Min(nodeCountX, 4);
        int previewY = Math.Min(nodeCountY, 4);
        for (int i = 0; i < previewX; i++)
        {
            double x = a + i * approximator.StepX;
            for (int j = 0; j < previewY; j++)
            {
                double y = c + j * approximator.StepY;
                writer.WriteLine("| {0,3} | {1,3} | {2,11:f6} | {3,11:f6} | {4,16:f10} |",
                    i, j, x, y, Function(x, y));
            }
        }

        writer.WriteLine();
        writer.WriteLine("bledy aproksymacji w calym obszarze:");
        writer.WriteLine("maksymalny blad bezwzgledny = {0:e6}", fullMetrics.MaxAbsoluteError);
        writer.WriteLine("RMSE = {0:e6}", fullMetrics.RootMeanSquareError);
        writer.WriteLine("punkt maksymalnego bledu = ({0:f8}, {1:f8})", fullMetrics.XAtMaxError, fullMetrics.YAtMaxError);
        writer.WriteLine();

        writer.WriteLine("bledy aproksymacji po obcieciu brzegow:");
        writer.WriteLine("maksymalny blad bezwzgledny = {0:e6}", trimmedMetrics.MaxAbsoluteError);
        writer.WriteLine("RMSE = {0:e6}", trimmedMetrics.RootMeanSquareError);
        writer.WriteLine("punkt maksymalnego bledu = ({0:f8}, {1:f8})", trimmedMetrics.XAtMaxError, trimmedMetrics.YAtMaxError);
        writer.WriteLine();

        writer.WriteLine("porownanie miar bledu:");
        writer.WriteLine(
            fullMetrics.MaxAbsoluteError > fullMetrics.RootMeanSquareError
                ? "dla calego obszaru blad maksymalny jest wiekszy od RMSE"
                : "dla calego obszaru RMSE nie jest mniejszy od bledu maksymalnego");
        writer.WriteLine(
            trimmedMetrics.MaxAbsoluteError > trimmedMetrics.RootMeanSquareError
                ? "po obcieciu brzegow blad maksymalny nadal przewaza nad RMSE"
                : "po obcieciu brzegow RMSE nie jest mniejszy od bledu maksymalnego");
        writer.WriteLine();

        writer.WriteLine("wplyw rozmiarow N, M oraz nosnika h na blad:");
        writer.WriteLine("|   N |   M |          h |      max full |       rmse full |      max trim |       rmse trim |");
        writer.WriteLine("+-----+-----+------------+---------------+-----------------+---------------+-----------------+");

        foreach (int nx in BuildSizeVariants(nodeCountX))
        {
            foreach (int ny in BuildSizeVariants(nodeCountY))
            {
                foreach (double hVariant in BuildSupportVariants(support))
                {
                    var variantApproximator = new Gauss2D(nx, ny);
                    variantApproximator.Apply(Function, a, b, c, d, hVariant);

                    ErrorMetrics variantFull = MeasureErrors(
                        variantApproximator, a, b, c, d, sampleCountX, sampleCountY, 0.0, null);

                    double variantTrimMargin = Math.Min(3.0 * hVariant, 0.24 * Math.Min(b - a, d - c));
                    ErrorMetrics variantTrimmed = MeasureErrors(
                        variantApproximator, a, b, c, d, sampleCountX, sampleCountY, variantTrimMargin, null);

                    writer.WriteLine(
                        "| {0,3} | {1,3} | {2,10:f6} | {3,13:e3} | {4,15:e3} | {5,13:e3} | {6,15:e3} |",
                        nx,
                        ny,
                        hVariant,
                        variantFull.MaxAbsoluteError,
                        variantFull.RootMeanSquareError,
                        variantTrimmed.MaxAbsoluteError,
                        variantTrimmed.RootMeanSquareError);
                }
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

    private static double Function(double x, double y)
    {
        return Math.Sin(x) * Math.Sin(y);
    }

    private static IReadOnlyList<int> BuildSizeVariants(int size)
    {
        return new[]
        {
            Math.Max(11, size / 2),
            size,
            Math.Max(size + 1, size * 2)
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
        Gauss2D approximator,
        double a,
        double b,
        double c,
        double d,
        int sampleCountX,
        int sampleCountY,
        double trimMargin,
        string? plotFilePath)
    {
        double fromX = a + trimMargin;
        double toX = b - trimMargin;
        double fromY = c + trimMargin;
        double toY = d - trimMargin;

        bool writePlot = !string.IsNullOrWhiteSpace(plotFilePath);
        using StreamWriter? plotWriter = writePlot ? new StreamWriter(plotFilePath!, false) : null;
        if (writePlot)
        {
            plotWriter!.WriteLine("x;y;f(x,y);g(x,y);blad_bezwzgledny");
        }

        double maxError = 0.0;
        double xAtMaxError = a;
        double yAtMaxError = c;
        double squaredErrorSum = 0.0;
        int usedCount = 0;

        for (int i = 0; i < sampleCountX; i++)
        {
            double x = a + (b - a) * i / (sampleCountX - 1);
            for (int j = 0; j < sampleCountY; j++)
            {
                double y = c + (d - c) * j / (sampleCountY - 1);
                double exact = Function(x, y);
                double approx = approximator.Value(x, y);
                double error = Math.Abs(exact - approx);

                if (writePlot)
                {
                    plotWriter!.WriteLine(string.Create(
                        CultureInfo.InvariantCulture,
                        $"{x:f8};{y:f8};{exact:f10};{approx:f10};{error:e6}"));
                }

                if (x < fromX || x > toX || y < fromY || y > toY)
                {
                    continue;
                }

                usedCount++;
                squaredErrorSum += error * error;
                if (error > maxError)
                {
                    maxError = error;
                    xAtMaxError = x;
                    yAtMaxError = y;
                }
            }
        }

        double rmse = usedCount == 0 ? 0.0 : Math.Sqrt(squaredErrorSum / usedCount);
        return new ErrorMetrics(maxError, rmse, xAtMaxError, yAtMaxError);
    }

    private readonly record struct ErrorMetrics(
        double MaxAbsoluteError,
        double RootMeanSquareError,
        double XAtMaxError,
        double YAtMaxError);
}
