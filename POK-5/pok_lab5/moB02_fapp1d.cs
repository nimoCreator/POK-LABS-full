using System;
//
// klasy narzedziowe do zajec laboratoryjnych z tekstu 'moB02.doc'
//
namespace LabTools
{
 public delegate double Function1D(double x); // delegacja aproksymowanej funkcji skalarnej y = f(x) z jednowymiarowa dziedzina

 public abstract class Approximator1D // klasa abstrakcyjnego jednowymiarowego aproksymatora SPH funkcji skalarnej
 {
  private readonly int Nx_; 
  private double xa_,xb_; 
  private double wx_; 
  private double hr_; 
  private double[] xo_; 
  private double[] yo_;
  
  public Approximator1D(int Nx) // utworzenie aproksymatora z zadanym rozmiarem aproksymacji czyli liczba wezlow dyskretyzacji
  {
   Nx_ = Nx;
   xo_ = new double[Nx];
   yo_ = new double[Nx];
  }
  
  public void Apply(Function1D f, double xa, double xb, double hr = 0.0) // zastosowanie aproksymatora dla podanej funkcji oraz dla podanego przedzialu i nosnika aproksymacji
  {
   xa_ = xa;
   xb_ = xb;
   wx_ = (xb_-xa_)/(Nx_-1);
   hr_ = hr<=0.0 ? 1.5*wx_ : hr; // uwaga: wartosc domyslna nosnika to polowa kroku dyskretyzacji przedzialu 

   for (int i=0; i<Nx_; i++) { // obliczanie wezlowych wartosci funkcji w przedziale aproksymacji
    double xi = xa_+i*wx_;
    double yi = f(xi);
    xo_[i] = xi;
    yo_[i] = yi;
   } 
  }
  
  public int Rank { // rzad dyskretryzacji przedzialu funkcji
   get { return Nx_-1; } 
  }

  public int Size { // rozmiar aproksymacji rowny liczbie wezlow dyskretyzacji
   get { return Nx_; } 
  }

  public double Begin { // poczatek przedzialu funkcji
   get { return xa_; } 
  }

  public double End { // koniec przedzialu funkcji 
   get { return xb_; } 
  }

  public double Length { // dlugosc przedzialu funkcji 
   get { return xb_-xa_; } 
  }

  public double Step { // krok dyskretyzacji przedzialu
   get { return wx_; } 
  }

  public double Support { // nosnik jadra aproksymacji (dlugosc wygladzania)
   get { return hr_; } 
  }

  public double Node(int i, out double xi) // przekazuje polozenie wezla o podanym indeksie oraz zwraca wartosc funkcji w tym wezle
  {
   double yi;
   Node(i,out xi,out yi); 
   return yi; 
  }
  
  public void Node(int i, out double xi, out double yi) // przekazuje dla podanego indeksu wezla jego polozenie i wartosc funkcji
  {
   xi = xo_[i];
   yi = yo_[i];
  }
  
  public double Value(double x) // zwraca wartosc aproksymanta funkcji dla podanego argumentu w pelnym zakresie aproksymacji
  {
   return Value(x, 0, Nx_-1);
  }
  
  public double Value(double x, int di) // zwraca wartosc aproksymanta funkcji dla podanego argumentu w obcietym zakresie aproksymacji z podanym marginesem do obciecia
  {
   return Value(x, di, Nx_-1-di);
  }
  
  public double Value(double x, int ia, int ib) // zwraca wartosc aproksymanta funkcji dla podanych argumentu oraz zakresu aproksymacji
  {
   double wx = wx_;
   double hr = hr_;
   double sum = 0.0;
   for (int i=ia; i<=ib; i++) {
    double xi = xo_[i];
    double yi = yo_[i];
    double ri = Math.Abs(x-xi);
    double Wi = kernel(ri,hr);
    sum += wx*yi*Wi;
   }
   return sum;
  }

  public double Derivative(double x) // zwraca pochodna aproksymanta funkcji dla podanego argumentu w pelnym zakresie aproksymacji
  {
   return Derivative(x, 0, Nx_-1);
  }

  public double Derivative(double x, int di) // zwraca pochodna aproksymanta funkcji dla podanego argumentu w obcietym zakresie aproksymacji z podanym marginesem do obciecia
  {
   return Derivative(x, di, Nx_-1-di);
  }

  public double Derivative(double x, int ia, int ib) // zwraca pochodna aproksymanta funkcji dla podanych argumentu oraz zakresu aproksymacji
  {
   double wx = wx_;
   double hr = hr_;
   double sum = 0.0;
   for (int i = ia; i<=ib; i++)
   {
    double xi = xo_[i];
    double yi = yo_[i];
    double ri = Math.Abs(x-xi);
    double Di = kerder(ri,hr);
    sum += wx*yi*Di*(x-xi);
   }
   return sum;
  }

  public double SecondDerivative(double x) // zwraca druga pochodna aproksymanta funkcji dla podanego argumentu w pelnym zakresie aproksymacji
  {
   return SecondDerivative(x, 0, Nx_-1);
  }

  public double SecondDerivative(double x, int di) // zwraca druga pochodna aproksymanta funkcji dla podanego argumentu w obcietym zakresie aproksymacji z podanym marginesem do obciecia
  {
   return SecondDerivative(x, di, Nx_-1-di);
  }

  public double SecondDerivative(double x, int ia, int ib) // zwraca druga pochodna aproksymanta funkcji dla podanych argumentu oraz zakresu aproksymacji
  {
   double wx = wx_;
   double hr = hr_;
   double sum = 0.0;
   for (int i = ia; i<=ib; i++)
   {
    double xi = xo_[i];
    double yi = yo_[i];
    double ri = Math.Abs(x-xi);
    double Gi = kerddr(ri,hr);
    sum += wx*yi*Gi;
   }
   return sum;
  }

  public abstract double kernel(double r, double h); // radialne jadro aproksymacji z nosnikiem
  public abstract double kerder(double r, double h); // pochodna jadra aproksymacji
  public abstract double kerddr(double r, double h); // druga pochodna jadra aproksymacji
 }
}
