namespace CreditScoringEngine.Application.Exceptions;

public class ClienteNotFoundException : Exception
{
    public ClienteNotFoundException(string? mensagem) : base(mensagem)
    {

    }
}
