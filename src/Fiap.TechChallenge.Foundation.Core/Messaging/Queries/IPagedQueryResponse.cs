namespace Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

public interface IPagedQueryResponse
{
    int Page { get; }
    int Limit { get; }
    int PageCount { get; }
    long ResultCount { get; }
}