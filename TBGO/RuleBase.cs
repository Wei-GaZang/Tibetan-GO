using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace TBGO
{
     public  class RuleBase
    {
        #region  公共变量
        public static string Sqlstr = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source =" + Application.StartupPath + @"\\TBGO.accdb";
        OleDbConnection myconn = new OleDbConnection(Sqlstr);
        /// <summary>
        /// 可以吃的两个坐标
        /// </summary>
        public struct GOs
        {
            public string Point;
            public int B0;
            public int B1;
            public int C0;
            public int C1;
        }
        /// <summary>
        ///自己可以吃的规则库
        /// </summary>
        public static List<GOs> Taking = new List<GOs>();

        /// <summary>
        /// 对方可能的吃法
        /// </summary>
        public struct GOc
        {
            public int B0;
            public int B1;
            public string Point;
            public int C0;
            public int C1;
        }
        /// <summary>
        ///自己可以吃的规则库
        /// </summary>
        public static List<GOc> TakingOther = new List<GOc>();

        /// <summary>
        ///长枪的头坐标 两个对角线的头坐标 并对角线上的棋子数
        /// </summary>
        public struct GOp
        {
            public string Point;
            public int AX;
            public int AY;
            public int Nb1;
            public int BX;
            public int BY;
            public int Nb2;
        }
        /// <summary>
        ///长枪规则库
        /// </summary>
        public static List<GOp> TakingRoul = new List<GOp>();

        /// <summary>
        ///存当前棋子四周位置
        /// </summary>
        public struct GOt
        {
            public int A1;
            public int A2;
            public string Point;
        }
        /// <summary>
        ///棋子四周位置集合规则库
        /// </summary>
        public static List<GOt> TakingWei = new List<GOt>();

        /// <summary>
        ///棋子四周位置集合规则库
        /// </summary>
        public static List<GOt> TakingD = new List<GOt>();
        #endregion

        #region  获取吃法规则库
        /// <summary>
        /// 查询吃法
        /// </summary>
        public void RuleBaseEat()
        {
            try
            {
                string textnipu = "";
                string file = Application.StartupPath + @"\\GOs.txt";
                StreamReader sr = new StreamReader(file);
                while (sr.EndOfStream != true)
                {
                    textnipu = sr.ReadLine();
                    if (textnipu != "")
                    {
                        string[] Row1 = textnipu.Split(' ');
                        GOs D = new GOs();
                        D.Point = Row1[0];
                        D.B0 = Convert.ToInt16(Row1[1]);
                        D.B1 = Convert.ToInt16(Row1[2]);
                        D.C0 = Convert.ToInt16(Row1[3]);
                        D.C1 = Convert.ToInt16(Row1[4]);
                        Taking.Add(D);
                    }
                }
                sr.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region   获取对方可以吃法规则库
        /// <summary>
        /// 获取对方可以吃法规则库
        /// </summary>
        public void BeiBase()
        {
            try
            {
                string textnipu = "";
                string file = Application.StartupPath + @"\\GOc.txt";
                StreamReader sr = new StreamReader(file);
                while (sr.EndOfStream != true)
                {
                    textnipu = sr.ReadLine();
                    if (textnipu != "")
                    {
                        string[] Row1 = textnipu.Split(' ');
                        GOc D = new GOc();
                        D.B0 = Convert.ToInt16(Row1[0]);
                        D.B1 = Convert.ToInt16(Row1[1]);
                        D.Point = Row1[2];
                        D.C0 = Convert.ToInt16(Row1[3]);
                        D.C1 = Convert.ToInt16(Row1[4]);
                        TakingOther.Add(D);
                    }
                }
                sr.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  获取长枪规则库
        /// <summary>
        /// 获取长枪规则库
        /// </summary>
        public void Rul()
        {
            try
            {
                string textnipu = "";
                string file = Application.StartupPath + @"\\GOp.txt";
                StreamReader sr = new StreamReader(file);
                while (sr.EndOfStream != true)
                {
                    textnipu = sr.ReadLine();
                    if (textnipu != "")
                    {
                        string[] Row1 = textnipu.Split(' ');
                        GOp D = new GOp();
                        D.AX = Convert.ToInt16(Row1[0]);
                        D.AY = Convert.ToInt16(Row1[1]);
                        D.Nb1 = Convert.ToInt16(Row1[2]);
                        D.BX = Convert.ToInt16(Row1[3]);
                        D.BY = Convert.ToInt16(Row1[4]);
                        D.Nb2 = Convert.ToInt16(Row1[5]);
                        D.Point = Row1[6];
                        TakingRoul.Add(D);
                    }
                }
                sr.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  获取着法规则库
        /// <summary>
        /// 获取着法规则库
        /// </summary>
        public void Lm()
        {
            try
            {
                string textnipu = "";
                string file = Application.StartupPath + @"\\GOt.txt";
                StreamReader sr = new StreamReader(file);
                while (sr.EndOfStream != true)
                {
                    textnipu = sr.ReadLine();
                    if (textnipu != "")
                    {
                        string[] Row1 = textnipu.Split(' ');
                        GOt D = new GOt();
                        D.Point = Row1[0];
                        D.A1 = Convert.ToInt16(Row1[1]);
                        D.A2 = Convert.ToInt16(Row1[2]);
                        TakingWei.Add(D);
                    }
                }
                sr.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  查询看对方棋子
        /// <summary>
        /// 查询看对方棋子
        /// </summary>
        public void Sfq()
        {
            try
            {
                string textnipu = "";
                string file = Application.StartupPath + @"\\GOD.txt";
                StreamReader sr = new StreamReader(file);
                while (sr.EndOfStream != true)
                {
                    textnipu = sr.ReadLine();
                    if (textnipu != "")
                    {
                        string[] Row1 = textnipu.Split(' ');
                        GOt D = new GOt();
                        D.A1 = Convert.ToInt16(Row1[0]);
                        D.A2 = Convert.ToInt16(Row1[1]);
                        D.Point = Row1[2];
                        TakingD.Add(D);
                    }
                }
                sr.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  获取藏式夹棋规则库

        /// <summary>
        /// 获取藏式夹棋规则库
        /// </summary>
        public void Gzk()
        {
            try
            {
             Taking.Clear();
             TakingOther.Clear();
             TakingRoul.Clear();
             TakingWei.Clear();
             TakingD.Clear();
             RuleBaseEat();
             BeiBase();
             Rul();
             Lm();
             Sfq();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion


        /// <summary>
        /// 可以吃的两个坐标
        /// </summary>
        public struct TreeA
        {
            public int B0;
            public int B1;
            public int C0;
            public int C1;
        }
        /// <summary>
        ///可以吃的坐标集合
        /// </summary>
        public static List<TreeA> temp1 = new List<TreeA>();

        #region  查询吃法
        /// <summary>
        /// 查询吃法
        /// </summary>
        public void Recailing(string XB)
        {
            try
            {
                temp1.Clear();
                foreach (GOs B in Taking)
                {
                    if(XB==B.Point)
                    {
                        TreeA D = new TreeA();
                        D.B0 =B.B0;
                        D.B1 = B.B1;
                        D.C0 = B.C0;
                        D.C1 =B.C1;
                        temp1.Add(D);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region   对方可以吃法查询
        /// <summary>
        /// 对方可以吃法查询
        /// </summary>
        public void BeiBa(string XB)
        {
            try
            {
                temp1.Clear();
                foreach (GOc B in TakingOther)
                {
                    if (XB == B.Point)
                    {
                        TreeA D = new TreeA();
                        D.B0 = B.B0;
                        D.B1 = B.B1;
                        D.C0 = B.C0;
                        D.C1 = B.C1;
                        temp1.Add(D);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion
        /// <summary>
        ///长枪头坐标集合 两个对角线的头坐标
        /// </summary>
        public static GOp Q1 = new GOp();

        #region  长枪规则
        /// <summary>
        /// 查询长枪
        /// </summary>
        public void RulSlct(string str)
        {
            try
            {

                foreach (GOp B in TakingRoul)
                {
                    if (str == B.Point)
                    {
                        Q1.AX =B.AX;
                        Q1.AY =B.AY;
                        Q1.Nb1 =B.Nb1;
                        Q1.BX =B.BX;
                        Q1.BY =B.BY;
                        Q1.Nb2 = B.Nb2;
                        break;
                    }
                }
                   
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        /// <summary>
        ///存当前棋子四周位置
        /// </summary>
        public struct TreeC
        {
            public int A1;
            public int A2;
        }
        /// <summary>
        ///存当前棋子四周位置集合
        /// </summary>
        public static List<TreeC> temp3 = new List<TreeC>();

        #region  着法规则
        /// <summary>
        /// 查询路
        /// </summary>
        public void Lmsql(string str)
        {
            try
            {
               
                temp3.Clear();
                foreach (GOt B in TakingWei)
                {
                    if (str == B.Point)
                    {
                        TreeC D = new TreeC();
                        D.A1 =B.A1;
                        D.A2 =B.A2;
                        temp3.Add(D);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        /// <summary>
        ///存当前棋子四周位置集合
        /// </summary>
        public static List<TreeC> New_temp3 = new List<TreeC>();
        #region  new着法规则
        /// <summary>
        /// 查询路
        /// </summary>
        public void Lm_New(string str)
        {
            try
            {
                
                New_temp3.Clear();
                foreach (GOt B in TakingWei)
                {
                    if (str == B.Point)
                    {
                        TreeC D = new TreeC();
                        D.A1 = B.A1;
                        D.A2 = B.A2;
                        New_temp3.Add(D);
                    }
                } 
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  查询看对方棋子
        /// <summary>
        /// 查询看对方棋子
        /// </summary>
        public void SoSql(string str)
        {
            try
            {
                temp3.Clear();
                foreach (GOt B in TakingD)
                {
                    if (str == B.Point)
                    {
                        TreeC D = new TreeC();
                        D.A1 = B.A1;
                        D.A2 = B.A2;
                        temp3.Add(D);
                    }
                }
               
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        /// <summary>
        ///存当前虚拟点的棋子四周位置集合
        /// </summary>
        public static List<TreeC> Dumm_Plac = new List<TreeC>();

        #region  查询虚拟点的周围
        /// <summary>
        /// 查询路
        /// </summary>
        public void Dumm_Move(string str)
        {
            try
            {
                Dumm_Plac.Clear();
                foreach (GOt B in TakingWei)
                {
                    if (str == B.Point)
                    {
                        TreeC D = new TreeC();
                        D.A1 = B.A1;
                        D.A2 = B.A2;
                        Dumm_Plac.Add(D);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion
    }
}
