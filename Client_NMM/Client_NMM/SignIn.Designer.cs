namespace Client_NMM
{
    partial class SignIn
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
            this.signB = new System.Windows.Forms.Button();
            this.repeatTB = new System.Windows.Forms.TextBox();
            this.passTB = new System.Windows.Forms.TextBox();
            this.nameTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // signB
            // 
            this.signB.Location = new System.Drawing.Point(364, 251);
            this.signB.Name = "signB";
            this.signB.Size = new System.Drawing.Size(133, 49);
            this.signB.TabIndex = 22;
            this.signB.Text = "Sign in";
            this.signB.UseVisualStyleBackColor = true;
            this.signB.Click += new System.EventHandler(this.signB_Click);
            // 
            // repeatTB
            // 
            this.repeatTB.Location = new System.Drawing.Point(181, 191);
            this.repeatTB.Name = "repeatTB";
            this.repeatTB.Size = new System.Drawing.Size(316, 22);
            this.repeatTB.TabIndex = 21;
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(181, 122);
            this.passTB.Name = "passTB";
            this.passTB.Size = new System.Drawing.Size(316, 22);
            this.passTB.TabIndex = 20;
            // 
            // nameTB
            // 
            this.nameTB.Location = new System.Drawing.Point(181, 52);
            this.nameTB.Name = "nameTB";
            this.nameTB.Size = new System.Drawing.Size(316, 22);
            this.nameTB.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Repeat Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "User name:";
            // 
            // SignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 338);
            this.Controls.Add(this.signB);
            this.Controls.Add(this.repeatTB);
            this.Controls.Add(this.passTB);
            this.Controls.Add(this.nameTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SignIn";
            this.Text = "SignIn";
            this.Load += new System.EventHandler(this.SignIn_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button signB;
        private System.Windows.Forms.TextBox repeatTB;
        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.TextBox nameTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}