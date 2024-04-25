using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace NewClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\nСоединение #" + i.ToString() + "\n");
                Connect("127.0.0.1", "HelloWorld! #" + i.ToString());
            }
            Console.WriteLine("\nНажмите Enter...");
            Console.Read();
        }
        static void Connect(string server, string message)
        {
            try
            {
                // Создаём TcpClient.
                int port = 9595;
                TcpClient client = new TcpClient(server, port);
                // Переводим наше сообщение в ASCII, а затем в массив Byte.
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Получаем поток для чтения и записи данных.
                NetworkStream stream = client.GetStream();
                // Отправляем сообщение нашему серверу,
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Отправлено: {0}", message);
                // Получаем ответ от сервера.
                // Буфер для хранения принятого массива bytes,
                data = new byte[256];
                // Строка для хранения полученных ASCII данных.
                string responseData = string.Empty;
                // Читаем первый пакет ответа сервера.
                // Можно читать весь ответное сообщение.
                // Для этого надо организовать чтение в цикле как на сервере.
                int bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Получено: {0}", responseData);
                // Закрываем всё.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}