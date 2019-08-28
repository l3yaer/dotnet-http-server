using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace SimpleHttpServer.Sockets
{
  public class DefaultSocketService: SocketService
  {
    public DefaultSocketService()
    {
    }

    public void Serve(TcpClient socket)
    {
      NetworkStream stream = socket.GetStream();
      string str = "<!DOCTYPE html>" +
        "<html>" +
        "<body>Hello</body>" +
        "</html>";
      byte[] response = Encoding.ASCII.GetBytes(str);
      byte[] bytes = Encoding.UTF8.GetBytes("HTTP/1.0 200 OK\r\n");
      stream.Write(bytes, 0, bytes.Length);
      bytes = Encoding.UTF8.GetBytes("Content-Type: text/html\r\n");
      stream.Write(bytes, 0, bytes.Length);
      bytes = Encoding.UTF8.GetBytes(String.Format("Content-Length: ${0}\r\n", response.Length));
      stream.Write(bytes, 0, bytes.Length);
      bytes = Encoding.UTF8.GetBytes("\r\n\r\n");
      stream.Write(bytes, 0, bytes.Length);
      stream.Write(response, 0, response.Length);
      stream.Flush();
      stream.Close();
      socket.Close();
    }
  }
}
