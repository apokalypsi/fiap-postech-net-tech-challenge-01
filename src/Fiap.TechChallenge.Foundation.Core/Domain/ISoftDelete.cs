namespace Fiap.TechChallenge.Foundation.Core.Domain;

/// <summary>
///     Abstração para soft delete.
/// </summary>
public interface ISoftDelete
{
    bool Removed { get; }
    DateTime? RemovedDate { get; }
    string RemovedBy { get; }

    void Remove(string user);
    void Remove(string user, DateTime date);
}