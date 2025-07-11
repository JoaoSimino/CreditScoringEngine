namespace CreditScoringEngine.Domain.Entities;

public class PropostaAprovadaEvent
{
    public string PropostaId { get; set; } = default!;
    public string ClienteId { get; set; } = default!;
    public decimal ValorAprovado { get; set; }
}
