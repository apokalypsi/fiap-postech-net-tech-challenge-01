namespace Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

public sealed class NullCommandSender : ICommandSender
{
    public Task<TCommandResult> ProcessAsync<TCommandResult>(ICommand<TCommandResult> commandRequest)
        where TCommandResult : ICommandResult, new()
    {
        return Task.FromResult(default(TCommandResult));
    }

    public Task<TCommandResult> SendAsync<TCommandResult>(ICommand<TCommandResult> commandRequest)
        where TCommandResult : ICommandResult, new()
    {
        return Task.FromResult(default(TCommandResult));
    }

    public Task<TCommandResult> SendAndWaitAsync<TCommandResult>(ICommand<TCommandResult> commandRequest,
        int milliSecondsTimeout)
        where TCommandResult : ICommandResult, new()
    {
        return Task.FromResult(default(TCommandResult));
    }
}