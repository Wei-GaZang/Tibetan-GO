using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGO
{
     public  class Chess
    {

        /// <summary>
        /// 棋子的类型
        /// </summary>
        public enum ChessType
        {
            Empty = 0,//空子
            Black = 1,//黑子
            White = 2,//白子
            MaybeBlack = 3,//
            MaybeWhite = 4,//
            MaybePending = 5,//
            DeadBlack = 6,//
            DeadWhite = 7//
        }

        /// <summary>
        /// 棋子的位置
        /// </summary>
        public struct POS
        {
            /// <summary>
            /// 棋子的横坐标
            /// </summary>
            public int posX;
            /// <summary>
            /// 棋子的纵坐标
            /// </summary>
            public int posY;
            /// <summary>
            /// 创建棋子的位置
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public POS(int x, int y)
            {
                posX = x;
                posY = y;
            }
            /// <summary>
            /// 判断两个棋子是否位置一样
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static bool operator ==(POS a, POS b)
            {
                return a.posX == b.posX && a.posY == b.posY;
            }
            /// <summary>
            /// 判断两个棋子是否位置不一样
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static bool operator !=(POS a, POS b)
            {
                return !(a == b);
            }
            /// <summary>
            /// 获取棋子是否在棋盘上
            /// </summary>
            public bool isValid
            {
                get
                {
                    return (posX >= 0 && posY >= 0 && posX <=8&& posY <=8);
                }
            }
            /// <summary>
            /// 把棋子移出棋盘
            /// </summary>
            public void setToInvalid()
            {
                this.posX = -1;
                this.posY = -1;
            }
            /// <summary>
            /// 获取棋子与左边缘是否还有位置
            /// </summary>
            public bool hasLeft { get { return isValid && posX > 0; } }
            /// <summary>
            /// 获取棋子与右边缘是否还有位置
            /// </summary>
            public bool hasRight { get { return isValid && posX < 18; } }
            /// <summary>
            /// 获取棋子与上边缘是否还有位置
            /// </summary>
            public bool hasUp { get { return isValid && posY > 0; } }
            /// <summary>
            /// 获取棋子与下边缘是否还有位置
            /// </summary>
            public bool hasDown { get { return isValid && posY < 18; } }

            // To avoid warning CS0660, CS0661
            /// <summary>
            /// 比较两个对象是否相等
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

        }// End   struct POS

        /// <summary>
        /// 棋子的类型
        /// </summary>
        private ChessType m_type;
        /// <summary>
        /// 棋子的位置
        /// </summary>
        private POS m_pos;
        /// <summary>
        /// 棋子的序号
        /// </summary>
        private int m_step;

        /// <summary>
        /// 获取或设置棋子的类型
        /// </summary>
        public ChessType type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// 获取或设置棋子的位置
        /// </summary>
        public POS pos
        {
            get { return m_pos; }
            set { m_pos = value; }
        }

        /// <summary>
        /// 获取或设置棋子的序号
        /// </summary>
        public int step
        {
            get { return m_step; }
            set { m_step = value; }
        }

        /// <summary>
        /// 创建空棋子
        /// </summary>
        public Chess()
        {
            this.m_pos = new POS(-1, -1);
        }



        /// <summary>
        /// 创建带序号的棋子
        /// </summary>
        public Chess(POS pos, ChessType type, int step)
        {
            m_pos = new POS(pos.posX, pos.posY);
            m_type = type;
            m_step = step;
        }

        /// <summary>
        /// 创建不带序号的棋子
        /// </summary>
        public Chess(POS pos, ChessType type) : this(pos, type, -1) { }


        /// <summary>
        /// 获取反方棋子的类型
        /// </summary>
        public static ChessType oppsiteType(ChessType type)
        {
            switch (type)
            {
                case ChessType.Black:
                    return ChessType.White;
                case ChessType.White:
                    return ChessType.Black;
                default:
                    return ChessType.Empty;
            }
        }

        /// <summary>
        /// 获取棋子的序号，横坐标，纵坐标，类型信息合成的字符串
        /// </summary>
        public override string ToString()
        {
            string str = m_step.ToString() + "," + m_pos.posX.ToString() + "," + m_pos.posY.ToString() + "," + m_type.ToString();
            return str;
        }

        /// <summary>
        /// 获取特定的棋子
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Chess Parse(string s)
        {
            char[] seperator = { ',' };
            string[] strArray = s.Split(seperator);
            if (strArray.Length != 4)   //格式错误
                return null;

            int step = int.Parse(strArray[0]);
            int posX = int.Parse(strArray[1]);
            int posY = int.Parse(strArray[2]);
            string strType = strArray[3];
            if (strType != "Black" && strType != "White")
                return null;
            Chess.ChessType type = (strType == "Black") ? Chess.ChessType.Black : Chess.ChessType.White;
            Chess.POS pos = new POS(posX, posY);

            return new Chess(pos, type, step);
        }

        /// <summary>
        /// 复制一个完整棋子
        /// </summary>
        /// <returns></returns>
        internal Chess Copy()
        {
            return new Chess(this.pos, this.type, this.step);
        }
    }
}
