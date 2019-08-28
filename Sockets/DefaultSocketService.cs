using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using SimpleHttpServer.Http;

namespace SimpleHttpServer.Sockets
{
  public class DefaultSocketService: ISocketService
  {
    private readonly Router router;

    public DefaultSocketService(Router router)
    {
      this.router = router;
    }

    public void Serve(TcpClient socket)
    {
      HttpRequest request = GetRequest(socket);
      string response = router.Route(request);
      WriteResponse(socket, response);
      socket.Close();
    }

    private HttpRequest GetRequest(TcpClient socket)
    {
      NetworkStream stream = socket.GetStream();
      StreamReader reader = new StreamReader(stream, Encoding.ASCII);
      string content = reader.ReadLine();
      string[] contents = content.Split(" ");
      return new HttpRequest(contents[0], contents[1]);
    }

    private void WriteResponse(TcpClient socket, string response)
    {
      NetworkStream stream = socket.GetStream();
      byte[] bytes = MakeResponse(response);
      stream.Write(bytes, 0, bytes.Length);
      stream.Flush();
      stream.Close();
    }

    private byte[] MakeResponse(string content)
    {
      byte[] contentBytes = Encoding.ASCII.GetBytes(content);
      string response = string.Format("HTTP/1.0 200 OK\r\nContent-Type: text/html\r\nContent-Length: {0}\r\n\r\n\r\n" + content, contentBytes.Length);
      return Encoding.ASCII.GetBytes(response);
    }
  }
}
