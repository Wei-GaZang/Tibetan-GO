namespace TBGO
{
    partial class Chessboard_Information
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chessboard_Information));
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.BoardIDBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BlakLabel = new System.Windows.Forms.Label();
            this.WhiteLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Show_Assess = new System.Windows.Forms.Button();
            this.Show_Chess = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Parameter_Depth = new System.Windows.Forms.TextBox();
            this.Parameter_C = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Jtime = new System.Windows.Forms.Label();
            this.Ftime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(850, 844);
            this.panel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.PeachPuff;
            this.statusStrip1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 847);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1084, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Maroon;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(261, 20);
            this.toolStripStatusLabel1.Text = "青海民族大学藏文信息处理与软件研究所";
            // 
            // BoardIDBox
            // 
            this.BoardIDBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BoardIDBox.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BoardIDBox.FormattingEnabled = true;
            this.BoardIDBox.Location = new System.Drawing.Point(922, 11);
            this.BoardIDBox.Name = "BoardIDBox";
            this.BoardIDBox.Size = new System.Drawing.Size(150, 27);
            this.BoardIDBox.TabIndex = 2;
            this.BoardIDBox.TextChanged += new System.EventHandler(this.BoardIDBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(857, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "桌号:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BlakLabel);
            this.groupBox1.Controls.Add(this.WhiteLabel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(861, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 146);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "角色信息";
            // 
            // BlakLabel
            // 
            this.BlakLabel.AutoSize = true;
            this.BlakLabel.Location = new System.Drawing.Point(71, 88);
            this.BlakLabel.Name = "BlakLabel";
            this.BlakLabel.Size = new System.Drawing.Size(99, 19);
            this.BlakLabel.TabIndex = 3;
            this.BlakLabel.Text = "BlakLabel";
            // 
            // WhiteLabel
            // 
            this.WhiteLabel.AutoSize = true;
            this.WhiteLabel.Location = new System.Drawing.Point(71, 29);
            this.WhiteLabel.Name = "WhiteLabel";
            this.WhiteLabel.Size = new System.Drawing.Size(109, 19);
            this.WhiteLabel.TabIndex = 2;
            this.WhiteLabel.Text = "WhiteLabel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "黑棋：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "白棋：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Show_Assess);
            this.groupBox2.Controls.Add(this.Show_Chess);
            this.groupBox2.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(869, 729);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(211, 106);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作区";
            // 
            // Show_Assess
            // 
            this.Show_Assess.Location = new System.Drawing.Point(41, 68);
            this.Show_Assess.Name = "Show_Assess";
            this.Show_Assess.Size = new System.Drawing.Size(129, 32);
            this.Show_Assess.TabIndex = 1;
            this.Show_Assess.Text = "显示评估值";
            this.Show_Assess.UseVisualStyleBackColor = true;
            this.Show_Assess.Click += new System.EventHandler(this.Show_Assess_Click);
            // 
            // Show_Chess
            // 
            this.Show_Chess.Location = new System.Drawing.Point(41, 28);
            this.Show_Chess.Name = "Show_Chess";
            this.Show_Chess.Size = new System.Drawing.Size(129, 33);
            this.Show_Chess.TabIndex = 0;
            this.Show_Chess.Text = "显示棋子";
            this.Show_Chess.UseVisualStyleBackColor = true;
            this.Show_Chess.Click += new System.EventHandler(this.Show_Chess_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Parameter_Depth);
            this.groupBox3.Controls.Add(this.Parameter_C);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(861, 229);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(211, 146);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "参数调整区";
            // 
            // Parameter_Depth
            // 
            this.Parameter_Depth.Location = new System.Drawing.Point(23, 111);
            this.Parameter_Depth.Name = "Parameter_Depth";
            this.Parameter_Depth.Size = new System.Drawing.Size(169, 29);
            this.Parameter_Depth.TabIndex = 3;
            // 
            // Parameter_C
            // 
            this.Parameter_C.Location = new System.Drawing.Point(23, 50);
            this.Parameter_C.Name = "Parameter_C";
            this.Parameter_C.Size = new System.Drawing.Size(169, 29);
            this.Parameter_C.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "搜索深度:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "参数C:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Jtime);
            this.groupBox4.Controls.Add(this.Ftime);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(861, 390);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(211, 143);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "时间区";
            // 
            // Jtime
            // 
            this.Jtime.AutoSize = true;
            this.Jtime.Location = new System.Drawing.Point(142, 94);
            this.Jtime.Name = "Jtime";
            this.Jtime.Size = new System.Drawing.Size(59, 19);
            this.Jtime.TabIndex = 3;
            this.Jtime.Text = "Jtime";
            // 
            // Ftime
            // 
            this.Ftime.AutoSize = true;
            this.Ftime.Location = new System.Drawing.Point(142, 34);
            this.Ftime.Name = "Ftime";
            this.Ftime.Size = new System.Drawing.Size(59, 19);
            this.Ftime.TabIndex = 2;
            this.Ftime.Text = "Ftime";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 19);
            this.label8.TabIndex = 1;
            this.label8.Text = "进攻计算用时:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(139, 19);
            this.label7.TabIndex = 0;
            this.label7.Text = "防术计算用时:";
            // 
            // Chessboard_Information
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 872);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BoardIDBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Chessboard_Information";
            this.Text = "Chessboard_Information";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Chessboard_Information_Paint);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ComboBox BoardIDBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label BlakLabel;
        private System.Windows.Forms.Label WhiteLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Show_Assess;
        private System.Windows.Forms.Button Show_Chess;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox Parameter_Depth;
        private System.Windows.Forms.TextBox Parameter_C;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label Jtime;
        private System.Windows.Forms.Label Ftime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}