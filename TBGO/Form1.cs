using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TBGO
{
    public partial class Form1 : Form
    {
        
        /// <summary>
        /// 游戏室允许进入的最多人数
        /// </summary>
        private int maxUsers;
        /// <summary>
        ///连接的用户
        /// </summary>
        List<User> userList = new List<User>();
        /// <summary>
        ///游戏室开出的桌数。
        /// </summary>
        private int maxTables;
        private Player[] gameTable; 
        /// <summary>
        ///使用的本机IP地址
        /// </summary>
        IPAddress localAddress;
        /// <summary>
        ///监听端口
        /// </summary>
        private int port = 51888;
        private TcpListener myListener;
        private Service service;
        /// <summary>
        /// 桌号
        /// </summary>
        int tableIndex = -1;    //桌号
       /// <summary>
       /// 颜色
       /// </summary>
        int side = -1;          //颜色
        int anotherSide = -1;   //对方座位号
        
        /// <summary>
        /// 棋盘集合
        /// </summary>
        public static List<Board> MyBd = new List<Board>();
        /// <summary>
        /// 当前用户名
        /// </summary>
        string UserName = string.Empty;
        public Chess.POS pos;
        public Form1()
        {
            InitializeComponent();
            service = new Service(listBox1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.HorizontalScrollbar = true;
            IPAddress[] addrIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipa in addrIP)
              {
                 if (ipa.AddressFamily == AddressFamily.InterNetwork)
                          localAddress = ipa;
              }
            
            buttonStop.Enabled = false;
        }
        //【开始服务】按钮的Click事件
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxMaxTables.Text, out maxTables) == false
                || int.TryParse(textBoxMaxUsers.Text, out maxUsers) == false)
            {
                MessageBox.Show("请输入在规定范围内的正整数");
                return;
            }
            if (maxUsers < 1 || maxUsers > 300)
            {
                MessageBox.Show("允许进入的人数只能在1-300之间");
                return;
            }
            if (maxTables < 1 || maxTables > 100)
            {
                MessageBox.Show("允许的桌数只能在1-100之间");
                return;
            }
            textBoxMaxUsers.Enabled = false;
            textBoxMaxTables.Enabled = false;
            //初始化数组
            gameTable = new Player[maxTables];
            for (int i = 0; i < maxTables; i++)
            {
                gameTable[i] = new Player(listBox1);
            }
            myListener = new TcpListener(localAddress, port);
            myListener.Start();
            service.SetListBox(string.Format("开始在{0}:{1}监听客户连接", localAddress, port));
            //创建一个线程监听客户端连接请求
            ThreadStart ts = new ThreadStart(ListenClientConnect);
            Thread myThread = new Thread(ts);
            myThread.Start();
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
        }
        //接收客户端连接
        private void ListenClientConnect()
        {
            while (true)
            {
                TcpClient newClient = null;
                try
                {
                    //等待用户进入
                    newClient = myListener.AcceptTcpClient();
                }
                catch
                {
                    //当单击“停止监听”或者退出此窗体时AcceptTcpClient()会产生异常
                    //因此可以利用此异常退出循环
                    break;
                }
                //每接受一个客户端连接,就创建一个对应的线程循环接收该客户端发来的信息
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ReceiveData);
                Thread threadReceive = new Thread(pts);
                User user = new User(newClient);
                threadReceive.Start(user);
                userList.Add(user);
                service.SetListBox(string.Format("{0}进入", newClient.Client.RemoteEndPoint));
                service.SetListBox(string.Format("当前连接用户数：{0}", userList.Count));
            }
        }
        //接收、处理客户端信息，每客户1个线程，参数用于区分是哪个客户
        private void ReceiveData(object obj)
        {
            User user = (User)obj;
            TcpClient client = user.client;
            //是否正常退出接收线程
            bool normalExit = false;
            //用于控制是否退出循环
            bool exitWhile = false;
            while (exitWhile == false)
            {
                string receiveString = null;
                try
                {
                    receiveString = user.sr.ReadLine();
                }
                catch
                {
                    //该客户底层套接字不存在时会出现异常
                    service.SetListBox("接收数据失败");
                }
                //TcpClient对象将套接字进行了封装，如果TcpClient对象关闭了，
                //但是底层套接字未关闭，并不产生异常，但是读取的结果为null
                if (receiveString == null)
                {
                    if (normalExit == false)
                    {
                        //如果停止了监听，Connected为false
                        if (client.Connected == true)
                        {
                            service.SetListBox(string.Format(
                                "与{0}失去联系，已终止接收该用户信息",
                                 client.Client.RemoteEndPoint));
                        }
                        //如果该用户正在游戏桌上，则退出游戏桌
                        RemoveClientfromPlayer(user);
                    }
                    //退出循环
                    break;
                }
                service.SetListBox(string.Format("来自{0}：{1}", user.userName, receiveString));
                string[] splitString = receiveString.Split(',');
                
                string sendString = "";
                switch (splitString[0])
                {
                    case "Login":
                        //格式：Login,昵称
                        //该用户刚刚登录
                        if (userList.Count > maxUsers)
                        {
                            sendString = "Sorry";
                            service.SendToOne(user, sendString);
                            service.SetListBox("人数已满，拒绝" +
                                splitString[1] + "进入游戏室");
                            exitWhile = true;
                        }
                        else
                        {
                            //将用户昵称保存到用户列表中
                            //由于是引用类型,因此直接给user赋值也就是给userList中对
                            //应的user赋值
                            //用户名中包含其IP和端口的目的是为了帮助理解，实际游戏
                            //中一般不会显示IP的
                            user.userName = string.Format("[{0}--{1}]", splitString[1],
                                client.Client.RemoteEndPoint);
                            //允许该用户进入游戏室，即将各桌是否有人情况发送给该用户
                            sendString = "Tables," + this.GetOnlineString();
                            service.SendToOne(user, sendString);
                        }
                        break;
                    case "Logout":
                        //格式：Logout
                        //用户退出游戏室
                        service.SetListBox(string.Format("{0}退出游戏室", user.userName));
                        normalExit = true;
                        exitWhile = true;
                        break;
                    case "SitDown":
                        //格式：SitDown,桌号,座位号
                        //该用户坐到某座位上
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].user = user;
                        gameTable[tableIndex].someone = true;
                        service.SetListBox(string.Format(
                            "{0}在第{1}桌第{2}座入座",
                            user.userName, tableIndex + 1, side + 1));
                        //得到对家座位号
                        anotherSide = (side + 1) % 2;
                        //判断对方是否有人
                        if (gameTable[tableIndex].someone == true)
                        {
                            //先告诉该用户对家已经入座
                            //发送格式：SitDown,座位号,用户名
                            sendString = string.Format("SitDown,{0},{1}", anotherSide,
                              gameTable[tableIndex].user.userName);
                            service.SendToOne(user, sendString);
                        }
                        //同时告诉两个用户该用户入座(也可能对方无人)
                        //发送格式：SitDown,座位号,用户名
                        sendString = string.Format("SitDown,{0},{1}", side, user.userName);
                        service.SendToBoth(gameTable[tableIndex], sendString);
                        //重新将游戏室各桌情况发送给所有用户
                        service.SendToAll(userList, "Tables," + this.GetOnlineString());
                        break;
                    case "GetUp":
                        //格式：GetUp,桌号,座位号
                        //用户离开座位,回到游戏室
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        service.SetListBox(
                             string.Format("{0}离座,返回游戏室", user.userName));
                        //将离座信息同时发送给两个用户，以便客户端作离座处理
                        //发送格式：GetUp,座位号,用户名
                        service.SendToBoth(gameTable[tableIndex],
                            string.Format("GetUp,{0},{1}", side, user.userName));
                        //服务器进行离座处理
                        gameTable[tableIndex].someone = false;
                        gameTable[tableIndex].started = false;
                        gameTable[tableIndex].grade = 0;
                        anotherSide = (side + 1) % 2;
                        if (gameTable[tableIndex].someone == true)
                        {
                            gameTable[tableIndex].started = false;
                            gameTable[tableIndex].grade = 0;
                        }
                        //重新将游戏室各桌情况发送给所有用户
                        service.SendToAll(userList, "Tables," + this.GetOnlineString());
                        break;
                    case "Level":
                        //格式：Time,桌号,难度级别
                        //设置难度级别
                        break;
                    case "Talk":
                        //格式：Talk,桌号,对话内容
                        tableIndex = int.Parse(splitString[1]);
                        //由于说话内容可能包含逗号，所以需要特殊处理
                        sendString = string.Format("Talk,{0},{1}", user.userName,
                            receiveString.Substring(splitString[0].Length +
                            splitString[1].Length));
                        service.SendToBoth(gameTable[tableIndex], sendString);
                        break;
                    case "Ready"://开局 
                        //格式：Start,桌号,座位号
                        //该用户单击了开始按钮
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].started = true;
                        Board One = new Board();
                        One.TableIndex = tableIndex;
                        One.ClearBoard();
                        One.UserName = user.userName;
                        MyBd.Add(One);
                        if (side ==1)
                        {
                            Board MD = DqMB(tableIndex);
                            MD.ChessAI=  Chess.ChessType.White;
                            anotherSide = 1;
                            service.SendToBoth(gameTable[tableIndex], "Start,0,མགོ་རྩོམ");
                            
                        }
                        else
                        {
                            anotherSide = 0;
                            service.SendToBoth(gameTable[tableIndex], "Start,1,མགོ་རྩོམ");
                            //调用MCTS
                            Board MD= DqMB(tableIndex);
                            MD.ChessAI = Chess.ChessType.Black;
                            MCTS Ct = new MCTS();
                            string wz = Ct.Mmonte_carlo_tree_search(MD,side);
                            //string wz = Ct.Mmonte_carlo_tree_search_Test(MD, side);
                            string[] Xy = wz.Split(',');
                            int Xx1 = int.Parse(Xy[0]);
                            int Yy2 = int.Parse(Xy[1]);
                            pos = new Chess.POS(Xx1, Yy2);
                            Chess.ChessType m_turn1 = Chess.ChessType.Black;
                            MD.addChess(pos, m_turn1);
                            service.SendToBoth(gameTable[tableIndex], string.Format("SetDot,{0},{1},{2},{3},{4}", tableIndex, side,Convert.ToInt16(Xy[0]), Convert.ToInt16(Xy[1]), 1));
                        }
                        break;
                    case "NotReady":
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].started = false;
                        if (side == 0)
                        {

                            sendString = "Message,རྡེའུ་ནག་པོས་གྲ་སྒྲིག་བྱས་མེད།";
                        }
                        else
                        {

                            sendString = "Message,རྡེའུ་དཀར་པོས་གྲ་སྒྲིག་བྱས་མེད།";
                        }
                        service.SendToBoth(gameTable[tableIndex], sendString);
                        //gameTable[tableIndex].StartTimer();

                        break;
                    case "UnsetDot":
                        //格式：UnsetDot,桌号,座位号,行,列,颜色
                        //消去客户单击的棋子
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        int xi = int.Parse(splitString[3]);
                        int xj = int.Parse(splitString[4]);
                        int color = int.Parse(splitString[5]);
                        //gameTable[tableIndex].UnsetDot(xi, xj, color);
                        break;
                    case "SetDot"://人放棋子
                        //格式：SetDot,桌号,颜色,行,列
                        //消去客户单击的棋子
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        int X= int.Parse(splitString[3]);
                        int Y= int.Parse(splitString[4]);
                        Board Bor1 = DqMB(tableIndex);
                        pos =new  Chess.POS(X,Y);
                        Chess.ChessType m_turn = Chess.ChessType.White;
                        if (side == 0)
                        {
                            m_turn = Chess.ChessType.Black;
                        }
                        Bor1.addChess(pos, m_turn);
                        //Bor1.PlayWl(pos, m_turn);
                        gameTable[tableIndex].pass = false;
                        break;
                    case "SetDotD"://人移动棋子
                        //格式：SetDot,桌号,座位号,行1,列1,行2,列2
                        //移动棋子
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        int X1 = int.Parse(splitString[3]);
                        int Y1 = int.Parse(splitString[4]);
                        int X2 = int.Parse(splitString[5]);
                        int Y2 = int.Parse(splitString[6]);
                        Bor1= DqMB(tableIndex);
                        pos = new Chess.POS(X1, Y1);
                        Chess.POS pos1 = new Chess.POS(X2, Y2);
                        m_turn = Chess.ChessType.White;
                        if (side == 0)
                        {
                            m_turn = Chess.ChessType.Black;
                        }
                        Bor1.YdQz(pos.posX, pos.posY, pos1.posX, pos1.posY, m_turn);//移动棋子
                        gameTable[tableIndex].pass = false;
                        break;
                    case "Pass":
                        //格式：Pass,桌号,座位号
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        anotherSide = (side + 1) % 2;
                        gameTable[tableIndex].pass = true;
                        if (gameTable[tableIndex].pass)
                        {
                            //双方都pass，棋局结束
                            service.SendToBoth(gameTable[tableIndex], "GameOver");
                            gameTable[tableIndex].pass = false;
                            gameTable[tableIndex].pass = false;
                            gameTable[tableIndex].started = false;
                            gameTable[tableIndex].started = false;
                        }
                        else
                        {
                            service.SendToOne(gameTable[tableIndex].user, receiveString);
                        }
                        break;
                    case "Lose":
                        //格式：Lose,桌号,座位号
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        Board MyBoard =DqMB(tableIndex);
                        MyBoard.ClearBoard();
                        gameTable[tableIndex].pass = false;
                        gameTable[tableIndex].pass = false;
                        gameTable[tableIndex].started = false;
                        gameTable[tableIndex].started = false;
                        break;
                    case "Switcher":                  //计算机放棋
                        //格式：Switcher,桌号,颜色
                        //消去客户单击的棋子
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].started = true;
                        Board M1 = DqMB(tableIndex);
                        MCTS C1 = new MCTS();
                        string w1 = C1.Mmonte_carlo_tree_search(M1, side);
                        //string w1 = C1.Mmonte_carlo_tree_search_Test(M1, side);
                        string[] Xy1 = w1.Split(',');
                        X1 = int.Parse(Xy1[0]);
                        Y2 = int.Parse(Xy1[1]);
                        pos = new Chess.POS(X1, Y2);
                        Chess.ChessType m_turn0 = Chess.ChessType.White;
                        if (side == 0)
                        { m_turn0 = Chess.ChessType.Black; }
                        M1.addChess(pos, m_turn0);
                        M1.PlayWl(pos, m_turn0);
                        if(M1.Full())
                        {
                            service.SendToBoth(gameTable[tableIndex], string.Format("SetDot,{0},{1},{2},{3},{4}", tableIndex, side, Convert.ToInt16(Xy1[0]), Convert.ToInt16(Xy1[1]),0));
                        }
                        else
                        {
                            service.SendToBoth(gameTable[tableIndex], string.Format("SetDot,{0},{1},{2},{3},{4}", tableIndex, side, Convert.ToInt16(Xy1[0]), Convert.ToInt16(Xy1[1]), 1));
                        }
                       
                        gameTable[tableIndex].pass = false;
                        break;
                    case "Color"://改变棋的颜色
                        //格式：Color,桌号,颜色,X轴，Y轴
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].started = true;
                        Board L1 = DqMB(tableIndex);
                        Chess.POS POP = new Chess.POS(int.Parse(splitString[3]), int.Parse(splitString[4]));
                        Chess.ChessType yanse = Chess.ChessType.White;
                        if (side == 0)
                        { yanse = Chess.ChessType.Black; }
                       
                        L1.GaiYan(POP, yanse);
                        break;
                    case "Move_chess"://计算机移动
                        //格式：Move_chess,桌号,颜色
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].started = true;
                        yanse = Chess.ChessType.White;
                        if (side == 0)
                        { yanse = Chess.ChessType.Black; }
                        L1 = DqMB(tableIndex);
                        MCTS Mc1 = new MCTS();
                        string Weiz= Mc1.Mcts_Move(L1, yanse);
                        string[] Myweiz = Weiz.Split(',');
                        X1 = int.Parse(Myweiz[0]);
                        Y2 = int.Parse(Myweiz[1]);
                        Chess OnePos = L1.Move();
                        L1.YdQz( OnePos.pos.posX, OnePos.pos.posY, X1, Y2, yanse);//移动棋子
                        //计算机还要吃棋子
                        Chess.POS pos3 = new Chess.POS(OnePos.pos.posX, OnePos.pos.posY);
                        L1.PlayWl(pos3, yanse);
                        service.SendToBoth(gameTable[tableIndex], string.Format("SetDotD,{0},{1},{2},{3},{4},{5}", tableIndex, side,  OnePos.pos.posX, OnePos.pos.posY,X1, Y2));
                        break;
                    case "Give_way"://计算机让路
                        tableIndex = int.Parse(splitString[1]);
                        side = int.Parse(splitString[2]);
                        gameTable[tableIndex].started = true;
                        yanse = Chess.ChessType.White;
                        if (side == 0)
                        { yanse = Chess.ChessType.Black; }
                        L1 = DqMB(tableIndex);
                        Mc1 = new MCTS();
                        Weiz = Mc1.Mcts_Give_Way(L1, yanse);
                        Myweiz = Weiz.Split(',');
                        X1 = int.Parse(Myweiz[0]);
                        Y2 = int.Parse(Myweiz[1]);
                        OnePos = L1.Move();
                        //当前空的位置，计算出来的位置
                        L1.YdQz(OnePos.pos.posX, OnePos.pos.posY, X1, Y2, yanse);//移动棋子
                       //计算机还要吃棋子
                        pos = new Chess.POS(X1, Y2);
                        L1.PlayWl(pos, yanse);
                        service.SendToBoth(gameTable[tableIndex], string.Format("SetDotD,{0},{1},{2},{3},{4},{5}", tableIndex, side, OnePos.pos.posX, OnePos.pos.posY, X1, Y2));
                        break;
                    default:
                        service.SendToAll(userList, "发了错误信息：" + receiveString);
                        break;
                }
            }
            userList.Remove(user);
            client.Close();
            service.SetListBox(string.Format("有一个退出，剩余连接用户数：{0}", userList.Count));
        }
        // 循环检测该用户是否坐到某游戏桌上,如果是,将其从游戏桌上移除，并终止该桌游戏
        private void RemoveClientfromPlayer(User user)
        {
            for (int i = 0; i < gameTable.Length; i++)
            {
                    if (gameTable[i].user != null)
                    {
                        //判断是否同一个对象
                        if (gameTable[i].user == user)
                        {
                            StopPlayer(i);
                            return;
                        }
                    }
            }
        }
        //停止第i桌游戏
        private void StopPlayer(int i)
        {

            gameTable[i].someone = false;
            gameTable[i].started = false;
            gameTable[i].grade = 0;
            //int otherSide = (j + 1) % 2;
            if (gameTable[i].someone == true)
            {
                gameTable[i].started = false;
                gameTable[i].grade = 0;
                if (gameTable[i].user.client.Connected == true)
                {
                    //发送格式：Lost,座位号,用户名
                    service.SendToOne(gameTable[i].user,
                        string.Format("Lost,{0},{1}",
                         i, gameTable[i].user.userName));
                }
            }
        }
        //获取每桌是否有人的字符串，每座用一位表示，0表示无人，1表示有人
        private string GetOnlineString()
        {
            string str = "";
            for (int i = 0; i < gameTable.Length; i++)
            {
                //for (int j = 0; j < 2; j++)
                //{
                    str += gameTable[i].someone == true ? "1" : "0";
                //}
            }
            return str;
        }



        public static Board DqMB(int qimh)
        {
            Board l = new Board();
           foreach(Board Db in MyBd)
            {
                if(Db.TableIndex==qimh)
                {
                    return Db;
                }
            }
            return l;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Chessboard_Information ChIF = new Chessboard_Information(textBoxMaxTables.Text);
            ChIF.Show();
        }
    }
}
