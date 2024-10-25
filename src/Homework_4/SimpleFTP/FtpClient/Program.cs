using SimpleFTP;
using System.Net;

var endPoint = new IPEndPoint(IPAddress.Any, 8080);
var client = new FTPClient(endPoint);

