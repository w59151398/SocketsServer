using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            const int BufferSize = 8192;//缓存大小8192字节
            Console.WriteLine("Server is running ...");
            IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
            // 获得IPAddress 对象的另外几种常用方法：
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            //IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener listener = new TcpListener(ip, 8500);
            listener.Start();//开始侦听;
            Console.WriteLine("Start Listening ...");




                //获取一个连接，中断方法
                TcpClient remoteClient = listener.AcceptTcpClient();
                //打印连接到的客户端信息
                Console.WriteLine("Client Connected! {0}<--{1}", remoteClient.Client.LocalEndPoint, remoteClient.Client.RemoteEndPoint);

                //获得流，并写入buffer中
                NetworkStream streamToClient = remoteClient.GetStream();
                do
                {
                byte[] buffer = new byte[BufferSize];
                int bytesRead = streamToClient.Read(buffer, 0, BufferSize);
                Console.WriteLine("Reading data,{0} bytes ...",bytesRead);
                //获得请求的字符串
                string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                #region 分次读取然后转存
                //分次读取然后转存
                //NetworkStream streamToClient = remoteClient.GetStream();
                //byte[] buffer = new byte[BufferSize];
                //int bytesRead;//读取的字节数
                //MemoryStream msStream = new MemoryStream();
                //do
                //{
                //    bytesRead = streamToClient.Read(buffer, 0, BufferSize);
                //    msStream.Write(buffer, 0, bytesRead);

                //} while (bytesRead > 0);
                //buffer = msStream.GetBuffer();
               // Console.WriteLine("Reding data ,{0}, bytes ...", bytesRead);
                //获取请求的字符串
                //string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                #endregion
                Console.WriteLine("Received:{0}", msg);
                    //转换成大写并发送
                msg = msg.ToUpper();
                buffer = Encoding.Unicode.GetBytes(msg);
                lock (streamToClient) 
                {
                    streamToClient.Write(buffer, 0, buffer.Length);
                }
            } while (true);
            Console.WriteLine("\n\n输入\"Q\",键退出。");
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Q);
        }
    }
}
