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
        private readonly IPEndPoint endPoint;
        private Socket serverSocket;
        private readonly SocketService service;
        private readonly Thread thread;


        public SocketServer(int port, SocketService service)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            endPoint = new IPEndPoint(ipAddress, port);
            this.service = service;
            ThreadStart threadDelegate = new ThreadStart(ConnectionHandler);
            this.thread = new Thread(threadDelegate);
        }

        public void Start()
        {
            serverSocket = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            this.thread.Start();
            Running = true;
        }

        private void ConnectionHandler()
        {

            try
            {
                serverSocket.Bind(endPoint);
                serverSocket.Listen(100);

                while (Running)
                {
                    serverSocket.BeginAccept(
                        new AsyncCallback((IAsyncResult ar) => {

                            Socket socket = (Socket)ar.AsyncState;
                            this.service.Serve(socket);
                        }),
                        serverSocket);
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
            serverSocket.Close();
            this.thread.Abort();
        }
    }
}
