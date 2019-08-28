using System;
using System.Threading.Tasks;

namespace SimpleHttpServer
{
  class Program
  {
    static void Main(string[] args)
    {
      Http.Router router = new Http.Router();
      router.Add("", new Http.Controllers.HelloController());
      Sockets.DefaultSocketService service = new Sockets.DefaultSocketService(router);
      Sockets.SocketServer server = new Sockets.SocketServer(8042, service);
      server.Start();
      Task.WaitAll(server.ServerTask);
    }
  }
}
