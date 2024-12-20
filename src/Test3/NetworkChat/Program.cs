using NetworkChat;

using System.Net;
using System.Net.Sockets;

static void PrintHelp()
{
    Console.WriteLine("""

    This is the network chat.

    To start server enter: dotnet run [port (<= 65536 and positive)]


    To start client enter: dotnet run [IP address] [port]


    To stop work enter: exit
    
    """);
      
}

if (args[0] == "-help")
{
    PrintHelp();
    return 0;
}

if (args.Length == 1)
{
    if (!int.TryParse(args[0], out var port) || port > 65536 || port < 0)
    {
        Console.WriteLine("Wrong input, to see more enter dotnet run -help");
        return 0;
    }

    var server = new Server(port);

    try
    {
        await server.StartAsync();
    }
    catch (SocketException)
    {
        Console.WriteLine("Connection with error.");
    }
    
    return 1;
}
else if (args.Length == 2)
{
    if (!IPAddress.TryParse(args[0], out var ip) || !int.TryParse(args[1], out var port) || port > 65536 || port < 0)
    {
        Console.WriteLine("Wrong input of IP address or number of port (it must be <= 65536 and positive), to see more enter dotnet run -help");
        return 0;
    }

    var endPoint = new IPEndPoint(ip, port);
    var client = new Client(endPoint);

    try
    {
        await client.WorkAsync();
    }
    catch (SocketException)
    {
        Console.WriteLine("Connection with error.");
    }

    return 1;
}
else
{
    PrintHelp();
    return 0;
}