using Fiap.TechChallenge.Contato;
using Fiap.TechChallenge.Contato.Request;
using Fiap.TechChallenge.Contract.v1.Contato.RemoverContato;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Command.v1.Contato;

public class RemoverContatoCommandHandler : ICommandHandler<RemoverContatoCommand, RemoverContatoCommandResult>
{
    private readonly ILogger<RemoverContatoCommandHandler> _logger;
    private readonly IContatoService _service;

    public RemoverContatoCommandHandler(ILogger<RemoverContatoCommandHandler> logger, IContatoService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<RemoverContatoCommandResult> Handle(RemoverContatoCommand commandRequest)
    {
        var result = await _service.RemoverContatoAsync(new RemoverContatoRequest(Guid.Parse(commandRequest.Id)));
        return new RemoverContatoCommandResult
        {
            Sucesso = result.Sucesso
        };
    }

    public Task OnError(Exception exception, RemoverContatoCommand commandRequest)
    {
        throw exception;
    }
}