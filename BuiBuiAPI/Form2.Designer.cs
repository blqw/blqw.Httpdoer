namespace BuiBuiAPI
{
    partial class Form2
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
            this.button1 = new System.Windows.Forms.Button();
            this.cbbLocation = new System.Windows.Forms.ComboBox();
            this.txtParamString = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(346, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbbMethod
            // 
            this.cbbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbLocation.FormattingEnabled = true;
            this.cbbLocation.Items.AddRange(new object[] {
            "Auto",
            "Get",
            "Post"});
            this.cbbLocation.Location = new System.Drawing.Point(212, 216);
            this.cbbLocation.Name = "cbbMethod";
            this.cbbLocation.Size = new System.Drawing.Size(121, 23);
            this.cbbLocation.TabIndex = 1;
            // 
            // txtParamString
            // 
            this.txtParamString.Location = new System.Drawing.Point(12, 12);
            this.txtParamString.Multiline = true;
            this.txtParamString.Name = "txtParamString";
            this.txtParamString.Size = new System.Drawing.Size(642, 185);
            this.txtParamString.TabIndex = 2;
            this.txtParamString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtParamString_KeyDown);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 251);
            this.Controls.Add(this.txtParamString);
            this.Controls.Add(this.cbbLocation);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ComboBox cbbLocation;
        public System.Windows.Forms.TextBox txtParamString;
    }
}