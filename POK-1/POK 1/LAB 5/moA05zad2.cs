using System.Globalization;
using POK_1.assets;

namespace POK_1;

internal sealed class MoA05Zad2 : IZadanie
{
    public string Nazwa => "moA05zad2";

    private const float Eps = 1.2e-7f;
    private static readonly CultureInfo PolishCulture = CultureInfo.GetCultureInfo("pl-PL");

    private readonly float _a;
    private readonly float _b;
    private readonly float _c;

    public MoA05Zad2(float a, float b, float c)
    {
        _a = a;
        _b = b;
        _c = c;
    }

    public void Run(TextWriter writer)
    {
        float w1 = _a + (_b + _c);
        float w2 = (_a + _b) + _c;

        WriteValue(writer, "a", _a, "0.00000000000");
        WriteValue(writer, "b", _b, "0.00000");
        WriteValue(writer, "c", _c, "0.00000");
        WriteValue(writer, "w1", w1, "0.00000");
        WriteValue(writer, "w2", w2, "0.00000");
        writer.WriteLine();

        if (w1 == w2)
        {
            writer.WriteLine("w1 jest dokladnie rowne w2");
            return;
        }

        writer.WriteLine("w1 nie jest dokladnie rowne w2");

        if (AreApproximatelyEqual(w1, w2))
        {
            writer.WriteLine("w1 jest w przyblizeniu rowne w2");
        }
        else
        {
            writer.WriteLine("w1 jest istotnie rozne od w2");
        }
    }

    private static bool AreApproximatelyEqual(float x, float y)
    {
        if (x == y)
        {
            return true;
        }

        if (Sign(x) != Sign(y))
        {
            return false;
        }

        float scale = MathF.Max(MathF.Abs(x), MathF.Abs(y));
        if (scale == 0f)
        {
            return true;
        }

        return MathF.Abs(x - y) <= Eps * scale;
    }

    private static int Sign(float x)
    {
        if (x > 0f)
        {
            return 1;
        }

        if (x < 0f)
        {
            return -1;
        }

        return 0;
    }

    private static void WriteValue(TextWriter writer, string label, float value, string format)
    {
        string text = value.ToString(format, PolishCulture);
        writer.WriteLine($"{label,-2} = {text,15}");
    }
}
