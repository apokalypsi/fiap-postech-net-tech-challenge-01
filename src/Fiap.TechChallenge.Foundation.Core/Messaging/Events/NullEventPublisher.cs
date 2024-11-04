namespace Fiap.TechChallenge.Foundation.Core.Messaging.Events;

public sealed class NullEventPublisher : IEventPublisher
{
    public Task Publish<TEvent>(TEvent @event) where TEvent : DomainEventBase
    {
        return Task.CompletedTask;
    }
}