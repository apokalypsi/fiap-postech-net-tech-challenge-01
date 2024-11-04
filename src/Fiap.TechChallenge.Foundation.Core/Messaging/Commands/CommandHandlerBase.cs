namespace Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

/// <summary>
///     Representa um handler de comando.
/// </summary>
/// <typeparam name="TCommandRequest"></typeparam>
/// <typeparam name="TCommandResult"></typeparam>
public abstract class
    CommandHandlerBase<TCommandRequest, TCommandResult> : ICommandHandler<TCommandRequest, TCommandResult>
    where TCommandRequest : ICommand<TCommandResult>
    where TCommandResult : ICommandResult, new()
{
    public abstract Task<TCommandResult> Handle(TCommandRequest commandRequest);

    public virtual Task OnError(Exception exception, TCommandRequest commandRequest)
    {
        return Task.FromException(exception);
    }
}