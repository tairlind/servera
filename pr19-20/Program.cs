using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OneThreadServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            Console.WriteLine("Однопоточный сервер запущен");

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 8888);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(ipEndPoint);
            listener.Listen(10);

            while (true)
            {
                Console.WriteLine("Слушаем, порт {0}", ipEndPoint);

                Socket clientSocket = listener.Accept();
                Thread clientThread = new Thread(() =>
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = clientSocket.Receive(buffer);

                        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Данные от клиента: " + data + "\n");

                        string reply = "Query size: " + data.Length.ToString() + " chars";
                        byte[] response = Encoding.UTF8.GetBytes(reply);
                        clientSocket.Send(response);

                        if (data.Contains("<TheEnd>"))
                        {
                            Console.WriteLine("Соединение завершено.");
                            clientSocket.Shutdown(SocketShutdown.Both);
                            clientSocket.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                });

                clientThread.Start();
            }

            listener.Close();
            Console.ReadLine();
        }
    }
}