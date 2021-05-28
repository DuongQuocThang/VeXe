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
using Newtonsoft.Json.Linq;

namespace VeXe
{
    public partial class server : Form
    {
        public server()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
            Connect();
            
        }
        List<ChuyenXe> chuyenXe = new List<ChuyenXe>();
        IPEndPoint ipe;
        Socket client;
        TcpListener tcpListener;
        static string yeuCauTuClient = "";
        static int tongTien;
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
            byte[] a = Encoding.UTF8.GetBytes(xuLy());
            client.Send(a);
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
                yeuCauTuClient = s;
                AddingMessage("Đã nhận yêu cầu từ client");
            }
            
        }
        
        public void AddingMessage(string mess)
        {
            listView1.Items.Add(mess);
            
        }
       List<ChuyenXe> ImportJSON()
        {
            string content = System.IO.File.ReadAllText("F:\\VeXe\\chuyenxe.json");
            dynamic data = JArray.Parse(content);
            //AddingMessage(content);
            List<ChuyenXe> chuyenXes = new List<ChuyenXe>();
            foreach (var item in data)
            {
                List<LoaiVe> loaiVes = new List<LoaiVe>();
                foreach (var loaiVe in item.loai)
                {
                    loaiVes.Add(new LoaiVe
                    {
                        TenLoaiVe = loaiVe.tenloai,
                        SoLuong = loaiVe.soluong,
                        GiaVe = loaiVe.gia
                    });
                }

                chuyenXes.Add(new ChuyenXe
                {
                    TenChuyenXe = item["ten"].ToString(),
                    loaiVeList = loaiVes
                });
                
            }
            
            
            //chuyenXe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChuyenXe>>(content);
            //string s = "" + chuyenXe.Count;
            //foreach(var x in chuyenXe)
            //{
            //   string a = x.TenChuyenXe;
            //}    
            //AddingMessage(s);
            return chuyenXes;
               
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            //check();
            Send(client);
        }
        
        string xuLy()
        {
            int lengthTEN = yeuCauTuClient.IndexOf("!") - yeuCauTuClient.IndexOf("@");
            int lengthLOAI = yeuCauTuClient.IndexOf("#") - yeuCauTuClient.IndexOf("!");
            int lengthSL = yeuCauTuClient.IndexOf("$") - yeuCauTuClient.IndexOf("#");
            string ten = yeuCauTuClient.Substring(yeuCauTuClient.IndexOf("@") + 1, lengthTEN - 1);
            string loai = yeuCauTuClient.Substring(yeuCauTuClient.IndexOf("!") + 1, lengthLOAI - 1);
            string sl = yeuCauTuClient.Substring(yeuCauTuClient.IndexOf("#") + 1, lengthSL - 1);
            chuyenXe = ImportJSON();
            foreach(var cx in chuyenXe)
            {
                List<LoaiVe> lv = cx.loaiVeList;
                if(cx.TenChuyenXe.Contains(ten))
                {
                    cx.loaiVeList = new List<LoaiVe>();
                    foreach (var ve in lv)
                    {
                        
                        if(ve.TenLoaiVe.Contains(loai))
                        {
                            if(int.Parse(sl)<=ve.SoLuong)
                            {
                                tongTien = int.Parse(sl) * ve.GiaVe;

                            }    
                        }    
                    }    
                }    
            }
            //AddingMessage(ten);
            //AddingMessage(loai);
            //AddingMessage(sl);
            return tongTien.ToString();
        }
    }
}
