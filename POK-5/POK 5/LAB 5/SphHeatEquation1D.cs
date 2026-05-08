namespace POK_5;

internal enum SphHeatApproach
{
    KernelLaplacian,
    TaylorLaplacian
}

internal readonly record struct SphHeatSimulationConfig(
    double SpatialBegin,
    double SpatialEnd,
    double TimeBegin,
    double TimeEnd,
    int NodeCount,
    int TimeLevelCount,
    double SupportFactor,
    double DiffusionFactor,
    double InitialTemperature,
    double LeftBoundaryTemperature,
    double RightBoundaryTemperature,
    int CaptureStartStep = 100,
    int CaptureCount = 11)
{
    public double SpatialStep => (SpatialEnd - SpatialBegin) / (NodeCount - 1);

    public double TimeStep => (TimeEnd - TimeBegin) / (TimeLevelCount - 1);

    public double Support => SupportFactor * SpatialStep;

    public double Diffusivity => DiffusionFactor * SpatialStep * SpatialStep / TimeStep;
}

internal sealed record SphHeatSnapshot(int Step, double Time, double[] Temperatures);

internal sealed record SphHeatSimulationResult(
    SphHeatApproach Approach,
    SphHeatSimulationConfig Config,
    double[] Nodes,
    IReadOnlyList<SphHeatSnapshot> Snapshots,
    double[] FinalTemperatures,
    bool IsStable,
    string StatusMessage);

internal sealed class SphHeatEquation1D
{
    private readonly SphHeatSimulationConfig _config;
    private readonly double[] _nodes;
    private readonly int[] _captureSteps;
    private readonly int _neighbourRadius;

    public SphHeatEquation1D(SphHeatSimulationConfig config)
    {
        if (config.SpatialEnd <= config.SpatialBegin)
        {
            throw new ArgumentOutOfRangeException(nameof(config), "Koniec przedzialu przestrzennego musi byc wiekszy od poczatku.");
        }

        if (config.TimeEnd <= config.TimeBegin)
        {
            throw new ArgumentOutOfRangeException(nameof(config), "Koniec przedzialu czasowego musi byc wiekszy od poczatku.");
        }

        if (config.NodeCount < 3)
        {
            throw new ArgumentOutOfRangeException(nameof(config), "Liczba wezlow przestrzennych musi byc co najmniej rowna 3.");
        }

        if (config.TimeLevelCount < 101)
        {
            throw new ArgumentOutOfRangeException(nameof(config), "Liczba poziomow czasowych musi byc co najmniej rowna 101.");
        }

        if (config.SupportFactor <= 0.0 || config.DiffusionFactor <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(config), "Parametry bezwymiarowe musza byc dodatnie.");
        }

        _config = config;
        _nodes = new double[config.NodeCount];
        for (int i = 0; i < config.NodeCount; i++)
        {
            _nodes[i] = config.SpatialBegin + i * config.SpatialStep;
        }

