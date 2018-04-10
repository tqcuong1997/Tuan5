using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Tuan5
{
    public partial class Form1 : Form
    {
        IPEndPoint ipep;
        Socket newsock;
        byte[] data = new byte[1024];
        int recv;
        Socket client;
        public Form1()
        {
            InitializeComponent();
            ipep = new IPEndPoint(IPAddress.Any, 9050);
            newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);
            newsock.Bind(ipep);
            newsock.Listen(10);
            client = newsock.Accept();
            listBox1.Items.Add("Waiting for a client...");
            //client.Connect(ipep);
            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
            textBox2.Text = clientep.Address.ToString();
            listBox1.Items.Add("Connected with " + clientep.Address + " at port" +
            clientep.Port);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            data = new byte[1024];
            data = Encoding.ASCII.GetBytes(textBox2.Text);
            client.Send(data, data.Length, SocketFlags.None);
            listBox1.Items.Add(textBox2.Text.ToString());
            TimeSpan time = TimeSpan.FromSeconds(1);
            IAsyncResult result = client.BeginReceive(data, 0, data.Length, SocketFlags.None, null, null);
            result.AsyncWaitHandle.WaitOne(time);
            if (result.IsCompleted)
            {
                recv = client.EndReceive(result);
                string stringData = Encoding.ASCII.GetString(data, 0, recv);
                listBox1.Items.Add(stringData.Substring(0, recv));
            }
        }
    }
}
