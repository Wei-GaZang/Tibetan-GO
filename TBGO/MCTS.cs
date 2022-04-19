using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGO
{
    public class MCTS
    {


        Node MYnode = new Node();
        public  int Times = 0;
       
        public static Board NowBoard { get; set; }
        /// <summary>
        /// 已访问节点
        /// </summary>
        Node Expand_node_Maxi = new Node();
        /// <summary>
        /// 当前AI的棋子颜色
        /// </summary>
       public static  Chess.ChessType Aitype = new Chess.ChessType();

        
        #region  MCTS
        public string Mmonte_carlo_tree_search(Board Ld,int side)
        {
            
            NowBoard = Ld;
            NowBoard.Side = side; //当前棋类型
           
            string str = "0,2";
            for (int i=0;i<3;i++)
            {
                //1.找到要扩展的最佳节点
                Node MaxNode = Tree_policy(MYnode);

                //2.随机运行添加节点并获得奖励
                double reward=Default_policy(MaxNode);

                //3.使用奖励更新所有传递的节点
                Backup(MaxNode,reward);
                
            }
            Times = 0;
            //N.获得最佳下一个节点
            Node Best_next_node = Best_child(MYnode,false);
            str = Best_next_node.pos.posX.ToString() + "," + Best_next_node.pos.posY.ToString();
            return str;
        }
        #endregion

        #region Selection和Expansion阶段
        /// <summary>
        ///  蒙特卡罗树搜索的Selection和Expansion阶段，传入当前需要开始搜索的节点（例如根节点），根据exploration/exploitation算法返回最好的需要expend的节点，注意如果节点是叶子结点直接返回。
        /// </summary>
        /// <param name="Mnode"></param>
        public Node Tree_policy(Node Mnode)
        {
            while (!Mnode.Self_State.Is_terminal())
            {//应该是调用着函数计算可走的路个数
                if (Mnode.Is_all_expand())
                {

                }
                else
                {
                    Node MLnode = Expand(Mnode);
                    return MLnode;
                }
            }
            return Mnode;
        }
       


        /// <summary>
        /// 输入一个节点，在该节点上拓展一个新的节点，使用random方法执行Action，返回新增的节点。注意，需要保证新增的节点与其他节点Action不同。
        /// </summary>
        /// <param name="Mnode"></param>
        public Node Expand(Node Mnode)
        {

            //while(  Mnode in Already_Visit)
           
           
            if (Times==0)
            {
                Node Mynode = new Node();
                Mynode = NowBoard.SearchTree();//当前根节点建立第一层子节点
                if(Mynode.Children.Count>3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Node Expand_node = Mynode.MaxiNoad();//在当前字节点中选出Eat最大的节点
                        Expand_node_Maxi.Children.Add(Expand_node);
                    }
                }
                else
                {
                    foreach(Node Expand_node in Mynode.Children)
                    {
                        Expand_node_Maxi.Children.Add(Expand_node);
                    }
                }
               
                Times++;
            }
            Node Expand__Maxi = new Node();
            while (Expand_node_Maxi.Children.Count!=0)
            {
                Mnode.Children.Add(Expand_node_Maxi.Children[0]);
                Expand__Maxi= Expand_node_Maxi.Children[0];
                Expand_node_Maxi.Children.Remove(Expand_node_Maxi.Children[0]);
                break;
            }
            return Expand__Maxi;
            
        }

        #endregion

        #region Simulation阶段
        /// <summary>
        /// 蒙特卡罗树搜索的Simulation阶段，输入一个需要expand的节点，随机操作后创建新的节点，返回新增节点的reward。注意输入的节点应该不是子节点，而且是有未执行的Action可以expend的。
        /// </summary>
        /// <param name="Mnode"></param>
        public double Default_policy(Node Mnode)
        {
            NowBoard.ClearBoard_Simulate();
            NowBoard.SimuChess();
            NowBoard.Dummy_AddChess(Mnode.pos, Mnode.Lix);
            State Current_state = Mnode.Self_State;
            while (Current_state.Is_terminal()==false)
            {
                Current_state = Current_state.Get_next_state_with_random_choice();
            }

            double Final_state_reward = Current_state.Compute_reward();

            return Final_state_reward;
        }

        #endregion

        #region Backpropagation阶段
        /// <summary>
        /// 蒙特卡洛树搜索的Backpropagation阶段，输入前面获取需要expend的节点和新执行Action的reward，反馈给expend节点和上游所有节点并更新对应数据。
        /// </summary>
        public void Backup(Node MNode,double reward)
         { 
            while (MNode != null)
            {
                MYnode.Visit_times_add_one();
                MNode.Quality_value_add_n(reward);
                MNode = MNode.Parent;
            }
         }

        #endregion

        #region UCB算法
        /// <summary>
        ///  使用UCB算法，权衡exploration和exploitation后选择得分最高的子节点，注意如果是预测阶段直接选择当前Q值得分最高的。
        /// </summary>
        public Node Best_child(Node Nyode,bool is_exploration)
        {
            double C = 0.0;
            double UCB = 0.0;
            double Ucb = 0.0;
            double left = 0.0;
            double right = 0.0;
            Node Best_sub_node = new Node();
            //忽略探索推理
            if (is_exploration) { C = 0.0; } else {  C = 1 / Math.Sqrt(2.0); }

            int Cishu = 0;
            foreach(Node Niexode in Nyode.Children)
            {
                right = Math.Log(Niexode.Visit_times);
                left = Niexode.Quality_value / Niexode.Visit_times;
                right = 2.0 * Math.Log(Nyode.Visit_times) / Niexode.Visit_times;
                Ucb = left + C * Math.Sqrt(right);
                if(Cishu==0)
                {
                    UCB = Ucb;
                    Best_sub_node = Niexode;
                    Cishu++;
                }

                if (Ucb> UCB)
                {
                    Best_sub_node = Niexode;
                    UCB = Ucb;
                }
            }
            return Best_sub_node;

        }
        #endregion

        #region 计算机移动棋子
        /// <summary>
        /// 计算机移动棋子
        /// </summary>
        /// <param name="Ld"></param>
        /// <param name="Dlx"></param>
        /// <returns></returns>
        public string Mcts_Move(Board Ld, Chess.ChessType Dlx)
        {
            NowBoard = Ld;
            Chess OnePos = NowBoard.Move();//当前移动位置
            Chess NewPos = OnePos.Copy();
            NewPos.type = Dlx;
            List<Chess.POS> MyPos = NowBoard.YiDongJiHe(NewPos);//获取可移动位置集合
            Chess.POS Mpos = NowBoard.YdpinGu(MyPos, NewPos);
            string Str = Mpos.posX.ToString() + "," + Mpos.posY.ToString();
            return Str;
        }
        #endregion

        #region 计算机跳棋让路
        /// <summary>
        /// 计算机跳棋让路
        /// </summary>
        /// <param name="Ld"></param>
        /// <param name="Dlx"></param>
        /// <returns></returns>
        public string Mcts_Give_Way(Board Ld, Chess.ChessType Dlx)
        {
            string str = "";
            NowBoard = Ld;
            Chess OnePos = NowBoard.Move();//当前移动位置
            Chess NewPos = OnePos.Copy();
            NewPos.type = Dlx;
            RuleBase.TreeC TreeMax = NowBoard.Maximum_Give_way(NewPos);
            str = TreeMax.A1.ToString() + "," + TreeMax.A2.ToString();
            return str;
         }
        #endregion

        #region 实验MCTS

        #region  MCTS
        public string Mmonte_carlo_tree_search_Test(Board Ld, int side)
        {

            NowBoard = Ld;
            NowBoard.Side = side; //当前棋类型
            string str = "0,2";
            for (int i = 0; i < 3; i++)
            {
                //1.找到要扩展的最佳节点
                Node MaxNode = Tree_policy_Test(MYnode);

                //2.随机运行添加节点并获得奖励
                string Value = New_Simulation(MaxNode);
                string[] Vstr = Value.Split(',');
                double reward = Convert.ToDouble(Vstr[0]);
                int control = Convert.ToInt16(Vstr[1]);

                //3.使用奖励更新所有传递的节点
                Backup_Test(MaxNode, reward, control);

            }
            Times = 0;
            
            //N.获得最佳下一个节点
            Node Best_next_node = Best_child(MYnode, false);
            str = Best_next_node.pos.posX.ToString() + "," + Best_next_node.pos.posY.ToString();
            return str;
           
        }
        #endregion

        #region Selection和Expansion阶段
        /// <summary>
        ///  蒙特卡罗树搜索的Selection和Expansion阶段，传入当前需要开始搜索的节点（例如根节点），根据exploration/exploitation算法返回最好的需要expend的节点，注意如果节点是叶子结点直接返回。
        /// </summary>
        /// <param name="Mnode"></param>
        public Node Tree_policy_Test(Node Mnode)
        {
            Node Best_next_node = new Node();
            while (!MYnode.Self_State.Is_terminal())
            {//应该是调用着函数计算可走的路个数
                if (MYnode.Is_all_expand())
                {
                    return Best_child(MYnode, false);
                }
                else
                {
                    return Expand_Test();
                }
            }
            return Best_next_node;
        }



        /// <summary>
        /// 输入一个节点，在该节点上拓展一个新的节点，使用random方法执行Action，返回新增的节点。注意，需要保证新增的节点与其他节点Action不同。
        /// </summary>
        /// <param name="Mnode"></param>
        public Node Expand_Test()
        {
            if (Times == 0)
            {
                Node Mynode = new Node();
                Mynode = NowBoard.SearchTree();//当前根节点建立第一层子节点
                if (Mynode.Children.Count > 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Node Expand_node = Mynode.MaxiNoad();//在当前字节点中选出Eat最大的节点
                        Expand_node_Maxi.Children.Add(Expand_node);
                    }
                }
                else
                {
                    foreach (Node Expand_node in Mynode.Children)
                    {
                        Expand_node_Maxi.Children.Add(Expand_node);
                    }
                }

                Times++;
            }
            Node Expand__Maxi = new Node();
            while (Expand_node_Maxi.Children.Count != 0)
            {
                Expand_node_Maxi.Children[0].Parent = MYnode;
                MYnode.Children.Add(Expand_node_Maxi.Children[0]);
                Expand__Maxi = Expand_node_Maxi.Children[0];
                Expand_node_Maxi.Children.Remove(Expand_node_Maxi.Children[0]);
                break;
            }
            return Expand__Maxi;

        }

        #endregion

        #region Simulation阶段
        /// <summary>
        /// 蒙特卡罗树搜索的Simulation阶段，输入一个需要expand的节点，随机操作后创建新的节点，返回新增节点的reward。注意输入的节点应该不是子节点，而且是有未执行的Action可以expend的。
        /// </summary>
        /// <param name="Mnode"></param>
        public double Default_policy_Test(Node Mnode)
        {
            
            State Current_state = Mnode.Self_State;
            while (Current_state.Is_terminal() == false)
            {
                Current_state = Current_state.Get_next_state_with_random_choice();
            }

            double Final_state_reward = Current_state.Compute_reward();

            return Final_state_reward;
        }

        #endregion

        #region Backpropagation阶段
        /// <summary>
        /// 蒙特卡洛树搜索的Backpropagation阶段，输入前面获取需要expend的节点和新执行Action的reward，反馈给expend节点和上游所有节点并更新对应数据。
        /// </summary>
        public void Backup_Test(Node MNode, double reward, int cont)
        {
            while (MNode != null)
            {
                MNode.New_Visit_times_add_one(cont);
                MNode.Nwe_Quality_value_add_n(reward);
                MNode = MNode.Parent;
            }
        }

        #endregion

        #region UCB算法
        /// <summary>
        ///  使用UCB算法，权衡exploration和exploitation后选择得分最高的子节点，注意如果是预测阶段直接选择当前Q值得分最高的。
        /// </summary>
        public Node Best_child_Test(Node Nyode, bool is_exploration)
        {
            double C = 0.0;
            //double UCB = 0.0;
            double Ucb = 0.0;
            double left = 0.0;
            double right = 0.0;
            Node Best_sub_node = new Node();
            //忽略探索推理
            if (is_exploration) { C = 0.0; } else { C = 1 / Math.Sqrt(2.0); }

           
            foreach (Node Niexode in Nyode.Children)
            {
                
                left = Niexode.Quality_value / Niexode.Visit_times;
                right = 2.0 * Math.Log(Nyode.Visit_times) / Niexode.Visit_times;
                Ucb = left + C * Math.Sqrt(right);
                Niexode.Eat = Ucb;
                //if (Cishu == 0)
                //{
                //    UCB = Ucb;
                //    Best_sub_node = Niexode;
                //    Cishu++;
                //}

                //if (Ucb > UCB)
                //{
                //    Best_sub_node = Niexode;
                //    UCB = Ucb;
                //}
            }
            return Nyode;

        }
        #endregion
        #endregion

        #region ------------------新MCTS算法-----------------------------------------------

        #region  MCTS
        public Node New_Mmonte_carlo_tree_search(Board Ld, int side)
        {

            NowBoard = Ld;
            NowBoard.Side = side; //当前棋类型
            Aitype = ((side == 1) ? Chess.ChessType.White : Chess.ChessType.Black);
            for (int i = 0; i <3; i++)
            {
                //1.找到要扩展的最佳节点
                Node MaxNode = New_Tree_policy();
               
                //2.随机运行添加节点并获得奖励
                string Value =New_Simulation(MaxNode);
                string[]Vstr = Value.Split(',');
                double reward = Convert.ToDouble(Vstr[0]);
                int control= Convert.ToInt16(Vstr[1]);
               
                //3.使用奖励更新所有传递的节点
                New_Backup(MaxNode, reward, control);

            }
            Times = 0;
            //N.获得最佳下一个节点
            return Best_child_Test(MYnode, false);
            
        }
        #endregion


        public Node New_Tree_policy()
        {
            Node Best_next_node = new Node();
            while (!MYnode.Self_State.Is_terminal())
            {//应该是调用着函数计算可走的路个数
                if (MYnode.Is_all_expand())
                {
                    return Best_child(MYnode, false);
                }
                else
                {
                    return New_Expand();
                }
            }
            return Best_next_node;
        }
        public Node New_Expand()
        {
            if (Times == 0)
            {
                Node Mynode = new Node();
                Mynode = NowBoard.Siyan_SearchTree(Aitype);//当前根节点建立第一层子节点
                if (Mynode.Children.Count > 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Node Expand_node = Mynode.MaxiNoad();//在当前字节点中选出Eat最大的节点
                        Expand_node_Maxi.Children.Add(Expand_node);
                    }
                }
                else
                {
                    foreach (Node Expand_node in Mynode.Children)
                    {
                        Expand_node_Maxi.Children.Add(Expand_node);
                    }
                }

                Times++;
            }
            Node Expand__Maxi = new Node();
            while (Expand_node_Maxi.Children.Count != 0)
            {
                Expand_node_Maxi.Children[0].Parent = MYnode;
                MYnode.Children.Add(Expand_node_Maxi.Children[0]);
                Expand__Maxi = Expand_node_Maxi.Children[0];
                Expand_node_Maxi.Children.Remove(Expand_node_Maxi.Children[0]);
                break;
            }
            return Expand__Maxi;
           
        }

        #region 模拟
        /// <summary>
        /// 随机模拟 返回访问次数和奖励
        /// </summary>
        /// <param name="Desig"></param>
        public string New_Simulation(Node Desig)
        {
            return NowBoard.Simulation(Desig.pos, Desig.Lix);
            
        }
        #endregion

        #region Backpropagation阶段
        /// <summary>
        /// 蒙特卡洛树搜索的Backpropagation阶段，输入前面获取需要expend的节点和新执行Action的reward，反馈给expend节点和上游所有节点并更新对应数据。
        /// </summary>
        public void New_Backup(Node MNode, double reward,int cont)
        {


            
            while (MNode != null)
            {
                MNode.New_Visit_times_add_one(cont);
                MNode.Nwe_Quality_value_add_n(reward);
                MNode = MNode.Parent;
            }
        }

        #endregion
        #endregion
    }
}
