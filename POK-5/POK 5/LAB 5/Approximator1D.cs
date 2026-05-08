namespace POK_5;

internal delegate double Function1D(double x);

internal abstract class Approximator1D
{
    private readonly int _nodeCount;
    private double _begin;
    private double _end;
    private double _step;
    private double _support;
    private readonly double[] _nodes;
    private readonly double[] _values;

    protected Approximator1D(int nodeCount)
    {
        if (nodeCount < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(nodeCount), "Liczba wezlow musi byc co najmniej rowna 2.");
        }

        _nodeCount = nodeCount;
        _nodes = new double[nodeCount];
        _values = new double[nodeCount];
    }

    public int Size => _nodeCount;
    public double Begin => _begin;
    public double End => _end;
    public double Length => _end - _begin;
    public double Step => _step;
    public double Support => _support;

    public void Apply(Function1D function, double begin, double end, double support = 0.0)
    {
        _begin = begin;
        _end = end;
        _step = (_end - _begin) / (_nodeCount - 1);
        _support = support <= 0.0 ? 1.5 * _step : support;

        for (int i = 0; i < _nodeCount; i++)
        {
            double x = _begin + i * _step;
            _nodes[i] = x;
            _values[i] = function(x);
        }
    }

    public double Value(double x)
    {
        return Value(x, 0, _nodeCount - 1);
    }

    public double Value(double x, int trim)
    {
        return Value(x, trim, _nodeCount - 1 - trim);
    }

    public double Value(double x, int fromIndex, int toIndex)
    {
        double sum = 0.0;
        for (int i = fromIndex; i <= toIndex; i++)
        {
            double distance = Math.Abs(x - _nodes[i]);
            double weight = Kernel(distance, _support);
            sum += _step * _values[i] * weight;
        }

        return sum;
    }

    public double Derivative(double x)
    {
        return Derivative(x, 0, _nodeCount - 1);
    }

    public double Derivative(double x, int trim)
    {
        return Derivative(x, trim, _nodeCount - 1 - trim);
    }

    public double Derivative(double x, int fromIndex, int toIndex)
    {
        double sum = 0.0;
        for (int i = fromIndex; i <= toIndex; i++)
        {
            double node = _nodes[i];
            double distance = Math.Abs(x - node);
            double kernelDerivative = KernelDerivative(distance, _support);
            sum += _step * _values[i] * kernelDerivative * (x - node);
        }

        return sum;
    }

    public double SecondDerivative(double x)
    {
        return SecondDerivative(x, 0, _nodeCount - 1);
    }

    public double SecondDerivative(double x, int trim)
    {
        return SecondDerivative(x, trim, _nodeCount - 1 - trim);
    }

    public double SecondDerivative(double x, int fromIndex, int toIndex)
    {
        double sum = 0.0;
        for (int i = fromIndex; i <= toIndex; i++)
        {
            double distance = Math.Abs(x - _nodes[i]);
            double kernelSecondDerivative = KernelSecondDerivative(distance, _support);
            sum += _step * _values[i] * kernelSecondDerivative;
        }

        return sum;
    }

    public abstract double Kernel(double distance, double support);
    public abstract double KernelDerivative(double distance, double support);
    public abstract double KernelSecondDerivative(double distance, double support);
}
