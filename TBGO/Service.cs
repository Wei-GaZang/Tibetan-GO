//---------------Service.cs-------------------//
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
namespace TBGO
{
    class Service
    {
        private ListBox listbox;
        //用于线程间互操作
        private delegate void SetListBoxCallback(string str);
        private SetListBoxCallback setListBoxCallback;
        public Service(ListBox listbox)
        {
            this.listbox = listbox;
            setListBoxCallback = new SetListBoxCallback(SetListBox);
        }
        public void SetListBox(string str)
        {
            //比较调用SetListBox方法的线程和创建listBox1的线程是否同一个线程
            //如果不是，则listBox1的InvokeRequired为true
            if (listbox.InvokeRequired)
            {
                //结果为true，则通过代理执行else中的代码,并传入需要的参数
                listbox.Invoke(setListBoxCallback, str);
            }
            else
            {
                //结果为false，直接执行
                listbox.Items.Add(str);
                listbox.SelectedIndex = listbox.Items.Count - 1;
                listbox.ClearSelected();
            }
        }
        /// <summary>
        /// 发生信息
        /// <summary>
        public void SendToOne(User user, string str)
        {
            try
            {
                user.sw.WriteLine(str);
                user.sw.Flush();
                SetListBox(string.Format("向{0}发送{1}", user.userName, str));
            }
            catch
            {
                SetListBox(string.Format("向{0}发送信息失败", user.userName));
            }
        }
        /// <summary>
        /// 发生信息1
        /// <summary>
        public void SendToBoth(Player gameTable, string str)
        {
            
                if (gameTable.someone == true)
                {
                    SendToOne(gameTable.user, str);
                }
            
        }
        public void SendToAll(System.Collections.Generic.List<User> userList, string str)
        {
            for (int i = 0; i < userList.Count; i++)
            {
                SendToOne(userList[i], str);
            }
        }
    }
}
