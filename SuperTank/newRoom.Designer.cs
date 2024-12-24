namespace SuperTank
{
    partial class newRoom
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
            this.label2 = new System.Windows.Forms.Label();
            this.cbb_listRoom = new System.Windows.Forms.ComboBox();
            this.roomID = new System.Windows.Forms.TextBox();
            this.btn_createRoom = new System.Windows.Forms.Button();
            this.btn_joinRoom = new System.Windows.Forms.Button();
            this.btn_findRoom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(97, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Available room";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(97, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Room ID";
            // 
            // cbb_listRoom
            // 
            this.cbb_listRoom.FormattingEnabled = true;
            this.cbb_listRoom.Location = new System.Drawing.Point(353, 72);
            this.cbb_listRoom.Name = "cbb_listRoom";
            this.cbb_listRoom.Size = new System.Drawing.Size(197, 28);
            this.cbb_listRoom.TabIndex = 2;
            // 
            // roomID
            // 
            this.roomID.Location = new System.Drawing.Point(353, 151);
            this.roomID.Name = "roomID";
            this.roomID.Size = new System.Drawing.Size(197, 26);
            this.roomID.TabIndex = 3;
            // 
            // btn_createRoom
            // 
            this.btn_createRoom.Location = new System.Drawing.Point(245, 240);
            this.btn_createRoom.Name = "btn_createRoom";
            this.btn_createRoom.Size = new System.Drawing.Size(108, 68);
            this.btn_createRoom.TabIndex = 5;
            this.btn_createRoom.Text = "Create";
            this.btn_createRoom.UseVisualStyleBackColor = true;
            // 
            // btn_joinRoom
            // 
            this.btn_joinRoom.Location = new System.Drawing.Point(437, 240);
            this.btn_joinRoom.Name = "btn_joinRoom";
            this.btn_joinRoom.Size = new System.Drawing.Size(113, 68);
            this.btn_joinRoom.TabIndex = 6;
            this.btn_joinRoom.Text = "Join";
            this.btn_joinRoom.UseVisualStyleBackColor = true;
            this.btn_joinRoom.Click += new System.EventHandler(this.btn_joinRoom_Click);
            // 
            // btn_findRoom
            // 
            this.btn_findRoom.Location = new System.Drawing.Point(633, 68);
            this.btn_findRoom.Name = "btn_findRoom";
            this.btn_findRoom.Size = new System.Drawing.Size(113, 32);
            this.btn_findRoom.TabIndex = 7;
            this.btn_findRoom.Text = "Find";
            this.btn_findRoom.UseVisualStyleBackColor = true;
            // 
            // newRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_findRoom);
            this.Controls.Add(this.btn_joinRoom);
            this.Controls.Add(this.btn_createRoom);
            this.Controls.Add(this.roomID);
            this.Controls.Add(this.cbb_listRoom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "newRoom";
            this.Text = "newRoom";
            this.Load += new System.EventHandler(this.newRoom_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbb_listRoom;
        private System.Windows.Forms.TextBox roomID;
        private System.Windows.Forms.Button btn_createRoom;
        private System.Windows.Forms.Button btn_joinRoom;
        private System.Windows.Forms.Button btn_findRoom;
    }
}