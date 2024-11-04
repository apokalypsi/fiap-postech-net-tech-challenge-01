namespace Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

/// <summary>
///     Representa a chamada para a execução de um comando.
/// </summary>
public interface ICommandSender
{
    /// <summary>
    ///     Processa um comando de formar síncrona.
    /// </summary>
    /// <typeparam name="TCommandResult"></typeparam>
    /// <param name="commandRequest"></param>
    /// <returns></returns>
    Task<TCommandResult> ProcessAsync<TCommandResult>(ICommand<TCommandResult> commandRequest)
        where TCommandResult : ICommandResult, new();

    /// <summary>
    ///     Envia um comando para ser processado em outro microsserviço.
    /// </summary>
    /// <typeparam name="TCommandResult"></typeparam>
    /// <param name="commandRequest"></param>
    /// <returns></returns>
    Task<TCommandResult> SendAsync<TCommandResult>(ICommand<TCommandResult> commandRequest)
        where TCommandResult : ICommandResult, new();

    /// <summary>
    ///     Envia um comando para ser executado via mensageria e aguarda o retorno.
    /// </summary>
    /// <typeparam name="TCommandResult"></typeparam>
    /// <param name="commandRequest"></param>
    /// <param name="milliSecondsTimeout"></param>
    /// <returns></returns>
    Task<TCommandResult> SendAndWaitAsync<TCommandResult>(ICommand<TCommandResult> commandRequest,
        int milliSecondsTimeout)
        where TCommandResult : ICommandResult, new();
}