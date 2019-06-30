using SIS.WebServer.Routing;

namespace SIS.WebServer
{
    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(ServerRoutingTable serverRoutingTable);
    }
}