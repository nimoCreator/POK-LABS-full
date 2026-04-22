using System;
using System.IO;
//
// cwiczenie nr 3 do tekstu 'moA06.doc'
//
namespace LabMOA
{
 class Program1
 {
  [STAThread]
  static void Main(string[] args)
  {
   int n, k;
   Console.Write("Podaj rzad ukladu, n: "); n=int.Parse(Console.ReadLine());
   Console.Write("Podaj ilosc iteracji, k: "); k=int.Parse(Console.ReadLine());
   Console.WriteLine();
   
   double[,] A = new double[n,n];
   double[] b = new double[n];
   double[] x = new double[n];
   double[] db = new double[n];

   for (int i=0; i<n; i++) {
    for (int j=0; j<n; j++) {
     Console.Write("Podaj parametr rownania, a"+i+j+": "); 
     A[i,j]=double.Parse(Console.ReadLine());
    } 
    
    Console.Write("Podaj wyraz wolny rownania, b"+i+": "); 
    b[i]=double.Parse(Console.ReadLine());
    Console.WriteLine();
   }

   double[,] H = new double[n,n];
   double[] g = new double[n];

   for (int i=0; i<n; i++) {
    double aii = A[i,i];
    if (Math.Abs(aii)<1e-10) {
     Console.WriteLine("Podales zerowy parametr rownania, a"+i+i);
     Console.ReadLine();
     return;
    }
    
    for (int j=0; j<n; j++) 
    H[i,j] = i!=j ? -A[i,j]/aii : 0.0;
    
    g[i] = b[i]/aii;
   }

   double[] xbiez = new double[n];
   double[] xnast = new double[n];

   for (int i=0; i<n; i++) xbiez[i] = g[i];

   for (int p=0; p<k; p++) 
   {
    Console.WriteLine(p+") biezace przyblizenie rozwiazania rownania -->");

    for (int i=0; i<n; i++)
    Console.WriteLine("xbiez_"+i+" = "+xbiez[i]);
    Console.ReadLine();

    for (int i=0; i<n; i++) {
     double sum = 0.0;
     for (int j=0; j<n; j++) sum += H[i,j]*xbiez[j];
     xnast[i] = g[i] + sum;
    }
    for (int i=0; i<n; i++) xbiez[i] = xnast[i];
   }

   for (int i=0; i<n; i++) x[i] = xbiez[i];

   Console.WriteLine("otrzymane rozwiazanie rownania -->");

   for (int i=0; i<n; i++)
   Console.WriteLine("x"+i+" = "+x[i]);

   for (int i=0; i<n; i++) {
    double sum = 0.0;
    for (int j=0; j<n; j++) sum += A[i,j]*x[j];
    db[i] = sum;
   } 
   for (int i=0; i<n; i++)
   db[i] = (b[i]-db[i])/db[i];

   Console.WriteLine();
   Console.WriteLine("sprawdzenie dokladnosci rozwiazania -->");
   for (int i=0; i<n; i++)
   Console.WriteLine("db"+i+" = "+db[i]);
   
   Console.ReadLine();
  }
 }
}


