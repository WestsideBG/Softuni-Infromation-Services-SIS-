using System.Linq;
using System.Net;
using System.Text;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.App.Extensions
{
    public static class EntityExtensions
    {
        public static string AlbumsToHtml(this Album album)
        {
            return $@"<div><a href=""/Albums/Details?id={album.Id}"">{WebUtility.UrlDecode(album.Name)}</a></div>";
        }

        public static string AlbumsDetailsToHtml(this RunesDbContext db, Album album)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ol>");

            foreach (var track in db.Tracks.Where(track => track.Album == album))
            {
                sb.Append($@"<li><a href=""/Track/Details?albumId={album.Id}&trackId={track.Id}"">{WebUtility.UrlDecode(track.Name)}</a></li>");
            }

            sb.Append("</ol>");

            return sb.ToString();
        }
    }
}
