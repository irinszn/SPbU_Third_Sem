using SimpleFTP;
using System.Net;

var server = new FTPServer(8080);
await server.Start();
