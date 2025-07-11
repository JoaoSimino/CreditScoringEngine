namespace CreditScoringEngine.Infrastructure.Messaging;

public interface IMessagePublisher
{
    Task PublicarAsync<T>(T mensagem, string routingKey);
}
