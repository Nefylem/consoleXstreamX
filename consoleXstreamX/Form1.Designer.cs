﻿namespace consoleXstreamX
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
            this.components = new System.ComponentModel.Container();
            this.display = new System.Windows.Forms.PictureBox();
            this.wait = new System.Windows.Forms.PictureBox();
            this.system = new System.Windows.Forms.Timer(this.components);
            this.controlPrimary = new System.Windows.Forms.Timer(this.components);
            this.controlSecondary = new System.Windows.Forms.Timer(this.components);
            this.controlTertiary = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.display)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wait)).BeginInit();
            this.SuspendLayout();
            // 
            // display
            // 
            this.display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.display.Location = new System.Drawing.Point(22, 29);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(270, 50);
            this.display.TabIndex = 0;
            this.display.TabStop = false;
            // 
            // wait
            // 
            this.wait.Location = new System.Drawing.Point(12, 106);
            this.wait.Name = "wait";
            this.wait.Size = new System.Drawing.Size(100, 50);
            this.wait.TabIndex = 3;
            this.wait.TabStop = false;
            this.wait.Visible = false;
            // 
            // system
            // 
            this.system.Interval = 1;
            this.system.Tick += new System.EventHandler(this.system_Tick);
            // 
            // controlPrimary
            // 
            this.controlPrimary.Interval = 1;
            this.controlPrimary.Tick += new System.EventHandler(this.controlPrimary_Tick);
            // 
            // controlSecondary
            // 
            this.controlSecondary.Interval = 1;
            this.controlSecondary.Tick += new System.EventHandler(this.controlSecondary_Tick);
            // 
            // controlTertiary
            // 
            this.controlTertiary.Tick += new System.EventHandler(this.controlTertiary_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 261);
            this.Controls.Add(this.wait);
            this.Controls.Add(this.display);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.display)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wait)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox display;
        public System.Windows.Forms.PictureBox wait;
        private System.Windows.Forms.Timer system;
        private System.Windows.Forms.Timer controlPrimary;
        private System.Windows.Forms.Timer controlSecondary;
        private System.Windows.Forms.Timer controlTertiary;
    }
}

