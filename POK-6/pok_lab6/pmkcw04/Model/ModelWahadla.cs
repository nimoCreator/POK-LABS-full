using System;
using System.ComponentModel;

namespace ModelowanieKomputerowe
{
 public class ModelWahadla : System.ComponentModel.Component
 {
  //// wewnetrzne zmienne w modelu wahadla, w jednostkach SI:

  private double g_, l_, fi0_; // przyspieszenie ziemskie, dlugosc wahadla i poczatkowy kat wychylenia w radianach
  private double delt_; // przyrost czasu w procedurze numerycznej
  private double To_; // okres drgan wahadla dla malych wychylen
  private double t_; // biezacy czas
  private double fi_, om_, al_; // kat wychylenia wahadla w radianach oraz jego pochodne - predkosc i przyspieszenie katowe 
  private double x_, y_, vx_, vy_, ax_, ay_; // polozenie, predkosc i przyspieszenie masy wahadla 
  private double chwT_, sumT_; // chwilowa i sumaryczna wartosc biezacego okresu drgan wahadla
  private int numT_; // licznik okresow drgan wahadla

  //// stale wielkosci ukladu fizycznego:

  public double g  // przyspieszenie ziemskie
  { get { return g_; } }

  public double l  // dlugosc wahadla
  { get { return l_; } set { if (l_ != value) { l_ = value; Poczatek(); } } }

  public double fi0 // poczatkowy kat wychylenia wahadla od pionu w stopniach
  { get { return fi0_ * 180.0 / Math.PI; } set { double afi0 = value * Math.PI / 180.0; if (fi0_ != afi0) { fi0_ = afi0; Poczatek(); } } }

  public double delt // przyrost czasu w procedurze numerycznej rozwiazywania rownan ruchu wahadla
  { get { return delt_; } }

  public double To // okres drgan wahadla dla malych wychylen
  { get { return To_; } }

  //// wielkosci fizyczne ukladu zalezne od czasu:

  public double t // biezacy czas
  { get { return t_; } }

  public double fi // kat wychylenia wahadla w radianach
  { get { return fi_; } }

  public double om // predkosc katowa dla kata w radianach 
  { get { return om_; } }

  public double al // przyspieszenie katowe dla kata w radianach 
  { get { return al_; } }

  public double x // polozenie liniowe masy wahadla 
  { get { return x_; } }
  public double y
  { get { return y_; } }

  public double vx // predkosc liniowa masy wahadla  
  { get { return vx_; } }
  public double vy
  { get { return vy_; } }

  public double ax // przyspieszenie liniwe masy wahadla 
  { get { return ax_; } }
  public double ay
  { get { return ay_; } }

  public double T // sredni okres drgan wahadla
  { get { return numT_ <= 0 ? 0.0 : sumT_ / numT_; } }

  //// utworzenie obiektu z modelem wahadla:

  public ModelWahadla()
  {
   // ustaw niezmienne wartosci dla parametrow stalych wahadla:
   g_ = 9.81; // m/s^2
   delt_ = 0.025; // s

   // ustaw domyslne wartosci dla pozostalych parametrow stalych wahadla:
   l_ = 1.2; // m
   fi0_ = 30.0 * Math.PI / 180.0; // rad
  }

  //// zapoczatkowanie obliczen dla ruchu wahadla:

  public void Poczatek()
  {
   // oblicz okres drgan wahadla dla malych wychylen:
   To_ = 2.0 * Math.PI * Math.Sqrt(l_ / g_);

   // ustaw stan poczatkowy w ruchu wahadla:
   t_ = 0.0;
   fi_ = fi0_;
   om_ = al_ = 0.0;
   x_ = l_ * Math.Sin(fi_);
   y_ = l_ * Math.Cos(fi_);
   vx_ = vy_ = ax_ = ay_ = 0.0;
   chwT_ = 0.0;
   sumT_ = 0.0;
   numT_ = 0;
  }

  //// zakonczenie obliczen dla ruchu wahadla:

  public void Koniec()
  {
  }

  //// wykonywanie obliczan dla ruchu wahadla:

  public void Postep()
  {
   // zmien stan wahadla zgodnie z jego rownaniem:
   double pom = om_;
   om_ = om_ - delt_ * (g_ / l_) * Math.Sin(fi_);
   al_ = (om_ - pom) / delt_;
   fi_ = fi_ + delt_ * om_;
   x_ = l_ * Math.Sin(fi_);
   y_ = l_ * Math.Cos(fi_);
   vx_ = l_ * om_ * Math.Cos(fi_);
   vy_ = -l_ * om_ * Math.Sin(fi_);
   ax_ = l_ * al_ * Math.Cos(fi_) - l_ * om_ * om_ * Math.Sin(fi_);
   ay_ = -l_ * al_ * Math.Sin(fi_) - l_ * om_ * om_ * Math.Cos(fi_);
   t_ += delt_;

   // oblicz sredni okres drgan wahadla:
   if (chwT_ > 0.0 && fi_ > 0.0 && Math.Sign(om_) != Math.Sign(pom))
   {
    sumT_ += chwT_;
    numT_++;
    chwT_ = 0.0;
   }
   chwT_ += delt_;
  }
 }
}
