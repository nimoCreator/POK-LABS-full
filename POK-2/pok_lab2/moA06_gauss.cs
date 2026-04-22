using System;
//
// klasy narzedziowe do zajec laboratoryjnych z tekstu 'moA06.doc'
//
namespace LabTools
{
 public abstract class LinEq // klasa bazowa (abstrakcyjna) algorytmow rozwiazywania ukladu rownan liniowych
 {
  public int n; // dany rzad ukladu
  public double[,] A; // macierz danych parametrow rownania
  public double[] b; // wektor danych wyrazow wolnych rownania
  public double[] x; // wektor szukanych pierwiastkow rownania
  public double[] db; // wektor wzglednych bledow w wyrazach wolnych jako oszacowanie bledu rozwiazania
  
  public LinEq(int num) // utworzenie obiektu rownania i jego struktur danych
  {
   n = num;
   
   A = new double[n,n];
   b = new double[n];
   x = new double[n];
   db = new double[n];
  }
  
  public bool solve() // rozwiazanie rownania, zwraca flage istnienia rozwiazania
  {
   if (!do_solve()) return false; // brak rozwiazania
   
   for (int i=0; i<n; i++) { // otrzymanie oszacowania dla wzglednych bledow rozwiazania
    double sum = 0.0;
    for (int j=0; j<n; j++) sum += A[i,j]*x[j];
    db[i] = sum;
   } 
   for (int i=0; i<n; i++)
   db[i] = (b[i]-db[i])/db[i];

   return true; // rozwiazanie istnieje
  }

  protected abstract bool do_solve(); // wlasciwa definicja algorytmu - nalezy podac w klasie pochodnej
 }
 
 public sealed class GaussLinEq : LinEq // klasa konkretnego algorytmu - elimincja Gaussa
 {
  private double[][] C; // pomocnicza macierz rozszerzona rownania

  public GaussLinEq(int num): base(num) // utworzenie obiektu rownania i jego struktur danych
  {
   C = new double[n][]; // macierz C zawiera n wierszy
   for (int i=0; i<n; i++)
   C[i] = new double[n+1]; // oraz n+1 kolumn 
  }
 
  protected override bool do_solve()
  {
//
// rozwiazywanie ukladu rownan postaci A*x=b
// przy uzyciu metody eliminacji Gaussa:
//
   double[] Crow = null; // pomocniczy wiersz macierzy C
   double mlt, maxel; // pomocnicze wyrazy
   int maxpos, i, j, K; // pomocnicze indeksy

   for (i=0; i<n; i++) {
    for (j=0; j<n; j++) C[i][j] = A[i,j]; // podstawienie wartosci parametrow rownania do macierzy rozszerzonej
    C[i][n]=b[i]; // oraz wartosci wyrazow wolnych do ostatniej, n+1 kolumny macierzy C
    x[i]=0.0; // wyzerowanie wektora niewiadomych
   } 

   for (K=0; K<n-1; K++) // petla po n-1 poczatkowych kolumnach macierzy C      
   {
    maxel=Math.Abs(C[K][K]); // szukanie najwiekszego elementu w biezacej kolumnie, czyli tzw. elementu podstawowego
    maxpos=K;                // poszukiwanie rozpoczyna sie od elementu diagonalnego (i==j==K)
    for (i=K+1; i<n; i++) {  // a nastepnie poszukiwanie kontynuuje sie w dol kolumny
     double el=Math.Abs(C[i][K]);
     if (el>maxel) {
      maxel=el;
      maxpos=i;
     }
    }
          
    if (Math.Abs(maxel)<1e-10) return false; // blad: biezaca kolumna macierzy C zawiera od diagonali w dol same zera - brak rozwiazania

    if (maxpos!=K) { // jezeli polozenie najwiekszego elementu w biezacej kolumnie jest rozne od diagonalnego
     Crow=C[maxpos]; // to nastepuje zamiana miejscami wierszy: diagonalnego i z elementem najwiekszym
     C[maxpos]=C[K]; // tak aby po zamianie polozenie najwiekszego elementu w biezacej kolumnie bylo diagonalne
     C[K]=Crow;
    }
    
    double CKK=C[K][K];     // element diagonalny rowny elementowi najwiekszemu, czyli element podstawowy; wiersz z tym elementem nazywany wierszem podstawowym
    for (i=K+1; i<n; i++) { // odjecie wiersza podstawowego od polozonych poni¿ej wierszy 
     mlt=C[i][K]/CKK;       // odejmowany wiersz jest pomnozony przez czynnik 'mult'
     C[i][K]=0.0;           // co w efekcie daje wyzerowanie elementow w biezacej kolumnie ponizej elementu podstawowego
     for (j=K+1; j<=n; j++) C[i][j]-=mlt*C[K][j];
    }
   }
                                                  // przedostatnia, n-ta kolumna macierzy C nie wymaga przetwarzania, gdyz zawiera jedenie jeden element - element diagonalny, ktory od razu staje sie elementem podstawowym
   if (Math.Abs(C[n-1][n-1])<1e-10) return false; // blad: przedostatnia kolumna macierzy C zawiera na diagonali zero - brak rozwiazania

   for (i=n-1; i>=0; i--) { // wyznaczenie rozwiazan z macierzy C, ktora po p/w operacjach ma postac trojkatna
    x[i]=C[i][n];
    for (j=i+1; j<n; j++) x[i]-=C[i][j]*x[j];
    x[i]/=C[i][i];        
   }

   return true; // rozwiazanie istnieje
  } 
 
 }
}
