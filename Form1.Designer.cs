namespace Pong
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 600);
            this.Name = "Form1";
            this.Text = "Form1";
            this.BackColor = System.Drawing.Color.FromArgb(37, 37, 38);
            this.ForeColor = System.Drawing.Color.FromArgb(204, 204, 204);
            // 
            // pictureBox1
            // 
            //this.pictureBox1 = new System.Windows.Forms.PictureBox();
            //this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            //this.pictureBox1.Name = "pictureBox1";
            //this.pictureBox1.Size = new System.Drawing.Size(800, 600);
            //this.pictureBox1.TabIndex = 0;
            //this.pictureBox1.ForeColor = System.Drawing.Color.FromArgb(204, 204, 204);
            //this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            //this.pictureBox1.Text = "PictureBox";
            //this.Controls.Add(this.pictureBox1);
            // 
            // buttonHost
            // 
            this.buttonHost = new System.Windows.Forms.Button();
            this.buttonHost.Location = new System.Drawing.Point(20, 50);
            this.buttonHost.Name = "buttonHost";
            this.buttonHost.Size = new System.Drawing.Size(100, 30);
            this.buttonHost.TabIndex = 0;
            this.buttonHost.ForeColor = System.Drawing.Color.FromArgb(204, 204, 204);
            this.buttonHost.BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.buttonHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHost.Text = "start host";
            this.Controls.Add(this.buttonHost);
            // 
            // buttonLeft
            // 
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonLeft.Location = new System.Drawing.Point(20, 100);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(100, 30);
            this.buttonLeft.TabIndex = 0;
            this.buttonLeft.ForeColor = System.Drawing.Color.FromArgb(204, 204, 204);
            this.buttonLeft.BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.buttonLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeft.Text = "join left";
            this.Controls.Add(this.buttonLeft);
            // 
            // buttonRight
            // 
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonRight.Location = new System.Drawing.Point(20, 150);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(100, 30);
            this.buttonRight.TabIndex = 0;
            this.buttonRight.ForeColor = System.Drawing.Color.FromArgb(204, 204, 204);
            this.buttonRight.BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
            this.buttonRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRight.Text = "join right";
            this.Controls.Add(this.buttonRight);
            // 
            // Form controls collection
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                //this.pictureBox1,
                this.buttonHost,
                this.buttonLeft,
                this.buttonRight
            });
            this.ResumeLayout(false);
        }

        #endregion

        // Control declarations
        //private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonHost;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonRight;
    }
}