using System.Diagnostics;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad11 : IZadanie
{
    public string Nazwa => "moB02zad11";

    public void Run(TextWriter writer)
    {
        int dataCount = 1000;
        int maxIterationCount = 5000;
        double learningRate = 0.18;
        double momentum = 0.82;
        double minimumCost = 1.0e-8;
        double derivativeStep = 1.0e-4;
        int seed = 301954;

        MoB02PerceptronTools.DataPair[] data = MoB02PerceptronTools.GenerateData(dataCount, seed);
        double[] parameters = MoB02PerceptronTools.RandomParameters(seed + 1, 0.35);
        double[] increments = new double[MoB02PerceptronTools.ParameterCount];

        var stopwatch = Stopwatch.StartNew();
        double cost = double.PositiveInfinity;
        int usedIterations = 0;

        for (int iteration = 1; iteration <= maxIterationCount; iteration++)
        {
            usedIterations = iteration;
            cost = MoB02PerceptronTools.Cost(data, parameters);
            if (cost <= minimumCost)
            {
                break;
            }

            double[] gradient = MoB02PerceptronTools.NumericalGradient(
                p => MoB02PerceptronTools.Cost(data, p),
                parameters,
                derivativeStep);

            for (int k = 0; k < parameters.Length; k++)
            {
                increments[k] = momentum * increments[k] - learningRate * gradient[k];
                parameters[k] += increments[k];
            }

            if (iteration % 500 == 0)
            {
                writer.WriteLine("iteracja {0,5}: koszt = {1:e6}", iteration, cost);
            }
        }

        stopwatch.Stop();
        double finalCost = MoB02PerceptronTools.Cost(data, parameters);
        MoB02PerceptronTools.DataPair verification = data[data.Length / 3];
        double[] prediction = MoB02PerceptronTools.Predict(verification.X, parameters);

        writer.WriteLine();
        writer.WriteLine("optymalizacja perceptronu metoda najszybszego spadku Cauchiego z bezwladnoscia");
        writer.WriteLine("wejscie ma rozmiar n = 3, wyjscie ma rozmiar m = 2");
        writer.WriteLine("liczba parametrow r = 8: bias i 3 wagi dla kazdego z 2 neuronow");
        writer.WriteLine("zbior rozwojowy: {0} par danych", dataCount);
        writer.WriteLine("liczba iteracji maksymalna = {0}", maxIterationCount);
        writer.WriteLine("tempo uczenia = {0:f6}", learningRate);
        writer.WriteLine("bezwladnosc = {0:f6}", momentum);
        writer.WriteLine("krok ilorazu roznicowego h = {0:e3}", derivativeStep);
        writer.WriteLine("minimalny koszt = {0:e3}", minimumCost);
        writer.WriteLine("czas obliczen = {0:f3} ms", stopwatch.Elapsed.TotalMilliseconds);
        writer.WriteLine("wykonane iteracje = {0}", usedIterations);
        writer.WriteLine("koszt koncowy = {0:e8}", finalCost);
        writer.WriteLine();

        writer.WriteLine("optymalne parametry perceptronu:");
        for (int k = 0; k < parameters.Length; k++)
        {
            writer.WriteLine("p[{0}] = {1,14:f10}", k, parameters[k]);
        }

        writer.WriteLine();
        writer.WriteLine("weryfikacja dla jednej pary ze zbioru rozwojowego:");
        writer.WriteLine("x = [{0:f8}, {1:f8}, {2:f8}]", verification.X[0], verification.X[1], verification.X[2]);
        writer.WriteLine("| skladowa |      y docelowe |      eta modelu | blad wzgledny |");
        writer.WriteLine("+----------+-----------------+-----------------+---------------+");
        for (int j = 0; j < prediction.Length; j++)
        {
            writer.WriteLine(
                "| {0,8} | {1,15:f8} | {2,15:f8} | {3,13:e4} |",
                j,
                verification.Y[j],
                prediction[j],
                MoB02PerceptronTools.RelativeError(prediction[j], verification.Y[j]));
        }

        writer.WriteLine();
        writer.WriteLine("funkcje docelowe uzyte do generacji danych:");
        writer.WriteLine("y1 = tanh(0.20 + 0.80*x1 - 0.40*x2 + 0.30*x3)");
        writer.WriteLine("y2 = tanh(-0.10 - 0.50*x1 + 0.70*x2 + 0.60*x3)");
        writer.WriteLine("Poniewaz generator danych ma taka sama postac jak trenowany perceptron, koszt powinien zejsc bardzo nisko.");
    }
}
