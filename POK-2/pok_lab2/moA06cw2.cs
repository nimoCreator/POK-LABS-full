using System;
using LabTools; // biblioteka klas narzedziowych 
//
// cwiczenie nr 2 do tekstu 'moA06.doc'
//
namespace LabMOA
{
 class Program1
 {
  [STAThread]
  static void Main(string[] args)
  {
   const int n = 3;

   LinEq rownanie = new GaussLinEq(n);

   rownanie.A = new double[n,n]
   { 
    {1,6,0},
    {2,1,0},
    {0,2,1},
   };
   
   rownanie.b = new double[n]
   { 
    3,
    1,
    1
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
