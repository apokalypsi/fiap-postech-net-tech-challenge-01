using Fiap.TechChallenge.Contato;
using Fiap.TechChallenge.Contato.Request;
using Fiap.TechChallenge.Contract.v1.Contato.CriarContato;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Command.v1.Contato;

public class CriarContatoCommandHandler : ICommandHandler<CriarContatoCommand, CriarContatoCommandResult>
{
    private readonly ILogger<CriarContatoCommandHandler> _logger;
    private readonly IContatoService _service;

    public CriarContatoCommandHandler(IContatoService service, ILogger<CriarContatoCommandHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<CriarContatoCommandResult> Handle(CriarContatoCommand commandRequest)
    {
        var result = await _service.CriarContatoAsync(new CriarContatoRequest(commandRequest.Nome, commandRequest.Telefone, commandRequest.Email, commandRequest.DDD));
        return new CriarContatoCommandResult
        {
            Id = result.Contato.Id,
            Nome = result.Contato.Nome,
            Telefone = result.Contato.Telefone,
            Email = result.Contato.Email,
            DDD = result.Contato.DDD
        };
    }

    public Task OnError(Exception exception, CriarContatoCommand commandRequest)
    {
        throw exception;
    }
}