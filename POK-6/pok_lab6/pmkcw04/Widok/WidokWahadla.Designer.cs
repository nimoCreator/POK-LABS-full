namespace ModelowanieKomputerowe
{
 partial class WidokWahadla
 {
  private System.ComponentModel.IContainer components = null;

  protected override void Dispose(bool disposing)
  {
   if (disposing && (components != null))
   {
    components.Dispose();
   }
   base.Dispose(disposing);
  }

  #region Component Designer generated code

  /// <summary>
  /// Required method for Designer support - do not modify 
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
   this.SuspendLayout();
   // 
   // WidokWahadla
   // 
   this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
   this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
   this.Name = "WidokWahadla";
   this.Size = new System.Drawing.Size(204, 204);
   this.Paint += new System.Windows.Forms.PaintEventHandler(this.Rysuj_Obraz_);
   this.Resize += new System.EventHandler(this.Zmieniono_Rozmiar_);
   this.ResumeLayout(false);
  }

  #endregion
 }
}
