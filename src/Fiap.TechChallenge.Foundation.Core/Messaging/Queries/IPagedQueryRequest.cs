namespace Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

/// <inheritdoc />
/// <summary>
///     Indica uma requisição de pesquisa com paginação
/// </summary>
/// <typeparam name="TQueryResponse"></typeparam>
public interface IPagedQueryRequest<TQueryResponse> : IQueryRequest<TQueryResponse>, IPagedQuery
    where TQueryResponse : IPagedQueryResponse
{
}