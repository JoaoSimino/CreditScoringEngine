namespace CreditScoringEngine.Domain.Entities;

public class Cliente
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public decimal RendaMensal { get; set; }
    public int Idade { get; set; }
    public string HistoricoCreditoSimulado { get; set; }

    public PropostaCredito PropostaCredito { get; set; }
    public Guid PropostaCreditoId { get; set; }
}
