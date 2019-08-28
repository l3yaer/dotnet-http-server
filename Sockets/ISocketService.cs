using System;
using System.Net.Sockets;

namespace SimpleHttpServer.Sockets
{
  public interface ISocketService
  {
    void Serve(TcpClient socket);
  }
}
