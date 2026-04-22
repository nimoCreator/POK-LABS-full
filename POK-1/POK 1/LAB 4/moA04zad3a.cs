using POK_1.assets;

namespace POK_1;

internal sealed class MoA04Zad3a : IZadanie
{
    public string Nazwa => "moA04zad3a";

    private readonly int _seed;
    private readonly int[] _sampleSizes;

    public MoA04Zad3a(int seed, params int[] sampleSizes)
    {
        _seed = seed;
        _sampleSizes = sampleSizes.OrderBy(value => value).ToArray();
    }

    public void Run(TextWriter writer)
    {
        var random = new Random(_seed);
        int insideCircle = 0;
        int targetIndex = 0;

        for (int i = 1; i <= _sampleSizes[^1]; i++)
        {
            double x = random.NextDouble() - 0.5;
            double y = random.NextDouble() - 0.5;

            if ((x * x) + (y * y) <= 0.25)
            {
                insideCircle++;
            }

            if (i == _sampleSizes[targetIndex])
            {
                double approximation = 4.0 * insideCircle / i;
                writer.WriteLine("w({0}) = {1:g15}", i, approximation);
                targetIndex++;

                if (targetIndex == _sampleSizes.Length)
                {
                    break;
                }
            }
        }

        writer.WriteLine("       pi = {0:g15}", Math.PI);
        writer.WriteLine();
    }
}
