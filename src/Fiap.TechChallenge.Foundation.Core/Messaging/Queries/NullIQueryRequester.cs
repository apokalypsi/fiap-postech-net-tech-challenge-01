namespace Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

public sealed class NullIQueryRequester : IQueryRequester
{
    public Task<TQueryResponse> Request<TQueryResponse>(IQueryRequest<TQueryResponse> queryRequest)
    {
        return Task.FromResult(default(TQueryResponse));
    }
}