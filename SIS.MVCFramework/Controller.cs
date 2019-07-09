using SIS.MVCFramework.Extensions;
using SIS.MVCFramework.Result;

namespace SIS.MVCFramework.Controller
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Result;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class Controller
    {
        protected void SignIn(IHttpRequest httpRequest, string userId,string username,string email)
        {
            httpRequest.Session.AddParameter("id", userId);
            httpRequest.Session.AddParameter("username", username);
            httpRequest.Session.AddParameter("email", email);
        }

        protected void SignOut(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();
        }

        protected Controller()
        {
            this.ViewData = new Dictionary<string, object>();
        }

        protected Dictionary<string,object> ViewData { get; set; }

        protected bool IsLoggedIn(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        private string ParseTemplate(string viewContent)
        {
            foreach (var param in ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", $"{param.Value.ToString()}");
            }

            return viewContent;
        }

        public ActionResult View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = System.IO.File.ReadAllText($"Views/{controllerName}/{viewName}.html");

            viewContent = this.ParseTemplate(viewContent);

            var htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);

            return htmlResult;
        }

        public ActionResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        public ActionResult Xml(object obj)
        {
            return new XmlResult(obj.ToXml());
        }

        public ActionResult Json(object obj)
        {
            return new JsonResult(obj.ToJson());
        }

        public ActionResult File()
        {
            return null;
        }
    }
}
