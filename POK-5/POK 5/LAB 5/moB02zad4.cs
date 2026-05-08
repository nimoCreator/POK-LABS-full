using System.Globalization;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad4 : IZadanie
{
    public string Nazwa => "moB02zad4";

    public void Run(TextWriter writer)
    {
        double a = ReadDouble("Podaj poczatek przedzialu a", 0.0);
        double b = ReadDouble("Podaj koniec przedzialu b", 10);
        if (b <= a)
        {
            throw new ArgumentOutOfRangeException(nameof(b), "Koniec przedzialu musi byc wiekszy od poczatku.");
        }

        int baseOrder = ReadInt("Podaj rzad N z zadania 3", 20);
        if (baseOrder <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(baseOrder), "Rzad dyskretyzacji musi byc dodatni.");
        }

        int doubledOrder = 2 * baseOrder;
        double h = (b - a) / doubledOrder;

        double[] nodes = new double[doubledOrder + 1];
        double[] values = new double[doubledOrder + 1];
        for (int i = 0; i <= doubledOrder; i++)
        {
            nodes[i] = a + i * h;
            values[i] = Function(nodes[i]);
        }

        const int denseSamplesPerSubinterval = 25;
        int sampleCount = doubledOrder * denseSamplesPerSubinterval + 1;

        string outputDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "POK_5_results"));
        Directory.CreateDirectory(outputDirectory);
        string plotFilePath = Path.Combine(outputDirectory, "moB02zad4_plot.csv");

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
                double gx = EvaluatePiecewiseQuadratic(x, nodes, values);
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

        writer.WriteLine("interpolacja kawalkami kwadratowa funkcji f(x) = sin(x)");
        writer.WriteLine("przedzial = <{0:f6}, {1:f6}>", a, b);
        writer.WriteLine("rzad N z zadania 3 = {0}", baseOrder);
        writer.WriteLine("rzad dyskretyzacji uzyty tutaj = {0}", doubledOrder);
        writer.WriteLine("krok h = {0:f8}", h);
        writer.WriteLine("liczba wielomianow kwadratowych = {0}", baseOrder);
        writer.WriteLine("liczba punktow ewaluacji = {0}", sampleCount);
        writer.WriteLine();

        writer.WriteLine("|   i |           x_i |         f(x_i) |");
        writer.WriteLine("+-----+---------------+----------------+");
        for (int i = 0; i <= doubledOrder; i++)
        {
            writer.WriteLine("| {0,3} | {1,13:f8} | {2,14:f10} |", i, nodes[i], values[i]);
        }

        writer.WriteLine();
        writer.WriteLine("przykladowe przedzialy interpolacji kwadratowej:");
        writer.WriteLine("| wiel. |             [x_2k, x_2k+2] |");
        writer.WriteLine("+-------+-----------------------------+");
        for (int segment = 0; segment < Math.Min(baseOrder, 6); segment++)
        {
            int left = 2 * segment;
            writer.WriteLine("| {0,5} | [{1,9:f5}, {2,9:f5}] |",
                segment, nodes[left], nodes[left + 2]);
        }

        writer.WriteLine();
        writer.WriteLine("przykladowe wspolczynniki wielomianow na bazie Lagrange'a stopnia 2:");
        writer.WriteLine("| wiel. |     x0      |     x1      |     x2      |");
        writer.WriteLine("+-------+-------------+-------------+-------------+");
        for (int segment = 0; segment < Math.Min(baseOrder, 6); segment++)
        {
            int left = 2 * segment;
            writer.WriteLine("| {0,5} | {1,11:f6} | {2,11:f6} | {3,11:f6} |",
                segment, nodes[left], nodes[left + 1], nodes[left + 2]);
        }

        writer.WriteLine();
        writer.WriteLine("miary bledu interpolacji:");
        writer.WriteLine("maksymalny blad bezwzgledny = {0:e6}", maxError);
        writer.WriteLine("punkt maksymalnego bledu x = {0:f8}", xAtMaxError);
        writer.WriteLine("blad sredniokwadratowy RMSE = {0:e6}", rmse);
        writer.WriteLine();

        if (maxError > rmse)
        {
            writer.WriteLine("porownanie: blad maksymalny jest wiekszy od RMSE, ale dla interpolacji kwadratowej obie miary powinny byc mniejsze niz w zadaniu 3.");
        }
        else if (maxError < rmse)
        {
            writer.WriteLine("porownanie: RMSE okazal sie wiekszy od bledu maksymalnego, co oznaczaloby szeroko rozlozony blad w przedziale.");
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

    private static double EvaluatePiecewiseQuadratic(double x, double[] nodes, double[] values)
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
        int quadraticSegment = Math.Min(segment / 2, (nodes.Length - 3) / 2);
        int left = 2 * quadraticSegment;

        return EvaluateQuadraticLagrange(
            x,
            nodes[left],
            values[left],
            nodes[left + 1],
            values[left + 1],
            nodes[left + 2],
            values[left + 2]);
    }

    private static double EvaluateQuadraticLagrange(
        double x,
        double x0,
        double y0,
        double x1,
        double y1,
        double x2,
        double y2)
    {
        double l0 = ((x - x1) * (x - x2)) / ((x0 - x1) * (x0 - x2));
        double l1 = ((x - x0) * (x - x2)) / ((x1 - x0) * (x1 - x2));
        double l2 = ((x - x0) * (x - x1)) / ((x2 - x0) * (x2 - x1));
        return y0 * l0 + y1 * l1 + y2 * l2;
    }
}
