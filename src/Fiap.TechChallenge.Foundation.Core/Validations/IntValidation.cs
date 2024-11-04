using Fiap.TechChallenge.Foundation.Core.Extensions;
using Fiap.TechChallenge.Foundation.Core.Languages;

namespace Fiap.TechChallenge.Foundation.Core.Validations;

public partial class Contract
{
    #region Between

    public Contract IsBetween(int val, int from, int to, string property)
    {
        if (!(val > from && val < to))
            AddValidation(property, Resources.COR_009.Args(from, to));

        return this;
    }

    #endregion

    #region IsGreaterThan

    public Contract IsGreaterThan(decimal val, int comparer, string property)
    {
        if ((double)val <= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterThan(double val, int comparer, string property)
    {
        if (val <= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterThan(float val, int comparer, string property)
    {
        if (val <= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterThan(long val, int comparer, string property)
    {
        if (val <= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterThan(int val, int comparer, string property)
    {
        if (val <= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterThan(int? val, int comparer, string property)
    {
        if (!val.HasValue || val.Value <= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    #endregion

    #region IsGreaterOrEqualsThan

    public Contract IsGreaterOrEqualsThan(decimal val, int comparer, string property)
    {
        if ((double)val < comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterOrEqualsThan(double val, int comparer, string property)
    {
        if (val < comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterOrEqualsThan(float val, int comparer, string property)
    {
        if (val < comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterOrEqualsThan(long val, int comparer, string property)
    {
        if (val < comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsGreaterOrEqualsThan(int val, int comparer, string property)
    {
        if (val < comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    #endregion

    #region IsLowerThan

    public Contract IsLowerThan(decimal val, int comparer, string property)
    {
        if ((double)val >= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerThan(double val, int comparer, string property)
    {
        if (val >= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerThan(float val, int comparer, string property)
    {
        if (val >= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerThan(long val, int comparer, string property)
    {
        if (val >= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerThan(int val, int comparer, string property)
    {
        if (val >= comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    #endregion

    #region IsLowerOrEqualsThan

    public Contract IsLowerOrEqualsThan(decimal val, int comparer, string property)
    {
        if ((double)val > comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerOrEqualsThan(double val, int comparer, string property)
    {
        if (val > comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerOrEqualsThan(float val, int comparer, string property)
    {
        if (val > comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerOrEqualsThan(long val, int comparer, string property)
    {
        if (val > comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerOrEqualsThan(int val, int comparer, string property)
    {
        if (val > comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    public Contract IsLowerOrEqualsThan(int? val, int comparer, string property)
    {
        if (!val.HasValue || val > comparer)
            AddValidation(property, Resources.COR_004.Args(comparer));

        return this;
    }

    #endregion

    #region AreEquals

    public Contract AreEquals(decimal val, int comparer, string property, string message)
    {
        if ((double)val != comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract AreEquals(decimal val, int comparer, string property)
    {
        return AreEquals(val, comparer, property, Resources.COR_007.Args(comparer));
    }

    public Contract AreEquals(double val, int comparer, string property, string message)
    {
        if (val != comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract AreEquals(double val, int comparer, string property)
    {
        return AreEquals(val, comparer, property, Resources.COR_007.Args(comparer));
    }

    public Contract AreEquals(float val, int comparer, string property, string message)
    {
        if (val != comparer)
            AddValidation(property, message);
        return this;
    }

    public Contract AreEquals(float val, int comparer, string property)
    {
        return AreEquals(val, comparer, property, Resources.COR_007.Args(comparer));
    }

    public Contract AreEquals(long val, int comparer, string property, string message)
    {
        if (val != comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract AreEquals(long val, int comparer, string property)
    {
        return AreEquals(val, comparer, property, Resources.COR_007.Args(comparer));
    }

    public Contract AreEquals(int val, int comparer, string property, string message)
    {
        if (val != comparer)
            AddValidation(property, message);

        return this;
    }

    public Contract AreEquals(int val, int comparer, string property)
    {
        return AreEquals(val, comparer, property, Resources.COR_007.Args(comparer));
    }

    #endregion

    #region AreNotEquals

    public Contract AreNotEquals(decimal val, int comparer, string property)
    {
        if ((double)val == comparer)
            AddValidation(property, Resources.COR_008.Args(comparer));

        return this;
    }

    public Contract AreNotEquals(double val, int comparer, string property)
    {
        if (val == comparer)
            AddValidation(property, Resources.COR_008.Args(comparer));

        return this;
    }

    public Contract AreNotEquals(float val, int comparer, string property)
    {
        if (val == comparer)
            AddValidation(property, Resources.COR_008.Args(comparer));

        return this;
    }

    public Contract AreNotEquals(long val, int comparer, string property)
    {
        if (val == comparer)
            AddValidation(property, Resources.COR_008.Args(comparer));

        return this;
    }

    public Contract AreNotEquals(int val, int comparer, string property)
    {
        if (val == comparer)
            AddValidation(property, Resources.COR_008.Args(comparer));

        return this;
    }

    #endregion
}