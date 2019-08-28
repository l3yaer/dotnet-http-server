using System;
using System.Collections.Generic;

namespace SimpleHttpServer.Http
{
  public class Router
  {
    private Dictionary<string, IController> controllers = new Dictionary<string, IController>();

    public void Add(string path, IController controller)
    {
      controllers.Add(path, controller);
    }

    public string Route(HttpRequest request)
    {
      string[] pathComponents = request.path.Split("/");
      string path = pathComponents.Length > 1 ? pathComponents[1] : "";
      IController controller = controllers[path];
      return controller.Handle(request);
    }
  }
}
