using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Contract.v1.Contato.RemoverContato;

public class RemoverContatoCommand : ICommand<RemoverContatoCommandResult>
{
    public string Id { get; set; }
}