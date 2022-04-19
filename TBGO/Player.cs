//---------------Player.cs-------------//
using System;
using System.Windows.Forms;

namespace TBGO
{
    class Player
    {
        public User user;     //User类的实例
        /// <summary>
        /// 是否已经开始
        /// </summary>
        public bool started; 
        public int grade;     //成绩
        /// <summary>
        /// 是否有人坐下
        /// </summary>
        public bool someone; 
        public bool pass;
        private ListBox listbox;
        Service service;
        public Player(ListBox listbox)
        {
            someone = false;
            started = false;
            grade = 0;
            user = null;
            pass = false;
            this.listbox = listbox;
            service = new Service(listbox);
        }
    }
}


