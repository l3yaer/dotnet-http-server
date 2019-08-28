using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHttpServer.Sockets
{

  public class SocketServer
  {
    public bool Running { get; private set; }
    public Task ServerTask { get; private set; }
    private readonly IPAddress ipAddress;
    private readonly TcpListener serverSocket;
    private readonly int port;
    private readonly ISocketService service;
    public static ManualResetEvent allDone = new ManualResetEvent(false);


    public SocketServer(int port, ISocketService service)
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
      ipAddress = ipHostInfo.AddressList[0];
      this.service = service;
      this.port = port;
      serverSocket = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
      Console.WriteLine("Servidor startado em: " + ipAddress + ":" + port);
      ServerTask = Task.Run(ConnectionHandler);
    }

    private async Task ConnectionHandler()
    {
      serverSocket.Start();
      Running = true;
      while (Running)
      {
        try
        {
          TcpClient client = await serverSocket.AcceptTcpClientAsync().ConfigureAwait(false);
          Task unawaited = Task.Run(() => { service.Serve(client); });
        }
        catch (Exception e)
        {
          Console.WriteLine(e.StackTrace);
        }

      }
    }

    public void Stop()
    {
      Running = false;
      serverSocket.Stop();
      ServerTask.Dispose();
    }
  }
}