using Fiap.TechChallenge.Foundation.Core.Extensions;
using Fiap.TechChallenge.Foundation.Core.Languages;

namespace Fiap.TechChallenge.Foundation.Core.Validations;

public partial class Contract
{
    public Contract IsNull(object obj, string property, string message)
    {
        if (obj != null)
            AddValidation(property, message);

        return this;
    }

    public Contract IsNotNull(object obj, string property, string message)
    {
        if (obj == null)
            AddValidation(property, message);

        return this;
    }

    public Contract IsNotNull(object obj, string property)
    {
        if (obj == null)
            AddValidation(property, Resources.COR_001.Args(property));

        return this;
    }

    public Contract AreEquals(object obj, object comparer, string property, string message)
    {
        if (!obj.Equals(comparer))
            AddValidation(property, message);

        return this;
    }

    public Contract AreNotEquals(object obj, object comparer, string property, string message)
    {
        if (obj.Equals(comparer))
            AddValidation(property, message);

        return this;
    }
}