using POK_5;
using POK_5.assets;

IZadanie[] zadania =
[
    //new MoB02Zad1(5, -2.0, 3.0),
    //new MoB02Zad2(5, -2.0, 3.0),
    //new MoB02Zad3(),
    //new MoB02Zad4(),
    //new MoB02Zad5(),
    //new MoB02Zad6(),
    //new MoB02Zad7(),
    //new MoB02Zad8(),
    //new MoB02Zad9(),
    new MoB02Zad10(),
    new MoB02Zad11(),
    new MoB02Zad12(),
    new MoB02Zad13(),
    new MoB02Zad14(),
];

string outputDirectory = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "POK_5_results"));

Directory.CreateDirectory(outputDirectory);

foreach (var zadanie in zadania)
{
    zadanie.ZapiszWynik(outputDirectory);
}

Console.WriteLine($"Wyniki zapisano do folderu: {outputDirectory}");
