using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class SerializerExtensions
{
    // Utilização de inicializador estático para garantir thread-safety e lazy initialization.
    private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new()
    {
        ContractResolver = new DefaultContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Include,
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
    };

    public static T ParseTo<T>(this byte[] source)
    {
        var sourceMessage = Encoding.UTF8.GetString(source);
        return JsonConvert.DeserializeObject<T>(sourceMessage, DefaultJsonSerializerSettings);
    }

    public static T ParseToJson<T>(this string source)
    {
        return JsonConvert.DeserializeObject<T>(source, DefaultJsonSerializerSettings);
    }

    public static T ParseFromJson<T>(this string source)
    {
        return JsonConvert.DeserializeObject<T>(source, DefaultJsonSerializerSettings);
    }

    public static T TryParseToJson<T>(this string source)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(source, DefaultJsonSerializerSettings);
        }
        catch
        {
            return default;
        }
    }

    public static T TryParseFromJson<T>(this string source)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(source, DefaultJsonSerializerSettings);
        }
        catch
        {
            return default;
        }
    }

    public static object ParseToType(this byte[] source, Type sourceType)
    {
        var sourceMessage = Encoding.UTF8.GetString(source);
        return JsonConvert.DeserializeObject(sourceMessage, sourceType, DefaultJsonSerializerSettings);
    }

    public static object ParseToType(this string source, Type sourceType)
    {
        return JsonConvert.DeserializeObject(source, sourceType, DefaultJsonSerializerSettings);
    }

    public static string Serialize(this object source)
    {
        return JsonConvert.SerializeObject(source, DefaultJsonSerializerSettings);
    }
}