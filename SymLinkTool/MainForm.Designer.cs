
namespace SymLinkTool
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnOpenSource = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.chkSource = new System.Windows.Forms.CheckedListBox();
            this.btnOpenTarget = new System.Windows.Forms.Button();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbOutput = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.fdbMain = new System.Windows.Forms.FolderBrowserDialog();
            this.bntChangeTheme = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTargetBackground = new System.Windows.Forms.Label();
            this.lsTarget = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 67);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtSource);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btnOpenSource);
            this.splitContainer1.Panel1.Controls.Add(this.chkSource);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lsTarget);
            this.splitContainer1.Panel2.Controls.Add(this.txtTarget);
            this.splitContainer1.Panel2.Controls.Add(this.btnOpenTarget);
            this.splitContainer1.Panel2.Controls.Add(this.lblTargetBackground);
            this.splitContainer1.Size = new System.Drawing.Size(766, 512);
            this.splitContainer1.SplitterDistance = 358;
            this.splitContainer1.TabIndex = 999;
            this.splitContainer1.TabStop = false;
            // 
            // btnOpenSource
            // 
            this.btnOpenSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenSource.AutoEllipsis = true;
            this.btnOpenSource.BackColor = System.Drawing.SystemColors.Control;
            this.btnOpenSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenSource.FlatAppearance.BorderSize = 0;
            this.btnOpenSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSource.Font = new System.Drawing.Font("Consolas", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSource.Location = new System.Drawing.Point(336, 3);
            this.btnOpenSource.Name = "btnOpenSource";
            this.btnOpenSource.Size = new System.Drawing.Size(20, 20);
            this.btnOpenSource.TabIndex = 1;
            this.btnOpenSource.Text = "...";
            this.btnOpenSource.UseVisualStyleBackColor = false;
            this.btnOpenSource.Click += new System.EventHandler(this.btnOpenSource_Click);
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.BackColor = System.Drawing.SystemColors.Control;
            this.txtSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSource.Location = new System.Drawing.Point(6, 7);
            this.txtSource.Margin = new System.Windows.Forms.Padding(0);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(324, 12);
            this.txtSource.TabIndex = 2;
            this.txtSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSource_KeyDown);
            // 
            // chkSource
            // 
            this.chkSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkSource.CheckOnClick = true;
            this.chkSource.Enabled = false;
            this.chkSource.FormattingEnabled = true;
            this.chkSource.Location = new System.Drawing.Point(3, 32);
            this.chkSource.Name = "chkSource";
            this.chkSource.Size = new System.Drawing.Size(354, 480);
            this.chkSource.TabIndex = 6;
            this.chkSource.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkSource_ItemCheck);
            this.chkSource.SelectedIndexChanged += new System.EventHandler(this.chkSource_SelectedIndexChanged);
            // 
            // btnOpenTarget
            // 
            this.btnOpenTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenTarget.AutoEllipsis = true;
            this.btnOpenTarget.BackColor = System.Drawing.SystemColors.Control;
            this.btnOpenTarget.Enabled = false;
            this.btnOpenTarget.FlatAppearance.BorderSize = 0;
            this.btnOpenTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenTarget.Font = new System.Drawing.Font("Consolas", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenTarget.Location = new System.Drawing.Point(381, 3);
            this.btnOpenTarget.Name = "btnOpenTarget";
            this.btnOpenTarget.Size = new System.Drawing.Size(20, 20);
            this.btnOpenTarget.TabIndex = 4;
            this.btnOpenTarget.Text = "...";
            this.btnOpenTarget.UseVisualStyleBackColor = false;
            this.btnOpenTarget.Click += new System.EventHandler(this.btnOpenTarget_Click);
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTarget.BackColor = System.Drawing.SystemColors.Control;
            this.txtTarget.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTarget.Enabled = false;
            this.txtTarget.Location = new System.Drawing.Point(9, 7);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(366, 12);
            this.txtTarget.TabIndex = 3;
            this.txtTarget.EnabledChanged += new System.EventHandler(this.txtTarget_EnabledChanged);
            this.txtTarget.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTarget_KeyDown);
            // 
            // pbMain
            // 
            this.pbMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMain.Location = new System.Drawing.Point(43, 592);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(735, 23);
            this.pbMain.TabIndex = 99;
            this.pbMain.Visible = false;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnStart.Enabled = false;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(656, 11);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(58, 49);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lbOutput
            // 
            this.lbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOutput.BackColor = System.Drawing.SystemColors.Control;
            this.lbOutput.Location = new System.Drawing.Point(12, 11);
            this.lbOutput.Name = "lbOutput";
            this.lbOutput.Size = new System.Drawing.Size(638, 49);
            this.lbOutput.TabIndex = 0;
            this.lbOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.Enabled = false;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(720, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(58, 49);
            this.btnCancel.TabIndex = 1000;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // fdbMain
            // 
            this.fdbMain.Description = "Directorio origen";
            // 
            // bntChangeTheme
            // 
            this.bntChangeTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bntChangeTheme.AutoEllipsis = true;
            this.bntChangeTheme.BackColor = System.Drawing.SystemColors.Control;
            this.bntChangeTheme.BackgroundImage = global::SymLinkTool.Properties.Resources.darkmode;
            this.bntChangeTheme.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bntChangeTheme.FlatAppearance.BorderSize = 0;
            this.bntChangeTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntChangeTheme.Font = new System.Drawing.Font("Consolas", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntChangeTheme.Location = new System.Drawing.Point(12, 592);
            this.bntChangeTheme.Name = "bntChangeTheme";
            this.bntChangeTheme.Size = new System.Drawing.Size(23, 23);
            this.bntChangeTheme.TabIndex = 8;
            this.bntChangeTheme.UseVisualStyleBackColor = false;
            this.bntChangeTheme.Click += new System.EventHandler(this.bntChangeTheme_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 20);
            this.label1.TabIndex = 9999;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTargetBackground
            // 
            this.lblTargetBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTargetBackground.BackColor = System.Drawing.SystemColors.Control;
            this.lblTargetBackground.Enabled = false;
            this.lblTargetBackground.Location = new System.Drawing.Point(6, 3);
            this.lblTargetBackground.Name = "lblTargetBackground";
            this.lblTargetBackground.Size = new System.Drawing.Size(369, 20);
            this.lblTargetBackground.TabIndex = 99999;
            this.lblTargetBackground.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lsTarget
            // 
            this.lsTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsTarget.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsTarget.Enabled = false;
            this.lsTarget.FormattingEnabled = true;
            this.lsTarget.Location = new System.Drawing.Point(6, 32);
            this.lsTarget.Name = "lsTarget";
            this.lsTarget.Size = new System.Drawing.Size(395, 481);
            this.lsTarget.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(790, 627);
            this.Controls.Add(this.bntChangeTheme);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbOutput);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pbMain);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "SymLink Tool";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox chkSource;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lbOutput;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOpenSource;
        private System.Windows.Forms.Button btnOpenTarget;
        private System.Windows.Forms.FolderBrowserDialog fdbMain;
        private System.Windows.Forms.Button bntChangeTheme;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTargetBackground;
        private System.Windows.Forms.ListBox lsTarget;
    }
}

