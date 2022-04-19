using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGO
{
   public partial class Board
    {
        #region 公共变量
        /// <summary>
        /// 桌号
        /// </summary> 
        public int TableIndex = -1;
        /// <summary>
        /// 棋盘的所有棋子
        /// </summary>
        public  Chess[,] MBoard = new Chess[9, 9];
        /// <summary>
        /// 最后一颗棋子
        /// </summary>
        public Chess m_LastChess;
        /// <summary>
        /// 最后一颗被吃的棋子
        /// </summary>
        public Chess m_LastEatten;
        /// <summary>
        /// 当前步数
        /// </summary>
        public  int m_currentStep;
        /// <summary>
        /// 棋盘备份
        /// </summary>
        public Chess[,] MBoard_backup = new Chess[9, 9];
        /// <summary>
        /// 备份最后一颗棋子
        /// </summary>
        public Chess MSimLastChess;
        /// <summary>
        /// 备份最后一颗被吃的棋子
        /// </summary>
        public Chess MSim_LastEatten;
        /// <summary>
        /// 模拟当前步数
        /// </summary>
        // private int m_Sim_currentStep;

        /// <summary>
        /// 模拟棋盘
        /// </summary>
        public Chess[,] MSim_Board = new Chess[9, 9];

        /// <summary>
        /// 最后一个模拟棋子
        /// </summary>
        public Chess m_LastChess_backup;
        /// <summary>
        /// 模拟最后被吃棋子
        /// </summary>
        public Chess m_LastEatten_backup;
        /// <summary>
        /// 参数随机数，用于参数随机棋步
        /// </summary>
        private Random R = new Random();
        /// <summary>
        /// 棋局结束
        /// </summary>
        public bool isGameOver = false;
        /// <summary>
        /// 可以吃棋子的走法
        /// </summary>
        public struct TreeB
        {
            public int N0;//存放X轴
            public int N1;//存放Y轴
        }
        //public DB Shj = new DB();
        public RuleBase GOsrule = new RuleBase();
        /// <summary>
        /// 随机产生的树
        /// </summary>
        List<TreeB> Random_Tree = new List<TreeB>();
        /// <summary>
        ///本次可以吃棋子
        /// </summary>
        public List<TreeB> ChiQI = new List<TreeB>();
        /// <summary>
        ///虚拟吃法评估数
        /// </summary>
        public double Cens = 0;
        /// <summary>
        ///虚拟被吃法数评估
        /// </summary>
        public double XnCens = 0;
        /// <summary>
        /// 当前棋的颜色
        /// </summary>  
        public Chess.ChessType mytype;

        /// <summary>
        /// 当前AI棋子的颜色
        /// </summary>
        public Chess.ChessType ChessAI;
        /// <summary>
        /// 当前用户名
        /// </summary>
       public  string UserName = string.Empty;
        public int Side
        {
            get
            {
                int rst = 1;
                if (this.mytype == Chess.ChessType.Black)
                    rst = 0;
                return rst;
            }
            set
            {
                if (value == 0)
                    this.mytype = Chess.ChessType.Black;
                else if (value == 1)
                    this.mytype = Chess.ChessType.White;
            }
        }
       /// <summary>
       /// 跳棋查询是分五组查询
       /// </summary>
        TreeB[] Five_Case = new TreeB[5];
        /// <summary>
        /// 用哈希消重
        /// </summary>
        Hashtable ht = new Hashtable();//定义一个哈希表
        /// <summary>
        /// 本次可以跳的棋
        /// </summary>
        public List<TreeB> TiaoQI = new List<TreeB>();
        /// <summary>
        /// 模拟着法集合
        /// </summary>
        public List<Chess.POS> GoChes = new List<Chess.POS>();
        /// <summary>
        /// 防术时间
        /// </summary>
        public static long FtimeTime;
        /// <summary>
        /// 进攻时间
        /// </summary>
        public static long JtimeTime;
        #endregion

        #region 初始化棋盘
        /// <summary>
        /// 初始化棋盘
        /// </summary>
        public  void ClearBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    MBoard[i, j] = new Chess(new Chess.POS(i, j), Chess.ChessType.Empty, 0);
                    MBoard_backup[i, j] = new Chess(new Chess.POS(i, j), Chess.ChessType.Empty, 0);
                }
            }

            m_LastChess = new Chess();
            m_LastEatten = new Chess();
            m_LastChess.pos.setToInvalid();
            m_LastEatten.pos.setToInvalid();
            m_currentStep = 1;

            GOsrule.Gzk();//获取规则库
        }
        #endregion

        /// <summary>
        /// 棋盘每条线之间的单位长度的一般
        /// </summary>
        private static float gap1 = 90;
        #region 并返回棋子的位置
        /// <summary>
        /// 调整棋子在棋盘中的位置，并返回棋子的位置
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        public Chess.POS mapPointsToBoard(float pointX, float pointY)
        {
            int posX = Convert.ToInt32((pointX - 35) / gap1);
            int posY = Convert.ToInt32((pointY - 35) / gap1);
            Chess.POS pos = new Chess.POS(posX, posY);
            if (!pos.isValid)
                pos.setToInvalid();
            return pos;
        }
        #endregion

        #region 棋盘备份准备搜索
        /// <summary>
        ///棋盘备份准备搜索
        /// </summary>
        public void SimuChess()
        {
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    MBoard_backup[i, j] = new Chess(MBoard[i, j].pos, MBoard[i, j].type, MBoard[i, j].step);
                }
            }
            this.MSimLastChess = this.m_LastChess.Copy();
            this.MSim_LastEatten = this.m_LastEatten.Copy();
        }
        #endregion
        
        #region 初始化模拟棋盘
        /// <summary>
        /// 初始化模拟棋盘
        /// </summary>
        public void ClearBoard_Simulate()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                   this.MBoard_backup[i, j] = new Chess(new Chess.POS(i, j), Chess.ChessType.Empty, 0);
                }
            }

            this.MSimLastChess = new Chess();
            this.MSim_LastEatten = new Chess();
            this.MSim_LastEatten.pos.setToInvalid();
            this.MSimLastChess.pos.setToInvalid();
        }
        #endregion

        #region 当前可以走步法
        /// <summary>
        /// 当前可以走的步法
        /// </summary>
        public Node SearchTree()
        {
            Node Mynode = new Node();
           this.ClearBoard_Simulate();
           this.SimuChess();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if (i != 4 || j != 4)
                        {
                            if (this.MBoard_backup[i, j].type == Chess.ChessType.Empty)
                            {
                                Node OneTree = new Node();
                                OneTree.pos = this.MBoard_backup[i, j].pos;
                                OneTree.Lix = Chess.oppsiteType(this.mytype);
                                OneTree.Visit_times++;
                                this.MBoard_backup[OneTree.pos.posX, OneTree.pos.posY] = new Chess(OneTree.pos, OneTree.Lix);
                                Function(OneTree.pos, OneTree.Lix);
                                OneTree.Eat = Cens;
                                Mynode.Children.Add(OneTree);
                                this.SimuChess();
                            }
                        }
                    }
                }
            }
            return Mynode;
        }
        #endregion

        #region 实验——当前可以走步法
        /// <summary>
        /// 实验——当前可以走的步法
        /// </summary>
        public Node Siyan_SearchTree(Chess.ChessType Aitp)
        {
            Node Mynode = new Node();
            this.ClearBoard_Simulate();
            this.SimuChess();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if (i != 4 || j != 4)
                        {
                            if (this.MBoard_backup[i, j].type == Chess.ChessType.Empty)
                            {
                                Node OneTree = new Node();
                                OneTree.pos = this.MBoard_backup[i, j].pos;
                                OneTree.Lix = Aitp;
                                //OneTree.Visit_times++;
                                //Mynode.Visit_times++;
                                OneTree.Parent = Mynode;
                                this.MBoard_backup[OneTree.pos.posX, OneTree.pos.posY] = new Chess(OneTree.pos, OneTree.Lix);
                                Function(OneTree.pos, OneTree.Lix);
                                OneTree.Eat = Cens;
                                Mynode.Children.Add(OneTree);
                                this.SimuChess();
                            }
                        }
                    }
                }
            }
            return Mynode;
        }
        #endregion

        #region New当前可以走步法
        /// <summary>
        /// 当前可以走的步法
        /// </summary>
        public Node New_SearchTree(Chess.ChessType Aitp)
        {
            Node Mynode = new Node();
            this.ClearBoard_Simulate();
            this.SimuChess();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if (i != 4 || j != 4)
                        {
                            if (this.MBoard_backup[i, j].type == Chess.ChessType.Empty)
                            {
                                Node OneTree = new Node();
                                OneTree.pos = this.MBoard_backup[i, j].pos;
                                OneTree.Lix = Aitp;
                                OneTree.Visit_times++;
                                Mynode.Visit_times++;
                                OneTree.Parent = Mynode;
                                Mynode.Children.Add(OneTree);
                                this.SimuChess();
                            }
                        }
                    }
                }
            }
            return Mynode;
        }
        #endregion



        #region 模拟
        public string Simulation(Chess.POS Pos,Chess.ChessType Mytype)
        {
            int Visit_times = 0;
            string Current_value =string.Empty;
            this.ClearBoard_Simulate();
            this.SimuChess();
            this.Dummy_AddChess(Pos, Mytype);
            this.ViPlay(Pos, Mytype);
            Chess.ChessType DangType= ((Mytype == Chess.ChessType.Black) ? Chess.ChessType.White : Chess.ChessType.Black);
            this.Rendom_Gether();
            while(GoChes.Count!=0)
            {
                Visit_times++;
                Chess.POS Ren_chess = Rendom_chess();
                this.Dummy_AddChess(Ren_chess, DangType);
                this.ViPlay(Ren_chess, DangType);
                //查看是否赢
                if(YingQi(DangType))
                {
                    if(DangType== Mytype)
                    {
                        Current_value = "1";
                    }
                    else
                    {
                        Current_value = "0";
                    }
                    return Current_value+","+Visit_times.ToString();
                }
                //切换棋颜色
                DangType = ((DangType == Chess.ChessType.Black) ? Chess.ChessType.White : Chess.ChessType.Black);
            }
            Current_value = Draw(Mytype).ToString();
            return Current_value + "," + Visit_times.ToString();
        }
        #endregion

        #region  着法集合随机获取一个棋子
        /// <summary>
        /// 着法集合随机获取一个棋子
        /// </summary>
        /// <returns></returns>
        public Chess.POS Rendom_chess()
        {
            Chess.POS Ren_chess = new Chess.POS();
            Random rm = new Random();
            int Rendom = -1;
            Rendom = rm.Next(0, GoChes.Count - 1);//随机产生一个棋子
            Ren_chess = GoChes[Rendom];//返回随机的棋子
            GoChes.Remove(GoChes[Rendom]);//随机选定的棋子在着法集合中删除
            return Ren_chess;
        }
        #endregion

        #region  获取着法集合
        /// <summary>
        /// 获取着法集合
        /// </summary>
        public void Rendom_Gether()
        {
            GoChes.Clear();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if (i != 4 || j != 4)
                        {
                            if (this.MBoard_backup[i, j].type == Chess.ChessType.Empty)
                            {
                                GoChes.Add(MBoard_backup[i, j].pos);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 查看模拟棋是否赢
        /// <summary>
        /// 查看模拟棋是否赢
        /// </summary>
        public bool YingQi(Chess.ChessType Leix)
        {
            int zongs = 0;
            int Fangq = 0;
            bool Yin = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if(this.MBoard_backup[i, j].type !=Chess.ChessType.Empty)
                        {
                            Fangq++;
                            if (this.MBoard_backup[i, j].type == Leix)
                            {
                                zongs++;
                            }
                        }
                        
                    }
                }
            }

           
            if (zongs == Fangq)
            {
                Yin = true;
            }
            return Yin;
        }
        #endregion

        #region 查看模拟平局
        /// <summary>
        /// 返回模拟平局评估值
        /// </summary>
        public Double Draw(Chess.ChessType Leix)
        {
            int zongs = 0;
            int Fangq = 0;
            double Yin = 0.0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if (this.MBoard_backup[i, j].type != Chess.ChessType.Empty)
                        {
                            Fangq++;
                            if (this.MBoard_backup[i, j].type == Leix)
                            {
                                zongs++;
                            }
                        }

                    }
                }
            }
            return Yin=Convert.ToDouble(zongs)/ Convert.ToDouble(Fangq);
        }
        #endregion

        #region  Nwe获取着法集合
        /// <summary>
        /// Nwe获取着法集合
        /// </summary>
        public void New_Rendom_Gether()
        {
            ClearBoard_Simulate();
            SimuChess();
            GoChes.Clear();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if (i != 4 || j != 4)
                        {
                            if (this.MBoard_backup[i, j].type == Chess.ChessType.Empty)
                            {
                                GoChes.Add(MBoard_backup[i, j].pos);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 静态评估函数
        /// <summary>
        /// 评估函数
        /// </summary>
        public void Function(Chess.POS pos, Chess.ChessType Yans)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            Cens = 0;
            ViPlay(pos, Yans);
            stopwatch.Stop();
            JtimeTime = JtimeTime+ stopwatch.ElapsedMilliseconds;
            //BeiChe(pos, Yans);
            if (Yans== Chess.ChessType.Black)
            {
                Yans = Chess.ChessType.White;
            }
            else
            {
                Yans = Chess.ChessType.Black;
            }
            System.Diagnostics.Stopwatch  start = new System.Diagnostics.Stopwatch();
            start.Start();
            ViPlay(pos, Yans);
            Cens = Cens - XnCens;
            start.Stop();
            FtimeTime = FtimeTime + start.ElapsedMilliseconds; ;
        }
        #endregion 

        #region 虚拟走棋
        /// <summary>
        ///虚拟返回已经被吃的棋子
        /// </summary>
        public void ViPlay(Chess.POS pos, Chess.ChessType Yans)
        {

            string Xb = "";
            Xb = pos.posX.ToString() + pos.posY.ToString();
            GOsrule.Recailing(Xb);
            List<TreeB> Ch2 = ViZhuofa(Yans);
            Cens += Ch2.Count;
            if (Ch2.Count != 0)
            { ViGaiQi(Ch2, Yans); }
            List<TreeB> q = ViGun(pos.posX, pos.posY, Yans);
            Cens += q.Count;
            if (q.Count != 0)
            { ViGaiQi(q, Yans); }

        }

        #region 虚拟已被吃的棋子改颜色
        /// <summary>
        ///虚拟已被吃的棋子改颜色
        /// </summary>
        public void ViGaiQi(List<TreeB> ChiZi, Chess.ChessType Dqlx)
        {

            foreach (TreeB i in ChiZi)
            {
                if (Dqlx == Chess.ChessType.White)
                {

                    this.MBoard_backup[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type = Chess.ChessType.White;
                    string Xb = i.N0.ToString() + i.N1.ToString();
                    GOsrule.Recailing(Xb);
                    List<TreeB> Ch2 = ViZhuofa(this.MBoard_backup[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type);
                    Cens += Ch2.Count;
                    if (Ch2.Count != 0)
                    { ViGaiQi(Ch2, Dqlx); }
                    List<TreeB> q = ViGun(i.N0, i.N1, this.MBoard_backup[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type);
                    Cens += q.Count;
                    if (q.Count != 0)
                    { ViGaiQi(q, Dqlx); }

                }
                else if (Dqlx == Chess.ChessType.Black)
                {
                    this.MBoard_backup[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type = Chess.ChessType.Black;
                    string Xb = i.N0.ToString() + i.N1.ToString();
                    GOsrule.Recailing(Xb);
                    List<TreeB> Ch2 = ViZhuofa(this.MBoard_backup[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type);
                    Cens += Ch2.Count;
                    if (Ch2.Count != 0)
                    { ViGaiQi(Ch2, Dqlx); }
                    List<TreeB> q = ViGun(i.N0, i.N1, this.MBoard_backup[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type);
                    Cens += q.Count;
                    if (q.Count != 0)
                    { ViGaiQi(q, Dqlx); }

                }
            }
        }
        #endregion

        /// <summary>
        ///虚拟遍历当前棋子可以所以吃法 当前可以吃的棋子存到N0和N1数组
        /// </summary>
        public List<TreeB> ViZhuofa(Chess.ChessType Dqlx)
        {
            List<TreeB> ChiZi = new List<TreeB>();
            foreach (RuleBase.TreeA S in RuleBase.temp1)
            {

                TreeB C1 = new TreeB();
                if (this.MBoard_backup[S.B0, S.B1].type != Dqlx && this.MBoard_backup[S.C0, S.C1].type == Dqlx)
                {
                   if( this.MBoard_backup[S.B0, S.B1].type != Chess.ChessType.Empty)
                    { 
                    C1.N0 = S.B0;
                    C1.N1 = S.B1;
                    ChiZi.Add(C1);
                    }
                }
            }
            return ChiZi;
        }
        #endregion 

        #region 虚拟长枪
        /// <summary>
        ///虚拟查看有无长枪规则 可吃的棋子
        /// </summary>
        public List<TreeB> ViGun(int X, int Y, Chess.ChessType Dq)
        {
            string Str = X.ToString() + Y.ToString();
            GOsrule.RulSlct(Str);
            List<TreeB> q1 = ViSomGun(X, Y, Dq);
            return q1;
        }
        #endregion

        #region 虚拟检查对角线有无长枪
        /// <summary>
        ///虚拟检查对角线有无长枪
        /// </summary>
        public List<TreeB> ViSomGun(int X, int Y, Chess.ChessType Dq)
        {
            List<TreeB> qiang = new List<TreeB>();
            int Qs = 0;
            if (RuleBase.Q1.Nb1 != -1)
            {
                for (int i = 0; i < RuleBase.Q1.Nb1 + 1; i++)
                {
                    if (this.MBoard_backup[RuleBase.Q1.AX + i, RuleBase.Q1.AY + i].type == Dq)
                    {
                        Qs++;
                    }
                }
                if (Qs == RuleBase.Q1.Nb1)
                {
                    TreeB B = new TreeB();
                    if (this.MBoard_backup[RuleBase.Q1.AX, RuleBase.Q1.AY].type != Dq && this.MBoard_backup[RuleBase.Q1.AX, RuleBase.Q1.AY].type != Chess.ChessType.Empty)
                    {
                        B.N0 = RuleBase.Q1.AX;
                        B.N1 = RuleBase.Q1.AY;
                        qiang.Add(B);
                    }
                    else if (this.MBoard_backup[RuleBase.Q1.AX + RuleBase.Q1.Nb1, RuleBase.Q1.AY + RuleBase.Q1.Nb1].type != Dq && this.MBoard_backup[RuleBase.Q1.AX + RuleBase.Q1.Nb1, RuleBase.Q1.AY + RuleBase.Q1.Nb1].type != Chess.ChessType.Empty)
                    {
                        B.N0 = RuleBase.Q1.AX + RuleBase.Q1.Nb1;
                        B.N1 = RuleBase.Q1.AY + RuleBase.Q1.Nb1;
                        qiang.Add(B);
                    }
                }
            }
            if (RuleBase.Q1.Nb2 != -1)
            {
                Qs = 0;
                for (int i = 0; i < RuleBase.Q1.Nb2 + 1; i++)
                {
                    if (this.MBoard_backup[RuleBase.Q1.BX - i, RuleBase.Q1.BY + i].type == Dq)
                    {
                        Qs++;
                    }
                }
                if (Qs == RuleBase.Q1.Nb2)
                {
                    TreeB B = new TreeB();
                    if (this.MBoard_backup[RuleBase.Q1.BX, RuleBase.Q1.BY].type != Dq && this.MBoard_backup[RuleBase.Q1.BX, RuleBase.Q1.BY].type != Chess.ChessType.Empty)
                    {
                        B.N0 = RuleBase.Q1.BX;
                        B.N1 = RuleBase.Q1.BY;
                        qiang.Add(B);
                    }
                    else if (this.MBoard_backup[RuleBase.Q1.BX - RuleBase.Q1.Nb2, RuleBase.Q1.BY + RuleBase.Q1.Nb2].type != Dq && this.MBoard_backup[RuleBase.Q1.BX - RuleBase.Q1.Nb2, RuleBase.Q1.BY + RuleBase.Q1.Nb2].type != Chess.ChessType.Empty)
                    {
                        B.N0 = RuleBase.Q1.BX - RuleBase.Q1.Nb2;
                        B.N1 = RuleBase.Q1.BY + RuleBase.Q1.Nb2;
                        qiang.Add(B);
                    }
                }
            }
            List<TreeB> q2 = ViXoGun(X, Y, Dq);
            foreach (TreeB q in q2)
            {
                qiang.Add(q);
            }
            return qiang;
        }

        #endregion

        #region 虚拟检查纵横有无长枪
        /// <summary>
        ///虚拟检查纵横有无长枪
        /// </summary>
        public List<TreeB> ViXoGun(int X, int Y, Chess.ChessType Dq)
        {
            List<TreeB> qiang = new List<TreeB>();
            int Qs = 0;
            if (X % 2 == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (this.MBoard_backup[i, Y].type == Dq && this.MBoard_backup[i, Y].type != Chess.ChessType.Empty)
                    {
                        Qs++;
                    }
                    i++;
                }
                if (Qs == 4)
                {
                    TreeB B = new TreeB();
                    if (this.MBoard_backup[0, Y].type != Dq && this.MBoard_backup[0, Y].type != Chess.ChessType.Empty)
                    {
                        B.N0 = 0;
                        B.N1 = Y;
                        qiang.Add(B);
                    }
                    else if (this.MBoard_backup[8, Y].type != Dq && this.MBoard_backup[8, Y].type != Chess.ChessType.Empty)
                    {
                        B.N0 = 8;
                        B.N1 = Y;
                        qiang.Add(B);
                    }
                }
                Qs = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (this.MBoard_backup[X, i].type == Dq && this.MBoard_backup[X, i].type != Chess.ChessType.Empty)
                    {
                        Qs++;
                    }
                    i++;
                }
                if (Qs == 4)
                {
                    TreeB B = new TreeB();
                    if (this.MBoard_backup[X, 0].type != Dq && this.MBoard_backup[X, 0].type != Chess.ChessType.Empty)
                    {
                        B.N0 = X;
                        B.N1 = 0;
                        qiang.Add(B);
                    }
                    else if (this.MBoard_backup[X, 8].type != Dq && this.MBoard_backup[X, 8].type != Chess.ChessType.Empty)
                    {
                        B.N0 = X;
                        B.N1 = 8;
                        qiang.Add(B);
                    }
                }
            }
            return qiang;
        }
        #endregion

        #region 虚拟计算可以被吃棋子
        public void BeiChe(Chess.POS pos, Chess.ChessType Yans)
        {
            string Xb = ""; XnCens = 0;
            Xb = pos.posX.ToString() + pos.posY.ToString();
            GOsrule.BeiBa(Xb);
            foreach (RuleBase.TreeA S in RuleBase.temp1)
            {
                if (this.MBoard_backup[S.B0, S.B1].type != Yans && this.MBoard_backup[S.B0, S.B1].type != Chess.ChessType.Empty && this.MBoard_backup[S.C0, S.C1].type == Chess.ChessType.Empty)
                {
                    XnCens += 0.5f;
                }
            }
        }
        #endregion

        #region 模拟增加一颗棋子在棋盘上
        /// <summary>
        /// 模拟增加一颗棋子在棋盘上
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Dummy_AddChess(Chess.POS pos, Chess.ChessType type)
        {
            if (this.m_currentStep > 41)
            {
                this.isGameOver = true;
                return false;
            }
            //只允许放黑棋或白棋
            if (type != Chess.ChessType.Black && type != Chess.ChessType.White)
                return false;
            // 只允许在空位置上放棋子。
            if (MBoard_backup[pos.posX, pos.posY].type != Chess.ChessType.Empty)
            { return false; }
            if ((pos.posX + pos.posY) % 2 == 1)
            { return false; }
            // 设置当前位置的棋子的类型
            MBoard_backup[pos.posX, pos.posY].type = type;
            //m_LastChess_backup.pos = pos;
            //m_LastChess_backup.type = type;
            MBoard_backup[pos.posX, pos.posY].pos = pos;
            return true;
        }
        #endregion

        #region 模拟棋盘备份准备搜索
        /// <summary>
        ///模拟棋盘备份准备搜索
        /// </summary>
        public void MnChess()
        {
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    MSim_Board[i, j] = new Chess(MBoard[i, j].pos, MBoard[i, j].type, MBoard[i, j].step);
                }
            }
            m_LastChess_backup =new Chess();
            m_LastChess_backup.pos.setToInvalid();
        }
        #endregion

        #region 随机产生一个节点
        /// <summary>
        /// 随机产生一个节点
        /// </summary>
        public Double Random_Node()
        {
            List<Node> LinShi = new List<Node>();//模拟临时节点
            Node RenNode = new Node();//中间变量
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if (i != 4 || j != 4)
                        {
                            if (this.MBoard_backup[i, j].type == Chess.ChessType.Empty)
                            {
                                Node OneTree = new Node();
                                OneTree.pos = this.MBoard_backup[i, j].pos;
                                OneTree.Lix = Chess.oppsiteType(this.MSimLastChess.type);
                                OneTree.Visit_times++;
                                LinShi.Add(OneTree);
                            }
                        }
                    }
                }
            }
            Random rm = new Random();
            int Rendom = -1;
            if (LinShi.Count != 0)
            {
                Rendom = rm.Next(0, LinShi.Count-1);
                RenNode=LinShi[Rendom];
                Chess.ChessType RuleBaselx = ((RenNode.Lix== Chess.ChessType.Black) ? Chess.ChessType.White: Chess.ChessType.Black);
                Dummy_AddChess(RenNode.pos, RuleBaselx);
                Function(RenNode.pos, RuleBaselx);
                return Cens;
            }
            return Cens;
        }
        #endregion

        #region 吃棋子 
        /// <summary>
        ///吃棋子
        /// </summary>
        public void PlayWl(Chess.POS pos, Chess.ChessType Dqlx)
        {
            ChiQI.Clear();//清空
            string Xb = "";
            Xb = pos.posX.ToString() + pos.posY.ToString();
            GOsrule.Recailing(Xb);
            List<TreeB> Ch2 = Zhuofa(Dqlx);
            if (Ch2.Count != 0)
            { GaiQi(Ch2); }
            List<TreeB> q = Gun(pos.posX, pos.posY, Dqlx);
            if (q.Count != 0)
            { GaiQi(q); }
        }
        #endregion

        #region 吃棋子并改变棋子颜色
        /// <summary>
        ///已被吃的棋子改颜色
        /// </summary>
        public void GaiQi(List<TreeB> ChiZi)
        {
            foreach (TreeB i in ChiZi)
            {
                ChiQI.Add(i);//保存可以吃的棋子
                if (m_LastChess.type == Chess.ChessType.White)
                {
                    this.MBoard[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type = Chess.ChessType.White;
                    string Xb = i.N0.ToString() + i.N1.ToString();
                    GOsrule.Recailing(Xb);
                    List<TreeB> Ch2 = Zhuofa(this.MBoard[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type);
                    if (Ch2.Count != 0)
                    { GaiQi(Ch2); }
                    List<TreeB> q = Gun(i.N0, i.N1, this.MBoard[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type);
                    if (q.Count != 0)
                    { GaiQi(q); }

                }
                else if (m_LastChess.type == Chess.ChessType.Black)
                {
                    this.MBoard[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type = Chess.ChessType.Black;
                    string Xb = i.N0.ToString() + i.N1.ToString();
                    GOsrule.Recailing(Xb);
                    List<TreeB> Ch2 = Zhuofa(this.MBoard[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type);
                    if (Ch2.Count != 0)
                    { GaiQi(Ch2); }
                    List<TreeB> q = Gun(i.N0, i.N1, this.MBoard[Convert.ToInt16(i.N0), Convert.ToInt16(i.N1)].type);
                    if (q.Count != 0)
                    { GaiQi(q); }
                }
            }
        }
        #endregion

        #region 遍历当前棋子可以吃法
        /// <summary>
        ///遍历当前棋子可以所以吃法 当前可以吃的棋子存到N0和N1数组
        /// </summary>
        public List<TreeB> Zhuofa(Chess.ChessType Dqlx)
        {
            List<TreeB> ChiZi = new List<TreeB>();
            foreach (RuleBase.TreeA S in RuleBase.temp1)
            {

                TreeB C1 = new TreeB();
                if (MBoard[S.B0, S.B1].type != Dqlx && MBoard[S.C0, S.C1].type == Dqlx && MBoard[S.B0, S.B1].type != Chess.ChessType.Empty)
                {
                    C1.N0 = S.B0;
                    C1.N1 = S.B1;
                    ChiZi.Add(C1);
                }
            }
            return ChiZi;
        }
        #endregion

        #region 长枪
        /// <summary>
        ///查看有无长枪规则 可吃的棋子
        /// </summary>
        public List<TreeB> Gun(int X, int Y, Chess.ChessType Dq)
        {
            string Str = X.ToString() + Y.ToString() ;
            GOsrule.RulSlct(Str);
            List<TreeB> q1 = SomGun(X, Y, Dq);
            return q1;
        }
        #endregion

        #region 检查对角线有无长枪
        /// <summary>
        ///检查对角线有无长枪
        /// </summary>
        public List<TreeB> SomGun(int X, int Y, Chess.ChessType Dq)
        {
            List<TreeB> qiang = new List<TreeB>();
            int Qs = 0;
            if (RuleBase.Q1.Nb1 != -1)
            {
                for (int i = 0; i < RuleBase.Q1.Nb1 + 1; i++)
                {
                    if (MBoard[RuleBase.Q1.AX + i, RuleBase.Q1.AY + i].type == Dq)
                    {
                        Qs++;
                    }
                }
                if (Qs == RuleBase.Q1.Nb1)
                {
                    TreeB B = new TreeB();
                    if (MBoard[RuleBase.Q1.AX, RuleBase.Q1.AY].type != Dq && MBoard[RuleBase.Q1.AX, RuleBase.Q1.AY].type != Chess.ChessType.Empty)
                    {
                        B.N0 = RuleBase.Q1.AX;
                        B.N1 = RuleBase.Q1.AY;
                        qiang.Add(B);
                    }
                    else if (MBoard[RuleBase.Q1.AX + RuleBase.Q1.Nb1, RuleBase.Q1.AY + RuleBase.Q1.Nb1].type != Dq && MBoard[RuleBase.Q1.AX + RuleBase.Q1.Nb1, RuleBase.Q1.AY + RuleBase.Q1.Nb1].type != Chess.ChessType.Empty)
                    {
                        B.N0 = RuleBase.Q1.AX + RuleBase.Q1.Nb1;
                        B.N1 = RuleBase.Q1.AY + RuleBase.Q1.Nb1;
                        qiang.Add(B);
                    }
                }
            }
            if (RuleBase.Q1.Nb2 != -1)
            {
                Qs = 0;
                for (int i = 0; i < RuleBase.Q1.Nb2 + 1; i++)
                {
                    if (MBoard[RuleBase.Q1.BX - i, RuleBase.Q1.BY + i].type == Dq)
                    {
                        Qs++;
                    }
                }
                if (Qs == RuleBase.Q1.Nb2)
                {
                    TreeB B = new TreeB();
                    if (MBoard[RuleBase.Q1.BX, RuleBase.Q1.BY].type != Dq && MBoard[RuleBase.Q1.BX, RuleBase.Q1.BY].type != Chess.ChessType.Empty)
                    {
                        B.N0 = RuleBase.Q1.BX;
                        B.N1 = RuleBase.Q1.BY;
                        qiang.Add(B);
                    }
                    else if (MBoard[RuleBase.Q1.BX - RuleBase.Q1.Nb2, RuleBase.Q1.BY + RuleBase.Q1.Nb2].type != Dq && MBoard[RuleBase.Q1.BX - RuleBase.Q1.Nb2, RuleBase.Q1.BY + RuleBase.Q1.Nb2].type != Chess.ChessType.Empty)
                    {
                        B.N0 = RuleBase.Q1.BX - RuleBase.Q1.Nb2;
                        B.N1 = RuleBase.Q1.BY + RuleBase.Q1.Nb2;
                        qiang.Add(B);
                    }
                }
            }
            List<TreeB> q2 = XoGun(X, Y, Dq);
            foreach (TreeB q in q2)
            {
                qiang.Add(q);
            }
            return qiang;
        }
        #endregion

        #region 检查纵横有无长枪
        /// <summary>
        ///检查纵横有无长枪
        /// </summary>
        public List<TreeB> XoGun(int X, int Y, Chess.ChessType Dq)
        {
            List<TreeB> qiang = new List<TreeB>();
            int Qs = 0;
            if (X % 2 == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (MBoard[i, Y].type == Dq && MBoard[i, Y].type != Chess.ChessType.Empty)
                    {
                        Qs++;
                    }
                    i++;
                }
                if (Qs == 4)
                {
                    TreeB B = new TreeB();
                    if (MBoard[0, Y].type != Dq && MBoard[0, Y].type != Chess.ChessType.Empty)
                    {
                        B.N0 = 0;
                        B.N1 = Y;
                        qiang.Add(B);
                    }
                    else if (MBoard[8, Y].type != Dq && MBoard[8, Y].type != Chess.ChessType.Empty)
                    {
                        B.N0 = 8;
                        B.N1 = Y;
                        qiang.Add(B);
                    }
                }
                Qs = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (MBoard[X, i].type == Dq && MBoard[X, i].type != Chess.ChessType.Empty)
                    {
                        Qs++;
                    }
                    i++;
                }
                if (Qs == 4)
                {
                    TreeB B = new TreeB();
                    if (MBoard[X, 0].type != Dq && MBoard[X, 0].type != Chess.ChessType.Empty)
                    {
                        B.N0 = X;
                        B.N1 = 0;
                        qiang.Add(B);
                    }
                    else if (MBoard[X, 8].type != Dq && MBoard[X, 8].type != Chess.ChessType.Empty)
                    {
                        B.N0 = X;
                        B.N1 = 8;
                        qiang.Add(B);
                    }
                }
            }
            return qiang;
        }
        #endregion

        #region 当前可以移动位置
        /// <summary>
        /// 当前可以移动位置
        /// </summary>
        public Chess Move()
        {
            Chess Kong = new Chess();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {

                        if (this.MBoard[i, j].type == Chess.ChessType.Empty)
                        {
                            Kong = this.MBoard[i, j];
                        }

                    }
                }
            }
            return Kong;
        }
        #endregion
        
        #region 当前可以移动棋子集合
        /// <summary>
        /// 当前可以移动棋子集合
        /// </summary>
        public List<Chess.POS> YiDongJiHe(Chess Kong)
        {
            List<Chess.POS> Kyd = new List<Chess.POS>();
            GOsrule.Lmsql(Kong.pos.posX.ToString() + Kong.pos.posY.ToString());
            foreach (RuleBase.TreeC S in RuleBase.temp3)
            {
                if (this.MBoard[S.A1, S.A2].type == Kong.type)
                {
                    Kyd.Add(this.MBoard[S.A1, S.A2].pos);
                }
            }
            return Kyd;
        }
        #endregion

        #region 计算当前最好的移动位置
        public Chess.POS YdpinGu(List<Chess.POS> Kyd, Chess Kong)
        {
            SimuChess();
            int i = 0;
            Cens = 0;
            double Pingg = 0;
            Chess.POS Zuikq = new Chess.POS();
            foreach (Chess.POS Sa in Kyd)
            {
                MBoard_backup[Sa.posX, Sa.posY].type = Chess.ChessType.Empty;
                MBoard_backup[Kong.pos.posX, Kong.pos.posY].type = Kong.type;
                ViPlay(Kong.pos, Kong.type);
                if (i == 0)
                {
                    Pingg = Cens;
                    Zuikq = Sa;
                }
                i++;
                if (Pingg < Cens)
                {
                    Zuikq = Sa;
                }
                SimuChess();
            }
            return Zuikq;
        }
        #endregion

        #region 计算当前最好的让路位置
        /// <summary>
        /// 计算当前最好的让路位置
        /// </summary>
        /// <param name="Kong"></param>
        /// <returns></returns>
        public RuleBase.TreeC Maximum_Give_way(Chess Kong)
        {
            double MaxNoed = 0;
            RuleBase.TreeC MAxMnm = new RuleBase.TreeC();
            MAxMnm.A1 = -1;
            MAxMnm.A2 = -1;
            int ONe = 0;
            GOsrule.Lmsql(Kong.pos.posX.ToString() + Kong.pos.posY.ToString());
            foreach (RuleBase.TreeC S in RuleBase.temp3)
            {
                if(Fen_chese(S, Kong.type))
                {
                    double MaxVale = this.Give_way_Evaluate(S, Kong);
                    if(ONe ==0)
                    {
                        MAxMnm.A1 =S.A1;
                        MAxMnm.A2 =S.A2;
                        MaxNoed = MaxVale;
                    }
                    ONe++;
                    if (MaxNoed <= MaxVale)
                    {
                        MaxNoed = MaxVale;
                        MAxMnm = S;
                    }
                }
               
            }

            if(MAxMnm.A1==-1 && MAxMnm.A2 == -1)
            {
                Jump_Chess(Kong);
                //调用挑起让路
                foreach (TreeB Ts in TiaoQI)
                {
                    RuleBase.TreeC S = new RuleBase.TreeC();
                    S.A1 = Ts.N0;
                    S.A2 = Ts.N1;
                    double MaxVale = this.Give_way_Evaluate(S, Kong);
                    if (MaxNoed <= MaxVale)
                    {
                        MaxNoed = MaxVale;
                        MAxMnm = S;
                    }
                }
            }
            return MAxMnm;
         }
        #endregion

        #region 模拟让路返回评估值
        /// <summary>
        /// 模拟让路返回评估值
        /// </summary>
        /// <param name="Place"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public double Give_way_Evaluate(RuleBase.TreeC Place, Chess Type)
        {
            double Max = 0;
            Cens = 0;
            SimuChess();
            this.MBoard_backup[Place.A1, Place.A2].type = Chess.ChessType.Empty;
            this.MBoard_backup[Type.pos.posX, Type.pos.posY].type = Type.type;
            ViPlay(this.MBoard_backup[Type.pos.posX, Type.pos.posY].pos, Type.type);//模拟吃子
            Max = Cens;
            Cens = 0;
            GOsrule.Dumm_Move(Place.A1.ToString() + Place.A2.ToString());
            foreach(RuleBase.TreeC node in RuleBase.Dumm_Plac)
            {
                if (this.MBoard_backup[node.A1, node.A2].type == Chess.oppsiteType(Type.type))
                {
                    this.MBoard_backup[node.A1, node.A2].type = Chess.ChessType.Empty;
                    this.MBoard_backup[Place.A1, Place.A2].type = Chess.oppsiteType(Type.type);
                    ViPlay(this.MBoard_backup[Place.A1, Place.A2].pos, Chess.oppsiteType(Type.type));//模拟吃子
                    Max = Max - Cens;
                }
            }
            return Max;
        }
        #endregion

        #region 跳棋让路
        /// <summary>
        /// 跳棋让路
        /// </summary>
        /// <param name="Dong"></param>
        public void Jump_Chess(Chess Dong)
        {
            SimuChess();
            for (int j=0;j<4;j+=2)
            {
                Five_Case[0].N0 = 1; Five_Case[0].N1 = 1;
                Five_Case[1].N0 = 0; Five_Case[1].N1 = 0;
                Five_Case[2].N0 = 0; Five_Case[2].N1 = 2;
                Five_Case[3].N0 = 2; Five_Case[3].N1 = 0;
                Five_Case[4].N0 = 2; Five_Case[4].N1 = 2;
                for (int i = 0; i < 4; i+= 2)
                {
                    Five_Case[0].N0 +=j; Five_Case[0].N1 +=i;
                    Five_Case[1].N0 +=j; Five_Case[1].N1 +=i;
                    Five_Case[2].N0 +=j; Five_Case[2].N1 +=i;
                    Five_Case[3].N0 +=j; Five_Case[3].N1 +=i;
                    Five_Case[4].N0 +=j; Five_Case[4].N1 +=i;

                    if( this.MBoard_backup[Five_Case[0].N0, Five_Case[0].N1].type ==Dong.type)
                    {
                        for(int k=1;k<5;k++)
                        {
                            if (this.MBoard_backup[Five_Case[k].N0, Five_Case[k].N1].type != Dong.type)
                            {
                                Hashcreate(Five_Case[k]);
                            }
                        }
                    }

                }
            }

            TreeB MY = new TreeB();
            if (this.MBoard_backup[0, 2].type == Dong.type)
            {
                if(this.MBoard_backup[1,1].type != Dong.type)
                {
                    MY.N0 = 1; MY.N1 = 1;
                    Hashcreate(MY);
                }
                if(this.MBoard_backup[1, 3].type != Dong.type)
                {
                    MY.N0 = 1; MY.N1 =3;
                    Hashcreate(MY);
                }
            }

            if (this.MBoard_backup[0, 6].type == Dong.type)
            {
                if (this.MBoard_backup[1, 5].type != Dong.type)
                {
                    MY.N0 = 1; MY.N1 = 5;
                    Hashcreate(MY);
                }
                if (this.MBoard_backup[1, 7].type != Dong.type)
                {
                    MY.N0 = 1; MY.N1 = 7;
                    Hashcreate(MY);
                }
            }

            if (this.MBoard_backup[4,2].type == Dong.type)
            {
                if (this.MBoard_backup[3, 1].type != Dong.type)
                {
                    MY.N0 = 3; MY.N1 =1;
                    Hashcreate(MY);
                }
                if (this.MBoard_backup[3, 3].type != Dong.type)
                {
                    MY.N0 =3; MY.N1 =3;
                    Hashcreate(MY);
                }

                if (this.MBoard_backup[5, 1].type != Dong.type)
                {
                    MY.N0 =5; MY.N1 = 1;
                    Hashcreate(MY);
                }
                if (this.MBoard_backup[5, 3].type != Dong.type)
                {
                    MY.N0 = 5; MY.N1 = 3;
                    Hashcreate(MY);
                }
            }

            if (this.MBoard_backup[4, 6].type == Dong.type)
            {
                if (this.MBoard_backup[3, 5].type != Dong.type)
                {
                    MY.N0 = 3; MY.N1 = 5;
                    Hashcreate(MY);
                }
                if (this.MBoard_backup[3, 7].type != Dong.type)
                {
                    MY.N0 = 3; MY.N1 = 7;
                    Hashcreate(MY);
                }

                if (this.MBoard_backup[5,5].type != Dong.type)
                {
                    MY.N0 = 5; MY.N1 =5;
                    Hashcreate(MY);
                }
                if (this.MBoard_backup[5, 7].type != Dong.type)
                {
                    MY.N0 = 5; MY.N1 = 7;
                    Hashcreate(MY);
                }
            }

            if (this.MBoard_backup[8, 2].type == Dong.type)
            {
                if (this.MBoard_backup[7, 1].type != Dong.type)
                {
                    MY.N0 = 7; MY.N1 = 1;
                    Hashcreate(MY);
                }
                if (this.MBoard_backup[7, 3].type != Dong.type)
                {
                    MY.N0 = 7; MY.N1 = 3;
                    Hashcreate(MY);
                }
            }

            if (this.MBoard_backup[8, 6].type == Dong.type)
            {
                if (this.MBoard_backup[7, 5].type != Dong.type)
                {
                    MY.N0 = 7; MY.N1 = 5;
                    Hashcreate(MY);
                }
                if (this.MBoard_backup[7, 7].type != Dong.type)
                {
                    MY.N0 = 7; MY.N1 = 7;
                    Hashcreate(MY);
                }
            }
        }

        /// <summary>
        /// 哈希遍历 去掉重复的点
        /// </summary>
        /// <param name="Wong"></param>
        public void Hashcreate(TreeB Wong)
        {
            string str = Wong.N0.ToString() + Wong.N1.ToString();
            if (!ht.Contains(str))//查找i是否在表中 不在是 
            {
                ht.Add(str, 1);    //把i插入到表中 并且频率为1
                TiaoQI.Add(Wong);
            }
               
        }
        #endregion

        #region 查看周围有无对方棋子
        /// <summary>
        /// 查看周围有无对方棋子
        /// </summary>
        /// <param name="S"></param>
        /// <param name="Mytype"></param>
        /// <returns></returns>
        public bool Fen_chese(RuleBase.TreeC S, Chess.ChessType Mytype)
        {
            bool You = false;
            GOsrule.Lm_New(S.A1.ToString() + S.A2.ToString());
            foreach (RuleBase.TreeC L in RuleBase.New_temp3)
            {
                if (this.MBoard[L.A1,L.A2].type == Chess.oppsiteType(Mytype))
                {
                    You =true;
                }
            }
                return You;
        }
        #endregion



        #region 增加一颗棋子在棋盘上
        /// <summary>
        /// 增加一颗棋子在棋盘上
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool addChess(Chess.POS pos, Chess.ChessType type)
        {
            if (this.m_currentStep > 41)
            {
                this.isGameOver = true;
                return false;
            }
            //只允许放黑棋或白棋
            if (type != Chess.ChessType.Black && type != Chess.ChessType.White)
                return false;
            // 只允许在空位置上放棋子。
            if (this.MBoard[pos.posX, pos.posY].type != Chess.ChessType.Empty)
            { return false; }
            if ((pos.posX + pos.posY) % 2 == 1)
            { return false; }
            // 设置当前位置的棋子的类型
            this.MBoard[pos.posX, pos.posY].type = type;
            //Save1.AppRC(pos.posX.ToString() + "," + pos.posY.ToString(), type.ToString());//记录棋步
            //formPlaying.InitChess(pos.posX, pos.posY, type);
            this.m_LastChess.pos = pos;
            this.m_LastChess.type = type;
            this.m_LastChess.step = this.m_currentStep;
            this.MBoard[pos.posX, pos.posY].step = this.m_currentStep;
            this.MBoard[pos.posX, pos.posY].pos = pos;
            this.m_currentStep++;

            return true;
        }
        #endregion

        #region 吃子改棋颜色

        public void GaiYan(Chess.POS pos, Chess.ChessType type)
        {
            if (type == Chess.ChessType.Black)
            {
                this.MBoard[pos.posX, pos.posY].type = Chess.ChessType.Black;
            }
            else if (type == Chess.ChessType.White)
            {
                this.MBoard[pos.posX, pos.posY].type = Chess.ChessType.White;
            }
        }
        #endregion

        #region 移动棋子
        /// <summary>
        ///移动棋子
        /// </summary>
        public void YdQz(int X, int Y, int Xyuan, int Yyuan, Chess.ChessType Lix)
        {
            this.MBoard[X, Y].type = Lix;
            this.MBoard[Xyuan, Yyuan].type = Chess.ChessType.Empty;
            m_LastChess.pos = this.MBoard[X, Y].pos;
            m_LastChess.type = Lix;
            m_LastChess.step = m_currentStep;
            //string Weizhi1 = Xyuan.ToString() + "," + Yyuan.ToString();
            //string Weizhi2 = X.ToString() + "," + Y.ToString();
            //Save1.AppTR(Weizhi1, Weizhi2, Lix.ToString());
        }
        #endregion

        #region 查看是否满盘
        public bool Full()
        {
            bool Fl = false;
           int zongs = 0;
           for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i + j) % 2 == 0 || i + j == 0)
                    {
                        if (this.MBoard[i, j].type != Chess.ChessType.Empty)
                        {
                            zongs++;
                        }
                    }
                }
            }
            if (zongs == 40)
            {
                Fl = true;
            }
            return Fl;
        }
        #endregion

    }
}
