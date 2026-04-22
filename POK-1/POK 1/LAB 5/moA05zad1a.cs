using POK_1.assets;

namespace POK_1;

internal sealed class MoA05Zad1a : IZadanie
{
    public string Nazwa => "moA05zad1a";

    private readonly float _a;
    private readonly float _d;
    public MoA05Zad1a(float a, float d)
    {
        _a = a;
        _d = d;
    }

    public void Run(TextWriter writer)
    {
        writer.WriteLine("a = {0}\n", _a);
        writer.WriteLine("d = {0}\n", _d);

        float x = _a;

        while (true)
        {
            writer.Write($"x = {x}");
            if(x + 1 == 1)
            {
                writer.WriteLine(" => 1+x jest rowne 1 !!!");
                writer.WriteLine($"precyzja dla typu float wynosi, eps = { x * _d }");
                break;
            }
            writer.WriteLine($"");
            x /= _d;
        }
    }
}
