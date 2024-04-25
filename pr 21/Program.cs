using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace MultiThreadServer
{
    class ExampleTcpListener
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                        ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                ThreadPool.SetMinThreads(2, 2);

                int port = 8888;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;

                server = new TcpListener(localAddr, port);
                Console.OutputEncoding = Encoding.GetEncoding(866);
                Console.WriteLine("Multi-threaded server configuration:");
                Console.WriteLine("IP Address: 127.0.0.1");
                Console.WriteLine("Port: " + port.ToString());
                Console.WriteLine("Threads: " + MaxThreadsCount.ToString());
                Console.WriteLine("Server is running");

                server.Start();

                while (true)
                {
                    Console.Write("Waiting for connection... ");
                    ThreadPool.QueueUserWorkItem(ClientProcessing, server.AcceptTcpClient());
                    counter++;
                    Console.Write("Connection #" + counter.ToString() + "!");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("Press Enter to exit...");
            Console.Read();
        }

        static void ClientProcessing(object client_obj)
        {
            byte[] bytes = new byte[256];
            string data = null;

            TcpClient client = (TcpClient)client_obj;
            data = null;

            NetworkStream stream = client.GetStream();
            int i;

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                data = data.ToUpper();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                stream.Write(msg, 0, msg.Length);
            }

            client.Close();
        }
    }
}