namespace ZilchCheats
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
            this.label1 = new System.Windows.Forms.Label();
            this.EnableBunnyhop = new System.Windows.Forms.CheckBox();
            this.EnableTrigger = new System.Windows.Forms.CheckBox();
            this.EnableWall = new System.Windows.Forms.CheckBox();
            this.EnableAim = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "ZilchCheats v0.1";
            // 
            // EnableBunnyhop
            // 
            this.EnableBunnyhop.AutoSize = true;
            this.EnableBunnyhop.Location = new System.Drawing.Point(17, 405);
            this.EnableBunnyhop.Name = "EnableBunnyhop";
            this.EnableBunnyhop.Size = new System.Drawing.Size(110, 17);
            this.EnableBunnyhop.TabIndex = 1;
            this.EnableBunnyhop.Text = "Enable Bunnyhop";
            this.EnableBunnyhop.UseVisualStyleBackColor = true;
            this.EnableBunnyhop.CheckedChanged += new System.EventHandler(this.EnableBunnyhop_CheckedChanged);
            // 
            // EnableTrigger
            // 
            this.EnableTrigger.AutoSize = true;
            this.EnableTrigger.Location = new System.Drawing.Point(17, 382);
            this.EnableTrigger.Name = "EnableTrigger";
            this.EnableTrigger.Size = new System.Drawing.Size(107, 17);
            this.EnableTrigger.TabIndex = 2;
            this.EnableTrigger.Text = "EnableTriggerbot";
            this.EnableTrigger.UseVisualStyleBackColor = true;
            this.EnableTrigger.CheckedChanged += new System.EventHandler(this.EnableTrigger_CheckedChanged);
            // 
            // EnableWall
            // 
            this.EnableWall.AutoSize = true;
            this.EnableWall.Location = new System.Drawing.Point(17, 359);
            this.EnableWall.Name = "EnableWall";
            this.EnableWall.Size = new System.Drawing.Size(107, 17);
            this.EnableWall.TabIndex = 3;
            this.EnableWall.Text = "Enable Wallhack";
            this.EnableWall.UseVisualStyleBackColor = true;
            this.EnableWall.CheckedChanged += new System.EventHandler(this.EnableWall_CheckedChanged);
            // 
            // EnableAim
            // 
            this.EnableAim.AutoSize = true;
            this.EnableAim.Location = new System.Drawing.Point(17, 336);
            this.EnableAim.Name = "EnableAim";
            this.EnableAim.Size = new System.Drawing.Size(94, 17);
            this.EnableAim.TabIndex = 4;
            this.EnableAim.Text = "Enable Aimbot";
            this.EnableAim.UseVisualStyleBackColor = true;
            this.EnableAim.CheckedChanged += new System.EventHandler(this.EnableAim_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EnableAim);
            this.Controls.Add(this.EnableWall);
            this.Controls.Add(this.EnableTrigger);
            this.Controls.Add(this.EnableBunnyhop);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox EnableBunnyhop;
        private System.Windows.Forms.CheckBox EnableTrigger;
        private System.Windows.Forms.CheckBox EnableWall;
        private System.Windows.Forms.CheckBox EnableAim;
    }
}

