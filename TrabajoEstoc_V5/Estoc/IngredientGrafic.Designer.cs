namespace Estoc
{
    partial class IngredientGrafic
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudQuantitat = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCmbProductes = new LabelComboBox();
            this.lblCmbReceptes = new LabelComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantitat)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 47);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IdProducte:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "IdRecepta:";
            // 
            // nudQuantitat
            // 
            this.nudQuantitat.DecimalPlaces = 2;
            this.nudQuantitat.Location = new System.Drawing.Point(245, 31);
            this.nudQuantitat.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudQuantitat.Name = "nudQuantitat";
            this.nudQuantitat.Size = new System.Drawing.Size(64, 20);
            this.nudQuantitat.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Quantitat";
            // 
            // lblCmbProductes
            // 
            this.lblCmbProductes.EsPotModificar = true;
            this.lblCmbProductes.LlistaObjectes = new object[0];
            this.lblCmbProductes.Location = new System.Drawing.Point(138, 15);
            this.lblCmbProductes.Name = "lblCmbProductes";
            this.lblCmbProductes.ObjecteSeleccionat = null;
            this.lblCmbProductes.Size = new System.Drawing.Size(0, 13);
            this.lblCmbProductes.TabIndex = 7;
            // 
            // lblCmbReceptes
            // 
            this.lblCmbReceptes.EsPotModificar = false;
            this.lblCmbReceptes.LlistaObjectes = new object[0];
            this.lblCmbReceptes.Location = new System.Drawing.Point(136, 38);
            this.lblCmbReceptes.Name = "lblCmbReceptes";
            this.lblCmbReceptes.ObjecteSeleccionat = null;
            this.lblCmbReceptes.Size = new System.Drawing.Size(0, 13);
            this.lblCmbReceptes.TabIndex = 8;
            // 
            // IngredientGrafic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCmbReceptes);
            this.Controls.Add(this.lblCmbProductes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudQuantitat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "IngredientGrafic";
            this.Size = new System.Drawing.Size(328, 67);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantitat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudQuantitat;
        private System.Windows.Forms.Label label3;
        private LabelComboBox lblCmbProductes;
        private LabelComboBox lblCmbReceptes;
    }
}
