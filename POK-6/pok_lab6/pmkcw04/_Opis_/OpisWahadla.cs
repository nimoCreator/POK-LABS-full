using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;

namespace ModelowanieKomputerowe
{
    public partial class OpisWahadla
    {
        private void Poczatek_Prezentacji_(object sender, System.EventArgs e)
        {
         this.modelowanieKomputerowe_PanelWahadla1.Poczatek(this.modelWahadla1, this.modelowanieKomputerowe_WidokWahadla1);
        }

        private void Koniec_Prezentacji_(object sender, System.EventArgs e)
        {
         this.modelowanieKomputerowe_PanelWahadla1.Koniec();
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Poczatek_Prezentacji_);
            this.Shutdown += new System.EventHandler(Koniec_Prezentacji_);
        }

        #endregion
    }
}
