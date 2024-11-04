namespace Fiap.TechChallenge.Foundation.Core.Domain;

/// <summary>
///     Entidade base do tipo Guid.
/// </summary>
public abstract class GuidEntityBase : EntityBase<Guid>
{
    protected GuidEntityBase()
    {
        Id = Guid.NewGuid();
    }
}