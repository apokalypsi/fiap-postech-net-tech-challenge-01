using System.Xml.Serialization;

namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class XmlSerializerExtensions
{
    public static T ParseTo<T>(this string source)
    {
        if (string.IsNullOrEmpty(source)) throw new ArgumentException(nameof(source));

        var serializer = new XmlSerializer(typeof(T));
        using (var stringReader = new StringReader(source))
        {
            TextReader textReader = new StringReader(source);
            var xml = (T)serializer.Deserialize(textReader);

            return xml;
        }
    }
}