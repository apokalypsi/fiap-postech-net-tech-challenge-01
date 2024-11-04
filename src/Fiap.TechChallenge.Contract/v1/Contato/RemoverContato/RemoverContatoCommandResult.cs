using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Contract.v1.Contato.RemoverContato;

public class RemoverContatoCommandResult : CommandResult
{
    public bool Sucesso { get; set; }
}