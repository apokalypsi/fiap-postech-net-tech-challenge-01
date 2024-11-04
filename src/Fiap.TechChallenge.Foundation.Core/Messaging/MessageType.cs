namespace Fiap.TechChallenge.Foundation.Core.Messaging;

/// <summary>
///     Identifica o tipo de uma mensagem.
/// </summary>
public enum MessageType
{
    /// <summary>
    ///     Comando - algo que será executado (uma ação).
    /// </summary>
    Command = 1,

    /// <summary>
    ///     Evento de domínio - algo que aconteceu no domínio interno do microsserviço.
    /// </summary>
    DomainEvent = 2,

    /// <summary>
    ///     Evento de integração - algo que aconteeu no domínio do microsserviço que deve ser publicado no mensageiro.
    /// </summary>
    IntegrationEvent = 3,

    /// <summary>
    ///     Query - consulta.
    /// </summary>
    Query = 4
}