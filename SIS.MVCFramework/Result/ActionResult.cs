namespace SIS.MVCFramework.Result
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses;

    public class ActionResult : HttpResponse
    {
        protected ActionResult(HttpResponseStatusCode statusCode) :base(statusCode)
        {

        }
    }
}
