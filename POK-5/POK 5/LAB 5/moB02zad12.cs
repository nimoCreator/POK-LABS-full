using System.Diagnostics;
using POK_5.assets;

namespace POK_5;

internal sealed class MoB02Zad12 : IZadanie
{
    public string Nazwa => "moB02zad12";

    public void Run(TextWriter writer)
    {
        int dataCount = 1000;
        int maxIterationCount = 50000;
        double learningRate = 0.06;
        double momentum = 0.80;
        double minimumEstimatedCost = 1.0e-5;
        double derivativeStep = 1.0e-4;
        int seed = 301954;

        var random = new Random(seed + 2);
        MoB02PerceptronTools.DataPair[] data = MoB02PerceptronTools.GenerateData(dataCount, seed);
        double[] parameters = MoB02PerceptronTools.RandomParameters(seed + 1, 0.35);
        double[] increments = new double[MoB02PerceptronTools.ParameterCount];

        double lossAccumulator = 0.0;
        int sampleCounter = 0;
        double estimatedCost = double.PositiveInfinity;
        int usedIterations = 0;

        var stopwatch = Stopwatch.StartNew();
        for (int iteration = 1; iteration <= maxIterationCount; iteration++)
        {
            usedIterations = iteration;
            MoB02PerceptronTools.DataPair pair = data[random.Next(data.Length)];
            double loss = MoB02PerceptronTools.Loss(pair, parameters);
            lossAccumulator += loss;
            sampleCounter++;
            estimatedCost = lossAccumulator / sampleCounter;

            if (iteration > 200 && estimatedCost <= minimumEstimatedCost)
            {
                break;
            }

            double[] gradient = MoB02PerceptronTools.NumericalGradient(
                p => MoB02PerceptronTools.Loss(pair, p),
                parameters,
                derivativeStep);

            for (int k = 0; k < parameters.Length; k++)
            {
                increments[k] = momentum * increments[k] - learningRate * gradient[k];
                parameters[k] += increments[k];
            }

            if (iteration % 5000 == 0)
            {
                double fullCostPreview = MoB02PerceptronTools.Cost(data, parameters);
                writer.WriteLine("iteracja {0,6}: strata srednia = {1:e6}, koszt pelny kontrolny = {2:e6}", iteration, estimatedCost, fullCostPreview);
            }
        }

        stopwatch.Stop();
        double fullCost = MoB02PerceptronTools.Cost(data, parameters);
        MoB02PerceptronTools.DataPair verification = data[data.Length / 3];
        double[] prediction = MoB02PerceptronTools.Predict(verification.X, parameters);

        writer.WriteLine();
        writer.WriteLine("optymalizacja perceptronu metoda stochastycznego spadku gradientu z bezwladnoscia");
        writer.WriteLine("w kazdym kroku losowana jest jedna para danych, a gradient liczony jest z pojedynczej straty");
        writer.WriteLine("zbior rozwojowy: {0} par danych", dataCount);
        writer.WriteLine("liczba iteracji maksymalna = {0}", maxIterationCount);
        writer.WriteLine("tempo uczenia = {0:f6}", learningRate);
        writer.WriteLine("bezwladnosc = {0:f6}", momentum);
        writer.WriteLine("krok ilorazu roznicowego h = {0:e3}", derivativeStep);
        writer.WriteLine("minimalna estymacja kosztu = {0:e3}", minimumEstimatedCost);
        writer.WriteLine("czas obliczen = {0:f3} ms", stopwatch.Elapsed.TotalMilliseconds);
        writer.WriteLine("wykonane iteracje = {0}", usedIterations);
        writer.WriteLine("srednia strata jako estymacja kosztu = {0:e8}", estimatedCost);
        writer.WriteLine("pelny koszt policzony kontrolnie po optymalizacji = {0:e8}", fullCost);
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
        writer.WriteLine("porownanie z zadaniem 11:");
        writer.WriteLine("SGD wykonuje wiecej krokow, ale pojedynczy krok jest tanszy, bo nie liczy kosztu po calym zbiorze danych.");
        writer.WriteLine("Dla malego zbioru metoda z zadania 11 moze byc stabilniejsza, a dla duzego zbioru SGD zwykle jest szybsze czasowo.");
    }
}
