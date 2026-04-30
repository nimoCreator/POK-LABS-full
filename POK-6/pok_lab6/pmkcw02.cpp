///////////////////////////////
///////
/////// Model matematyczny przyblizony
/////// i model numeryczny
/////// prostego wahadla
///////
///////////////////////////////

#include <iostream>  // biblioteki standardowe jezyka C++ 
#include <iomanip>   // uzyte zgodnie z nowym standardem
#include <cmath>     // jezyka C++, czyli ISO C++98
#include <fstream>   // obsluga plikow dyskowych
#include <string>    // operacje na danych lancuchowych

using namespace std; // uzycie nazw biblioteki std. w programie

typedef long double real; // symbol typu rzeczywistego

const real Pi = 3.14159265358979323846; // konwersja z radianow na stopnie
const real stopnie = 180.0/Pi;

// Funkcja liczy wartosc przyblizonego rozwiazania analitycznego:
//  zaklada sie, ze czas poczatkowy jest rowny zero, 
//  natomiast predkosc poczatkowa jest zerowa

real wahadlo_Analityczne(real t,real fi0,real sqrt_g_div_l) 
{
 return fi0*cos(t*sqrt_g_div_l);
}

// Funkcje licza wartosc rozwiazania numerycznego:

real wahadlo_Euler_step(real fi,real om,real t,real delt,real g_div_l) // metoda Eulera
{
 return om - delt*g_div_l*sin(fi);
}
real wahadlo_MidPoint_step(real fi,real om,real t,real delt,real g_div_l) // metoda punktu srodkowego
{
 // ... uzupelnic
 return om;
}
real wahadlo_RungeKutta_step(real fi,real om,real t,real delt,real g_div_l) // metoda Rungego-Kutty
{
 // ... uzupelnic
 return om;
}

// Test powyzszych funkcji:
void main()
{
 const real g = 9.81; // przyspieszenie ziemskie
 real l; // dlugosc wahadla
 real delt; // skok czasu
 real tk; // czas koncowy
 real fi0; // kat poczatkowy
 
 cout << "dlugosc wahadla w metrach: "; cin >> l;
 real g_div_l = g/l;
 real sqrt_g_div_l = sqrt(g_div_l);
 real T = 2*Pi/sqrt_g_div_l;
 cout << "okres wahan dla malych wychylen wynosi: " << T << " sekund" << endl;
 cout << "kat poczatkowy w stopniach: "; cin >> fi0; 
 fi0/=stopnie;
 cout << "skok czasu w okresach: "; cin >> delt;
 delt*=T;
 cout << "czas koncowy w okresach: "; cin >> tk;
 tk*=T;
 string nazwa_pliku; // typ lanuchowy w nowym standardzie jezyka C++
 cout << "podaj nazwe pliku z wynikami: "; cin >> nazwa_pliku;
 ofstream plik(nazwa_pliku.c_str()); // utworzenie i otwarcie pliku do zapisu wynikow
 cout << endl;

 real fi, fi_e, fi_mp, fi_rk;
 fi_e = fi_mp = fi_rk = fi0; // kat poczatkowy
 real om_e, om_mp, om_rk;
 om_e = om_mp = om_rk = 0.0; // predkosc poczatkowa

 cout << "Rozwiazanie modelu wahadla w sekundach i stopniach:" << endl;
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
  
  fi=wahadlo_Analityczne(t, fi0, sqrt_g_div_l);
  cout << setw(szerkol) << fi*stopnie;
  plik << setw(szerkol) << fi*stopnie;
  
  cout << setw(szerkol) << fi_e*stopnie;
  plik << setw(szerkol) << fi_e*stopnie;
  om_e=wahadlo_Euler_step(fi_e, om_e, t, delt, g_div_l);
  fi_e = fi_e + delt*om_e;
  
  cout << setw(szerkol) << fi_mp*stopnie;
  plik << setw(szerkol) << fi_mp*stopnie;
  om_mp=wahadlo_MidPoint_step(fi_mp, om_mp, t, delt, g_div_l);
  fi_mp = fi_mp + delt*om_mp;
  
  cout << setw(szerkol) << fi_rk*stopnie;
  plik << setw(szerkol) << fi_rk*stopnie;
  om_rk=wahadlo_RungeKutta_step(fi_rk, om_rk, t, delt, g_div_l);
  fi_rk = fi_rk + delt*om_rk;
 
  cout << endl;
  plik << endl;
 }

 cout << endl;
 cin.ignore(2);
}
