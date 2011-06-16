namespace HellevatorEmulator
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
            if(disposing && (components != null))
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
            this.motorUpButton = new System.Windows.Forms.Button();
            this.motorDownButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // motorUpButton
            // 
            this.motorUpButton.Location = new System.Drawing.Point(12, 12);
            this.motorUpButton.Name = "motorUpButton";
            this.motorUpButton.Size = new System.Drawing.Size(75, 23);
            this.motorUpButton.TabIndex = 0;
            this.motorUpButton.Text = "Up";
            this.motorUpButton.UseVisualStyleBackColor = true;
            this.motorUpButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.motorUpButton_MouseDown);
            this.motorUpButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.motorUpButton_MouseUp);
            // 
            // motorDownButton
            // 
            this.motorDownButton.Location = new System.Drawing.Point(13, 42);
            this.motorDownButton.Name = "motorDownButton";
            this.motorDownButton.Size = new System.Drawing.Size(75, 23);
            this.motorDownButton.TabIndex = 1;
            this.motorDownButton.Text = "Down";
            this.motorDownButton.UseVisualStyleBackColor = true;
            this.motorDownButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.motorDownButton_MouseDown);
            this.motorDownButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.motorDownButton_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.motorDownButton);
            this.Controls.Add(this.motorUpButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button motorUpButton;
        private System.Windows.Forms.Button motorDownButton;
    }
}

