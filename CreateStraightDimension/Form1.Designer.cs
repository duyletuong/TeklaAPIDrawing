namespace CreateStraightDimension
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
            this.btnCreateDim = new System.Windows.Forms.Button();
            this.txtDimDistance = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCreateDim
            // 
            this.btnCreateDim.Location = new System.Drawing.Point(346, 250);
            this.btnCreateDim.Name = "btnCreateDim";
            this.btnCreateDim.Size = new System.Drawing.Size(154, 56);
            this.btnCreateDim.TabIndex = 0;
            this.btnCreateDim.Text = "Create dimension";
            this.btnCreateDim.UseVisualStyleBackColor = true;
            this.btnCreateDim.Click += new System.EventHandler(this.btnCreateDim_Click);
            // 
            // txtDimDistance
            // 
            this.txtDimDistance.Location = new System.Drawing.Point(102, 30);
            this.txtDimDistance.Name = "txtDimDistance";
            this.txtDimDistance.Size = new System.Drawing.Size(116, 20);
            this.txtDimDistance.TabIndex = 1;
            this.txtDimDistance.Text = "200";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Distance";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 318);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDimDistance);
            this.Controls.Add(this.btnCreateDim);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Autodim vjp pro";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateDim;
        private System.Windows.Forms.TextBox txtDimDistance;
        private System.Windows.Forms.Label label1;
    }
}

