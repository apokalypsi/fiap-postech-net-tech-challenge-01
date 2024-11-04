using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TechChallenge.Contract.v1.Contato.ObterContatosPorDdd;

public class ObterContatosPorDddQueryRequest : IQueryRequest<ObterContatosPorDddQueryResult>
{
    public int Ddd { get; set; }
}