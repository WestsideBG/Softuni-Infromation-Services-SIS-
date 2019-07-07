using System;
using SIS.HTTP.Enums;
using SIS.MVCFramework.Controller;
using SIS.WebServer.Routing;
using SIS.WebServer.Routing.Contracts;
using System.Linq;
using System.Reflection;
using SIS.HTTP.Responses.Contracts;
using SIS.MVCFramework.Attributes.Action;
using SIS.MVCFramework.Attributes.Http;

namespace SIS.WebServer
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            AutoRegisterRoutes(application, serverRoutingTable);
            application.ConfigureServices();
            application.Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(IMvcApplication application,IServerRoutingTable serverRoutingTable)
        {
            var controllers = application.GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                    && typeof(Controller).IsAssignableFrom(type));

            foreach (var controllerType in controllers)
            {
                var actions = controllerType
                    .GetMethods(BindingFlags.DeclaredOnly
                    | BindingFlags.Public
                    | BindingFlags.Instance)
                    .Where(x => !x.IsSpecialName && x.DeclaringType == controllerType)
                    .Where(x => x.GetCustomAttributes().All(a => a.GetType() != typeof(NonActionAttribute)));

                foreach (var action in actions)
                {
                    var path = $"/{controllerType.Name.Replace("Controller", string.Empty)}/{action.Name}";
                    var attribute = action.GetCustomAttributes().Where(
                        x => x.GetType().IsSubclassOf(typeof(BaseHttpAttribute))).LastOrDefault() as BaseHttpAttribute;
                    var httpMethod = HttpRequestMethod.Get;
                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (attribute?.Url != null)
                    {
                        path = attribute.Url;
                    }

                    if (attribute?.ActionName != null)
                    {
                        path = $"/{controllerType.Name.Replace("Controller", string.Empty)}/{attribute.ActionName}";
                    }

                    serverRoutingTable.Add(httpMethod, path,
                        request =>
                        {
                            var controllerInstance = Activator.CreateInstance(controllerType);
                            var response = action.Invoke(controllerInstance, new [] {request}) as IHttpResponse;
                            return response;
                        });


                    Console.WriteLine(httpMethod + " " + path);
                }
            }

            Console.WriteLine(string.Join(Environment.NewLine,serverRoutingTable));
        }
    }
}
