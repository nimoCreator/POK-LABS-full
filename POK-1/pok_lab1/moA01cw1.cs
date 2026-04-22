using System;
using System.IO; // biblioteka operacji we/wy
//
// cwiczenie nr 1 do tekstu 'moA01.doc'
//
namespace LabMOA
{
 class Program1
 {
        [STAThread]
        static void Main(string[] args)
        {
            StreamWriter LogFile = new StreamWriter("wyniki.txt"); // utworzenie strumienia do zapisu tekstu w pliku o nazwie 'wyniki.txt'
            Console.WriteLine("| {0,3} | {1,7} |", " x ", " y "); // formatowanie naglowka tabeli danych do wyswietlania na ekranie konsoli i zapisu w pliku
            LogFile.WriteLine("| {0,3} | {1,7} |", " x ", " y ");
            Console.WriteLine("+-{0,3}-+-{1,7}-+", "---", "-------");
            LogFile.WriteLine("+-{0,3}-+-{1,7}-+", "---", "-------");

            double x, y;
            for (x = 0.0; x < 1.1; x += 0.1)
            { // generowanie danych i przedstawienie ich w kolejnych wierszach tabeli
                y = 2.13 * x;
                Console.WriteLine("| {0,3:f1} | {1,7:f3} |", x, y);
                LogFile.WriteLine("| {0,3:f1} | {1,7:f3} |", x, y);
            }

            Console.ReadLine();
            LogFile.Close(); // zamkniecie strumienia zwiazanego z plikiem
        }
 }
}
