namespace POK_5;

internal delegate double Function2D(double x, double y);

internal abstract class Approximator2D
{
    private readonly int _nodeCountX;
    private readonly int _nodeCountY;
    private double _beginX;
    private double _endX;
    private double _beginY;
    private double _endY;
    private double _stepX;
    private double _stepY;
    private double _support;
    private readonly double[] _xNodes;
    private readonly double[] _yNodes;
    private readonly double[,] _values;

    protected Approximator2D(int nodeCountX, int nodeCountY)
    {
        if (nodeCountX < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(nodeCountX), "Liczba wezlow w osi X musi byc co najmniej rowna 2.");
        }

        if (nodeCountY < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(nodeCountY), "Liczba wezlow w osi Y musi byc co najmniej rowna 2.");
        }

        _nodeCountX = nodeCountX;
        _nodeCountY = nodeCountY;
        _xNodes = new double[nodeCountX];
        _yNodes = new double[nodeCountY];
        _values = new double[nodeCountX, nodeCountY];
    }

    public int SizeX => _nodeCountX;
    public int SizeY => _nodeCountY;
    public double BeginX => _beginX;
    public double EndX => _endX;
    public double BeginY => _beginY;
    public double EndY => _endY;
    public double StepX => _stepX;
    public double StepY => _stepY;
    public double CellArea => _stepX * _stepY;
    public double Support => _support;

    public void Apply(Function2D function, double beginX, double endX, double beginY, double endY, double support = 0.0)
    {
        _beginX = beginX;
        _endX = endX;
        _beginY = beginY;
        _endY = endY;

        _stepX = (_endX - _beginX) / (_nodeCountX - 1);
        _stepY = (_endY - _beginY) / (_nodeCountY - 1);
        _support = support <= 0.0 ? 1.5 * Math.Max(_stepX, _stepY) : support;

        for (int i = 0; i < _nodeCountX; i++)
        {
            _xNodes[i] = _beginX + i * _stepX;
        }

        for (int j = 0; j < _nodeCountY; j++)
        {
            _yNodes[j] = _beginY + j * _stepY;
        }

        for (int i = 0; i < _nodeCountX; i++)
        {
            for (int j = 0; j < _nodeCountY; j++)
            {
                _values[i, j] = function(_xNodes[i], _yNodes[j]);
            }
        }
    }

    public double Value(double x, double y)
    {
        return Value(x, y, 0, _nodeCountX - 1, 0, _nodeCountY - 1);
    }

    public double Value(double x, double y, int trimX, int trimY)
    {
        return Value(x, y, trimX, _nodeCountX - 1 - trimX, trimY, _nodeCountY - 1 - trimY);
    }

    public double Value(double x, double y, int fromX, int toX, int fromY, int toY)
    {
        double sum = 0.0;
        for (int i = fromX; i <= toX; i++)
        {
            for (int j = fromY; j <= toY; j++)
            {
                double dx = x - _xNodes[i];
                double dy = y - _yNodes[j];
                double radius = Math.Sqrt(dx * dx + dy * dy);
                double weight = Kernel(radius, _support);
                sum += CellArea * _values[i, j] * weight;
            }
        }

        return sum;
    }

    public abstract double Kernel(double radius, double support);
}
