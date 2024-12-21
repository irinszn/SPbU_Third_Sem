using System.Net;
using System.Net.Sockets;

using SimpleFTP;

if (args.Length != 2 || args[0] == "-help")
{
    Console.WriteLine("""
                        This is the client which supports SimpleFTP protocol.

                        There are two requests to the server:

                        1. To list files in a directory on the server:

                           Input: "1 <path: String>\n"

                        2. To get a file from the server:

                            Input: "2 <path: String>\n"
                        
                        3. To stop work enter: empty string

                        To start program enter: dotnet run [IP address] [port]

                        """);
    return 0;
}

if (!IPAddress.TryParse(args[0], out var ip) || !int.TryParse(args[1], out var port) || port > 65536 || port < 0)
{
    Console.WriteLine("Wrong input of IP address or number of port (it must be <= 65536 and positive), enter dotnet run -help to see more.");
    return 0;
}

var endPoint = new IPEndPoint(ip, port);
var client = new FTPClient(endPoint);

try
{
    var request = Console.ReadLine();

    while (request != string.Empty)
    {
        if (request is null)
        {
            return 0;
        }

        if (request[0].ToString() == "1")
        {
            Console.WriteLine(await client.ListAsync(request));
        }
        else if (request[0].ToString() == "2")
        {
            Console.WriteLine(await client.GetAsync(request));
        }
        else
        {
            Console.WriteLine("Wrong input. Enter dotnet run -help to see more.");
        }

        request = Console.ReadLine();
    }
}
catch (SocketException)
{
    Console.WriteLine("Connection error. Port is busy");
    return 0;
}

return 1;
