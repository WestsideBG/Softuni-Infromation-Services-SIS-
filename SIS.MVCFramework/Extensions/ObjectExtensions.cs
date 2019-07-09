using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SIS.MVCFramework.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToXml(this object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(stringWriter,obj);
                return stringWriter.ToString();
            }
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
