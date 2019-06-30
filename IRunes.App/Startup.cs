using IRunes.App.Controllers;
using IRunes.Data;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Result;
using SIS.WebServer.Routing;

namespace IRunes.App
{
    public class Startup : IMvcApplication
    {

        public void Configure(ServerRoutingTable serverRoutingTable)
        {
            using (var context = new RunesDbContext())
            {
                context.Database.EnsureCreated();
            }

            
            ////Index Routes
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/", request => new RedirectResult("Home/Index"));
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Home/Index", request => new HomeController().Index(request));

            ////Users Routes
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Login", request => new UsersController().Login());
            //serverRoutingTable.Add(HttpRequestMethod.Post, "/Users/Login", request => new UsersController().LoginConfirm(request));

            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Register", request => new UsersController().Register());
            //serverRoutingTable.Add(HttpRequestMethod.Post, "/Users/Register", request => new UsersController().RegisterConfirm(request));

            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Logout", request => new UsersController().Logout(request));

            ////Track Routes
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Track/Create", request => new TrackController().Create(request));
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Track/Details", request => new TrackController().Details(request));
            //serverRoutingTable.Add(HttpRequestMethod.Post, "/Track/Create", request => new TrackController().CreateConfirm(request));


            ////Albums Routes
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Albums/All", request => new AlbumsController().AllAlbums(request));
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Albums/Create", request => new AlbumsController().Create(request));
            //serverRoutingTable.Add(HttpRequestMethod.Post, "/Albums/Create", request => new AlbumsController().CreateConfirm(request));
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Albums/Details", request => new AlbumsController().Details(request));
        }

        public void ConfigureServices()
        {
           
        }
    }
}