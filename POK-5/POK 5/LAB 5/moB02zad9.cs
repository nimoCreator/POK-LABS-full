using System.Globalization;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad9 : IZadanie
{
    public string Nazwa => "moB02zad9";

    public void Run(TextWriter writer)
    {
        double a = ReadDouble("Podaj poczatek przedzialu przestrzennego a", 0.0);
        double b = ReadDouble("Podaj koniec przedzialu przestrzennego b", 1.0);
        double t0 = ReadDouble("Podaj poczatek przedzialu czasowego t0", 0.0);
        double te = ReadDouble("Podaj koniec przedzialu czasowego te", 1.0);
        int nodeCount = ReadInt("Podaj liczbe wezlow przestrzennych N", 101);
        int timeLevelCount = ReadInt("Podaj liczbe poziomow czasowych M", 1201);
        double diffusionFactor = ReadDouble("Podaj mnoznik dyfuzyjnosci q", 0.25);
        double supportFactor = ReadDouble("Podaj mnoznik nosnika eta", 1.25);

        var config = new SphHeatSimulationConfig(
            SpatialBegin: a,
            SpatialEnd: b,
            TimeBegin: t0,
            TimeEnd: te,
            NodeCount: nodeCount,
            TimeLevelCount: timeLevelCount,
            SupportFactor: supportFactor,
            DiffusionFactor: diffusionFactor,
            InitialTemperature: 20.0,
            LeftBoundaryTemperature: 100.0,
            RightBoundaryTemperature: 0.0);

        string outputDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "POK_5_results"));
        Directory.CreateDirectory(outputDirectory);

        string approach1FilePath = Path.Combine(outputDirectory, "moB02zad9_podejscie1.xls");
        string approach2FilePath = Path.Combine(outputDirectory, "moB02zad9_podejscie2.xls");

        var exact = new HeatConductionExactSolution1D(
            a,
            b,
            t0,
            config.Diffusivity,
            config.LeftBoundaryTemperature,
            config.RightBoundaryTemperature,
            config.InitialTemperature);

        var solver = new SphHeatEquation1D(config);
        SphHeatSimulationResult approach1Result = solver.Simulate(SphHeatApproach.KernelLaplacian);
        SphHeatSimulationResult approach2Result = solver.Simulate(SphHeatApproach.TaylorLaplacian);

        EvaluatedMethod approach1 = EvaluateMethod(
            "podejscie 1 - laplasjan jadra Monaghana",
            approach1Result,
            exact);
        EvaluatedMethod approach2 = EvaluateMethod(
            "podejscie 2 - dyskretyzacja Taylora",
            approach2Result,
            exact);

        SaveSpreadsheet(approach1FilePath, approach1);
        SaveSpreadsheet(approach2FilePath, approach2);

        ErrorMetrics methodsDifference = MeasureMethodDifference(approach1, approach2);

        writer.WriteLine("symulacja SPH przewodnictwa cieplnego w 1D dla rownania Fouriera");
        writer.WriteLine("warunek poczatkowy: T(x,t0) = 20");
        writer.WriteLine("warunki brzegowe stale: T(a,t) = 100, T(b,t) = 0");
        writer.WriteLine("przedzial przestrzenny = <{0:f6}, {1:f6}>", a, b);
        writer.WriteLine("przedzial czasowy = <{0:f6}, {1:f6}>", t0, te);
        writer.WriteLine("N = {0}, M = {1}", nodeCount, timeLevelCount);
        writer.WriteLine("krok przestrzenny w = {0:f8}", config.SpatialStep);
        writer.WriteLine("krok czasowy dt = {0:f8}", config.TimeStep);
        writer.WriteLine("mnoznik dyfuzyjnosci q = {0:f6}", diffusionFactor);
        writer.WriteLine("mnoznik nosnika eta = {0:f6}", supportFactor);
        writer.WriteLine("dyfuzyjnosc kappa = {0:f8}", config.Diffusivity);
        writer.WriteLine("nosnik jadra h = {0:f8}", config.Support);
        writer.WriteLine("promien obciecia do wykresow = <a + 3w, b - 1w>");
        writer.WriteLine();

        writer.WriteLine("czasy zapisanych serii:");
        for (int i = 0; i < approach1.Series.Count; i++)
        {
            writer.WriteLine("t[{0,2}] = {1:f8}", i, approach1.Series[i].Time);
        }

        writer.WriteLine();
        writer.WriteLine("przykladowe wartosci w pierwszym zapisanym czasie dla podejscia 1:");
        writer.WriteLine("|   i |           x |       T dokl. |        T SPH |    T gladkie |");
        writer.WriteLine("+-----+-------------+---------------+--------------+--------------+");
        int previewCount = Math.Min(8, approach1.SamplePoints.Length);
        OutputSeries firstSeries = approach1.Series[0];
        for (int i = 0; i < previewCount; i++)
        {
            writer.WriteLine(
                "| {0,3} | {1,11:f6} | {2,13:f6} | {3,12:f6} | {4,12:f6} |",
                i,
                approach1.SamplePoints[i],
                firstSeries.Exact[i],
                firstSeries.Raw[i],
                firstSeries.Smoothed[i]);
        }

        WriteMethodSummary(writer, approach1);
        WriteMethodSummary(writer, approach2);

        writer.WriteLine("roznica miedzy podejsciami w tych samych punktach kontrolnych:");
        writer.WriteLine("maksymalna roznica bezwzgledna = {0:e6}", methodsDifference.MaxAbsoluteError);
        writer.WriteLine("RMSE roznicy = {0:e6}", methodsDifference.RootMeanSquareError);
        writer.WriteLine("punkt maksymalnej roznicy: x = {0:f8}, t = {1:f8}", methodsDifference.XAtMaxError, methodsDifference.TimeAtMaxError);
        writer.WriteLine();

        writer.WriteLine("wplyw liczby wezlow N na blad koncowy:");
        writer.WriteLine("| parametr | wartosc |      met1 max |     met1 rmse |      met2 max |     met2 rmse |      status |");
        writer.WriteLine("+----------+---------+---------------+---------------+---------------+---------------+-------------+");
        foreach (int nVariant in BuildNodeVariants(nodeCount))
        {
            VariantSummary variant = SummarizeVariant(config with { NodeCount = nVariant }, exactTerms: 400);
            writer.WriteLine(
                "| {0,-8} | {1,7} | {2,13:e3} | {3,13:e3} | {4,13:e3} | {5,13:e3} | {6,-11} |",
                "N",
                nVariant,
                variant.Method1FinalError.MaxAbsoluteError,
                variant.Method1FinalError.RootMeanSquareError,
                variant.Method2FinalError.MaxAbsoluteError,
                variant.Method2FinalError.RootMeanSquareError,
                variant.StatusLabel);
        }

        writer.WriteLine();
        writer.WriteLine("wplyw mnoznika nosnika eta na blad koncowy:");
        writer.WriteLine("| parametr | wartosc |      met1 max |     met1 rmse |      met2 max |     met2 rmse |      status |");
        writer.WriteLine("+----------+---------+---------------+---------------+---------------+---------------+-------------+");
        foreach (double supportVariant in BuildSupportFactorVariants(supportFactor))
        {
            VariantSummary variant = SummarizeVariant(config with { SupportFactor = supportVariant }, exactTerms: 400);
            writer.WriteLine(
                "| {0,-8} | {1,7:f3} | {2,13:e3} | {3,13:e3} | {4,13:e3} | {5,13:e3} | {6,-11} |",
                "eta",
                supportVariant,
                variant.Method1FinalError.MaxAbsoluteError,
                variant.Method1FinalError.RootMeanSquareError,
                variant.Method2FinalError.MaxAbsoluteError,
                variant.Method2FinalError.RootMeanSquareError,
                variant.StatusLabel);
        }

        writer.WriteLine();
        writer.WriteLine("wplyw mnoznika dyfuzyjnosci q na blad koncowy i stabilnosc:");
        writer.WriteLine("| parametr | wartosc |      met1 max |     met1 rmse |      met2 max |     met2 rmse |      status |");
        writer.WriteLine("+----------+---------+---------------+---------------+---------------+---------------+-------------+");
        foreach (double diffusionVariant in BuildDiffusionVariants(diffusionFactor))
        {
            VariantSummary variant = SummarizeVariant(config with { DiffusionFactor = diffusionVariant }, exactTerms: 400);
            writer.WriteLine(
                "| {0,-8} | {1,7:f3} | {2,13:e3} | {3,13:e3} | {4,13:e3} | {5,13:e3} | {6,-11} |",
                "q",
                diffusionVariant,
                variant.Method1FinalError.MaxAbsoluteError,
                variant.Method1FinalError.RootMeanSquareError,
                variant.Method2FinalError.MaxAbsoluteError,
                variant.Method2FinalError.RootMeanSquareError,
                variant.StatusLabel);
        }

        writer.WriteLine();
        writer.WriteLine("plik wynikowy podejscia 1 = {0}", approach1FilePath);
        writer.WriteLine("plik wynikowy podejscia 2 = {0}", approach2FilePath);
        writer.WriteLine("uwaga: pliki .xls sa zapisane jako tekst rozdzielany srednikami i mozna je bezposrednio otworzyc w Excelu.");
    }

    private static void WriteMethodSummary(TextWriter writer, EvaluatedMethod method)
    {
        writer.WriteLine(method.Name + ":");
        writer.WriteLine("status = {0}", method.StatusMessage);
        writer.WriteLine("blad aproksymacji we wszystkich 11 seriach:");
        writer.WriteLine("surowe dane:   max = {0:e6}, rmse = {1:e6}, punkt = (x = {2:f8}, t = {3:f8})",
            method.RawError.MaxAbsoluteError,
            method.RawError.RootMeanSquareError,
            method.RawError.XAtMaxError,
            method.RawError.TimeAtMaxError);
        writer.WriteLine("po wygladzeniu: max = {0:e6}, rmse = {1:e6}, punkt = (x = {2:f8}, t = {3:f8})",
            method.SmoothedError.MaxAbsoluteError,
            method.SmoothedError.RootMeanSquareError,
            method.SmoothedError.XAtMaxError,
            method.SmoothedError.TimeAtMaxError);
        writer.WriteLine("blad w chwili koncowej te:");
        writer.WriteLine("surowe dane:   max = {0:e6}, rmse = {1:e6}",
            method.FinalRawError.MaxAbsoluteError,
            method.FinalRawError.RootMeanSquareError);
        writer.WriteLine("po wygladzeniu: max = {0:e6}, rmse = {1:e6}",
            method.FinalSmoothedError.MaxAbsoluteError,
            method.FinalSmoothedError.RootMeanSquareError);
        writer.WriteLine();
    }

    private static EvaluatedMethod EvaluateMethod(
        string name,
        SphHeatSimulationResult result,
        HeatConductionExactSolution1D exact)
    {
        double spatialStep = result.Config.SpatialStep;
        double sampleStart = result.Config.SpatialBegin + 3.0 * spatialStep;
        double sampleEnd = result.Config.SpatialEnd - spatialStep;
        int pointCount = 101;

        double[] samplePoints = new double[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            samplePoints[i] = sampleStart + (sampleEnd - sampleStart) * i / (pointCount - 1);
        }

        var series = new List<OutputSeries>(result.Snapshots.Count);
        double rawSquaredError = 0.0;
        double smoothSquaredError = 0.0;
        int totalCount = 0;
        double rawMaxError = 0.0;
        double rawXAtMaxError = samplePoints[0];
        double rawTimeAtMaxError = result.Snapshots[0].Time;
        double smoothMaxError = 0.0;
        double smoothXAtMaxError = samplePoints[0];
        double smoothTimeAtMaxError = result.Snapshots[0].Time;

        foreach (SphHeatSnapshot snapshot in result.Snapshots)
        {
            double[] raw = SampleState(result.Nodes, snapshot.Temperatures, samplePoints);
            double[] exactValues = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                exactValues[i] = exact.Value(samplePoints[i], snapshot.Time);
            }

            double[] smoothed = Smooth(samplePoints, raw, result.Config.Support);

            var rawError = new double[pointCount];
            var smoothError = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                rawError[i] = Math.Abs(raw[i] - exactValues[i]);
                smoothError[i] = Math.Abs(smoothed[i] - exactValues[i]);

                totalCount++;
                rawSquaredError += rawError[i] * rawError[i];
                smoothSquaredError += smoothError[i] * smoothError[i];

                if (rawError[i] > rawMaxError)
                {
                    rawMaxError = rawError[i];
                    rawXAtMaxError = samplePoints[i];
                    rawTimeAtMaxError = snapshot.Time;
                }

                if (smoothError[i] > smoothMaxError)
                {
                    smoothMaxError = smoothError[i];
                    smoothXAtMaxError = samplePoints[i];
                    smoothTimeAtMaxError = snapshot.Time;
                }
            }

            series.Add(new OutputSeries(snapshot.Time, exactValues, raw, smoothed, rawError));
        }

        OutputSeries lastSeries = series[^1];
        ErrorMetrics finalRaw = MeasureSnapshotError(samplePoints, lastSeries.Time, lastSeries.Raw, lastSeries.Exact);
        ErrorMetrics finalSmoothed = MeasureSnapshotError(samplePoints, lastSeries.Time, lastSeries.Smoothed, lastSeries.Exact);

        return new EvaluatedMethod(
            name,
            samplePoints,
            series,
            new ErrorMetrics(rawMaxError, Math.Sqrt(rawSquaredError / totalCount), rawXAtMaxError, rawTimeAtMaxError),
            new ErrorMetrics(smoothMaxError, Math.Sqrt(smoothSquaredError / totalCount), smoothXAtMaxError, smoothTimeAtMaxError),
            finalRaw,
            finalSmoothed,
            result.IsStable,
            result.StatusMessage);
    }

    private static ErrorMetrics MeasureMethodDifference(EvaluatedMethod first, EvaluatedMethod second)
    {
        double maxError = 0.0;
        double xAtMaxError = first.SamplePoints[0];
        double timeAtMaxError = first.Series[0].Time;
        double squaredError = 0.0;
        int count = 0;

        for (int k = 0; k < first.Series.Count; k++)
        {
            for (int i = 0; i < first.SamplePoints.Length; i++)
            {
                double error = Math.Abs(first.Series[k].Raw[i] - second.Series[k].Raw[i]);
                count++;
                squaredError += error * error;
                if (error > maxError)
                {
                    maxError = error;
                    xAtMaxError = first.SamplePoints[i];
                    timeAtMaxError = first.Series[k].Time;
                }
            }
        }

        return new ErrorMetrics(maxError, Math.Sqrt(squaredError / count), xAtMaxError, timeAtMaxError);
    }

    private static ErrorMetrics MeasureSnapshotError(double[] samplePoints, double time, double[] values, double[] exact)
    {
        double maxError = 0.0;
        double xAtMaxError = samplePoints[0];
        double squaredError = 0.0;
        for (int i = 0; i < samplePoints.Length; i++)
        {
            double error = Math.Abs(values[i] - exact[i]);
            squaredError += error * error;
            if (error > maxError)
            {
                maxError = error;
                xAtMaxError = samplePoints[i];
            }
        }

        return new ErrorMetrics(maxError, Math.Sqrt(squaredError / samplePoints.Length), xAtMaxError, time);
    }

    private static double[] SampleState(double[] nodes, double[] state, double[] samplePoints)
    {
        double[] values = new double[samplePoints.Length];
        double a = nodes[0];
        double step = nodes[1] - nodes[0];

        for (int i = 0; i < samplePoints.Length; i++)
        {
            double position = (samplePoints[i] - a) / step;
            int left = (int)Math.Floor(position);
            if (left <= 0)
            {
                values[i] = state[0];
                continue;
            }

            if (left >= state.Length - 1)
            {
                values[i] = state[^1];
                continue;
            }

            double alpha = position - left;
            values[i] = (1.0 - alpha) * state[left] + alpha * state[left + 1];
        }

        return values;
    }

    private static double[] Smooth(double[] samplePoints, double[] values, double support)
    {
        double[] result = new double[values.Length];
        double cutOff = 3.0 * support;

        for (int i = 0; i < values.Length; i++)
        {
            double numerator = 0.0;
            double denominator = 0.0;
            for (int j = 0; j < values.Length; j++)
            {
                double distance = Math.Abs(samplePoints[i] - samplePoints[j]);
                if (distance > cutOff)
                {
                    continue;
                }

                double weight = Math.Exp(-(distance * distance) / (support * support)) / (support * Math.Sqrt(Math.PI));
                numerator += weight * values[j];
                denominator += weight;
            }

            result[i] = denominator > 0.0 ? numerator / denominator : values[i];
        }

        return result;
    }

    private static void SaveSpreadsheet(string filePath, EvaluatedMethod method)
    {
        using var writer = new StreamWriter(filePath, false);
        writer.Write("x");
        foreach (OutputSeries series in method.Series)
        {
            string timeLabel = series.Time.ToString("f6", CultureInfo.InvariantCulture);
            writer.Write($";exact_t={timeLabel};T_t={timeLabel};T_gladkie_t={timeLabel};blad_t={timeLabel}");
        }

        writer.WriteLine();

        for (int i = 0; i < method.SamplePoints.Length; i++)
        {
            writer.Write(method.SamplePoints[i].ToString("f8", CultureInfo.InvariantCulture));
            foreach (OutputSeries series in method.Series)
            {
                writer.Write(";" + series.Exact[i].ToString("f8", CultureInfo.InvariantCulture));
                writer.Write(";" + series.Raw[i].ToString("f8", CultureInfo.InvariantCulture));
                writer.Write(";" + series.Smoothed[i].ToString("f8", CultureInfo.InvariantCulture));
                writer.Write(";" + series.RawError[i].ToString("e6", CultureInfo.InvariantCulture));
            }

            writer.WriteLine();
        }
    }

    private static VariantSummary SummarizeVariant(SphHeatSimulationConfig config, int exactTerms)
    {
        var exact = new HeatConductionExactSolution1D(
            config.SpatialBegin,
            config.SpatialEnd,
            config.TimeBegin,
            config.Diffusivity,
            config.LeftBoundaryTemperature,
            config.RightBoundaryTemperature,
            config.InitialTemperature,
            exactTerms);

        var solver = new SphHeatEquation1D(config);
        SphHeatSimulationResult method1 = solver.Simulate(SphHeatApproach.KernelLaplacian);
        SphHeatSimulationResult method2 = solver.Simulate(SphHeatApproach.TaylorLaplacian);

        ErrorMetrics method1Error = method1.IsStable
            ? MeasureNodeError(method1.Nodes, method1.FinalTemperatures, config.TimeEnd, exact)
            : new ErrorMetrics(double.NaN, double.NaN, double.NaN, config.TimeEnd);

        ErrorMetrics method2Error = method2.IsStable
            ? MeasureNodeError(method2.Nodes, method2.FinalTemperatures, config.TimeEnd, exact)
            : new ErrorMetrics(double.NaN, double.NaN, double.NaN, config.TimeEnd);

        string status = method1.IsStable && method2.IsStable ? "stabilne" : "niestabilne";
        return new VariantSummary(method1Error, method2Error, status);
    }

    private static ErrorMetrics MeasureNodeError(
        double[] nodes,
        double[] values,
        double time,
        HeatConductionExactSolution1D exact)
    {
        double maxError = 0.0;
        double xAtMaxError = nodes[0];
        double squaredError = 0.0;

        for (int i = 0; i < nodes.Length; i++)
        {
            double error = Math.Abs(values[i] - exact.Value(nodes[i], time));
            squaredError += error * error;
            if (error > maxError)
            {
                maxError = error;
                xAtMaxError = nodes[i];
            }
        }

        return new ErrorMetrics(maxError, Math.Sqrt(squaredError / nodes.Length), xAtMaxError, time);
    }

    private static IReadOnlyList<int> BuildNodeVariants(int nodeCount)
    {
        return new[]
        {
            Math.Max(31, nodeCount / 2 + 1),
            nodeCount,
            Math.Max(nodeCount + 2, nodeCount * 2 - 1)
        }
        .Distinct()
        .OrderBy(x => x)
        .ToArray();
    }

    private static IReadOnlyList<double> BuildSupportFactorVariants(double supportFactor)
    {
        return new[]
        {
            Math.Max(0.5, 0.75 * supportFactor),
            supportFactor,
            Math.Min(1.5, 1.25 * supportFactor)
        }
        .Distinct()
        .OrderBy(x => x)
        .ToArray();
    }

    private static IReadOnlyList<double> BuildDiffusionVariants(double diffusionFactor)
    {
        return new[]
        {
            Math.Max(0.1, 0.6 * diffusionFactor),
            diffusionFactor,
            Math.Min(1.1, 1.4 * diffusionFactor)
        }
        .Distinct()
        .OrderBy(x => x)
        .ToArray();
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

    private sealed class HeatConductionExactSolution1D
    {
        private readonly double _a;
        private readonly double _length;
        private readonly double _t0;
        private readonly double _kappa;
        private readonly double _leftBoundaryTemperature;
        private readonly double _rightBoundaryTemperature;
        private readonly double[] _coefficients;

        public HeatConductionExactSolution1D(
            double a,
            double b,
            double t0,
            double kappa,
            double leftBoundaryTemperature,
            double rightBoundaryTemperature,
            double initialTemperature,
            int termCount = 600)
        {
            _a = a;
            _length = b - a;
            _t0 = t0;
            _kappa = kappa;
            _leftBoundaryTemperature = leftBoundaryTemperature;
            _rightBoundaryTemperature = rightBoundaryTemperature;
            _coefficients = new double[termCount];

            double a0 = initialTemperature - leftBoundaryTemperature;
            double slope = rightBoundaryTemperature - leftBoundaryTemperature;

            for (int n = 1; n <= termCount; n++)
            {
                double parity = n % 2 == 0 ? 1.0 : -1.0;
                _coefficients[n - 1] = 2.0 / (n * Math.PI) * (a0 * (1.0 - parity) + slope * parity);
            }
        }

        public double Value(double x, double time)
        {
            double y = x - _a;
            double steady = _leftBoundaryTemperature
                + (_rightBoundaryTemperature - _leftBoundaryTemperature) * y / _length;

            if (time <= _t0)
            {
                return steady + SeriesValue(y, 0.0);
            }

            return steady + SeriesValue(y, time - _t0);
        }

        private double SeriesValue(double y, double dt)
        {
            double sum = 0.0;
            for (int n = 1; n <= _coefficients.Length; n++)
            {
                double wave = n * Math.PI / _length;
                double decay = Math.Exp(-_kappa * wave * wave * dt);
                sum += _coefficients[n - 1] * Math.Sin(wave * y) * decay;
            }

            return sum;
        }
    }

    private sealed record OutputSeries(
        double Time,
        double[] Exact,
        double[] Raw,
        double[] Smoothed,
        double[] RawError);

    private sealed record EvaluatedMethod(
        string Name,
        double[] SamplePoints,
        IReadOnlyList<OutputSeries> Series,
        ErrorMetrics RawError,
        ErrorMetrics SmoothedError,
        ErrorMetrics FinalRawError,
        ErrorMetrics FinalSmoothedError,
        bool IsStable,
        string StatusMessage);

    private readonly record struct ErrorMetrics(
        double MaxAbsoluteError,
        double RootMeanSquareError,
        double XAtMaxError,
        double TimeAtMaxError);

    private readonly record struct VariantSummary(
        ErrorMetrics Method1FinalError,
        ErrorMetrics Method2FinalError,
        string StatusLabel);
}
