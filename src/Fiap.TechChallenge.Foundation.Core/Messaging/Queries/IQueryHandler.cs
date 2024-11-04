namespace Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

/// <summary>
///     Handler para consultas.
/// </summary>
/// <typeparam name="TQueryRequest"></typeparam>
/// <typeparam name="TQueryResponse"></typeparam>
public interface IQueryHandler<in TQueryRequest, TQueryResponse> where TQueryRequest : IQueryRequest<TQueryResponse>
{
    Task<TQueryResponse> Handle(TQueryRequest queryRequest);
}