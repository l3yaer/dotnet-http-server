using System;
using System.Threading.Tasks;

namespace SimpleHttpServer
{
  class Program
  {
    static void Main(string[] args)
    {
      Sockets.DefaultSocketService service = new Sockets.DefaultSocketService();
      Sockets.SocketServer server = new Sockets.SocketServer(8042, service);
      server.Start();
      Task.WaitAll(server.ServerTask);
    }
  }
}
