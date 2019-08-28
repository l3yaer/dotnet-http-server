using System;
namespace SimpleHttpServer.Http.Controllers
{
  public class HelloController: IController
  {
    public HelloController()
    {
    }

    public string Handle(HttpRequest request)
    {
      return "<!DOCTYPE html><html><body>Hello world!</body></html>";
    }
  }
}
