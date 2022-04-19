//-------------------GameTable.cs-----------------//
using System;
using System.Timers;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
namespace TBGO
{
    class GameTable
    {
        
        public Player[] gamePlayer;
        private System.Timers.Timer timer;       //用于定时产生棋子
        private ListBox listbox;
        Service service;
        public GameTable(ListBox listbox)
        {
            gamePlayer = new Player[2];
            gamePlayer[0] = new Player(listbox);
            gamePlayer[1] = new Player(listbox);
            timer = new System.Timers.Timer();
            timer.Enabled = false;
            this.listbox = listbox;
            service = new Service(listbox);
        } 
    }
}
