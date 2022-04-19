using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Stone;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBGO
{
    public partial class Chessboard_Information : Form
    {
        Board InfBoard ;
        int side;
        #region 构造函数
        public Chessboard_Information(string MaxTables)
        {
            InitializeComponent();
            int BoardID = Convert.ToInt16(MaxTables);
            for(int i=0;i< BoardID;i++)
            {
                this.BoardIDBox.Items.Add(i.ToString());
            }
            this.groupBox2.Enabled = false;

            this.groupBox3.Enabled = false;
        }
        #endregion

        #region 创建棋盘
        private void Chessboard_Information_Paint(object sender, PaintEventArgs e)
        {
            #region 创建绘画对象
            float wei = 180;
            Point point = new Point(65, 65);//期盼的左上角第一个点坐标
            Graphics g = this.panel1.CreateGraphics();
            Pen p = new Pen(Color.Black, 3);
            //横向线和竖线
            for (int i = 0; i <= 4; i++)
            {
                g.DrawLine(p, point.X, point.Y + wei * i, point.X + wei * 4, point.Y + wei * i);//横线
                g.DrawLine(p, point.X + wei * i, point.Y, point.X + wei * i, point.Y + wei * 4);//竖线
            }
            #region 画斜线
            for (int i = 0; i <= 4; i++)//画\
            {
                g.DrawLine(p, point.X, point.Y + wei * i, point.X + wei * 4 - wei * i, point.Y + wei * 4);
                g.DrawLine(p, point.X + wei * i, point.Y, point.X + wei * 4, point.Y + wei * 4 - wei * i);

            }
            for (int i = 0; i <= 4; i++)//画/
            {
                g.DrawLine(p, point.X + wei * 4 - wei * i, point.Y, point.X, point.Y + wei * 4 - wei * i);//画/上部分
                g.DrawLine(p, point.X + wei * 4, point.Y + wei * i, point.X + wei * i, point.Y + wei * 4);//画/下部分

            }
            #endregion

            #region 绘制星位
            float starSize = 15;
            Rectangle rect = new Rectangle();
            rect.X = Convert.ToInt32((point.X + point.X + wei * 4) / 2 - starSize);
            rect.Y = Convert.ToInt32((point.Y + point.Y + wei * 4) / 2 - starSize);
            rect.Width = Convert.ToInt32(starSize * 2);
            rect.Height = Convert.ToInt32(starSize * 2);
            g.FillEllipse(Brushes.Black, rect);
            //drawStar(g, point.X, point.Y, wei);
            #endregion
            #endregion
        }



        #endregion

        #region 选择桌子
        private void BoardIDBox_TextChanged(object sender, EventArgs e)
        {
            int tableIndex = Convert.ToInt16(BoardIDBox.SelectedItem);
            InfBoard = Form1.DqMB(tableIndex);
            if(InfBoard.m_currentStep>0)
            {
                this.groupBox2.Enabled = true;
                this.groupBox3.Enabled = true;
                if (InfBoard.ChessAI == Chess.ChessType.Black)
                {
                    BlakLabel.Text = "TibetanGo_AI";
                    int Wei = InfBoard.UserName.IndexOf('-');
                    string str = InfBoard.UserName.Substring(1, Wei-1);
                    WhiteLabel.Text = str;
                    side = 0;
                }
                else
                {
                    int Wei = InfBoard.UserName.IndexOf('-');
                    string str = InfBoard.UserName.Substring(1, Wei-1);
                    BlakLabel.Text = str;
                    WhiteLabel.Text = "TibetanGo_AI";
                    side = 1;
                }
            }
            else
            {
                MessageBox.Show(BoardIDBox.SelectedItem+"桌没人！", "提示信息");
            } 
        }
        #endregion

        #region 显示棋子按钮
        private void Show_Chess_Click(object sender, EventArgs e)
        {
            foreach(Chess Ons in InfBoard.MBoard)
            {
                if(Ons.type!=Chess.ChessType.Empty)
                {
                    InitChess(Ons.pos.posX, Ons.pos.posY, Ons.type);
                }
            }
        }
        #endregion

        #region 在棋盘上添加棋子
        /// <summary>
        /// 在棋盘上添加棋子
        /// </summary>
        public bool InitChess(int GridX, int GridY, Chess.ChessType DQL)
        {
            bool CG = false;
            if ((GridX + GridY) % 2 != 1)
            {
                switch (DQL)
                {
                    case Chess.ChessType.Black:
                        stone black1 = new stone();
                        black1.Location = new Point(GridX * 90 + 65 - 25, GridY * 90 + 65 - 25);
                        black1.ImageLocation = Application.StartupPath + @"\\heiqi.png";
                        black1.type = stone.ChessType.Black;
                        black1.Lis = 0;
                        black1.GridX = GridX;
                        black1.GridY = GridY;
                        this.panel1.Controls.Add(black1);
                        CG = true;
                        break;
                    case Chess.ChessType.White:
                        stone White1 = new stone();
                        White1.Location = new Point(GridX * 90 + 65 - 25, GridY * 90 + 65 - 25);
                        White1.ImageLocation = Application.StartupPath + @"\\baiqi.png";
                        White1.type = stone.ChessType.White;
                        White1.Lis = 1;
                        White1.GridX = GridX;
                        White1.GridY = GridY;
                        this.panel1.Controls.Add(White1);
                        CG = true;
                        break;
                }
            }
            return CG;
        }
        #endregion

        public void Show_Asse(int i,int j,double Ery)
        {

            Label Alabel1 = new Label();
            Alabel1.Location = new Point(i * 90 + 65 - 40, j * 90 + 65 - 15);
            Alabel1.Text =Ery.ToString();
            Alabel1.BackColor = Color.Transparent;
            Alabel1.Size = new Size(80, 30);
            Alabel1.TextAlign = ContentAlignment.MiddleCenter;
            Alabel1.Font = new Font("楷体", 14.25F,FontStyle.Bold);
            Alabel1.ForeColor = Color.Red;
            this.panel1.Controls.Add(Alabel1);

        }
        #region 显示当前每个点的评估值
        private void Show_Assess_Click(object sender, EventArgs e)
        {
           
            MCTS Mcts = new MCTS();
            Node MM= Mcts.New_Mmonte_carlo_tree_search(InfBoard, side);
           
            //string str = Mcts.Mmonte_carlo_tree_search_Test(InfBoard, side);
            //string[] wei = str.Split(',');
            //InitChess(Convert.ToInt16(wei[0]), Convert.ToInt16(wei[1]), InfBoard.ChessAI);
            InfBoard.New_Rendom_Gether();
            foreach (Chess.POS Mypos in InfBoard.GoChes)
            {
                double Ery = Shuwp(Mypos, MM);
                Show_Asse(Mypos.posX, Mypos.posY, Ery);
            }
            Ftime.Text = (Board.FtimeTime).ToString()+"ms";
            Jtime.Text = (Board.JtimeTime).ToString() + "ms";
        }

        private double Shuwp(Chess.POS Mypos,Node Mmj)
        {
            double Value = 0.0;
            foreach(Node No in Mmj.Children)
            {
                if(No.pos== Mypos)
                {
                    Value = No.Eat;
                    return Value;
                }
            }
            return Value;
        }
        #endregion
    }
}
