using POK_1;
using POK_1.assets;

IZadanie[] zadania =
[
    //new MoA06Zad1(
    //    new double[,]
    //    {
    //        { 1, 6, 0 },
    //        { 2, 1, 0 },
    //        { 0, 2, 1 },
    //    },
    //    [3, 1, 1]),
    //new MoA06Zad2(5, 1000, 301954),
    new MoA06Zad3(100, 150, 301954),

    //new MoA07Zad1a(0.1, 2.0, 60),
    //new MoA07Zad1b(0.1, 2.0, 1.0e-10),
    //new MoA07Zad2a(0.0, 2.0, 60),
    //new MoA07Zad2b(0.0, 2.0, 1.0e-10),
    //new MoA07Zad3a(2.0, 4.0, 8),
    //new MoA07Zad3b(2.0, 4.0, 1.0e-10),
];

const string outputDirectory = @"C:\POK_2_results";

Directory.CreateDirectory(outputDirectory);

foreach (var zadanie in zadania)
{
    zadanie.ZapiszWynik(outputDirectory);
}

Console.WriteLine($"Wyniki zapisano do folderu: {outputDirectory}");
