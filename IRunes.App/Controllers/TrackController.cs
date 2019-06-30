using System.Net;

namespace IRunes.App.Controllers
{
    using Data;
    using Models;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MVCFramework.Attributes;
    using SIS.MVCFramework.Controller;
    using System.Collections.Generic;
    using System.Linq;

    public class TrackController : Controller
    {
        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("../Users/Login");
            }
            var albumId = httpRequest.QueryData["albumId"].ToString();

            this.ViewData["AlbumId"] = albumId;

            return this.View();
        }

        [HttpPost(Method = HttpRequestMethod.Post, ActionName = "Create")]
        public IHttpResponse CreateConfirm(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("../Users/Login");
            }
            var albumId = httpRequest.QueryData["albumId"].ToString();

            using (var db = new RunesDbContext())
            {
                string name = ((ISet<string>)httpRequest.FormData["name"]).FirstOrDefault();
                string link = ((ISet<string>)httpRequest.FormData["link"]).FirstOrDefault();
                decimal price = decimal.Parse(((ISet<string>)httpRequest.FormData["price"]).FirstOrDefault());


                var albumFromDb = db.Albums.Find(albumId);
                var albumPrice = db.Tracks.Where(track => track.Album == albumFromDb).Sum(track => track.Price);

                Track trackForDb = new Track
                {
                    Album = albumFromDb,
                    Name = name,
                    Link = link,
                    Price = price
                };

                this.ViewData["AlbumId"] = albumId;


                albumFromDb.Tracks.Add(trackForDb);
                db.Tracks.Add(trackForDb);
                db.Update(albumFromDb);
                db.SaveChanges();

                albumFromDb.Price = (albumPrice * 87) / 100;
                db.Update(albumFromDb);
                db.SaveChanges();
            }

            return this.Redirect($"../Albums/Details?id={albumId}");
        }

        public IHttpResponse Details(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("../Users/Login");
            }

            var albumId = httpRequest.QueryData["albumId"].ToString();
            var trackId = httpRequest.QueryData["trackId"].ToString();
            using (var db = new RunesDbContext())
            {
                var albumFromDb = db.Albums.Find(albumId);
                var trackFromDb = db.Tracks.Find(trackId);

                if (albumFromDb == null || trackFromDb == null)
                {
                    return this.Redirect("../Albums/All");
                }
                this.ViewData["Link"] = WebUtility.UrlDecode(trackFromDb.Link);
                this.ViewData["AlbumId"] = albumId;
                this.ViewData["Name"] = trackFromDb.Name;
                this.ViewData["Price"] = trackFromDb.Price;
            }

            return this.View();
        }
    }
}
