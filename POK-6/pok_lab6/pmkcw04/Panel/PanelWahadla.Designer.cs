namespace ModelowanieKomputerowe
{
 partial class PanelWahadla
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
   this.components = new System.ComponentModel.Container();
   this.zegar_ = new System.Windows.Forms.Timer(this.components);
   this.lb_wlkT = new System.Windows.Forms.Label();
   this.lb_jdnfi0 = new System.Windows.Forms.Label();
   this.tb_wlkT = new System.Windows.Forms.TextBox();
   this.lb_jdnl = new System.Windows.Forms.Label();
   this.lb_jdng = new System.Windows.Forms.Label();
   this.lb_jdnTo = new System.Windows.Forms.Label();
   this.gb_parametry = new System.Windows.Forms.GroupBox();
   this.tb_wlkfi0 = new System.Windows.Forms.TextBox();
   this.tb_wlkl = new System.Windows.Forms.TextBox();
   this.tb_wlkg = new System.Windows.Forms.TextBox();
   this.lb_wlkg = new System.Windows.Forms.Label();
   this.lb_wlkl = new System.Windows.Forms.Label();
   this.lb_wlkfi0 = new System.Windows.Forms.Label();
   this.tb_wlkTo = new System.Windows.Forms.TextBox();
   this.lb_wlkTo = new System.Windows.Forms.Label();
   this.bt_start = new System.Windows.Forms.Button();
   this.bt_stop = new System.Windows.Forms.Button();
   this.lb_jdnT = new System.Windows.Forms.Label();
   this.gb_okresy = new System.Windows.Forms.GroupBox();
   this.gb_parametry.SuspendLayout();
   this.gb_okresy.SuspendLayout();
   this.SuspendLayout();
   // 
   // zegar_
   // 
   this.zegar_.Interval = 30;
   this.zegar_.Tick += new System.EventHandler(this.Postep_Symulacji_);
   // 
   // lb_wlkT
   // 
   this.lb_wlkT.Location = new System.Drawing.Point(1, 39);
   this.lb_wlkT.Name = "lb_wlkT";
   this.lb_wlkT.Size = new System.Drawing.Size(162, 23);
   this.lb_wlkT.TabIndex = 15;
   this.lb_wlkT.Text = "okres dla dowolnych wychyleń: ";
   this.lb_wlkT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
   // 
   // lb_jdnfi0
   // 
   this.lb_jdnfi0.Location = new System.Drawing.Point(190, 66);
   this.lb_jdnfi0.Name = "lb_jdnfi0";
   this.lb_jdnfi0.Size = new System.Drawing.Size(31, 23);
   this.lb_jdnfi0.TabIndex = 18;
   this.lb_jdnfi0.Text = "deg";
   this.lb_jdnfi0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
   // 
   // tb_wlkT
   // 
   this.tb_wlkT.Location = new System.Drawing.Point(163, 42);
   this.tb_wlkT.Name = "tb_wlkT";
   this.tb_wlkT.ReadOnly = true;
   this.tb_wlkT.Size = new System.Drawing.Size(52, 20);
   this.tb_wlkT.TabIndex = 14;
   // 
   // lb_jdnl
   // 
   this.lb_jdnl.Location = new System.Drawing.Point(190, 40);
   this.lb_jdnl.Name = "lb_jdnl";
   this.lb_jdnl.Size = new System.Drawing.Size(20, 23);
   this.lb_jdnl.TabIndex = 17;
   this.lb_jdnl.Text = "m";
   this.lb_jdnl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
   // 
   // lb_jdng
   // 
   this.lb_jdng.Location = new System.Drawing.Point(190, 14);
   this.lb_jdng.Name = "lb_jdng";
   this.lb_jdng.Size = new System.Drawing.Size(31, 23);
   this.lb_jdng.TabIndex = 16;
   this.lb_jdng.Text = "m/s2";
   this.lb_jdng.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
   // 
   // lb_jdnTo
   // 
   this.lb_jdnTo.Location = new System.Drawing.Point(216, 13);
   this.lb_jdnTo.Name = "lb_jdnTo";
   this.lb_jdnTo.Size = new System.Drawing.Size(10, 23);
   this.lb_jdnTo.TabIndex = 19;
   this.lb_jdnTo.Text = "s";
   this.lb_jdnTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
   // 
   // gb_parametry
   // 
   this.gb_parametry.Controls.Add(this.tb_wlkfi0);
   this.gb_parametry.Controls.Add(this.tb_wlkl);
   this.gb_parametry.Controls.Add(this.tb_wlkg);
   this.gb_parametry.Controls.Add(this.lb_jdng);
   this.gb_parametry.Controls.Add(this.lb_jdnl);
   this.gb_parametry.Controls.Add(this.lb_jdnfi0);
   this.gb_parametry.Controls.Add(this.lb_wlkg);
   this.gb_parametry.Controls.Add(this.lb_wlkl);
   this.gb_parametry.Controls.Add(this.lb_wlkfi0);
   this.gb_parametry.Location = new System.Drawing.Point(7, 73);
   this.gb_parametry.Name = "gb_parametry";
   this.gb_parametry.Size = new System.Drawing.Size(227, 93);
   this.gb_parametry.TabIndex = 25;
   this.gb_parametry.TabStop = false;
   this.gb_parametry.Text = "Parametry modelu:";
   // 
   // tb_wlkfi0
   // 
   this.tb_wlkfi0.Location = new System.Drawing.Point(137, 66);
   this.tb_wlkfi0.Name = "tb_wlkfi0";
   this.tb_wlkfi0.Size = new System.Drawing.Size(52, 20);
   this.tb_wlkfi0.TabIndex = 10;
   // 
   // tb_wlkl
   // 
   this.tb_wlkl.Location = new System.Drawing.Point(137, 40);
   this.tb_wlkl.Name = "tb_wlkl";
   this.tb_wlkl.Size = new System.Drawing.Size(52, 20);
   this.tb_wlkl.TabIndex = 8;
   // 
   // tb_wlkg
   // 
   this.tb_wlkg.Location = new System.Drawing.Point(137, 14);
   this.tb_wlkg.Name = "tb_wlkg";
   this.tb_wlkg.ReadOnly = true;
   this.tb_wlkg.Size = new System.Drawing.Size(52, 20);
   this.tb_wlkg.TabIndex = 6;
   // 
   // lb_wlkg
   // 
   this.lb_wlkg.Location = new System.Drawing.Point(4, 14);
   this.lb_wlkg.Name = "lb_wlkg";
   this.lb_wlkg.Size = new System.Drawing.Size(133, 23);
   this.lb_wlkg.TabIndex = 7;
   this.lb_wlkg.Text = "przyspieszenie ziemskie: ";
   this.lb_wlkg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
   // 
   // lb_wlkl
   // 
   this.lb_wlkl.Location = new System.Drawing.Point(4, 40);
   this.lb_wlkl.Name = "lb_wlkl";
   this.lb_wlkl.Size = new System.Drawing.Size(133, 23);
   this.lb_wlkl.TabIndex = 9;
   this.lb_wlkl.Text = "długość wahadła: ";
   this.lb_wlkl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
   // 
   // lb_wlkfi0
   // 
   this.lb_wlkfi0.Location = new System.Drawing.Point(4, 64);
   this.lb_wlkfi0.Name = "lb_wlkfi0";
   this.lb_wlkfi0.Size = new System.Drawing.Size(133, 23);
   this.lb_wlkfi0.TabIndex = 11;
   this.lb_wlkfi0.Text = "wychylenie początkowe: ";
   this.lb_wlkfi0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
   // 
   // tb_wlkTo
   // 
   this.tb_wlkTo.Location = new System.Drawing.Point(163, 16);
   this.tb_wlkTo.Name = "tb_wlkTo";
   this.tb_wlkTo.ReadOnly = true;
   this.tb_wlkTo.Size = new System.Drawing.Size(52, 20);
   this.tb_wlkTo.TabIndex = 12;
   // 
   // lb_wlkTo
   // 
   this.lb_wlkTo.Location = new System.Drawing.Point(4, 14);
   this.lb_wlkTo.Name = "lb_wlkTo";
   this.lb_wlkTo.Size = new System.Drawing.Size(159, 23);
   this.lb_wlkTo.TabIndex = 13;
   this.lb_wlkTo.Text = "okres dla małych wychyleń: ";
   this.lb_wlkTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
   // 
   // bt_start
   // 
   this.bt_start.BackColor = System.Drawing.Color.Gainsboro;
   this.bt_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
   this.bt_start.Location = new System.Drawing.Point(7, 11);
   this.bt_start.Name = "bt_start";
   this.bt_start.Size = new System.Drawing.Size(110, 56);
   this.bt_start.TabIndex = 24;
   this.bt_start.Text = "Start";
   this.bt_start.UseVisualStyleBackColor = false;
   this.bt_start.Click += new System.EventHandler(this.Wznow_Symulacje_);
   // 
   // bt_stop
   // 
   this.bt_stop.BackColor = System.Drawing.Color.Gainsboro;
   this.bt_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
   this.bt_stop.Location = new System.Drawing.Point(124, 11);
   this.bt_stop.Name = "bt_stop";
   this.bt_stop.Size = new System.Drawing.Size(110, 56);
   this.bt_stop.TabIndex = 23;
   this.bt_stop.Text = "Stop";
   this.bt_stop.UseVisualStyleBackColor = false;
   this.bt_stop.Click += new System.EventHandler(this.Wstrzymaj_Symulacje_);
   // 
   // lb_jdnT
   // 
   this.lb_jdnT.Location = new System.Drawing.Point(216, 39);
   this.lb_jdnT.Name = "lb_jdnT";
   this.lb_jdnT.Size = new System.Drawing.Size(10, 23);
   this.lb_jdnT.TabIndex = 20;
   this.lb_jdnT.Text = "s";
   this.lb_jdnT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
   // 
   // gb_okresy
   // 
   this.gb_okresy.Controls.Add(this.tb_wlkT);
   this.gb_okresy.Controls.Add(this.tb_wlkTo);
   this.gb_okresy.Controls.Add(this.lb_jdnTo);
   this.gb_okresy.Controls.Add(this.lb_wlkTo);
   this.gb_okresy.Controls.Add(this.lb_jdnT);
   this.gb_okresy.Controls.Add(this.lb_wlkT);
   this.gb_okresy.Location = new System.Drawing.Point(7, 172);
   this.gb_okresy.Name = "gb_okresy";
   this.gb_okresy.Size = new System.Drawing.Size(227, 70);
   this.gb_okresy.TabIndex = 26;
   this.gb_okresy.TabStop = false;
   this.gb_okresy.Text = "Okres drgań wahadła:";
   // 
   // PanelWahadla
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.BackColor = System.Drawing.Color.PowderBlue;
   this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
   this.Controls.Add(this.gb_parametry);
   this.Controls.Add(this.bt_start);
   this.Controls.Add(this.bt_stop);
   this.Controls.Add(this.gb_okresy);
   this.Name = "PanelWahadla";
   this.Size = new System.Drawing.Size(242, 253);
   this.gb_parametry.ResumeLayout(false);
   this.gb_parametry.PerformLayout();
   this.gb_okresy.ResumeLayout(false);
   this.gb_okresy.PerformLayout();
   this.ResumeLayout(false);

  }

  #endregion

  private System.Windows.Forms.Timer zegar_;
  private System.Windows.Forms.Label lb_wlkT;
  private System.Windows.Forms.Label lb_jdnfi0;
  private System.Windows.Forms.TextBox tb_wlkT;
  private System.Windows.Forms.Label lb_jdnl;
  private System.Windows.Forms.Label lb_jdng;
  private System.Windows.Forms.Label lb_jdnTo;
  private System.Windows.Forms.GroupBox gb_parametry;
  private System.Windows.Forms.Label lb_wlkl;
  private System.Windows.Forms.TextBox tb_wlkfi0;
  private System.Windows.Forms.Label lb_wlkfi0;
  private System.Windows.Forms.TextBox tb_wlkl;
  private System.Windows.Forms.Label lb_wlkg;
  private System.Windows.Forms.TextBox tb_wlkg;
  private System.Windows.Forms.TextBox tb_wlkTo;
  private System.Windows.Forms.Label lb_wlkTo;
  private System.Windows.Forms.Button bt_start;
  private System.Windows.Forms.Button bt_stop;
  private System.Windows.Forms.Label lb_jdnT;
  private System.Windows.Forms.GroupBox gb_okresy;
 }
}
