namespace BovespaBot
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkDados = new System.Windows.Forms.CheckBox();
            this.chkValores = new System.Windows.Forms.CheckBox();
            this.chkRelatorios = new System.Windows.Forms.CheckBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkDados
            // 
            this.chkDados.AutoSize = true;
            this.chkDados.Checked = true;
            this.chkDados.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDados.Location = new System.Drawing.Point(29, 27);
            this.chkDados.Name = "chkDados";
            this.chkDados.Size = new System.Drawing.Size(126, 17);
            this.chkDados.TabIndex = 0;
            this.chkDados.Text = "Dados das Empresas";
            this.chkDados.UseVisualStyleBackColor = true;
            // 
            // chkValores
            // 
            this.chkValores.AutoSize = true;
            this.chkValores.Checked = true;
            this.chkValores.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkValores.Location = new System.Drawing.Point(29, 51);
            this.chkValores.Name = "chkValores";
            this.chkValores.Size = new System.Drawing.Size(109, 17);
            this.chkValores.TabIndex = 1;
            this.chkValores.Text = "Valores de Ações";
            this.chkValores.UseVisualStyleBackColor = true;
            // 
            // chkRelatorios
            // 
            this.chkRelatorios.AutoSize = true;
            this.chkRelatorios.Checked = true;
            this.chkRelatorios.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRelatorios.Location = new System.Drawing.Point(29, 75);
            this.chkRelatorios.Name = "chkRelatorios";
            this.chkRelatorios.Size = new System.Drawing.Size(130, 17);
            this.chkRelatorios.TabIndex = 2;
            this.chkRelatorios.Text = "Relatórios Financeiros";
            this.chkRelatorios.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(213, 51);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(149, 41);
            this.btnBuscar.TabIndex = 3;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 139);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.chkRelatorios);
            this.Controls.Add(this.chkValores);
            this.Controls.Add(this.chkDados);
            this.Name = "Form1";
            this.Text = "BovespaBot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDados;
        private System.Windows.Forms.CheckBox chkValores;
        private System.Windows.Forms.CheckBox chkRelatorios;
        private System.Windows.Forms.Button btnBuscar;
    }
}

