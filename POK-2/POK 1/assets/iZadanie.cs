using System.Diagnostics;

namespace POK_1.assets;

internal interface IZadanie
{
    string Nazwa { get; }

    void Run(TextWriter writer);

    string FileName => $"{Nazwa}.txt";

    void ZapiszWynik(string outputDirectory)
    {
        var filePath = Path.Combine(outputDirectory, FileName);

        using var fileWriter = new StreamWriter(filePath, false);
        using var writer = CreateWriter(fileWriter);
        var stopwatch = Stopwatch.StartNew();

        writer.WriteLine("====================================");
        writer.WriteLine($"ZADANIE {Nazwa}");
        writer.WriteLine("------------------------------------");

        Run(writer);

        stopwatch.Stop();
        writer.WriteLine("------------------------------------");
        writer.WriteLine($"rozwiazanie zajelo: {stopwatch.Elapsed.TotalMilliseconds:f3} ms");
        writer.WriteLine("====================================");
        writer.Flush();
    }

    private static TextWriter CreateWriter(TextWriter fileWriter)
    {
#if NO_CONSOLE_OUTPUT
        return fileWriter;
#else
        return new DualWriter(Console.Out, fileWriter);
#endif
    }
}
