using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TechChallenge.Contract.v1.Contato.ObterContatoPorId;

public class ObterContatoPorIdQueryRequest : IQueryRequest<ObterContatoPorIdQueryResult>
{
    public string Id { get; set; }
}