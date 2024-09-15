using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program {
    static void Main(string[] args) 
        {
            TcpClient client = new("127.0.0.1", 8080);
            var stream = client.GetStream();

            string request = "GET / HTTP/1.1\r\n" +
                             "Content-Type: text/html; charset=UTF-8\r\n" +
                             "Connection: close\r\n\r\n";

            byte[] buffer = Encoding.ASCII.GetBytes(request);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            byte[] response = new byte[1024];
            int bytesRead = stream.Read(response, 0, response.Length);
            string message = Encoding.ASCII.GetString(response, 0, bytesRead);
            Console.WriteLine("Received message: " + message);
        }
}
