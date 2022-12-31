namespace B3
{
    partial class B3Config
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
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.NumBalls = new System.Windows.Forms.TextBox();
            this.Radius = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MinVel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MaxVel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(121, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(121, 259);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 46);
            this.button2.TabIndex = 1;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of Balls";
            // 
            // NumBalls
            // 
            this.NumBalls.Location = new System.Drawing.Point(151, 12);
            this.NumBalls.MaxLength = 3;
            this.NumBalls.Name = "NumBalls";
            this.NumBalls.Size = new System.Drawing.Size(57, 23);
            this.NumBalls.TabIndex = 3;
            this.NumBalls.TextChanged += new System.EventHandler(this.NumBalls_TextChanged);
            // 
            // Radius
            // 
            this.Radius.Location = new System.Drawing.Point(151, 57);
            this.Radius.MaxLength = 3;
            this.Radius.Name = "Radius";
            this.Radius.Size = new System.Drawing.Size(57, 23);
            this.Radius.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(70, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Raidus (px)";
            // 
            // MinVel
            // 
            this.MinVel.Location = new System.Drawing.Point(151, 104);
            this.MinVel.MaxLength = 3;
            this.MinVel.Name = "MinVel";
            this.MinVel.Size = new System.Drawing.Size(57, 23);
            this.MinVel.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Velocity Range";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(223, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "to";
            // 
            // MaxVel
            // 
            this.MaxVel.Location = new System.Drawing.Point(247, 104);
            this.MaxVel.MaxLength = 3;
            this.MaxVel.Name = "MaxVel";
            this.MaxVel.Size = new System.Drawing.Size(57, 23);
            this.MaxVel.TabIndex = 9;
            // 
            // B3Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 328);
            this.Controls.Add(this.MaxVel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.MinVel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Radius);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NumBalls);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "B3Config";
            this.Text = "B3 Config";
            this.Load += new System.EventHandler(this.B3Config_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private TextBox NumBalls;
        private TextBox Radius;
        private Label label2;
        private TextBox MinVel;
        private Label label3;
        private Label label4;
        private TextBox MaxVel;
    }
}