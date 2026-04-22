using System;
using LabTools; // biblioteka klas narzedziowych, nalezy dolaczyc plik: moA07_gauss.cs 
//
// cwiczenie nr 1 do tekstu 'moA06.doc'
//
namespace LabMOA
{
 class Program1
 {
  [STAThread]
  static void Main(string[] args)
  {
   const int n = 2;

   LinEq rownanie = new GaussLinEq(n);

   rownanie.A = new double[n,n]
   { 
    {1,6},
    {2,4}
   };
   
   rownanie.b = new double[n]
   { 
    3,
    5
   };
                            
   rownanie.solve();

   Console.WriteLine("otrzymane rozwiazanie rownania -->");
   Console.WriteLine();

   for (int i=0; i<n; i++)
   Console.WriteLine("x"+(i+1)+" = "+rownanie.x[i]);

   Console.ReadLine();
  }
 }
}
