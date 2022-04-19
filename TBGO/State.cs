using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBGO
{
    /// <summary>
    /// 蒙特卡罗树搜索的游戏状态，记录在某一个Node节点下的状态数据，包含当前的游戏得分、当前的游戏round数、从开始到当前的执行记录。
    /// </summary>
   public  class State
    {
        /// <summary>
        /// 最大循环数
        /// </summary>
        public  int MAX_ROUND_NUMBER = 5;// #最大循环数
        /// <summary>
        /// 当前UCB值
        /// </summary>
        public double current_value{ get; set; }// 当前值                         
     /// <summary>
     /// 本次循环次数
     /// </summary>
       public int current_round_index { get; set; }//#本轮指数
                                                   /// <summary>
                                                   /// 累积选择
                                                   /// </summary> 
        public ArrayList Cumulative_choices = new ArrayList(); //#累积选择

        /// <summary>
        /// 循环次数是否到了最大轮数（是否叶子节点）
        /// </summary>
        /// <returns></returns>
        public bool Is_terminal()
        {
            bool Finish = false;
            if(current_round_index == MAX_ROUND_NUMBER)
            { Finish = true; }
            else { Finish = false; }
            return Finish;
        }

        public State Get_next_state_with_random_choice()
        {
            double Moni = MCTS.NowBoard.Random_Node();
            State myState = new State();
            myState.current_value =this.current_value+Moni;
            myState.current_round_index = this.current_round_index + 1;
            myState.Cumulative_choices.Add(Moni);
            return myState;
        }

        public double  Compute_reward()
        { return Math.Abs(1 - this.current_value); }
    
    }
}
