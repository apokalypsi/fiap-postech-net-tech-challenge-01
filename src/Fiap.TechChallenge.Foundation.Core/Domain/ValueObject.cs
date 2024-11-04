namespace Fiap.TechChallenge.Foundation.Core.Domain;

/// <summary>
///     Representa um valor de objeto - imutável.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    ///     Retornar as propriedades que garantem a unicidade do valor de objeto.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<object> GetAtomicValues();

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (GetType() != obj.GetType()) return false;

        var valueObject = (ValueObject)obj;
        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });
    }

    public static bool operator ==(ValueObject a, ValueObject b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }
}