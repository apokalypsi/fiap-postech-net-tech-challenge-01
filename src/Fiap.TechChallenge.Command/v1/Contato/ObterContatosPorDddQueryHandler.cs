using Fiap.TechChallenge.Contato;
using Fiap.TechChallenge.Contato.Request;
using Fiap.TechChallenge.Contract.v1.Contato.ObterContatosPorDdd;
using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Command.v1.Contato;

public class ObterContatosPorDddQueryHandler : IQueryHandler<ObterContatosPorDddQueryRequest, ObterContatosPorDddQueryResult>
{
    private readonly ILogger<ObterContatosPorDddQueryHandler> _logger;
    private readonly IContatoService _service;

    public ObterContatosPorDddQueryHandler(ILogger<ObterContatosPorDddQueryHandler> logger, IContatoService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<ObterContatosPorDddQueryResult> Handle(ObterContatosPorDddQueryRequest queryRequest)
    {
        var porDddResult = await _service.ObterContatosPorDddAsync(new ObterContatosPorDddRequest(queryRequest.Ddd));
        var result = porDddResult.Contatos.Select(contato => new ContatoQueryResult(contato.Id, contato.Nome, contato.Telefone, contato.Email, contato.DDD)).ToList();

        return new ObterContatosPorDddQueryResult
        {
            Contatos = result
        };
    }
}