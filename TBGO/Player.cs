//---------------Player.cs-------------//
using System;
using System.Windows.Forms;

namespace TBGO
{
    class Player
    {
        public User user;     //User���ʵ��
        /// <summary>
        /// �Ƿ��Ѿ���ʼ
        /// </summary>
        public bool started; 
        public int grade;     //�ɼ�
        /// <summary>
        /// �Ƿ���������
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


