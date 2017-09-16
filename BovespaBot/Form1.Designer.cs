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
            this.chkRelatorios = new System.Windows.Forms.CheckBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.chkListEmpresas = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSelecionarTodas = new System.Windows.Forms.CheckBox();
            this.lblAtualizada = new System.Windows.Forms.Label();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.ddlPeriodo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkDados
            // 
            this.chkDados.AutoSize = true;
            this.chkDados.Checked = true;
            this.chkDados.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDados.Location = new System.Drawing.Point(339, 229);
            this.chkDados.Name = "chkDados";
            this.chkDados.Size = new System.Drawing.Size(126, 17);
            this.chkDados.TabIndex = 0;
            this.chkDados.Text = "Dados das Empresas";
            this.chkDados.UseVisualStyleBackColor = true;
            this.chkDados.CheckedChanged += new System.EventHandler(this.chkDados_CheckedChanged);
            // 
            // chkRelatorios
            // 
            this.chkRelatorios.AutoSize = true;
            this.chkRelatorios.Checked = true;
            this.chkRelatorios.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRelatorios.Location = new System.Drawing.Point(339, 252);
            this.chkRelatorios.Name = "chkRelatorios";
            this.chkRelatorios.Size = new System.Drawing.Size(130, 17);
            this.chkRelatorios.TabIndex = 2;
            this.chkRelatorios.Text = "Relatórios Financeiros";
            this.chkRelatorios.UseVisualStyleBackColor = true;
            this.chkRelatorios.CheckedChanged += new System.EventHandler(this.chkRelatorios_CheckedChanged);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(339, 315);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(149, 41);
            this.btnBuscar.TabIndex = 3;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // chkListEmpresas
            // 
            this.chkListEmpresas.CheckOnClick = true;
            this.chkListEmpresas.FormattingEnabled = true;
            this.chkListEmpresas.Location = new System.Drawing.Point(12, 67);
            this.chkListEmpresas.Name = "chkListEmpresas";
            this.chkListEmpresas.Size = new System.Drawing.Size(311, 289);
            this.chkListEmpresas.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Lista de Empresas";
            // 
            // chkSelecionarTodas
            // 
            this.chkSelecionarTodas.AutoSize = true;
            this.chkSelecionarTodas.Location = new System.Drawing.Point(12, 44);
            this.chkSelecionarTodas.Name = "chkSelecionarTodas";
            this.chkSelecionarTodas.Size = new System.Drawing.Size(109, 17);
            this.chkSelecionarTodas.TabIndex = 6;
            this.chkSelecionarTodas.Text = "Selecionar Todas";
            this.chkSelecionarTodas.UseVisualStyleBackColor = true;
            this.chkSelecionarTodas.CheckedChanged += new System.EventHandler(this.chkSelecionarTodas_CheckedChanged);
            // 
            // lblAtualizada
            // 
            this.lblAtualizada.AutoSize = true;
            this.lblAtualizada.Location = new System.Drawing.Point(120, 24);
            this.lblAtualizada.Name = "lblAtualizada";
            this.lblAtualizada.Size = new System.Drawing.Size(0, 13);
            this.lblAtualizada.TabIndex = 7;
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Location = new System.Drawing.Point(339, 67);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(149, 41);
            this.btnAtualizar.TabIndex = 8;
            this.btnAtualizar.Text = "Atualizar Lista de Empresas";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // ddlPeriodo
            // 
            this.ddlPeriodo.FormattingEnabled = true;
            this.ddlPeriodo.Items.AddRange(new object[] {
            "30/06/2017",
            "31/03/2017",
            "30/09/2016",
            "30/06/2016",
            "31/03/2016",
            "30/09/2015",
            "30/06/2015",
            "31/03/2015",
            "30/09/2014",
            "30/06/2014",
            "31/03/2014",
            "30/09/2013",
            "30/06/2013",
            "31/03/2013",
            "30/09/2012",
            "30/06/2012",
            "31/03/2012",
            "30/09/2011",
            "30/06/2011",
            "31/03/2011"});
            this.ddlPeriodo.Location = new System.Drawing.Point(339, 181);
            this.ddlPeriodo.Name = "ddlPeriodo";
            this.ddlPeriodo.Size = new System.Drawing.Size(121, 21);
            this.ddlPeriodo.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Trimestre";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 368);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ddlPeriodo);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.lblAtualizada);
            this.Controls.Add(this.chkSelecionarTodas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkListEmpresas);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.chkRelatorios);
            this.Controls.Add(this.chkDados);
            this.Name = "Form1";
            this.Text = "BovespaBot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDados;
        private System.Windows.Forms.CheckBox chkRelatorios;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.CheckedListBox chkListEmpresas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSelecionarTodas;
        private System.Windows.Forms.Label lblAtualizada;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.ComboBox ddlPeriodo;
        private System.Windows.Forms.Label label2;
    }
}

