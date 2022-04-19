using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGO
{
    /// <summary>
    /// 蒙特卡罗树搜索的树结构的Node，包含了父节点和直接点等信息，还有用于计算UCB的遍历次数和quality值，还有游戏选择这个Node的State。
    /// </summary>
     public   class Node
    {
       
        /// <summary>
        /// 父节点
        /// </summary>
        public  Node Parent { get; set; }
        /// <summary>
        /// 孩子节点
        /// </summary>
        public   List<Node> Children = new List<Node>();
        /// <summary>
        /// 访问次数N
        /// </summary>
        public  int Visit_times { get; set; }
        /// <summary>
        /// 收益值W
        /// </summary>
        public  double Quality_value { get; set; }
        /// <summary>
        /// 当前节点状态
        /// </summary>
       public  State Self_State = new State();
        /// <summary>
        /// 棋步位置
        /// </summary>
        public Chess.POS pos;
        /// <summary>
        /// 棋类
        /// </summary>
        public Chess.ChessType Lix = new Chess.ChessType();
        /// <summary>
        /// 评估值 
        /// </summary>
        public double Eat { get; set; }
        
       
        /// <summary>
        /// 是否被展开
        /// </summary>
        /// <returns></returns>
        public bool Is_all_expand()
        {
            bool Unfold = true;
            if(this.Children.Count<=2)
            {
                Unfold = false;
            }
            return Unfold;
        }

        public bool New_Is_all_expand()
        {
            bool Unfold = true;
            if (this.Visit_times == 0)
            {
                Unfold = false;
            }
            return Unfold;
        }

        public Node MaxiNoad()
        {
            Node Maxi = new Node();
            Maxi.Eat = 0.0f;
            Double cont = 0;
            foreach (Node MM in this.Children)
                {
                    if (MM.Eat > Maxi.Eat)
                    {
                       Maxi = MM;
                    }
                    else if(MM.Eat ==Maxi.Eat)
                    {
                     cont++;
                    }
                    else if(MM.Eat< Maxi.Eat)
                    {
                      cont++;
                    }
                }
            double Gel = cont/Convert.ToDouble( this.Children.Count);
            Random rm = new Random();
            if (Gel==1.0)
            {
              int  Rendom = rm.Next(0, this.Children.Count - 1);
                Maxi = this.Children[Rendom];
            }
            this.Children.Remove(Maxi);
            //Maxi.Visit_times++;
            return Maxi;
        }

        public Node CoNode()
        {
            Node Mm = new Node();
           
            while (this.Children.Count!=0)
            {
                if (this.Children[0].Children.Count!=0)
                {
                    Mm = Mm.CoNode();
                }
                else
                {
                    Mm = this.Children[0].Parent;
                    break;
                }
            } 
            return Mm;
        }

        public Node GetNode(Node Mm, Node Maxe)
        {
            Node Mn = new Node();
            foreach (Node M in Mm.Children)
            {
                if (M.pos == Maxe.pos)
                {
                    Mn = M;
                    break;
                }
            }
            return Mn;
        }
        public void Visit_times_add_one()
        {
            this.Visit_times += 1;
        }

        public void Quality_value_add_n(double n)
        {
            this.Quality_value += n;
        }

        public void New_Visit_times_add_one(int Visit)
        {
            this.Visit_times += Visit;
        }

        public void Nwe_Quality_value_add_n(double n)
        {
            this.Quality_value += n;
        }

    }
}
