namespace Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

/// <summary>
///     Requisição de uma consulta.
/// </summary>
public interface IQueryRequester
{
    Task<TQueryResponse> Request<TQueryResponse>(IQueryRequest<TQueryResponse> queryRequest);
}