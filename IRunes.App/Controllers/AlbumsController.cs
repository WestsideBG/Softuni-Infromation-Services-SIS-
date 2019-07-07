using System.Collections.Generic;
using System.Linq;
using System.Net;
using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MVCFramework.Attributes.Http;
using SIS.MVCFramework.Controller;

namespace IRunes.App.Controllers
{
    public class AlbumsController : Controller
    {
        [HttpGet(ActionName = "All")]
        public IHttpResponse AllAlbums(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("../Users/Login");
            }

            using (var db = new RunesDbContext())
            {
                if (!db.Albums.Any())
                {
                    this.ViewData["Albums"] = "There are currently no albums.";
                }
                else
                {
                    this.ViewData["Albums"] =
                        string.Join("<hr />", db.Albums.Select(album => album.AlbumsToHtml()));
                }
            }

            return this.View("All");
        }

        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            if (this.IsLoggedIn(httpRequest))
            {
                return this.View();
            }

            return this.Redirect("../Users/Login");
        }

        [HttpPost(Method = HttpRequestMethod.Post,ActionName = "Create")]
        public IHttpResponse CreateConfirm(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("../Users/Login");
            }

            using (var db = new RunesDbContext())
            {
                string name = ((ISet<string>)httpRequest.FormData["name"]).FirstOrDefault();
                string cover = ((ISet<string>)httpRequest.FormData["cover"]).FirstOrDefault();

                var newAlbum = new Album
                {
                    Name = name,
                    Cover = cover,
                    Price = 0M
                };

                db.Albums.Add(newAlbum);
                db.SaveChanges();
            }

            return this.Redirect("All");
        }

        public IHttpResponse Details(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("../Users/Login");
            }

            using (var db = new RunesDbContext())
            {
                var albumId = httpRequest.QueryData["id"].ToString();
                var albumFromDb = db.Albums.Find(albumId);
                var albumPrice = db.Tracks.Where(track => track.Album.Id == albumFromDb.Id).Sum(track => track.Price);
                albumPrice = (albumPrice * 87) / 100;
                if (albumFromDb == null)
                {
                    return this.Redirect("All");
                }

                //this.ViewData["Album"] = albumFromDb.AlbumsDetailsToHtml();
                this.ViewData["Cover"] = WebUtility.UrlDecode(albumFromDb.Cover);
                this.ViewData["Price"] = $"{albumPrice:F2}";
                this.ViewData["Name"] = albumFromDb.Name;
                this.ViewData["AlbumId"] = albumId;
                if (!db.Tracks.Any())
                {
                    this.ViewData["Tracks"] = "There are currently no tracks!";
                }
                else
                {
                    this.ViewData["Tracks"] = db.AlbumsDetailsToHtml(albumFromDb);
                }

                albumFromDb.Price = albumPrice;
                db.Update(albumFromDb);
                db.SaveChanges();
            }

            return this.View();
        }
    }
}
