using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MVCFramework.Attributes.Http;
using SIS.MVCFramework.Controller;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public IHttpResponse Index(IHttpRequest httpRequest)
        {
            if (this.IsLoggedIn(httpRequest))
            {
                this.ViewData["Username"] = httpRequest.Session.GetParameter("username");

                return this.View("Home");
            }

            return this.View();
        }
    }
}
