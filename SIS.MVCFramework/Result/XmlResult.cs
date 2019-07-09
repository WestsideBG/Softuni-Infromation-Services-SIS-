namespace SIS.MVCFramework.Result
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using System.Text;

    public class XmlResult : ActionResult
    {
        public XmlResult(string content,HttpResponseStatusCode statusCode = HttpResponseStatusCode.Ok) : base(statusCode)
        {
            this.AddHeader(new HttpHeader(HttpHeader.ContentType,"application/xml"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
