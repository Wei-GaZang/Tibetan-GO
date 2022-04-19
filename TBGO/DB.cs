using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
namespace TBGO
{
     public  class DB
    {
        public static string Sqlstr = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source ="+Application.StartupPath +@"\\TBGO.accdb";
        OleDbConnection myconn = new OleDbConnection(Sqlstr);
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
        public static  List<TreeA> temp1 = new List<TreeA>();
        /// <summary>
        ///长枪的头坐标 两个对角线的头坐标 并对角线上的棋子数
        /// </summary>
        public struct TreeB
        {
            public int AX;
            public int AY;
            public int Nb1;
            public int BX;
            public int BY;
            public int Nb2;
        }
        /// <summary>
        ///长枪头坐标集合 两个对角线的头坐标
        /// </summary>
        public static TreeB Q1 = new TreeB();
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
        public static List<TreeC> temp3= new List<TreeC>();
        /// <summary>
        ///存当前棋子四周位置集合
        /// </summary>
        public static List<TreeC> New_temp3 = new List<TreeC>();
        /// <summary>
        ///存当前虚拟点的棋子四周位置集合
        /// </summary>
        public static List<TreeC> Dumm_Plac = new List<TreeC>();

        #region  查询吃法
        /// <summary>
        /// 查询吃法
        /// </summary>
        public void RuleBase(string XB)
        {
            try
            {
                myconn.Open();
                OleDbCommand mycom = new OleDbCommand("select Mid1,Mid2,Term1,Term2 from GOs where Point='"+XB+"'", myconn);
                OleDbDataReader dr = mycom.ExecuteReader();
                temp1.Clear();
                while (dr.Read())//数据库中的规则导入到字典中
                {
                    TreeA D = new TreeA();
                    D.B0= Convert.ToInt16( dr["Mid1"]);
                    D.B1 = Convert.ToInt16(dr["Mid2"]);
                    D.C0 = Convert.ToInt16(dr["Term1"]);
                    D.C1 = Convert.ToInt16(dr["Term2"]);
                    temp1.Add(D);
                }
                dr.Close();
                myconn.Close();
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
        public void BeiBase(string XB)
        {
            try
            {
                myconn.Open();
                OleDbCommand mycom = new OleDbCommand("select Point1,Point2,Mid1,Mid2 from GOc where Point='" + XB + "'", myconn);
                OleDbDataReader dr = mycom.ExecuteReader();
                temp1.Clear();
                while (dr.Read())//数据库中的规则导入到字典中
                {
                    TreeA D = new TreeA();
                    D.B0 = Convert.ToInt16(dr["Point1"]);
                    D.B1 = Convert.ToInt16(dr["Point2"]);
                    D.C0 = Convert.ToInt16(dr["Mid1"]);
                    D.C1 = Convert.ToInt16(dr["Mid2"]);
                    temp1.Add(D);
                }
                dr.Close();
                myconn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  长枪规则
        /// <summary>
        /// 查询长枪
        /// </summary>
        public void Rul(string str)
        {
            try
            {
                myconn.Open();
                OleDbCommand mycom = new OleDbCommand(str, myconn);
                OleDbDataReader dr = mycom.ExecuteReader();
                while (dr.Read())//数据库中的规则导入到字典中
                {
                   
                    Q1.AX= Convert.ToInt16(dr["X1"]);
                    Q1.AY = Convert.ToInt16(dr["Y1"]);
                    Q1.Nb1 = Convert.ToInt16(dr["Max1"]);
                    Q1.BX = Convert.ToInt16(dr["X2"]);
                    Q1.BY= Convert.ToInt16(dr["Y2"]);
                    Q1.Nb2 = Convert.ToInt16(dr["Max2"]);
                    
                }
                dr.Close();
                myconn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  着法规则
        /// <summary>
        /// 查询路
        /// </summary>
        public void Lm(string str)
        {
            try
            {
                myconn.Open();
                OleDbCommand mycom = new OleDbCommand("select Mid1, Mid2 from GOt where Point = '"+str+"'", myconn);
                OleDbDataReader dr = mycom.ExecuteReader();
                temp3.Clear();
                while (dr.Read())//数据库中的规则导入到字典中
                {
                    TreeC D = new TreeC();
                    D.A1= Convert.ToInt16(dr["Mid1"]);
                    D.A2= Convert.ToInt16(dr["Mid2"]);
                    temp3.Add(D);
                }
                dr.Close();
                myconn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  着法规则
        /// <summary>
        /// 查询路
        /// </summary>
        public void Lm_New(string str)
        {
            try
            {
                myconn.Open();
                OleDbCommand mycom = new OleDbCommand("select Mid1, Mid2 from GOt where Point = '" + str + "'", myconn);
                OleDbDataReader dr = mycom.ExecuteReader();
                New_temp3.Clear();
                while (dr.Read())//数据库中的规则导入到字典中
                {
                    TreeC D = new TreeC();
                    D.A1 = Convert.ToInt16(dr["Mid1"]);
                    D.A2 = Convert.ToInt16(dr["Mid2"]);
                    New_temp3.Add(D);
                }
                dr.Close();
                myconn.Close();
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
        public void Sfq(string str)
        {
            try
            {
             
               myconn.Open();
                OleDbCommand mycom = new OleDbCommand("SELECT Term1, Term2 FROM GOD WHERE  Point ='"+str+"'", myconn);
                OleDbDataReader dr = mycom.ExecuteReader();
                temp3.Clear();
                while (dr.Read())//数据库中的规则导入到字典中
                {
                    TreeC D = new TreeC();
                    D.A1 = Convert.ToInt16(dr["Term1"]);
                    D.A2 = Convert.ToInt16(dr["Term2"]);
                    temp3.Add(D);
                }
                dr.Close();
                myconn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion

        #region  查询虚拟点的周围
        /// <summary>
        /// 查询路
        /// </summary>
        public void Dumm_Move(string str)
        {
            try
            {
                myconn.Open();
                OleDbCommand mycom = new OleDbCommand("select Mid1, Mid2 from GOt where Point = '" + str + "'", myconn);
                OleDbDataReader dr = mycom.ExecuteReader();
                Dumm_Plac.Clear();
                while (dr.Read())//数据库中的规则导入到字典中
                {
                    TreeC D = new TreeC();
                    D.A1 = Convert.ToInt16(dr["Mid1"]);
                    D.A2 = Convert.ToInt16(dr["Mid2"]);
                    Dumm_Plac.Add(D);
                }
                dr.Close();
                myconn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        #endregion
    }
}
