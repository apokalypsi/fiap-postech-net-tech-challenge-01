namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class EnumExtensions
{
    public static string ToStringDescription(this Enum value)
    {
        // get attributes
        var field = value.GetType().GetField(value.ToString());
        var attributes = field.GetCustomAttributes(false);

        // Description is in a hidden Attribute class called DisplayAttribute
        // Not to be confused with DisplayNameAttribute
        dynamic displayAttribute = null;

        if (attributes.Any()) displayAttribute = attributes.ElementAt(0);

        // return description
        return displayAttribute?.Description ?? "Descrição não encontrada";
    }

    public static string TryToStringDescription(this Enum value, string defaultValue = null)
    {
        try
        {
            return ToStringDescription(value);
        }
        catch
        {
            return defaultValue;
        }
    }
}