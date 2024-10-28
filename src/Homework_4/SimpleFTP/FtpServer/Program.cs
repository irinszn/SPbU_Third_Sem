using System.Net;

using SimpleFTP;

if (args.Length != 1 || args[0] == "-help")
{
    Console.WriteLine("""
                        This is the server which supports SimpleFTP protocol.

                        To start work enter: dotnet run [port (<= 65536 and positive)]

                        To stop work enter: stop
                        
                        """);
    return 0;
}

if (!int.TryParse(args[0], out var port) || port > 65536 || port < 0)
{
    Console.WriteLine("Wrong input, to see more enter dotnet run -help");
    return 0;
}

var server = new FTPServer(port);
await server.Start();

return 1;