        _captureSteps = BuildCaptureSteps(config);
        _neighbourRadius = Math.Max(1, (int)Math.Ceiling(2.0 * config.Support / config.SpatialStep));
    }

    public SphHeatSimulationResult Simulate(SphHeatApproach approach)
    {
        double[] current = CreateInitialState();
        double[] next = new double[current.Length];
        var snapshots = new List<SphHeatSnapshot>(_captureSteps.Length);
        int captureIndex = 0;

        for (int step = 0; step < _config.TimeLevelCount; step++)
        {
            if (captureIndex < _captureSteps.Length && step == _captureSteps[captureIndex])
            {
                snapshots.Add(new SphHeatSnapshot(step, TimeAtStep(step), (double[])current.Clone()));
                captureIndex++;
            }

            if (step == _config.TimeLevelCount - 1)
            {
                break;
            }

            next[0] = _config.LeftBoundaryTemperature;
            next[^1] = _config.RightBoundaryTemperature;

            for (int i = 1; i < current.Length - 1; i++)
            {
                double laplacian = approach == SphHeatApproach.KernelLaplacian
                    ? ComputeKernelLaplacian(current, i)
                    : ComputeTaylorLaplacian(current, i);

                next[i] = current[i] + _config.TimeStep * _config.Diffusivity * laplacian;
            }

            if (!IsFinite(next))
            {
                return new SphHeatSimulationResult(
                    approach,
                    _config,
                    (double[])_nodes.Clone(),
                    snapshots,
                    (double[])next.Clone(),
                    false,
                    $"niestabilnosc numeryczna po kroku {step + 1}");
            }

            (current, next) = (next, current);
        }

        return new SphHeatSimulationResult(
            approach,
            _config,
            (double[])_nodes.Clone(),
            snapshots,
            (double[])current.Clone(),
            true,
            "symulacja zakonczona poprawnie");
    }

    private double[] CreateInitialState()
    {
        double[] state = new double[_config.NodeCount];
        for (int i = 0; i < state.Length; i++)
        {
            state[i] = _config.InitialTemperature;
        }

        state[0] = _config.LeftBoundaryTemperature;
        state[^1] = _config.RightBoundaryTemperature;
        return state;
    }

    private double ComputeKernelLaplacian(double[] state, int index)
    {
        int from = Math.Max(0, index - _neighbourRadius);
        int to = Math.Min(state.Length - 1, index + _neighbourRadius);

        double xi = _nodes[index];
        double ti = state[index];
        double sum = 0.0;
        for (int j = from; j <= to; j++)
        {
            double distance = Math.Abs(xi - _nodes[j]);
            sum += _config.SpatialStep * (state[j] - ti) * Monaghan1D.Laplacian(distance, _config.Support);
        }

        for (int ghost = 1; ghost <= _neighbourRadius; ghost++)
        {
            double leftGhost = _config.SpatialBegin - ghost * _config.SpatialStep;
            double rightGhost = _config.SpatialEnd + ghost * _config.SpatialStep;

            double leftDistance = Math.Abs(xi - leftGhost);
            double rightDistance = Math.Abs(xi - rightGhost);

            sum += _config.SpatialStep * (_config.LeftBoundaryTemperature - ti) * Monaghan1D.Laplacian(leftDistance, _config.Support);
            sum += _config.SpatialStep * (_config.RightBoundaryTemperature - ti) * Monaghan1D.Laplacian(rightDistance, _config.Support);
        }

        return sum;
    }

    private double ComputeTaylorLaplacian(double[] state, int index)
    {
        int from = Math.Max(0, index - _neighbourRadius);
        int to = Math.Min(state.Length - 1, index + _neighbourRadius);

        double xi = _nodes[index];
        double ti = state[index];
        double sum = 0.0;
        for (int j = from; j <= to; j++)
        {
            if (j == index)
            {
                continue;
            }

            double distance = Math.Abs(xi - _nodes[j]);
            double derivativeOverRadius = Monaghan1D.DerivativeOverRadius(distance, _config.Support);
            sum += 2.0 * _config.SpatialStep * (ti - state[j]) * derivativeOverRadius;
        }

        return sum;
    }

    private double TimeAtStep(int step)
    {
        return _config.TimeBegin + step * _config.TimeStep;
    }

    private static int[] BuildCaptureSteps(SphHeatSimulationConfig config)
    {
        int firstStep = Math.Min(Math.Max(0, config.CaptureStartStep), config.TimeLevelCount - 1);
        int lastStep = config.TimeLevelCount - 1;
        if (config.CaptureCount <= 1 || firstStep == lastStep)
        {
            return new[] { firstStep };
        }

        var steps = new HashSet<int>();
        double increment = (lastStep - firstStep) / (double)(config.CaptureCount - 1);
        for (int i = 0; i < config.CaptureCount; i++)
        {
            int step = (int)Math.Round(firstStep + i * increment);
            step = Math.Clamp(step, firstStep, lastStep);
            steps.Add(step);
        }

        return steps.OrderBy(x => x).ToArray();
    }

    private static bool IsFinite(double[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (double.IsNaN(values[i]) || double.IsInfinity(values[i]))
            {
                return false;
            }
        }

        return true;
    }
}
