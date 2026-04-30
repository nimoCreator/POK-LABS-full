using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ModelowanieKomputerowe
{
 public partial class WidokWahadla : UserControl
 {
  //// wewnetrzne zmienne w widoku wahadla:

  private ModelWahadla model_ = null;

  //// model ukladu fizycznego:

  public ModelWahadla model
  { get { return model_; } }

  //// utworzenie obiektu z widokiem wahadla:

  public WidokWahadla()
  {
   InitializeComponent();

   // wlaczenie podwojnego buforowania eliminujacego migotanie obrazu:
   SetStyle(ControlStyles.UserPaint, true);
   SetStyle(ControlStyles.AllPaintingInWmPaint, true);
   SetStyle(ControlStyles.DoubleBuffer, true);
  }

  //// zapoczatkowanie animacji ukladu:

  public void Poczatek(ModelWahadla m)
  {
   // przypisz nowy obiekt modelu ukladu;
   model_ = m;

   // ustaw stan poczatkowy ukladu:
   model.Poczatek();
  }

  //// zakonczenie animacji ukladu:

  public void Koniec()
  {
   // obsluz stan koncowy ukladu:
   model.Koniec();

   // usun obiekt modelu ukladu;
   model_ = null;
  }

  //// wykonywanie animacji ukladu:

  public void Postep()
  {
   // generuj stan biezacy ukladu:
   model.Postep();

   // odrysuj widok ukladu w nowym stanie:
   Invalidate();
  }

  //// procedura rysowania widoku wahadla w biezacym stanie:

  private void Rysuj_Obraz_(object sender, System.Windows.Forms.PaintEventArgs e)
  {
   Graphics g = e.Graphics; // grafika okna na ekranie
   Rectangle obszar = ClientRectangle; // obszar roboczy okna
   Pen gmpioro1 = new Pen(Color.Red, 1); // pioro do rysowania dowolnych elementow geometrycznych (osie, wektory, luki)
   Pen gmpioro2 = new Pen(Color.DarkRed, 1); // pioro do rysowania strzalek dla zorientowanych elementow geometrycznych
   gmpioro2.CustomEndCap = new AdjustableArrowCap(0.025f, 0.025f, true);
   Pen whpioro = new Pen(Color.Silver, 4); // pioro do rysowania linii w modelu wahadla
   SolidBrush whpedzel = new SolidBrush(Color.Silver); // pedzel do malowania powierzchni w modelu wahadla
   SolidBrush smpedzel = new SolidBrush(Color.DarkGreen); // pedzel do malowania dowolnych symboli
   Font smkroj = new Font("Times New Roman", 0.05f, FontStyle.Bold); // kroj czcionki do rysowania dowolnych symboli

   g.ScaleTransform(obszar.Width / 2f, obszar.Height / 2f); // zmiana pikseli na jednostki fizyczne w obszarze roboczym
   gmpioro1.ScaleTransform(2f / obszar.Width, 2f / obszar.Height); // pozostawienie pikseli jako jenostek dla grubosci piora
   gmpioro2.ScaleTransform(2f / obszar.Width, 2f / obszar.Height); // pozostawienie pikseli jako jenostek dla grubosci piora
   whpioro.ScaleTransform(2f / obszar.Width, 2f / obszar.Height); // pozostawienie pikseli jako jenostek dla grubosci piora
   g.TranslateTransform(1f, 0.25f); // przesuniecie poczatku ukladu wspolrzednych

   g.DrawLine(gmpioro1, -1f, 0f, 0.95f, 0f); // rysuj os OX
   g.DrawLine(gmpioro2, 0.94f, 0f, 0.95f, 0f);
   g.DrawString("x", smkroj, smpedzel, 0.9f, 0.01f);

   g.DrawLine(gmpioro1, 0f, -0.25f, 0f, 1.7f); // rysuj os OY
   g.DrawLine(gmpioro2, 0f, 1.69f, 0f, 1.7f);
   g.DrawString("y", smkroj, smpedzel, 0.03f, 1.63f);

   g.FillRectangle(whpedzel, -0.5f, -0.1f, 1f, 0.1f); // rysuj gorna sciane z zaczepem wahadla
   g.FillEllipse(whpedzel, -0.025f, -0.025f, 0.05f, 0.05f); // rysuj zaczep wahadla
   g.DrawString("0", smkroj, smpedzel, 0.02f, 0.02f);

   if (model == null) return;

   float dl = 0.4f * (float)model.l; // parametry modelu graficznego wahadla
   float fi = 180f * (float)model.fi / (float)Math.PI;
   float dfi = fi > 0f ? 0.17f : -0.07f;
   float x = (float)model.x, y = (float)model.y;
   const float s = 0.2f;
   float vx = s * (float)model.vx, vy = s * (float)model.vy;
   float ax = s * (float)model.ax, ay = s * (float)model.ay;

   g.DrawLine(whpioro, 0f, 0f, x, y); // rysuj nic wahadla
   g.DrawString("l", smkroj, smpedzel, 0.6f * (x + y * 0.07f), 0.6f * (y - x * 0.07f));

   g.FillEllipse(whpedzel, x - 0.1f, y - 0.1f, 0.2f, 0.2f); // rysuj mase wahadla
   g.DrawString("m", smkroj, smpedzel, x + 0.06f, y + 0.06f);

   g.DrawArc(gmpioro1, -dl, -dl, 2 * dl, 2 * dl, 90f - fi, fi); // rysuj kat wychylenia wahadla
   g.DrawString("\x3C6", smkroj, smpedzel, 0.3f * (x - y * dfi), 0.3f * (y + x * dfi));

   g.DrawLine(gmpioro1, x, y, x + vx, y + vy); // rysuj predkosc masy wahadla
   g.DrawLine(gmpioro2, x + 0.99f * vx, y + 0.99f * vy, x + vx, y + vy);
   g.DrawString("v", smkroj, smpedzel, x + vx, y + vy);

   g.DrawLine(gmpioro1, x, y, x + ax, y + ay); // rysuj przyspieszenie masy wahadla
   g.DrawLine(gmpioro2, x + 0.99f * ax, y + 0.99f * ay, x + ax, y + ay);
   g.DrawString("a", smkroj, smpedzel, x + ax, y + ay);
  }

  //// procedura odrysowania widoku wahadla po zmianie rozmiarow widoku:

  private void Zmieniono_Rozmiar_(object sender, System.EventArgs e)
  {
   Invalidate();
  }
 }
}
