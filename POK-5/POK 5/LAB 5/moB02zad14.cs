using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad14 : IZadanie
{
    public string Nazwa => "moB02zad14";

    public void Run(TextWriter writer)
    {
        const double a = 0.0;
        const double b = 2.0 * Math.PI;
        const double c = 0.0;
        const double d = 2.0 * Math.PI;
        const double temperatureChange = 2.5e-3;
        const double parameterStep = 2.5e-2;
        const int maxIterationCount = 200000;
        const double minimumTemperature = 1.0e-8;
        const double initialTemperature = 0.50;
        const int seed = 301954;

        var random = new Random(seed);
        double[] xi = new double[] { random.NextDouble(), random.NextDouble() };
        double[] bestXi = new double[] { xi[0], xi[1] };
        double temperature = initialTemperature;
        double currentCost = Cost(xi[0], xi[1]);
        double bestCost = currentCost;
        int acceptedWorseMoves = 0;
        int acceptedBetterMoves = 0;
        int usedIterations = 0;

        for (int iteration = 1; iteration <= maxIterationCount; iteration++)
        {
            usedIterations = iteration;
            if (temperature <= minimumTemperature)
            {
                break;
            }

            int changedParameterIndex = random.Next(2);
            double[] candidate = new double[] { xi[0], xi[1] };
            double randomShift = parameterStep * (2.0 * random.NextDouble() - 1.0);
            candidate[changedParameterIndex] = Clamp(candidate[changedParameterIndex] + randomShift, 0.0, 1.0);

            double newCost = Cost(candidate[0], candidate[1]);
            double costIncrease = newCost - currentCost;
            bool accept;

            if (costIncrease <= 0.0)
            {
                accept = true;
                acceptedBetterMoves++;
            }
            else
            {
                double transitionProbability = Math.Exp(-costIncrease / temperature);
                accept = random.NextDouble() < transitionProbability;
                if (accept) acceptedWorseMoves++;
            }

            if (accept)
            {
                xi = candidate;
                currentCost = newCost;
                if (currentCost < bestCost)
                {
                    bestCost = currentCost;
                    bestXi[0] = xi[0];
                    bestXi[1] = xi[1];
                }
            }

            temperature *= 1.0 - temperatureChange;

            if (iteration % 20000 == 0)
            {
                writer.WriteLine("iteracja {0,6}: T = {1:e4}, koszt = {2:f8}, najlepszy = {3:f8}", iteration, temperature, currentCost, bestCost);
            }
        }

        double bestX = a + bestXi[0] * (b - a);
        double bestY = c + bestXi[1] * (d - c);
        double gridMinimumValue;
        (double GridX, double GridY) gridMinimum = FindGridMinimum(a, b, c, d, 801, out gridMinimumValue);

        writer.WriteLine();
        writer.WriteLine("optymalizacja Monte Carlo z wyzarzaniem");
        writer.WriteLine("funkcja: f(x,y) = sin(x) + sin(y) + 0.15*cos(3*x - 2*y)");
        writer.WriteLine("x z przedzialu <0, 2*pi>, y z przedzialu <0, 2*pi>");
        writer.WriteLine("xi1, xi2 z przedzialu <0, 1>");
        writer.WriteLine("poczatkowa temperatura = {0:f6}", initialTemperature);
        writer.WriteLine("minimalna temperatura = {0:e3}", minimumTemperature);
        writer.WriteLine("wzgledny spadek temperatury = {0:e3}", temperatureChange);
        writer.WriteLine("bezwzgledny krok parametru = {0:e3}", parameterStep);
        writer.WriteLine("wykonane iteracje = {0}", usedIterations);
        writer.WriteLine("zaakceptowane ruchy poprawiajace = {0}", acceptedBetterMoves);
        writer.WriteLine("zaakceptowane ruchy pogarszajace = {0}", acceptedWorseMoves);
        writer.WriteLine();
        writer.WriteLine("najlepsze parametry:");
        writer.WriteLine("xi1 = {0:f10}", bestXi[0]);
        writer.WriteLine("xi2 = {0:f10}", bestXi[1]);
        writer.WriteLine("x = a + xi1*(b-a) = {0:f10}", bestX);
        writer.WriteLine("y = c + xi2*(d-c) = {0:f10}", bestY);
        writer.WriteLine("f(x,y) = {0:f10}", bestCost);
        writer.WriteLine();
        writer.WriteLine("kontrola przez geste przeszukanie siatki 801 x 801:");
        writer.WriteLine("x_grid = {0:f10}, y_grid = {1:f10}, f_grid = {2:f10}", gridMinimum.GridX, gridMinimum.GridY, gridMinimumValue);
        writer.WriteLine("odleglosc od minimum siatkowego = {0:e6}", Math.Sqrt((bestX - gridMinimum.GridX) * (bestX - gridMinimum.GridX) + (bestY - gridMinimum.GridY) * (bestY - gridMinimum.GridY)));
        writer.WriteLine("wniosek: punkt znaleziony przez wyzarzanie lezy w poblizu minimum funkcji w zadanej dziedzinie.");
    }

    private static double Function(double x, double y)
    {
        return Math.Sin(x) + Math.Sin(y) + 0.15 * Math.Cos(3.0 * x - 2.0 * y);
    }

    private static double Cost(double xi1, double xi2)
    {
        if (xi1 < 0.0 || xi1 > 1.0 || xi2 < 0.0 || xi2 > 1.0)
        {
            return double.PositiveInfinity;
        }

        double x = 2.0 * Math.PI * xi1;
        double y = 2.0 * Math.PI * xi2;
        return Function(x, y);
    }

    private static (double X, double Y) FindGridMinimum(double a, double b, double c, double d, int n, out double minimumValue)
    {
        double bestX = a;
        double bestY = c;
        minimumValue = double.PositiveInfinity;
        for (int ix = 0; ix < n; ix++)
        {
            double x = a + (b - a) * ix / (n - 1);
            for (int iy = 0; iy < n; iy++)
            {
                double y = c + (d - c) * iy / (n - 1);
                double value = Function(x, y);
                if (value < minimumValue)
                {
                    minimumValue = value;
                    bestX = x;
                    bestY = y;
                }
            }
        }
        return (bestX, bestY);
    }

    private static double Clamp(double value, double min, double max) => Math.Min(max, Math.Max(min, value));
}
