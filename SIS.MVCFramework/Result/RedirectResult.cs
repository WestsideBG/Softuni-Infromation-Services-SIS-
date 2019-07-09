using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;
using SIS.MVCFramework.Result;

namespace SIS.WebServer.Result
{
    public class RedirectResult : ActionResult
    {
        public RedirectResult(string location) : base(HttpResponseStatusCode.SeeOther)
        {
            this.Headers.AddHeader(new HttpHeader("Location", location));
        }
    }
}
