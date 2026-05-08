using System;
using System.IO;
//
// drugi dodatek do zadania nr 13 z tekstu 'moB02.doc'
//
namespace LabMOB
{
 static partial class Program
 {
  public delegate double Function(double x); // delegacja funkcji y = f(x) dla lewej strony rownania

  static bool NonLinEq(int n, double a, double b, Function f, ref double x)
  {
//
// rozwiazywanie w przedziale [a,b]
// rownania nieliniowego f(x) = 0
// metoda bisekcji o n krokach:
//
   for (int i=0; i<n; i++) {
    double xo = 0.5*(a+b); // polowienie przedzialu od a do b
    double yo = f(xo);     // i sprawdzenie wartosci srodowej
    int so = Math.Sign(yo);
    if (so == 0 || i == n-1) { x = xo; break; }

    double ya = f(a); // sprawdzenie wartosci skrajnych
    int sa = Math.Sign(ya);
    double yb = f(b); 
    int sb = Math.Sign(yb);
    if (sa == so) a = xo; // porownanie z wartoscia srodkowa
    if (sb == so) b = xo;
    if (a==b) return false; // brak miejsca zerowego
   }
   return true; // rozwiazanie istnieje
  }
 }
}
