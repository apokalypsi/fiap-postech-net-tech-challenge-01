namespace Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

/// <inheritdoc cref="IMessage" />
/// <summary>
///     Representa um comando para o sistema.
/// </summary>
public interface ICommand<TCommandResult> : IMessage where TCommandResult : ICommandResult
{
}

public interface ICommand : ICommand<CommandResult>
{
}