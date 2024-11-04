using System.Text.RegularExpressions;
using Fiap.TechChallenge.Foundation.Core.Extensions;
using Fiap.TechChallenge.Foundation.Core.Languages;

namespace Fiap.TechChallenge.Foundation.Core.Validations;

public partial class Contract
{
    public Contract IsNotNullOrEmpty(string val, string property)
    {
        if (string.IsNullOrEmpty(val))
            AddValidation(property, Resources.COR_005.Args(property));

        return this;
    }

    public Contract StartsWith(string text, string find)
    {
        if (!text.StartsWith(find))
            AddValidation(Resources.COR_006.Args(text, find));

        return this;
    }

    public Contract IsNotNullOrEmpty(string val, string property, string message)
    {
        if (string.IsNullOrEmpty(val))
            AddValidation(property, message);

        return this;
    }

    public Contract IsNotNullOrWhiteSpace(string val, string property)
    {
        if (string.IsNullOrWhiteSpace(val))
            AddValidation(property, Resources.COR_005.Args(property));

        return this;
    }

    public Contract IsNotNullOrWhiteSpace(string val, string property, string message)
    {
        if (string.IsNullOrWhiteSpace(val))
            AddValidation(property, message);

        return this;
    }


    public Contract IsNullOrEmpty(string val, string property, string message)
    {
        if (!string.IsNullOrEmpty(val))
            AddValidation(property, message);

        return this;
    }

    public Contract HasMinLen(string val, int min, string property, string message)
    {
        if (string.IsNullOrEmpty(val) || val.Length < min)
            AddValidation(property, message);

        return this;
    }

    public Contract HasMinLen(string val, int min, string property)
    {
        if (string.IsNullOrEmpty(val) || val.Length < min)
            AddValidation(property, Resources.COR_004.Args(min));

        return this;
    }

    public Contract HasMaxLen(string val, int max, string property, string message)
    {
        if (string.IsNullOrEmpty(val) || val.Length > max)
            AddValidation(property, message);

        return this;
    }

    public Contract HasMaxLen(string val, int max, string property)
    {
        if (string.IsNullOrEmpty(val) || val.Length > max)
            AddValidation(property, Resources.COR_003.Args(max));

        return this;
    }

    public Contract HasLen(string val, int len, string property, string message)
    {
        if (string.IsNullOrEmpty(val) || val.Length != len)
            AddValidation(property, message);

        return this;
    }

    public Contract Contains(string val, string text, string property, string message)
    {
        if (!val.Contains(text))
            AddValidation(property, message);

        return this;
    }

    public Contract AreEquals(string val, string text, string property, string message,
        StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
    {
        if (Equals(val != text, comparisonType))
            AddValidation(property, message);

        return this;
    }

    public Contract AreNotEquals(string val, string text, string property, string message,
        StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
    {
        if (Equals(val == text, comparisonType))
            AddValidation(property, message);

        return this;
    }

    public Contract IsEmail(string email, string property)
    {
        const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        return Matchs(email, pattern, property, Resources.COR_010);
    }

    public Contract IsEmailOrEmpty(string email, string property)
    {
        if (string.IsNullOrEmpty(email))
            return this;

        return IsEmail(email, property);
    }

    public Contract IsUrl(string url, string property)
    {
        const string pattern =
            @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$";
        return Matchs(url, pattern, property, Resources.COR_011);
    }

    public Contract IsUrlOrEmpty(string url, string property)
    {
        if (string.IsNullOrEmpty(url))
            return this;

        return IsUrl(url, property);
    }

    public Contract Matchs(string text, string pattern, string property, string message)
    {
        if (!Regex.IsMatch(text ?? "", pattern))
            AddValidation(property, message);

        return this;
    }

    public Contract IsDigit(string text, string property, string message)
    {
        const string pattern = @"^\d+$";
        return Matchs(text, pattern, property, message);
    }
}