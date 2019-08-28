using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleHttpServer.Sockets
{

  public class SocketServer
  {
    public bool Running { get; set; }
    private readonly IPAddress ipAddress;
    private TcpListener serverSocket;
    private readonly int port;
    private readonly SocketService service;
    private readonly Thread thread;


    public SocketServer(int port, SocketService service)
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
      ipAddress = ipHostInfo.AddressList[0];
      this.service = service;
      this.port = port;
      ThreadStart threadDelegate = new ThreadStart(ConnectionHandler);
      thread = new Thread(threadDelegate);
    }

    public void Start()
    {
      serverSocket = new TcpListener(ipAddress, port);
      thread.Start();
      Running = true;
    }

    private void ConnectionHandler()
    {

      try
      {
        while (Running)
        {
          TcpClient client = serverSocket.AcceptTcpClient();
          Thread clientHandler = new Thread(new ParameterizedThreadStart((object obj) =>
          {
            TcpClient socket = (TcpClient)obj;
            service.Serve(socket);
          }));
          clientHandler.Start();
        }
      }
      catch (ThreadAbortException ex)
      {
        Console.WriteLine("Closing server. code: " + ex.ExceptionState);
      }
      catch (Exception e)
      {
        if (Running)
          Console.WriteLine(e.StackTrace);
      }
    }

    public void Stop()
    {
      Running = false;
      serverSocket.Stop();
      thread.Abort();
    }
  }
}
