namespace TBGO
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMaxUsers = new System.Windows.Forms.TextBox();
            this.textBoxMaxTables = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 19;
            this.listBox1.Location = new System.Drawing.Point(4, 5);
            this.listBox1.Margin = new System.Windows.Forms.Padding(5);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(741, 289);
            this.listBox1.TabIndex = 0;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(463, 302);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(127, 31);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "启动引擎";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(618, 302);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(127, 31);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "关闭引擎";
            this.buttonStop.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 299);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "游戏室允许进入的最多人数(1-300)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 332);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(339, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "游戏室同时开出的游戏桌数(1-100)：";
            // 
            // textBoxMaxUsers
            // 
            this.textBoxMaxUsers.Location = new System.Drawing.Point(343, 296);
            this.textBoxMaxUsers.Name = "textBoxMaxUsers";
            this.textBoxMaxUsers.Size = new System.Drawing.Size(43, 29);
            this.textBoxMaxUsers.TabIndex = 4;
            this.textBoxMaxUsers.Text = "15";
            // 
            // textBoxMaxTables
            // 
            this.textBoxMaxTables.Location = new System.Drawing.Point(343, 332);
            this.textBoxMaxTables.Name = "textBoxMaxTables";
            this.textBoxMaxTables.Size = new System.Drawing.Size(43, 29);
            this.textBoxMaxTables.TabIndex = 4;
            this.textBoxMaxTables.Text = "25";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(753, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 31);
            this.button1.TabIndex = 5;
            this.button1.Text = "查看棋盘信息";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 372);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxMaxTables);
            this.Controls.Add(this.textBoxMaxUsers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.listBox1);
            this.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form1";
            this.Text = "TibetanGO";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMaxUsers;
        private System.Windows.Forms.TextBox textBoxMaxTables;
        private System.Windows.Forms.Button button1;
    }
}

