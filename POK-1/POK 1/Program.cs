using POK_1;
using POK_1.assets;

IZadanie[] zadania =
[
    new MoA02Zad1(0.0, 1.0, 0.1),
    new MoA02Zad2A(2, 1.5, 10),
    new MoA02Zad2B(2, 1.5, 10),
    new MoA02Zad2C(1.0, 2.0, 0.1, 1.5, 2),
    new MoA02Zad3A(0.0001, 1.0),
    new MoA02Zad3B(1.0, 30),
    new MoA02Zad3C(1.0, 15),
    new MoA03Zad2a(3.14, 0.01, 0.00001, 1.25, 0.02, 0.00001),
    new MoA03Zad3(2.0, 15),
    new MoA03Zad4(20, 1.0, 1.0 / 3.0),
    new MoA04Zad1a(20000, 1000),
    new MoA04Zad1b(17),
    new MoA04Zad2a((0.1, 20000, 1000), (10.0, 20000, 1000)),
    new MoA04Zad2b((0.1, 10), (10.0, 40), (-10.0, 40)),
    new MoA04Zad2c((-10.0, 40)),
    new MoA04Zad3a(45, 1000, 10000, 100000, 1000000, 10000000),
    new MoA04Zad3b(10),
    new MoA05Zad1a(2, 2),
    new MoA05Zad1b(2, 2),
    new MoA05Zad2(0.00010234876f, 236.36098f, -509.27887f),
];

const string outputDirectory = @"C:\POK_results";

Directory.CreateDirectory(outputDirectory);

foreach (var zadanie in zadania)
{
    zadanie.ZapiszWynik(outputDirectory);
}

Console.WriteLine($"Wyniki zapisano do folderu: {outputDirectory}");
