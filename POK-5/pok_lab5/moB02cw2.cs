using System;
using System.IO;
using LabTools; // biblioteka klas narzedziowych, nalezy dolaczyc plik: moB02_fappr1d.cs 

namespace LabMOB
{
 public class Gauss1D : Approximator1D // klasa jednowymiarowego aproksymatora gausowskiego
 {
  public Gauss1D(int Nx) : base(Nx) // utworzenie aproksymatora
  { }

  public override double kernel(double r, double h) // definicja radialnego jadra aproksymacji
  {
   return Math.Exp(-(r * r) / (h * h)) / (h * Math.Sqrt(Math.PI));
  }

  public override double kerder(double r, double h) // pochodna jadra aproksymacji podzielona przez promien
  {
   return -2.0*Math.Exp(-(r * r) / (h * h)) / (h * h * h * Math.Sqrt(Math.PI));
  }

  public override double kerddr(double r, double h) // druga pochdna jadra aproksymacji
  {
   return -4.0 * (0.5 - (r * r) / (h * h)) * Math.Exp(-(r * r) / (h * h)) / (h * h * h * Math.Sqrt(Math.PI));
  }
 }
 static class Data // rekord danych wejsciowych programu
 {
  public static int numx = 1000000; // liczba wezlow w przedziale
  public static double xbeg = 0.0; // poczatek przedzialu
  public static double xend = Math.PI; // koniec przedzialu
  public static double incx = 0.01; // wzgledny przyrost argumentu na wyresie
  public static double rsup = 0.001; // nosnik jadra aproksymacji

  public static double funval(double x) // wartosc aproksymowanej funkcji
  {
   return Math.Sin(x);
  }

  public static double funder(double x) // pochodna funkcji
  {
   return Math.Cos(x);
  }

  public static double funddr(double x) // druga pochodna funkcji
  {
   return -Math.Sin(x);
  }
 }
 static class Program // glowny kod programu 
 {
  static void Main(string[] args)
  {
   Gauss1D appr = new Gauss1D(Data.numx); // utworzenie i zastosowanie aproksymatora 
   appr.Apply(new Function1D(Data.funval),Data.xbeg,Data.xend,Data.rsup);

   StreamWriter plot=null;
   { // rozpoczecie wydruku danych
    Console.WriteLine();
    Console.WriteLine(">>> One-dimension Approximation Results <<<");
    Console.WriteLine();
    
    plot = new StreamWriter("wyniki.txt"); // wyniki do wykresu funkcji lub jej pochdnych 
    Console.WriteLine("| {0,5} | {1,7} | {2,7} |", " x ", " f(x) ", " g(x) ");
    Console.WriteLine("+-{0,5}-+-{1,7}-+-{2,7}-+", "-----", "-------", "-------");
   }
   
   int nvals = 0;
   double x = appr.Begin;
   do { // obliczanie wartosci lub pochodnych aproksymanta i wydruk wynikow obliczen dla wykresu funkcji
    double yo = Data.funder(x); // pochodna funkcji
    double yp = appr.Derivative(x); // pochodna aproksymanta

    Console.WriteLine("| {0,5:f3} | {1,7:f5} | {2,7:f5} |", x, yo, yp);
    plot.WriteLine("{0,10:f3}{1,15:f5}{2,15:f5}", x, yo, yp);
    nvals++;

    x += Data.incx * appr.Length;
   } while (x<=appr.End);
   Console.WriteLine();
   plot.Close();

   Console.WriteLine("function plot size = "+nvals+" points");
   Console.WriteLine("approximation size = "+appr.Size+" nodes");
   Console.WriteLine();
  }
 }
}
