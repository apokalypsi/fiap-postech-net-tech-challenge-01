namespace Fiap.TechChallenge.Foundation.Core.Domain;

/// <summary>
///     Entidade base do tipo Guid - com soft delete.
/// </summary>
public abstract class GuidEntityBaseWithSoftDelete : EntityBaseWithSoftDelete<Guid>
{
    protected GuidEntityBaseWithSoftDelete()
    {
        Id = Guid.NewGuid();
    }
}