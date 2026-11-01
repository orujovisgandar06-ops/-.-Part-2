using System.Net;
using System.Net.Sockets;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

int port = 5000;

using Socket clientSocket = new Socket(AddressFamily.InterNetwork,
                              SocketType.Stream,
                              ProtocolType.Tcp);

var Ipendpoint = new IPEndPoint(IPAddress.Loopback, port);
clientSocket.Connect(Ipendpoint);

Console.WriteLine("Команды: UP, DOWN, LEFT, RIGHT, POSITION, EXIT");

var buffer = new byte[1024];

while (true)
{
    Console.Write("Введите команду: ");

    var command = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(command)) 
    { 
        continue; 
    }

    command = command.Trim().ToUpper();

    byte[] commandBytes = Encoding.UTF8.GetBytes(command);
    clientSocket.Send(commandBytes);

    int length = clientSocket.Receive(buffer);

    if (length == 0) 
    { 
        break;
    }

    string response = Encoding.UTF8.GetString(buffer, 0, length);

    Console.WriteLine($"Ответ: {response}");

    if (command == "EXIT") 
    { 
        break;
    }
}

clientSocket.Shutdown(SocketShutdown.Both);
clientSocket.Close();