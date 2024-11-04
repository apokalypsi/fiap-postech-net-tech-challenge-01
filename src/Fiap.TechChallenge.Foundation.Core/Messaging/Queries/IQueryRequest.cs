namespace Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

/// <inheritdoc cref="IMessage" />
/// <summary>
///     Indica uma requisição de pesquisa
/// </summary>
/// <typeparam name="TQueryResponse"></typeparam>
public interface IQueryRequest<TQueryResponse> : IMessage
{
}