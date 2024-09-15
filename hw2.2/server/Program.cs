using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using Newtonsoft.Json;

class RequestQuery {
    public string query;
}

class HttpServer
{
    private readonly TcpListener _listener;
    private readonly string _fileRootPath;

    private CalculatorNS.Calculator calculator;

    public HttpServer(int port, string fileRootPath)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _fileRootPath = fileRootPath;

        calculator = new();
    }

    ~HttpServer()
    {
        _listener.Stop();
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Server started.");
        while (true)
        {
            TcpClient client = _listener.AcceptTcpClient();
            Console.WriteLine("Request accepted");

            Thread thread = new(new ParameterizedThreadStart(ProcessClient));
            thread.Start(client);
        }
    }

    private void ProcessClient(object obj)
    {
        TcpClient client = obj as TcpClient;
        if (client == null)
            return;
        using (NetworkStream stream = client.GetStream())
        {
            byte[] request = new byte[1024];
            int bytesRead = stream.Read(request, 0, request.Length);
            string message = Encoding.ASCII.GetString(request, 0, bytesRead);
            Console.WriteLine("Received message: " + message);

            var messageLines = message.Split('\n');

            foreach (var item in messageLines)
            {
                Console.WriteLine(item);
            }

            var messageHeader = messageLines[0];

            var requestPath = messageHeader.Split(' ')[1];

            if (requestPath == "/calc") {
                var body = message.Split("\r\n\r\n")[1];

                RequestQuery query = JsonConvert.DeserializeObject<RequestQuery>(body);

                Console.WriteLine(body);

                SendResponse(stream, calculator.Calculate(query.query).ToString());
            } else {
                SendFile(stream, requestPath);
            }

        }
        client.Close();
    }

    private void SendResponse(NetworkStream stream, string response)
    {
        string responseBase = "HTTP/1.1 200 OK\r\n" +
                    "Content-Type: text/html; charset=UTF-8\r\n" +
                    "Content-Legnth: " + response.Length + "\r\n" +
                    "Connection: close\r\n\r\n";

        byte[] buffer = Encoding.UTF8.GetBytes(responseBase + response);
        stream.Write(buffer, 0, buffer.Length);
        stream.Flush();
    }

    private void SendFile(NetworkStream stream, string filename)
    {
        var filePath = _fileRootPath + filename;

        if (!File.Exists(filePath))
        {
            HandleNotFoundError(stream);
            return;
        }

        var fileData = File.ReadAllBytes(filePath);
        SendResponse(stream, Encoding.UTF8.GetString(fileData));
    }

    private void HandleNotFoundError(NetworkStream stream)
    {
        SendFile(stream, "/404.html");
    }
}

class Program {
    static void Main(string[] args) 
        {
            HttpServer server = new HttpServer(49115, "./pages");
            server.Start();
        }
}
