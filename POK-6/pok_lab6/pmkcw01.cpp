///////////////////////////////
///////
/////// Model matematyczny i numeryczny
/////// ukladu elektrycznego typu RC
///////
///////////////////////////////

#include <iostream>  // biblioteki standardowe jezyka C++ 
#include <iomanip>   // uzyte zgodnie z nowym standardem
#include <cmath>     // jezyka C++, czyli ISO C++98
#include <fstream>   // obsluga plikow dyskowych
#include <string>    // operacje na danych lancuchowych

using namespace std; // uzycie nazw biblioteki std. w programie

typedef long double real; // symbol typu rzeczywistego

// Funkcja liczy wartosc rozwiazania analitycznego:
//
//  zakladamy, ze ladunek poczatkowy i stala czasowa
//  sa jednostkowe
//
real RC_Analityczne(real t) 
{
 return exp(-t);
}

// Funkcje licza wartosc rozwiazania numerycznego:

real RC_Euler_step(real q,real t,real delt) // metoda Eulera
{
 return q*(1.0-delt);
}
real RC_MidPoint_step(real q,real t,real delt) // metoda punktu srodkowego
{
 real w1 = -delt*q;
 real w2 = -delt*(q+0.5*w1);
 return q+w2;
}
real RC_RungeKutta_step(real q,real t,real delt) // metoda Rungego-Kutty
{
 real w1 = -delt*q;
 real w2 = -delt*(q+0.5*w1);
 real w3 = -delt*(q+0.5*w2);
 real w4 = -delt*(q+w3);
 return q+(w1+2.0*w2+2.0*w3+w4)/6.0;
}

// Test powyzszych funkcji:
void main()
{
 real delt;
 real tk;
 real q=1.0; // wartosc poczatkowa ladunku

 real q_e, q_mp, q_rk;
 q_e = q_mp = q_rk = q;
 
 cout << "skok czasu: "; cin >> delt;
 cout << "czas koncowy: "; cin >> tk;
 string nazwa_pliku; // typ lanuchowy w nowym standardzie jezyka C++
 cout << "podaj nazwe pliku z wynikami: "; cin >> nazwa_pliku;
 ofstream plik(nazwa_pliku.c_str()); // utworzenie i otwarcie pliku do zapisu wynikow
 cout << endl;

 cout << "Rozwiazanie modelu ukladu RC:" << endl;
 cout << endl;

 const int szerkol = 12;
 cout << setprecision(3);
 cout << left;
 cout << setw(szerkol) << "czas";
 cout << setw(szerkol) << "analit";
 cout << setw(szerkol) << "euler";
 cout << setw(szerkol) << "midpnt";
 cout << setw(szerkol) << "rungkut";
 cout << endl;
 plik << setprecision(3);
 plik << left;
 plik << setw(szerkol) << "czas";
 plik << setw(szerkol) << "analit";
 plik << setw(szerkol) << "euler";
 plik << setw(szerkol) << "midpnt";
 plik << setw(szerkol) << "rungkut";
 plik << endl;

 for (real t=0.0; t<=tk; t+=delt) {
  cout << setw(szerkol) << t;
  plik << setw(szerkol) << t;
  
  q=RC_Analityczne(t);
  cout << setw(szerkol) << q;
  plik << setw(szerkol) << q;
  
  cout << setw(szerkol) << q_e;
  plik << setw(szerkol) << q_e;
  q_e=RC_Euler_step(q_e, t, delt);
  
  cout << setw(szerkol) << q_mp;
  plik << setw(szerkol) << q_mp;
  q_mp=RC_MidPoint_step(q_mp, t, delt);
  
  cout << setw(szerkol) << q_rk;
  plik << setw(szerkol) << q_rk;
  q_rk=RC_RungeKutta_step(q_rk, t, delt);
 
  cout << endl;
  plik << endl;
 }

 cout << endl;
 cin.ignore(2);
}
