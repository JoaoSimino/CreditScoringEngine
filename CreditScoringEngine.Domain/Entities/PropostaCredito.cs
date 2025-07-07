namespace CreditScoringEngine.Domain.Entities;

public class PropostaCredito
{
    public Guid Id { get; set; }
    public decimal ValorSolicitado { get; set; }
    public DateTime DataProposta { get; set; }
    public StatusProposta Status { get; set; }
    public ScoreInterno Score { get; set; }
    public string Justificativa { get; set; }

    public Cliente Cliente { get; set; } = null!; //propriedade de navegacao, nuca pode ser nulo
    public Guid ClienteId { get; set; } //fk explicita
}
