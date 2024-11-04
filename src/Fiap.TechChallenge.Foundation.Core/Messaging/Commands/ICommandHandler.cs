namespace Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

/// <summary>
///     Representa um handler de comando.
/// </summary>
/// <typeparam name="TCommandRequest"></typeparam>
/// <typeparam name="TCommandResult"></typeparam>
public interface ICommandHandler<in TCommandRequest, TCommandResult>
    where TCommandRequest : ICommand<TCommandResult>
    where TCommandResult : ICommandResult, new()
{
    /// <summary>
    ///     Processes the command request asynchronously.
    /// </summary>
    /// <param name="commandRequest">The command request to be processed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the command result.</returns>
    Task<TCommandResult> Handle(TCommandRequest commandRequest);

    /// <summary>
    ///     Handles any exception that occurs during the processing of the command request.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="commandRequest">The command request during which the exception occurred.</param>
    /// <returns>A task that represents the asynchronous error handling operation.</returns>
    Task OnError(Exception exception, TCommandRequest commandRequest);
}