namespace Fiap.TechChallenge.Foundation.Core.Domain;

/// <summary>
///     Entidade base com soft delete.
/// </summary>
/// <typeparam name="TIdType"></typeparam>
public abstract class EntityBaseWithSoftDelete<TIdType> : EntityBase<TIdType>, ISoftDelete
{
    protected EntityBaseWithSoftDelete()
    {
        Removed = false;
    }

    public bool Removed { get; }
    public DateTime? RemovedDate { get; private set; }
    public string RemovedBy { get; private set; }

    public void Remove(string user)
    {
        RemovedBy = user;
    }

    public void Remove(string user, DateTime date)
    {
        Remove(user);
        RemovedDate = date;
    }
}