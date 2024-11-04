namespace Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

/// <summary>
///     Classe que encapsula o retorno de um resultado de comando processando em outro microsserviço.
/// </summary>
public sealed class CommandResponse
{
    public CommandResponse(string correlationId, ICommandResult commandResult)
    {
        CorrelationId = correlationId;
        CommandResult = commandResult;
    }

    public string CorrelationId { get; }
    public ICommandResult CommandResult { get; }
}