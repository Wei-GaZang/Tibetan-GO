//-------------User.cs----------------//
using System.Net.Sockets;
using System.IO;
namespace TBGO
{
    class User
    {
        public TcpClient client;
        public StreamReader sr;
        public StreamWriter sw;
        public string userName;
        public User(TcpClient client)
        {
            this.client = client;
            this.userName = "";
            NetworkStream netStream = client.GetStream();
            sr = new StreamReader(netStream, System.Text.Encoding.UTF8);
            sw = new StreamWriter(netStream, System.Text.Encoding.UTF8);
        }
    }
}
