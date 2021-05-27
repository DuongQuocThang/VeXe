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
using System.Threading;
using VeXeServer;

namespace VeXe
{
    public partial class server : Form
    {
        public server()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Connect();
            ImportJSON();
        }
        List<ChuyenXe> chuyenXe = new List<ChuyenXe>();
        IPEndPoint ipe;
        Socket client;
        TcpListener tcpListener;

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void Connect()
        {
            ipe = new IPEndPoint(IPAddress.Any, 9999);
            tcpListener = new TcpListener(ipe);


            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    tcpListener.Start();
                    client = tcpListener.AcceptSocket();
                    Thread rec = new Thread(Receive);
                    rec.IsBackground = true;
                    rec.Start(client);
                }

            });
            thread.IsBackground = true;
            thread.Start();

        }
        void Send(Socket client)
        {
             
        }
        void Receive(Object obj)
        {
            string s;
            while (true)
            { 
                Socket client = obj as Socket;
                byte[] recv = new byte[1024];
                client.Receive(recv);
                s = Encoding.UTF8.GetString(recv);
               
                AddingMessage(s);
       
            }
           
           
        }
        void AddingMessage(string mess)
        {
            listView1.Items.Add(mess);
            
        }
       void ImportJSON()
        {
            string content = System.IO.File.ReadAllText("F:\\VeXe\\chuyenxe.json");
            AddingMessage(content);
            chuyenXe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChuyenXe>>(content);
            string s = "" + chuyenXe.Count;
            AddingMessage(s);

               
        }
    }
}
