namespace BuiBuiAPI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelSide = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.listHistories = new System.Windows.Forms.ListBox();
            this.tabFavorite = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckbDock = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabParams = new System.Windows.Forms.TabPage();
            this.tabHeaders = new System.Windows.Forms.TabPage();
            this.tabRequest = new System.Windows.Forms.TabPage();
            this.tabResponse = new System.Windows.Forms.TabPage();
            this.tabLogs = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGO = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbbHttpMethod = new System.Windows.Forms.ComboBox();
            this.tipHistory = new System.Windows.Forms.ToolTip(this.components);
            this.panelSide.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabHistory.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSide
            // 
            this.panelSide.BackColor = System.Drawing.SystemColors.Control;
            this.panelSide.Controls.Add(this.tabControl2);
            this.panelSide.Controls.Add(this.panel1);
            this.panelSide.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSide.Location = new System.Drawing.Point(679, 0);
            this.panelSide.Name = "panelSide";
            this.panelSide.Size = new System.Drawing.Size(200, 622);
            this.panelSide.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabHistory);
            this.tabControl2.Controls.Add(this.tabFavorite);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 52);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(200, 570);
            this.tabControl2.TabIndex = 3;
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.listHistories);
            this.tabHistory.Location = new System.Drawing.Point(4, 22);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabHistory.Size = new System.Drawing.Size(192, 544);
            this.tabHistory.TabIndex = 0;
            this.tabHistory.Text = "历史";
            this.tabHistory.UseVisualStyleBackColor = true;
            // 
            // listHistories
            // 
            this.listHistories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listHistories.FormattingEnabled = true;
            this.listHistories.IntegralHeight = false;
            this.listHistories.ItemHeight = 12;
            this.listHistories.Location = new System.Drawing.Point(3, 3);
            this.listHistories.Name = "listHistories";
            this.listHistories.Size = new System.Drawing.Size(186, 538);
            this.listHistories.TabIndex = 0;
            this.tipHistory.SetToolTip(this.listHistories, "11122233\r\n");
            this.listHistories.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listHistories_MouseMove);
            // 
            // tabFavorite
            // 
            this.tabFavorite.Location = new System.Drawing.Point(4, 22);
            this.tabFavorite.Name = "tabFavorite";
            this.tabFavorite.Padding = new System.Windows.Forms.Padding(3);
            this.tabFavorite.Size = new System.Drawing.Size(192, 544);
            this.tabFavorite.TabIndex = 1;
            this.tabFavorite.Text = "收藏";
            this.tabFavorite.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckbDock);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 52);
            this.panel1.TabIndex = 4;
            // 
            // ckbDock
            // 
            this.ckbDock.AutoSize = true;
            this.ckbDock.Location = new System.Drawing.Point(16, 18);
            this.ckbDock.Name = "ckbDock";
            this.ckbDock.Size = new System.Drawing.Size(48, 16);
            this.ckbDock.TabIndex = 0;
            this.ckbDock.Text = "靠左";
            this.ckbDock.UseVisualStyleBackColor = true;
            this.ckbDock.CheckedChanged += new System.EventHandler(this.ckbDock_CheckedChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(145, 17);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(52, 21);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown1.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "历史";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabParams);
            this.tabControl1.Controls.Add(this.tabHeaders);
            this.tabControl1.Controls.Add(this.tabRequest);
            this.tabControl1.Controls.Add(this.tabResponse);
            this.tabControl1.Controls.Add(this.tabLogs);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(679, 570);
            this.tabControl1.TabIndex = 8;
            // 
            // tabParams
            // 
            this.tabParams.Location = new System.Drawing.Point(4, 22);
            this.tabParams.Name = "tabParams";
            this.tabParams.Padding = new System.Windows.Forms.Padding(3);
            this.tabParams.Size = new System.Drawing.Size(671, 544);
            this.tabParams.TabIndex = 0;
            this.tabParams.Text = "Params";
            this.tabParams.UseVisualStyleBackColor = true;
            // 
            // tabHeaders
            // 
            this.tabHeaders.Location = new System.Drawing.Point(4, 22);
            this.tabHeaders.Name = "tabHeaders";
            this.tabHeaders.Padding = new System.Windows.Forms.Padding(3);
            this.tabHeaders.Size = new System.Drawing.Size(671, 544);
            this.tabHeaders.TabIndex = 1;
            this.tabHeaders.Text = "Headers";
            this.tabHeaders.UseVisualStyleBackColor = true;
            // 
            // tabRequest
            // 
            this.tabRequest.Location = new System.Drawing.Point(4, 22);
            this.tabRequest.Name = "tabRequest";
            this.tabRequest.Size = new System.Drawing.Size(671, 544);
            this.tabRequest.TabIndex = 2;
            this.tabRequest.Text = "Request";
            this.tabRequest.UseVisualStyleBackColor = true;
            // 
            // tabResponse
            // 
            this.tabResponse.Location = new System.Drawing.Point(4, 22);
            this.tabResponse.Name = "tabResponse";
            this.tabResponse.Size = new System.Drawing.Size(671, 544);
            this.tabResponse.TabIndex = 3;
            this.tabResponse.Text = "Response";
            this.tabResponse.UseVisualStyleBackColor = true;
            // 
            // tabLogs
            // 
            this.tabLogs.Location = new System.Drawing.Point(4, 22);
            this.tabLogs.Name = "tabLogs";
            this.tabLogs.Size = new System.Drawing.Size(671, 544);
            this.tabLogs.TabIndex = 4;
            this.tabLogs.Text = "Logs";
            this.tabLogs.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnGO);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.cbbHttpMethod);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(679, 52);
            this.panel2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL:";
            // 
            // btnGO
            // 
            this.btnGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGO.Location = new System.Drawing.Point(580, 15);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(75, 23);
            this.btnGO.TabIndex = 3;
            this.btnGO.Text = "GO";
            this.btnGO.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(48, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(426, 21);
            this.textBox1.TabIndex = 1;
            // 
            // cbbHttpMethod
            // 
            this.cbbHttpMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbHttpMethod.FormattingEnabled = true;
            this.cbbHttpMethod.Location = new System.Drawing.Point(480, 16);
            this.cbbHttpMethod.Name = "cbbHttpMethod";
            this.cbbHttpMethod.Size = new System.Drawing.Size(94, 20);
            this.cbbHttpMethod.TabIndex = 2;
            // 
            // tipHistory
            // 
            this.tipHistory.ToolTipTitle = "11111111111";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 622);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelSide);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelSide.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabHistory.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSide;
        private System.Windows.Forms.CheckBox ckbDock;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabParams;
        private System.Windows.Forms.TabPage tabHeaders;
        private System.Windows.Forms.TabPage tabRequest;
        private System.Windows.Forms.TabPage tabResponse;
        private System.Windows.Forms.TabPage tabLogs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox cbbHttpMethod;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.TabPage tabFavorite;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listHistories;
        private System.Windows.Forms.ToolTip tipHistory;
    }
}

