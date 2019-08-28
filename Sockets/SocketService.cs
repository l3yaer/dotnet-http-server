using System;
using System.Net.Sockets;

namespace SimpleHttpServer.Sockets
{
  public interface SocketService
  {
    void Serve(Socket socket);
  }
}
