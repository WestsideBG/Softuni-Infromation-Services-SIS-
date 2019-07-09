namespace SIS.MVCFramework.Attributes.Http
{
    using SIS.HTTP.Enums;
    using System;

    public abstract class BaseHttpAttribute : Attribute
    {
        public HttpRequestMethod Method { get; set; }

        public string Url { get; set; }

        public string ActionName { get; set; }
    }
}
