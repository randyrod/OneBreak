using System;
using System.IO;
using System.Xml.Serialization;

namespace OneBreakUtils
{
    public class XmlSerializationHelper
    {
        public static T DeserializeStringToXmlObject<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);

            try
            {
                T result = default(T);
                using (TextReader reader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    result = (T)serializer.Deserialize(reader);
                }

                return result;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
