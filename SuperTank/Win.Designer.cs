namespace SuperTank
{
    partial class Win
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Win));
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkrank_bt = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.checkrank_bt);
            this.panel1.Location = new System.Drawing.Point(2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(740, 572);
            this.panel1.TabIndex = 0;
            // 
            // checkrank_bt
            // 
            this.checkrank_bt.Location = new System.Drawing.Point(288, 461);
            this.checkrank_bt.Name = "checkrank_bt";
            this.checkrank_bt.Size = new System.Drawing.Size(181, 53);
            this.checkrank_bt.TabIndex = 1;
            this.checkrank_bt.Text = "Xem hạng ";
            this.checkrank_bt.UseVisualStyleBackColor = true;
            this.checkrank_bt.Click += new System.EventHandler(this.checkrank_bt_Click);
            // 
            // Win
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 576);
            this.Controls.Add(this.panel1);
            this.Name = "Win";
            this.Text = "Win";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button checkrank_bt;
    }
}