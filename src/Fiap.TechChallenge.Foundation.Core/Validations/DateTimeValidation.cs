namespace Fiap.TechChallenge.Foundation.Core.Validations;

public partial class Contract
{
    public Contract IsGreaterThan(DateTime val, DateTime comparer, string property, string message)
    {
        if (val <= comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract IsGreaterThan(DateTime? val, DateTime comparer, string property, string message)
    {
        if (val.HasValue && val <= comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract IsGreaterOrEqualsThan(DateTime val, DateTime comparer, string property, string message)
    {
        if (val < comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract IsGreaterOrEqualsThan(DateTime? val, DateTime comparer, string property, string message)
    {
        if (val.HasValue && val < comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract IsLowerThan(DateTime val, DateTime comparer, string property, string message)
    {
        if (val >= comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract IsLowerThan(DateTime? val, DateTime comparer, string property, string message)
    {
        if (val.HasValue && val >= comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract IsLowerOrEqualsThan(DateTime val, DateTime comparer, string property, string message)
    {
        if (val > comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract IsBetween(DateTime val, DateTime from, DateTime to, string property, string message)
    {
        if (!(val > from && val < to))
            AddValidation(property, message);

        return this;
    }
}