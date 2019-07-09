using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;
using SIS.MVCFramework.Result;

namespace SIS.WebServer.Result
{
    public class InlineRecourseResult : ActionResult
    {
        public InlineRecourseResult(byte[] content, HttpResponseStatusCode statusCode) : base(statusCode)
        {
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentLength,content.Length.ToString()));
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentDisposition,"inline"));
            this.Content = content;
        }
    }
}
