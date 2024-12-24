namespace SuperTank
{
    partial class Lose
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lose));
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
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 617);
            this.panel1.TabIndex = 0;
            // 
            // checkrank_bt
            // 
            this.checkrank_bt.Location = new System.Drawing.Point(316, 511);
            this.checkrank_bt.Name = "checkrank_bt";
            this.checkrank_bt.Size = new System.Drawing.Size(211, 84);
            this.checkrank_bt.TabIndex = 0;
            this.checkrank_bt.Text = "Xem hạng ";
            this.checkrank_bt.UseVisualStyleBackColor = true;
            this.checkrank_bt.Click += new System.EventHandler(this.checkrank_bt_Click);
            // 
            // Lose
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(798, 618);
            this.Controls.Add(this.panel1);
            this.Name = "Lose";
            this.Text = "Lose";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button checkrank_bt;
    }
}