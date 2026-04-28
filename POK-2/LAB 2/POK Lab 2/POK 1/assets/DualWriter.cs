using System.Text;

namespace POK_1.assets;

internal sealed class DualWriter : TextWriter
{
    private readonly TextWriter[] _writers;

    public DualWriter(params TextWriter[] writers)
    {
        _writers = writers;
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char value)
    {
        foreach (var writer in _writers)
        {
            writer.Write(value);
        }
    }

    public override void Write(string? value)
    {
        foreach (var writer in _writers)
        {
            writer.Write(value);
        }
    }

    public override void WriteLine(string? value)
    {
        foreach (var writer in _writers)
        {
            writer.WriteLine(value);
        }
    }

    public override void Flush()
    {
        foreach (var writer in _writers)
        {
            writer.Flush();
        }
    }
}
