using System.Text;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MVCFramework.Result
{
    public class JsonResult : ActionResult
    {
        public JsonResult(string content, HttpResponseStatusCode statusCode = HttpResponseStatusCode.Ok) : base(statusCode)
        {
            this.AddHeader(new HttpHeader(HttpHeader.ContentType, "application/json"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
