using System.Globalization;

var outputDirectory = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "POK_results"));
Directory.CreateDirectory(outputDirectory);

var cases = new[]
{
    new SimulationCase(
        "mdkzad01_sinus",
        new CircuitParameters(80.0, 0.050, 100e-6),
        0.20,
        0.00005,
        time => 10.0 * Math.Sin(2.0 * Math.PI * 50.0 * time)),
    new SimulationCase(
        "mdkzad01_skok",
        new CircuitParameters(120.0, 0.060, 150e-6),
        0.12,
        0.00005,
        _ => 12.0),
    new SimulationCase(
        "mdkzad01_impuls",
        new CircuitParameters(60.0, 0.040, 68e-6),
        0.08,
        0.00002,
        time => time <= 0.020 ? 8.0 : 0.0),
};

foreach (var simulationCase in cases)
{
    var summary = Simulator.Run(simulationCase, outputDirectory);
    Console.WriteLine($"PRZYPADEK {summary.Name}");
    Console.WriteLine($"plik = {summary.FilePath}");
    Console.WriteLine($"R = {summary.Parameters.ResistanceOhms:f1} ohm, L = {summary.Parameters.InductanceHenries * 1000.0:f1} mH, C = {summary.Parameters.CapacitanceFarads * 1_000_000.0:f1} uF");
    Console.WriteLine($"czas = {summary.DurationSeconds * 1000.0:f1} ms, krok = {summary.StepSeconds * 1000.0:f3} ms");
    Console.WriteLine($"maksymalny prad = {summary.MaxCurrent:f6} A");
    Console.WriteLine($"maksymalne napiecie kondensatora = {summary.MaxCapacitorVoltage:f6} V");
    Console.WriteLine($"stan koncowy: i = {summary.FinalCurrent:f6} A, uc = {summary.FinalCapacitorVoltage:f6} V");
    Console.WriteLine();
}

internal delegate double InputSignal(double timeSeconds);

internal sealed record CircuitParameters(
    double ResistanceOhms,
    double InductanceHenries,
    double CapacitanceFarads);

internal sealed record SimulationCase(
    string Name,
    CircuitParameters Parameters,
    double DurationSeconds,
    double StepSeconds,
    InputSignal Signal);

internal sealed record SimulationSummary(
    string Name,
    string FilePath,
    CircuitParameters Parameters,
    double DurationSeconds,
    double StepSeconds,
    double MaxCurrent,
    double MaxCapacitorVoltage,
    double FinalCurrent,
    double FinalCapacitorVoltage);

internal readonly record struct RlcState(double Current, double CapacitorVoltage);

internal static class Simulator
{
    public static SimulationSummary Run(SimulationCase simulationCase, string outputDirectory)
    {
        string filePath = Path.Combine(outputDirectory, simulationCase.Name + ".csv");
        var parameters = simulationCase.Parameters;
        var state = new RlcState(0.0, 0.0);

        double maxCurrent = 0.0;
        double maxCapacitorVoltage = 0.0;

        using var writer = new StreamWriter(filePath, false);
        writer.WriteLine("czas_ms;napiecie_zrodla_V;prad_A;napiecie_kondensatora_V;napiecie_rezystora_V;napiecie_cewki_V");

        for (double t = 0.0; t <= simulationCase.DurationSeconds + 1e-12; t += simulationCase.StepSeconds)
        {
            double sourceVoltage = simulationCase.Signal(t);
            double resistorVoltage = parameters.ResistanceOhms * state.Current;
            double inductorVoltage = sourceVoltage - resistorVoltage - state.CapacitorVoltage;

            maxCurrent = Math.Max(maxCurrent, Math.Abs(state.Current));
            maxCapacitorVoltage = Math.Max(maxCapacitorVoltage, Math.Abs(state.CapacitorVoltage));

            writer.WriteLine(string.Create(
                CultureInfo.InvariantCulture,
                $"{t * 1000.0:f6};{sourceVoltage:f6};{state.Current:f9};{state.CapacitorVoltage:f9};{resistorVoltage:f9};{inductorVoltage:f9}"));

            state = StepRungeKutta(state, t, simulationCase.StepSeconds, parameters, simulationCase.Signal);
        }

        return new SimulationSummary(
            simulationCase.Name,
            filePath,
            parameters,
            simulationCase.DurationSeconds,
            simulationCase.StepSeconds,
            maxCurrent,
            maxCapacitorVoltage,
            state.Current,
            state.CapacitorVoltage);
    }

    private static RlcState StepRungeKutta(
        RlcState state,
        double timeSeconds,
        double stepSeconds,
        CircuitParameters parameters,
        InputSignal signal)
    {
        var k1 = Derivative(state, timeSeconds, parameters, signal);
        var k2 = Derivative(Add(state, Scale(k1, 0.5 * stepSeconds)), timeSeconds + 0.5 * stepSeconds, parameters, signal);
        var k3 = Derivative(Add(state, Scale(k2, 0.5 * stepSeconds)), timeSeconds + 0.5 * stepSeconds, parameters, signal);
        var k4 = Derivative(Add(state, Scale(k3, stepSeconds)), timeSeconds + stepSeconds, parameters, signal);

        return Add(state, Scale(Add(k1, Add(Scale(k2, 2.0), Add(Scale(k3, 2.0), k4))), stepSeconds / 6.0));
    }

    private static RlcState Derivative(RlcState state, double timeSeconds, CircuitParameters parameters, InputSignal signal)
    {
        double sourceVoltage = signal(timeSeconds);
        double di = (sourceVoltage - parameters.ResistanceOhms * state.Current - state.CapacitorVoltage) / parameters.InductanceHenries;
        double du = state.Current / parameters.CapacitanceFarads;
        return new RlcState(di, du);
    }

    private static RlcState Add(RlcState left, RlcState right)
    {
        return new RlcState(left.Current + right.Current, left.CapacitorVoltage + right.CapacitorVoltage);
    }

    private static RlcState Scale(RlcState state, double factor)
    {
        return new RlcState(state.Current * factor, state.CapacitorVoltage * factor);
    }
}
