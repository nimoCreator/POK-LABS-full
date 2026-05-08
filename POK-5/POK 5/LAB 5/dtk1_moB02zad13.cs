using System;
using System.IO;
//
// pierwszy dodatek do zadania nr 13 z tekstu 'moB02.doc'
//
namespace LabMOB
{
 static partial class Program
 {
  private static double[][] C_ = null; // pomocnicza macierz rozszerzona rownania
  static bool LinEq(int n, double[,] A, double[] b, ref double[] x)
  {
//
// rozwiazywanie ukladu rownan postaci A*x=b
// przy uzyciu metody eliminacji Gaussa:
//
   double[] Crow = null; // pomocniczy wiersz macierzy C
   double mlt, maxel; // pomocnicze wyrazy
   int maxpos, i, j, K; // pomocnicze indeksy
   
   if (C_==null || C_.GetLength(0)!=n) { // jednorazowe utworzenie pomocniczej macierzy
    C_ = new double[n][]; // macierz C zawiera n wierszy
    for (i=0; i<n; i++)
    C_[i] = new double[n+1]; // oraz n+1 kolumn 
   }

   for (i=0; i<n; i++) {
    for (j=0; j<n; j++) C_[i][j] = A[i,j]; // podstawienie wartosci parametrow rownania do macierzy rozszerzonej
    C_[i][n]=b[i]; // oraz wartosci wyrazow wolnych do ostatniej, n+1 kolumny macierzy C
   } 

   for (K=0; K<n-1; K++) // petla po n-1 poczatkowych kolumnach macierzy C      
   {
    maxel=Math.Abs(C_[K][K]); // szukanie najwiekszego elementu w biezacej kolumnie, czyli tzw. elementu podstawowego
    maxpos=K;                 // poszukiwanie rozpoczyna sie od elementu diagonalnego (i==j==K)
    for (i=K+1; i<n; i++) {   // a nastepnie poszukiwanie kontynuuje sie w dol kolumny
     double el=Math.Abs(C_[i][K]);
     if (el>maxel) {
      maxel=el;
      maxpos=i;
     }
    }
          
    if (Math.Abs(maxel)<1e-10) return false; // blad: biezaca kolumna macierzy C zawiera od diagonali w dol same zera - brak rozwiazania

    if (maxpos!=K) {   // jezeli polozenie najwiekszego elementu w biezacej kolumnie jest rozne od diagonalnego
     Crow=C_[maxpos];  // to nastepuje zamiana miejscami wierszy: diagonalnego i z elementem najwiekszym
     C_[maxpos]=C_[K]; // tak aby po zamianie polozenie najwiekszego elementu w biezacej kolumnie bylo diagonalne
     C_[K]=Crow;
    }
    
    double CKK=C_[K][K];    // element diagonalny rowny elementowi najwiekszemu, czyli element podstawowy; wiersz z tym elementem nazywany wierszem podstawowym
    for (i=K+1; i<n; i++) { // odjecie wiersza podstawowego od polozonych poni ej wierszy 
     mlt=C_[i][K]/CKK;      // odejmowany wiersz jest pomnozony przez czynnik 'mult'
     C_[i][K]=0.0;          // co w efekcie daje wyzerowanie elementow w biezacej kolumnie ponizej elementu podstawowego
     for (j=K+1; j<=n; j++) C_[i][j]-=mlt*C_[K][j];
    }
   }
                                                   // przedostatnia, n-ta kolumna macierzy C nie wymaga przetwarzania, gdyz zawiera jedenie jeden element - element diagonalny, ktory od razu staje sie elementem podstawowym
   if (Math.Abs(C_[n-1][n-1])<1e-10) return false; // blad: przedostatnia kolumna macierzy C zawiera na diagonali zero - brak rozwiazania

   for (i=n-1; i>=0; i--) { // wyznaczenie rozwiazan z macierzy C, ktora po p/w operacjach ma postac trojkatna
    x[i]=C_[i][n];
    for (j=i+1; j<n; j++) x[i]-=C_[i][j]*x[j];
    x[i]/=C_[i][i];        
   }

   return true; // rozwiazanie istnieje
  }
 }
}


