using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ModelowanieKomputerowe
{
 public partial class PanelWahadla : UserControl
 {
  //// wewnetrzne zmienne w panelu wahadla:

  private WidokWahadla widok_ = null;

  //// widok ukladu fizycznego:

  public WidokWahadla widok
  { get { return widok_; } }

  //// utworzenie obiektu z panelem wahadla:

  public PanelWahadla()
  {
   InitializeComponent();
  }

  //// zapoczatkowanie sterowania ukladem:

  public void Poczatek(ModelWahadla m, WidokWahadla w)
  {
   widok_ = w;
   widok_.Poczatek(m);
   tb_wlkg.Text = "  " + widok_.model.g.ToString("f2");
   tb_wlkl.Text = "  " + widok_.model.l.ToString("f2");
   tb_wlkfi0.Text = "  " + widok_.model.fi0.ToString("f1");
   tb_wlkTo.Text = "  " + widok_.model.To.ToString("f3");
   tb_wlkl.ReadOnly = true;
   tb_wlkfi0.ReadOnly = true;
   zegar_.Enabled = true;
  }

  //// zakonczenie sterowania ukladem:

  public void Koniec()
  {
   zegar_.Enabled = false;
   widok_.Koniec();
  }

  //// procedury sterowania symulacja wahadla w panelu:

  private void Postep_Symulacji_(object sender, System.EventArgs e)
  {
   widok_.Postep();
   tb_wlkT.Text = "  " + widok_.model.T.ToString("f3");
  }

  private void Wstrzymaj_Symulacje_(object sender, System.EventArgs e)
  {
   zegar_.Enabled = false;
   tb_wlkl.ReadOnly = false;
   tb_wlkfi0.ReadOnly = false;
  }

  private void Wznow_Symulacje_(object sender, System.EventArgs e)
  {
   widok_.model.l = double.Parse(tb_wlkl.Text);
   widok_.model.fi0 = double.Parse(tb_wlkfi0.Text);
   tb_wlkTo.Text = "  " + widok_.model.To.ToString("f3");
   tb_wlkl.ReadOnly = true;
   tb_wlkfi0.ReadOnly = true;
   zegar_.Enabled = true;
  }
 }
}
