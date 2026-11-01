using System.Net;
using System.Net.Sockets;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

int port = 5000;

int x = 0, y = 0;

using Socket tcpsocket = new Socket(AddressFamily.InterNetwork,
                              SocketType.Stream,
                              ProtocolType.Tcp);

IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, port);

tcpsocket.Bind(endPoint);

tcpsocket.Listen(10);

Console.WriteLine("Сервер запущен");

using var clientSocket = tcpsocket.Accept();

Console.WriteLine("Клиент подключён");

var buffer = new byte[1024];
int length;
bool running = true;

while (running && (length = clientSocket.Receive(buffer)) > 0)
{
    string command = Encoding.UTF8.GetString(buffer, 0, length).Trim().ToUpper();

    Console.WriteLine($"Команда: {command}");

    string response;

    switch (command)
    {
        case "UP":
            y++;
            response = $"Robot position: ({x}, {y})";
            break;
        case "DOWN":
            y--;
            response = $"Robot position: ({x}, {y})";
            break;
        case "LEFT":
            x--;
            response = $"Robot position: ({x}, {y})";
            break;
        case "RIGHT":
            x++;
            response = $"Robot position: ({x}, {y})";
            break;
        case "POSITION":
            response = $"Robot position: ({x}, {y})";
            break;
        case "EXIT":
            response = "Bye!";
            running = false;
            break;
        default:
            response = $"Error: unknown command {command}";
            break;
    }

    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
    clientSocket.Send(responseBytes);
}

clientSocket.Shutdown(SocketShutdown.Both);
clientSocket.Close();

Console.WriteLine("Сервер остановлен");