using Fiap.TechChallenge.Contato;
using Fiap.TechChallenge.Contato.Request;
using Fiap.TechChallenge.Contract.v1.Contato.AtualizarContato;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Command.v1.Contato;

public class AtualizarContatoCommandHandler : ICommandHandler<AtualizarContatoCommand, AtualizarContatoCommandResult>
{
    private readonly ILogger<AtualizarContatoCommandHandler> _logger;
    private readonly IContatoService _service;

    public AtualizarContatoCommandHandler(ILogger<AtualizarContatoCommandHandler> logger, IContatoService service)
    {
        _logger = logger;
        _service = service;
    }


    public async Task<AtualizarContatoCommandResult> Handle(AtualizarContatoCommand commandRequest)
    {
        var result = await _service.AtualizarContatoAsync(new AtualizarContatoRequest(Guid.Parse(commandRequest.Id), commandRequest.Nome, commandRequest.Telefone, commandRequest.Email, commandRequest.DDD));
        return new AtualizarContatoCommandResult
        {
            Id = result.Contato.Id,
            Nome = result.Contato.Nome,
            Telefone = result.Contato.Telefone,
            Email = result.Contato.Email,
            DDD = result.Contato.DDD
        };
    }

    public Task OnError(Exception exception, AtualizarContatoCommand commandRequest)
    {
        throw exception;
    }
}