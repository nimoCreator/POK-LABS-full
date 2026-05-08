using System.Globalization;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad3 : IZadanie
{
    public string Nazwa => "moB02zad3";

    public void Run(TextWriter writer)
    {
        double a = ReadDouble("Podaj poczatek przedzialu a", 0.0);
        double b = ReadDouble("Podaj koniec przedzialu b", 10);
        if (b <= a)
        {
            throw new ArgumentOutOfRangeException(nameof(b), "Koniec przedzialu musi byc wiekszy od poczatku.");
        }

        int n = ReadInt("Podaj rzad dyskretyzacji N", 20);
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Rzad dyskretyzacji musi byc dodatni.");
        }

        double h = (b - a) / n;
        double[] nodes = new double[n + 1];
        double[] values = new double[n + 1];

        for (int i = 0; i <= n; i++)
        {
            nodes[i] = a + i * h;
            values[i] = Function(nodes[i]);
        }

        const int denseSamplesPerInterval = 25;
        int sampleCount = n * denseSamplesPerInterval + 1;

        string outputDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "POK_5_results"));
        Directory.CreateDirectory(outputDirectory);
        string plotFilePath = Path.Combine(outputDirectory, "moB02zad3_plot.csv");

        double maxError = 0.0;
        double errorSquaredSum = 0.0;
        double xAtMaxError = a;

        using (var plotWriter = new StreamWriter(plotFilePath, false))
        {
            plotWriter.WriteLine("x;f(x);g(x);blad_bezwzgledny");

            for (int k = 0; k < sampleCount; k++)
            {
                double x = a + (b - a) * k / (sampleCount - 1);
                double fx = Function(x);
                double gx = EvaluatePiecewiseLinear(x, nodes, values);
                double error = Math.Abs(fx - gx);

                if (error > maxError)
                {
                    maxError = error;
                    xAtMaxError = x;
                }

                errorSquaredSum += error * error;

                plotWriter.WriteLine(string.Create(
                    CultureInfo.InvariantCulture,
                    $"{x:f8};{fx:f10};{gx:f10};{error:e6}"));
            }
        }

        double rmse = Math.Sqrt(errorSquaredSum / sampleCount);

        writer.WriteLine("interpolacja kawalkami liniowa funkcji f(x) = sin(x)");
        writer.WriteLine("przedzial = <{0:f6}, {1:f6}>", a, b);
        writer.WriteLine("rzad dyskretyzacji N = {0}", n);
        writer.WriteLine("krok h = {0:f8}", h);
        writer.WriteLine("liczba punktow ewaluacji = {0}", sampleCount);
        writer.WriteLine();

        writer.WriteLine("|   i |           x_i |         f(x_i) |");
        writer.WriteLine("+-----+---------------+----------------+");
        for (int i = 0; i <= n; i++)
        {
            writer.WriteLine("| {0,3} | {1,13:f8} | {2,14:f10} |", i, nodes[i], values[i]);
        }

        writer.WriteLine();
        writer.WriteLine("przykladowe odcinki splajnow liniowych:");
        writer.WriteLine("| odcinek |            [x_i, x_i+1] |      wsp. kier. |");
        writer.WriteLine("+---------+--------------------------+-----------------+");
        for (int i = 0; i < Math.Min(n, 6); i++)
        {
            double slope = (values[i + 1] - values[i]) / (nodes[i + 1] - nodes[i]);
            writer.WriteLine("| {0,7} | [{1,8:f5}, {2,8:f5}] | {3,15:f10} |",
                i, nodes[i], nodes[i + 1], slope);
        }

        writer.WriteLine();
        writer.WriteLine("miary bledu interpolacji:");
        writer.WriteLine("maksymalny blad bezwzgledny = {0:e6}", maxError);
        writer.WriteLine("punkt maksymalnego bledu x = {0:f8}", xAtMaxError);
        writer.WriteLine("blad sredniokwadratowy RMSE = {0:e6}", rmse);
        writer.WriteLine();

        if (maxError > rmse)
        {
            writer.WriteLine("porownanie: blad maksymalny jest wiekszy od RMSE, wiec pojedyncze lokalne odchylenia sa silniejsze niz blad sredni.");
        }
        else if (maxError < rmse)
        {
            writer.WriteLine("porownanie: RMSE okazal sie wiekszy od bledu maksymalnego, co oznaczaloby duze rozproszenie bledu w calym przedziale.");
        }
        else
        {
            writer.WriteLine("porownanie: obie miary bledu sa rowne.");
        }

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

    private static double EvaluatePiecewiseLinear(double x, double[] nodes, double[] values)
    {
        if (x <= nodes[0])
        {
            return values[0];
        }

        if (x >= nodes[^1])
        {
            return values[^1];
        }

        int segment = Array.BinarySearch(nodes, x);
        if (segment >= 0)
        {
            return values[segment];
        }

        segment = ~segment - 1;
        double x1 = nodes[segment];
        double x2 = nodes[segment + 1];
        double y1 = values[segment];
        double y2 = values[segment + 1];

        double t = (x - x1) / (x2 - x1);
        return y1 + t * (y2 - y1);
    }
}
