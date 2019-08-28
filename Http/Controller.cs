using System;
namespace SimpleHttpServer.Http
{
  public interface IController
  {
    string Handle(HttpRequest request);
  }
}
