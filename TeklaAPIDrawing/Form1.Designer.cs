namespace TeklaAPIDrawing
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
            this.btnRun = new System.Windows.Forms.Button();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStartNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPostfix = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbWriteTo = new System.Windows.Forms.ComboBox();
            this.txtDigits = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.Color.Fuchsia;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Location = new System.Drawing.Point(429, 226);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(112, 39);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(138, 18);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(181, 20);
            this.txtPrefix.TabIndex = 1;
            this.txtPrefix.Text = "A";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(533, 208);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbWriteTo);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtPostfix);
            this.tabPage1.Controls.Add(this.txtDigits);
            this.tabPage1.Controls.Add(this.txtStartNumber);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtPrefix);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(525, 182);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Numbering setting";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(525, 257);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Prefix";
            // 
            // txtStartNumber
            // 
            this.txtStartNumber.Location = new System.Drawing.Point(138, 49);
            this.txtStartNumber.Name = "txtStartNumber";
            this.txtStartNumber.Size = new System.Drawing.Size(181, 20);
            this.txtStartNumber.TabIndex = 1;
            this.txtStartNumber.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start number";
            // 
            // txtPostfix
            // 
            this.txtPostfix.Location = new System.Drawing.Point(137, 82);
            this.txtPostfix.Name = "txtPostfix";
            this.txtPostfix.Size = new System.Drawing.Size(181, 20);
            this.txtPostfix.TabIndex = 1;
            this.txtPostfix.Text = "B";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Postfix";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Write to";
            // 
            // cbWriteTo
            // 
            this.cbWriteTo.FormattingEnabled = true;
            this.cbWriteTo.Items.AddRange(new object[] {
            "NAME",
            "Title1",
            "Title2",
            "Title3",
            "Userfield1"});
            this.cbWriteTo.Location = new System.Drawing.Point(138, 114);
            this.cbWriteTo.Name = "cbWriteTo";
            this.cbWriteTo.Size = new System.Drawing.Size(180, 21);
            this.cbWriteTo.TabIndex = 3;
            // 
            // txtDigits
            // 
            this.txtDigits.Location = new System.Drawing.Point(416, 49);
            this.txtDigits.Name = "txtDigits";
            this.txtDigits.Size = new System.Drawing.Size(62, 20);
            this.txtDigits.TabIndex = 1;
            this.txtDigits.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(377, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Digits";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 273);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Danh so ban ve";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox cbWriteTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPostfix;
        private System.Windows.Forms.TextBox txtDigits;
        private System.Windows.Forms.TextBox txtStartNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage4;
    }
}

