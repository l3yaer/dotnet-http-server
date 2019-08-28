using System;
namespace SimpleHttpServer.Http
{
  public struct HttpRequest
  {
    public string path;
    public string method;

    public HttpRequest(string path, string method)
    {
      this.path = path;
      this.method = method;
    }
  }
}
